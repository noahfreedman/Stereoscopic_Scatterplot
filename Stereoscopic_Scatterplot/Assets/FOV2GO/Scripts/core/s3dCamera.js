/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* s3dCamera.js revised 5/28/12. Usage:
 * Attach to camera. Creates, manages and renders stereoscopic view.
 * NOTE: interaxial is measured in millimeters; zeroPrlxDist is measured in meters 
 * Has companion Editor script to create custom inspector 
 */

#pragma strict
// 1. Camera
public var leftCam : GameObject; // left view camera
public var rightCam : GameObject; // right view camera
public var maskCam : GameObject; // black mask for mobile formats
public var guiCam : GameObject; // mask for gui overlay for mobile formats
// Stereo Parameters
public var interaxial : float = 65; // Distance (in millimeters) between cameras
public var zeroPrlxDist : float = 3.0; // Distance (in meters) at which left and right images overlap exactly
// 3D Camera Configuration // 
public var toedIn : boolean = false; // Angle cameras inward to converge. Bad idea!
// Camera Selection // 
//enum cams3D {Left_Right, Left_Only, Right_Only, Right_Left} // declared in s3dEnums.js
public var cameraSelect = cams3D.Left_Right; // View order - swap cameras for cross-eyed free-viewing
public var H_I_T: float = 0;
public var offAxisFrustum : float = 0;
private var cameraWidth : float;
// Options
public var useCustomStereoLayer : boolean = false; // Set a custom layer to use multiple stereo cameras
public var stereoLayer : int = 0; // Camera will render this layer only
public var useLeftRightOnlyLayers : boolean = true; // Enable layers seen by only one camera
public var leftOnlyLayer : int = 20; // Layer seen by left camera only
public var rightOnlyLayer : int = 21; // Layer seen by right camera only
public var guiOnlyLayer : int = 22; // Layer seen by gui camera only
public var renderOrderDepth : int = 0; // For multiple stereo cameras - higher layers are rendered on top of lower layers

// 2. Render
// enable useStereoShader to use RenderTextures & stereo shader (Unity Pro only) for desktop applications - allows anaglyph format
// turn off for Unity Free, Android and iOS (allows side by side mode only)
public var useStereoShader : boolean = false;
private var useStereoShaderPrev : boolean = false; // track changes to useStereoShader
// Stereo Material + Stereo Shader (uses FOV2GO/Shaders/stereo3DViewMethods)
public var stereoMaterial : Material; // Assign FOV2GO/Materials/stereoMat material in inspector
// Render Textures
public var leftCamRT : RenderTexture; // Assign FOV2GO/RenderTextures/RenderTextureL in Inspector
public var rightCamRT: RenderTexture; // Assign FOV2GO/RenderTextures/RenderTextureR in Inspector
// Scene Screen Mode: the stereo camera can be assigned to two planes with RenderTextures in the scene (Unity Pro only)
public var sceneScreenL : RenderTexture; // Assign RenderTexture for Left Scene Screen in inspector
public var sceneScreenR : RenderTexture; // Assign RenderTexture for Right Scene Screen in inspector

// 3D Display Mode // 
//enum mode3D {SideBySide, Anaglyph, OverUnder, Interlace, Checkerboard}; // declared in s3dEnums.js
public var format3D = mode3D.SideBySide;

// Anaglyph Mode
//enum anaType {Monochrome, HalfColor, FullColor, Optimized, Purple};  // declared in s3dEnums.js
public var anaglyphOptions = anaType.HalfColor;
// Side by Side Mode
public var sideBySideSqueezed : boolean = false;
public var usePhoneMask : boolean = true;
public var leftViewRect : Vector4 = Vector4(0,0,0.5,1);
public var rightViewRect : Vector4 = Vector4(0.5,0,1,1);
// Interlace Variables
public var interlaceRows : int = 1080;
public var checkerboardColumns : int = 1920;
public var checkerboardRows : int = 1080;

public var planes : Plane[];

@script AddComponentMenu ("stereoskopix/s3d Camera")
@script ExecuteInEditMode()

function Awake () {
	initStereoCamera();
}

function initStereoCamera () {
	var lcam = transform.Find("leftCam"); // check if we've already created a leftCam
	if (lcam) {
		leftCam = lcam.gameObject;
		leftCam.camera.CopyFrom (camera);
	} else {
		leftCam = new GameObject ("leftCam", Camera);
		leftCam.AddComponent(GUILayer);
		leftCam.camera.CopyFrom (camera);
		leftCam.transform.parent = transform;
	}

	var rcam = transform.Find("rightCam"); // check if we've already created a rightCam
	if (rcam) {
		rightCam = rcam.gameObject;
		rightCam.camera.CopyFrom (camera);
	} else {
		rightCam = new GameObject("rightCam", Camera);
		rightCam.AddComponent(GUILayer);
		rightCam.camera.CopyFrom (camera);
		rightCam.transform.parent = transform;
	}
	
	var mcam = transform.Find("maskCam"); // check if we've already created a maskCam
	if (mcam) {
		maskCam = mcam.gameObject;
	} else {
		maskCam = new GameObject("maskCam", Camera);
		maskCam.AddComponent(GUILayer);
		maskCam.camera.CopyFrom (camera);
		maskCam.transform.parent = transform;
	}
	
	var gcam = transform.Find("guiCam"); // check if we've already created a maskCam
	if (gcam) {
		guiCam = gcam.gameObject;
	} else {
		guiCam = new GameObject("guiCam", Camera);
		guiCam.AddComponent(GUILayer);
		guiCam.camera.CopyFrom (camera);
		guiCam.transform.parent = transform;
	}

	var guiC : GUILayer = GetComponent(GUILayer);
	guiC.enabled = false;
	
	camera.depth = -2; // rendering order (back to front): centerCam/maskCam/leftCam1/rightCam1/leftCam2/rightCam2/ etc
	
	var horizontalFOV : float = (2 * Mathf.Atan(Mathf.Tan((camera.fieldOfView * Mathf.Deg2Rad) / 2) * camera.aspect))*Mathf.Rad2Deg;
	cameraWidth = Mathf.Tan(horizontalFOV/2*Mathf.Deg2Rad)*zeroPrlxDist*2;
	
	leftCam.camera.depth = camera.depth + (renderOrderDepth*2) + 2;
	rightCam.camera.depth = camera.depth + ((renderOrderDepth*2)+1) + 3;
	
	if (useLeftRightOnlyLayers) {
		if (useCustomStereoLayer) {
			leftCam.camera.cullingMask = (1 << stereoLayer | 1 << leftOnlyLayer); // show stereo + left only
			rightCam.camera.cullingMask = (1 << stereoLayer | 1 << rightOnlyLayer); // show stereo + right only
		} else {
			leftCam.camera.cullingMask = ~(1 << rightOnlyLayer | 1 << guiOnlyLayer); // show everything but right only layer & mask only layer
			rightCam.camera.cullingMask = ~(1 << leftOnlyLayer | 1 << guiOnlyLayer); // show everything but left only layer & mask only layer
		}
	} else {
		if (useCustomStereoLayer) {
			leftCam.camera.cullingMask = (1 << stereoLayer); // show stereo layer only
			rightCam.camera.cullingMask = (1 << stereoLayer); // show stereo layer only
		}
	}
		
	maskCam.camera.depth = leftCam.camera.depth-1;
	guiCam.camera.depth = rightCam.camera.depth+1;
	
	maskCam.camera.cullingMask = 0;
	guiCam.camera.cullingMask = 1 << guiOnlyLayer; // only show what's in the guiOnly layer
	maskCam.camera.clearFlags = CameraClearFlags.SolidColor;
	guiCam.camera.clearFlags = CameraClearFlags.Depth;
	maskCam.camera.backgroundColor = Color.black;
	
	// scene screens format incompatible with useStereoShader
	if (format3D == mode3D.SceneScreens) {
		useStereoShader = false;
	}

	if (useStereoShader) {
		/* as of Unity 3.5.2, this gives an error - probably not necessary 
		leftCamRT.width = Screen.width;
		rightCamRT.width = Screen.width;
		leftCamRT.height = Screen.height;
		rightCamRT.height = Screen.height;
		leftCamRT.depth = 24;
		rightCamRT.depth = 24;
		*/
		stereoMaterial.SetTexture ("_LeftTex", leftCamRT);
		stereoMaterial.SetTexture ("_RightTex", rightCamRT);
	
		leftCam.camera.targetTexture = leftCamRT;
		rightCam.camera.targetTexture = rightCamRT;
	} else {
		if (format3D != mode3D.SceneScreens) {
			format3D = mode3D.SideBySide;
			if (!usePhoneMask) {
				leftCam.camera.rect = Rect(0,0,0.5,1);
				rightCam.camera.rect = Rect(0.5,0,0.5,1);
			} else {
				leftCam.camera.rect = Vector4toRect(leftViewRect);
				rightCam.camera.rect = Vector4toRect(rightViewRect);
			}
			leftViewRect = RectToVector4(leftCam.camera.rect);
			rightViewRect = RectToVector4(rightCam.camera.rect);
			fixCameraAspect();
		} else {
			leftCam.camera.rect = Rect(0,0,1,1);
			rightCam.camera.rect = Rect(0,0,1,1);
			leftCam.camera.targetTexture = sceneScreenL;
			rightCam.camera.targetTexture = sceneScreenR;
		}
	}
	
	switch (format3D) {
		case (mode3D.SideBySide):
			if (useStereoShader) {
				maskCam.active = false;
			} else {
				maskCam.active = true;
			}
			break;
		case (mode3D.Anaglyph):
			maskCam.active = false;
			SetAnaType();
			break;
		case (mode3D.Interlace):
			maskCam.active = false;
			SetWeave(false);
			break;
		case (mode3D.Checkerboard):
			maskCam.active = false;
			SetWeave(true);
			break;
	}
	#if !UNITY_EDITOR
		if (!useStereoShader) {
			camera.enabled = false; // speeds up rendering, especially on Android, but doesn't work if useStereoShader is true
		}
	#endif
	
}

// called from initStereoCamera (above), and from s3dGyroCam.js (when phone orientation is changed due to AutoRotation)
function fixCameraAspect() {
	camera.ResetAspect();
	//yield WaitForSeconds(0.25);
	camera.aspect *= leftCam.camera.rect.width*2/leftCam.camera.rect.height;
	leftCam.camera.aspect = camera.aspect;
	rightCam.camera.aspect = camera.aspect;
}

function Vector4toRect(v : Vector4) {
	var r : Rect =  Rect(v.x,v.y,v.z,v.w);
	return r;
}
	
function RectToVector4(r : Rect) {
	var v : Vector4 = Vector4(r.x,r.y,r.width,r.height);
	return v;
}
	
function Update() {
	#if UNITY_EDITOR
	if (!useStereoShader) {
		if (EditorApplication.isPlaying) {  // speeds up rendering while in play mode, but doesn't work if useStereoShader is true
			camera.enabled = false;
		} else {
			camera.enabled = true; // need camera enabled when in edit mode
		}
	}
	if (useStereoShader) {
		if (useStereoShaderPrev == false) {
			releaseRenderTextures();
			initStereoCamera();
		}
	} else {
		if (useStereoShaderPrev == true) {
			releaseRenderTextures();
			initStereoCamera();
		}
	}
	useStereoShaderPrev = useStereoShader;
	#endif
	planes = GeometryUtility.CalculateFrustumPlanes(camera);
	UpdateView();
}

function UpdateView() {
	switch (cameraSelect) {
		case cams3D.Left_Right:
			leftCam.transform.position = transform.position + transform.TransformDirection(-interaxial/2000.0, 0, 0);
			rightCam.transform.position = transform.position + transform.TransformDirection(interaxial/2000.0, 0, 0);
			break;
		case cams3D.Left_Only:
			leftCam.transform.position = transform.position + transform.TransformDirection(-interaxial/2000.0, 0, 0);
			rightCam.transform.position = transform.position + transform.TransformDirection(-interaxial/2000.0, 0, 0);
			break;
		case cams3D.Right_Only:
			leftCam.transform.position = transform.position + transform.TransformDirection(interaxial/2000.0, 0, 0);
			rightCam.transform.position = transform.position + transform.TransformDirection(interaxial/2000.0, 0, 0);
			break;
		case cams3D.Right_Left:
			leftCam.transform.position = transform.position + transform.TransformDirection(interaxial/2000.0, 0, 0);
			rightCam.transform.position = transform.position + transform.TransformDirection(-interaxial/2000.0, 0, 0);
			break;
	}
	if (toedIn) {
		leftCam.camera.projectionMatrix = camera.projectionMatrix;
		rightCam.camera.projectionMatrix = camera.projectionMatrix;
		leftCam.transform.LookAt (transform.position + (transform.TransformDirection (Vector3.forward) * zeroPrlxDist));
		rightCam.transform.LookAt (transform.position + (transform.TransformDirection (Vector3.forward) * zeroPrlxDist));
	} else {
		leftCam.transform.rotation = transform.rotation; 
		rightCam.transform.rotation = transform.rotation;
		switch (cameraSelect) {
			case cams3D.Left_Right:
				leftCam.camera.projectionMatrix = projectionMatrix(true);
				rightCam.camera.projectionMatrix = projectionMatrix(false);
				break;
			case cams3D.Left_Only:
				leftCam.camera.projectionMatrix = projectionMatrix(true);
				rightCam.camera.projectionMatrix = projectionMatrix(true);
				break;
			case cams3D.Right_Only:
				leftCam.camera.projectionMatrix = projectionMatrix(false);
				rightCam.camera.projectionMatrix = projectionMatrix(false);
				break;
			case cams3D.Right_Left:
				leftCam.camera.projectionMatrix = projectionMatrix(false);
				rightCam.camera.projectionMatrix = projectionMatrix(true);
				break;
		}
	}
}

// Calculate Stereo Projection Matrix
function projectionMatrix(isLeftCam : boolean) : Matrix4x4 {
	var left : float;
	var right : float;
	var a : float;
	var b : float;
	var FOVrad : float;
	var tempAspect: float = camera.aspect;
	FOVrad = camera.fieldOfView / 180.0 * Mathf.PI;
	if (format3D == mode3D.SideBySide) {
		if (!sideBySideSqueezed) {
			tempAspect /= 2;	// if side by side unsqueezed, double width
		}
	}
	a = camera.nearClipPlane * Mathf.Tan(FOVrad * 0.5);
	b = camera.nearClipPlane / zeroPrlxDist;
	if (isLeftCam) {
		left  = (-tempAspect * a) + (interaxial/2000.0 * b) + (H_I_T/100) + (offAxisFrustum/100);
		right =	(tempAspect * a) + (interaxial/2000.0 * b) + (H_I_T/100) + (offAxisFrustum/100);
	} else {
		left  = (-tempAspect * a) - (interaxial/2000.0 * b) - (H_I_T/100) + (offAxisFrustum/100);
		right =	(tempAspect * a) - (interaxial/2000.0 * b) - (H_I_T/100) + (offAxisFrustum/100);
	}
	return PerspectiveOffCenter(left, right, -a, a, camera.nearClipPlane, camera.farClipPlane);
} 

function PerspectiveOffCenter(
	left : float, right : float,
	bottom : float, top : float,
	near : float, far : float ) : Matrix4x4 {
	var x =  (2.0 * near) / (right - left);
	var y =  (2.0 * near) / (top - bottom);
	var a =  (right + left) / (right - left);
	var b =  (top + bottom) / (top - bottom);
	var c = -(far + near) / (far - near);
	var d = -(2.0 * far * near) / (far - near);
	var e = -1.0;

	var m : Matrix4x4;
	m[0,0] = x;  m[0,1] = 0;  m[0,2] = a;  m[0,3] = 0;
	m[1,0] = 0;  m[1,1] = y;  m[1,2] = b;  m[1,3] = 0;
	m[2,0] = 0;  m[2,1] = 0;  m[2,2] = c;  m[2,3] = d;
	m[3,0] = 0;  m[3,1] = 0;  m[3,2] = e;  m[3,3] = 0;
	return m;
}

function releaseRenderTextures() {
	leftCam.camera.targetTexture = null;
	rightCam.camera.targetTexture = null;
	leftCamRT.Release();
	rightCamRT.Release();
}	

// Draw Scene Gizmos
function OnDrawGizmos () {
	var gizmoLeft : Vector3 = transform.position + transform.TransformDirection(-interaxial/2000.0, 0, 0); // interaxial/2/1000mm
	var gizmoRight : Vector3 = transform.position + transform.TransformDirection(interaxial/2000.0, 0, 0);
	var gizmoTarget : Vector3 = transform.position + transform.TransformDirection (Vector3.forward) * zeroPrlxDist;
	Gizmos.color = Color (1,1,1,1);
	Gizmos.DrawLine (gizmoLeft, gizmoTarget);
	Gizmos.DrawLine (gizmoRight, gizmoTarget);
	Gizmos.DrawLine (gizmoLeft, gizmoRight);
	Gizmos.DrawSphere (gizmoLeft, 0.02);
	Gizmos.DrawSphere (gizmoRight, 0.02);
	Gizmos.DrawSphere (gizmoTarget, 0.02);
}

function OnRenderImage (source:RenderTexture, destination:RenderTexture) {
	if (useStereoShader) {
	   RenderTexture.active = destination;
	   GL.PushMatrix();
	   GL.LoadOrtho();
	   switch (format3D) {
	   	case mode3D.Anaglyph:
	   	    stereoMaterial.SetPass(0);
	      	DrawQuad(0);
	   		break;
	   	case mode3D.SideBySide:
	   	case mode3D.OverUnder:
			for(var i:int = 1; i <= 2; i++) {
				stereoMaterial.SetPass(i);
				DrawQuad(i);
			}
			break;
		case mode3D.Interlace:
		case mode3D.Checkerboard:
			stereoMaterial.SetPass(3);
			DrawQuad(3);
	   		break;
	   	default:
	   		break;
	   }
	   GL.PopMatrix();
	}
}

// Interlace & Checkerboard Modes
function SetWeave(xy) {
	if (xy) {
		stereoMaterial.SetFloat("_Weave_X", checkerboardColumns);
		stereoMaterial.SetFloat("_Weave_Y", checkerboardRows);
	} else {
		stereoMaterial.SetFloat("_Weave_X", 1);
		stereoMaterial.SetFloat("_Weave_Y", interlaceRows);
	}
}

// Anaglyph Mode
function SetAnaType() {
   switch (anaglyphOptions) {
   		case anaType.Monochrome:
   			stereoMaterial.SetVector("_Balance_Left_R", Vector4(0.299,0.587,0.114,0));
   			stereoMaterial.SetVector("_Balance_Left_G", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Left_B", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Right_R", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Right_G", Vector4(0.299,0.587,0.114,0));
  			stereoMaterial.SetVector("_Balance_Right_B", Vector4(0.299,0.587,0.114,0));
	   	break;
   		case anaType.HalfColor:
   			stereoMaterial.SetVector("_Balance_Left_R", Vector4(0.299,0.587,0.114,0));
   			stereoMaterial.SetVector("_Balance_Left_G", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Left_B", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Right_R", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Right_G", Vector4(0,1,0,0));
  			stereoMaterial.SetVector("_Balance_Right_B", Vector4(0,0,1,0));
	   	break;
   		case anaType.FullColor:
   			stereoMaterial.SetVector("_Balance_Left_R", Vector4(1,0,0,0));
   			stereoMaterial.SetVector("_Balance_Left_G", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Left_B", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Right_R", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Right_G", Vector4(0,1,0,0));
  			stereoMaterial.SetVector("_Balance_Right_B", Vector4(0,0,1,0));
	   	break;
   		case anaType.Optimized:
   			stereoMaterial.SetVector("_Balance_Left_R", Vector4(0,0.7,0.3,0));
   			stereoMaterial.SetVector("_Balance_Left_G", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Left_B", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Right_R", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Right_G", Vector4(0,1,0,0));
  			stereoMaterial.SetVector("_Balance_Right_B", Vector4(0,0,1,0));
	   	break;
   		case anaType.Purple:
   			stereoMaterial.SetVector("_Balance_Left_R", Vector4(0.299,0.587,0.114,0));
   			stereoMaterial.SetVector("_Balance_Left_G", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Left_B", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Right_R", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Right_G", Vector4(0,0,0,0));
  			stereoMaterial.SetVector("_Balance_Right_B", Vector4(0.299,0.587,0.114,0));
	   	break;
   }
}

// Draw Render Textures Quads
function DrawQuad(cam) {
	if (format3D == mode3D.Anaglyph) {
   		GL.Begin (GL.QUADS);      
      	GL.TexCoord3( 0.0, 0.0, 0.0 ); GL.Vertex3( 0.0, 0.0, 0.0 );
      	GL.TexCoord3( 1.0, 0.0, 0.0 ); GL.Vertex3( 1.0, 0.0, 0.0 );
      	GL.TexCoord3( 1.0, 1.0, 0.0 ); GL.Vertex3( 1.0, 1.0, 0.0 );
      	GL.TexCoord3( 0.0, 1.0, 0.0 ); GL.Vertex3( 0.0, 1.0, 0.0 );
   		GL.End();
	} else {
		if (format3D==mode3D.SideBySide) {
			if (cam==1) {
		   		GL.Begin (GL.QUADS);      
		      	GL.TexCoord2( 0.0, 0.0 ); GL.Vertex3( 0.0, 0.0, 0.1 );
		      	GL.TexCoord2( 1.0, 0.0 ); GL.Vertex3( 0.5, 0.0, 0.1 );
		      	GL.TexCoord2( 1.0, 1.0 ); GL.Vertex3( 0.5, 1.0, 0.1 );
		      	GL.TexCoord2( 0.0, 1.0 ); GL.Vertex3( 0.0, 1.0, 0.1 );
		   		GL.End();
			} else {
		   		GL.Begin (GL.QUADS);      
		      	GL.TexCoord2( 0.0, 0.0 ); GL.Vertex3( 0.5, 0.0, 0.1 );
		      	GL.TexCoord2( 1.0, 0.0 ); GL.Vertex3( 1.0, 0.0, 0.1 );
		      	GL.TexCoord2( 1.0, 1.0 ); GL.Vertex3( 1.0, 1.0, 0.1 );
		      	GL.TexCoord2( 0.0, 1.0 ); GL.Vertex3( 0.5, 1.0, 0.1 );
		   		GL.End();
			}
		} else if (format3D == mode3D.OverUnder) {
			if (cam==1) {
		   		GL.Begin (GL.QUADS);      
		      	GL.TexCoord2( 0.0, 0.0 ); GL.Vertex3( 0.0, 0.5, 0.1 );
		      	GL.TexCoord2( 1.0, 0.0 ); GL.Vertex3( 1.0, 0.5, 0.1 );
		      	GL.TexCoord2( 1.0, 1.0 ); GL.Vertex3( 1.0, 1.0, 0.1 );
		      	GL.TexCoord2( 0.0, 1.0 ); GL.Vertex3( 0.0, 1.0, 0.1 );
		   		GL.End();
			} else {
		   		GL.Begin (GL.QUADS);      
		      	GL.TexCoord2( 0.0, 0.0 ); GL.Vertex3( 0.0, 0.0, 0.1 );
		      	GL.TexCoord2( 1.0, 0.0 ); GL.Vertex3( 1.0, 0.0, 0.1 );
		      	GL.TexCoord2( 1.0, 1.0 ); GL.Vertex3( 1.0, 0.5, 0.1 );
		      	GL.TexCoord2( 0.0, 1.0 ); GL.Vertex3( 0.0, 0.5, 0.1 );
		   		GL.End();
			} 
		} else if (format3D == mode3D.Interlace || format3D == mode3D.Checkerboard) {
	   		GL.Begin (GL.QUADS);      
	      	GL.TexCoord2( 0.0, 0.0 ); GL.Vertex3( 0.0, 0.0, 0.1 );
	      	GL.TexCoord2( 1.0, 0.0 ); GL.Vertex3( 1, 0.0, 0.1 );
	      	GL.TexCoord2( 1.0, 1.0 ); GL.Vertex3( 1, 1.0, 0.1 );
	      	GL.TexCoord2( 0.0, 1.0 ); GL.Vertex3( 0.0, 1.0, 0.1 );
	   		GL.End();
		}
	}
} 

