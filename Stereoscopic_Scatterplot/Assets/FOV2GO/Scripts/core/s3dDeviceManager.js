/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* s3d Device Manager.
 * Usage: Manages layout and interface elements for Android & iOS phones and tablets 
 * Has companion Editor script to create custom inspector 
 */

#pragma strict
@script ExecuteInEditMode()
// enum phoneType {GalaxyNexus_LandLeft, GalaxyNote_LandLeft, iPad2_LandLeft, iPad2_Portrait, iPad3_LandLeft, iPad3_Portrait, iPhone4_LandLeft, OneS_LandLeft, Rezound_LandLeft, Thrill_LandLeft, my3D_LandLeft};
// declared in s3dEnums.js
public var phoneLayout : phoneType = phoneType.iPhone4_LandLeft;
private var prevPhoneLayout : phoneType;
public var phoneResolution : Vector2;
public var camera3D : s3dCamera;

// cursor controls
public var use3dCursor : boolean = true;
public var cursor3D : s3dGuiCursor;
private var cursorSize : float;

// interface controls
//enum controlPos {off,left,center,right}
public var movePadPosition : controlPos = controlPos.left;
public var turnPadPosition : controlPos = controlPos.center;
public var pointPadPosition : controlPos = controlPos.right;

public var moveTouchpad : s3dTouchpad;
public var turnTouchpad : s3dTouchpad;
public var pointTouchpad : s3dTouchpad;

// stereo parameter controls
public var useStereoParamsTouchpad : boolean;
public var stereoParamsTouchpad : s3dTouchpad;
private var stereoParamsRect : Rect;
public var interaxialTouchpad : s3dTouchpad;
private var interaxialRect : Rect;
public var zeroPrlxTouchpad : s3dTouchpad;
private var zeroPrlxRect : Rect;
public var hitTouchpad : s3dTouchpad;
private var hitRect : Rect;

// load new scene control
public var showLoadNewScenePad : boolean;
public var loadNewSceneTouchpad : s3dTouchpad;
private var loadNewSceneRect : Rect;

// first-person tool controls
public var showFpsTool01 : boolean;
public var fpsTool01 : s3dTouchpad;
public var showFpsTool02 : boolean;
public var fpsTool02 : s3dTouchpad;
private var fpsTool01Rect : Rect;
private var fpsTool02Rect : Rect;

function Start () {
	findS3dCamera();
	prevPhoneLayout = phoneLayout;
	setPhoneLayout();
}

function findS3dCamera () {
    var cameras3D : s3dCamera[] = FindObjectsOfType(s3dCamera) as s3dCamera[];
    if (cameras3D.length == 1) {
    	camera3D = cameras3D[0];
    } else if (cameras3D.length > 1) {
    	print("There is more than one s3dCamera in this scene.");
    } else {
    	print ("There is no s3dCamera in this scene.");
    }
}


function setPhoneLayout() {
	var leftControlPos : Vector3;
	var leftControlRect : Rect;
	var middleControlPos : Vector3;
	var middleControlRect : Rect;
	var rightControlPos : Vector3;
	var rightControlRect : Rect;
	camera3D.useStereoShader = false;
	camera3D.format3D = mode3D.SideBySide;
	camera3D.sideBySideSqueezed = false;
	camera3D.usePhoneMask = true;
	switch (phoneLayout) {
		case (phoneType.GalaxyNexus_LandLeft):
			camera3D.leftViewRect = Vector4(0.0, 0.19, 0.49, 0.81);
			camera3D.rightViewRect = Vector4(0.51, 0.19, 0.49, 0.81);
			setCameraRect(camera3D.leftViewRect,camera3D.rightViewRect);
			camera3D.H_I_T = 2;
			cursorSize = 64;
			
			leftControlPos = Vector3(0, 0, 0);
			leftControlRect = Rect(0, 0, 400, 200);
			middleControlPos = Vector3(0.5, 0, 0);
			middleControlRect = Rect(-200, 0, 400, 200);
			rightControlPos = Vector3(1, 0, 0);
			rightControlRect = Rect(-400, 0, 400, 200);
			
			stereoParamsRect = Rect(-64, 280, 128, 128);
			interaxialRect = Rect(150, 200, 300, 80);
			zeroPrlxRect = Rect(-150, 200, 300, 80);
			hitRect = Rect(-450, 200, 300, 80);
			
			loadNewSceneRect = Rect(-64, 440, 128, 128);
			
			fpsTool01Rect = Rect(0, 200, 128, 128);
			fpsTool02Rect = Rect(-128, 200, 128, 128);
			break;
		case (phoneType.GalaxyNote_LandLeft):
			camera3D.leftViewRect = Vector4(0.0, 0.22, 0.49, 0.78);
			camera3D.rightViewRect = Vector4(0.51, 0.22, 0.49, 0.78);
			setCameraRect(camera3D.leftViewRect,camera3D.rightViewRect);
			camera3D.H_I_T = 0;
			cursorSize = 64;
			
			leftControlPos = Vector3(0, 0, 0);
			leftControlRect = Rect(0, 0, 400, 200);
			middleControlPos = Vector3(0.5, 0, 0);
			middleControlRect = Rect(-200, 0, 400, 200);
			rightControlPos = Vector3(1, 0, 0);
			rightControlRect = Rect(-400, 0, 400, 200);
			
			stereoParamsRect = Rect(-64, 300, 128, 128);
			interaxialRect = Rect(150, 200, 300, 80);
			zeroPrlxRect = Rect(-150, 200, 300, 80);
			hitRect = Rect(-450, 200, 300, 80);
			
			loadNewSceneRect = Rect(-64, 450, 128, 128);

			fpsTool01Rect = Rect(0, 200, 128, 128);
			fpsTool02Rect = Rect(-128, 200, 128, 128);
			break;
		case (phoneType.iPad2_LandLeft):
			camera3D.leftViewRect = Vector4(0, 0.33, 0.5, 0.67);
			camera3D.rightViewRect = Vector4(0.5, 0.33, 0.5, 0.67);
			setCameraRect(camera3D.leftViewRect,camera3D.rightViewRect);
			camera3D.H_I_T = -12;
			cursorSize = 48;
			
			leftControlPos = Vector3(0, 0, 0);
			leftControlRect = Rect(0, 256, 300, 150);
			middleControlPos = Vector3(0.5, 0, 0);
			middleControlRect = Rect(-100, 160, 200, 100);
			rightControlPos = Vector3(1, 0, 0);
			rightControlRect = Rect(-300, 256, 300, 150);
			
			stereoParamsRect = Rect(-24, 96, 48, 48);
			interaxialRect = Rect(120, 64, 240, 32);
			zeroPrlxRect = Rect(-120, 64, 240, 32);
			hitRect = Rect(-360, 64, 240, 32);

			loadNewSceneRect = Rect(-24, 16, 48, 48);

			fpsTool01Rect = Rect(128, 192, 64, 64);
			fpsTool02Rect = Rect(-192, 192, 64, 64);
			
			break;
		case (phoneType.iPad2_Portrait):
			camera3D.leftViewRect = Vector4(0.08, 0.6875, 0.42, 0.3125);
			camera3D.rightViewRect = Vector4(0.5, 0.6875, 0.42, 0.3125);
			setCameraRect(camera3D.leftViewRect,camera3D.rightViewRect);
			camera3D.H_I_T = 0;
			cursorSize = 48;
			
			leftControlPos = Vector3(0, 1, 0);
			leftControlRect = Rect(0, -640, 384, 192);
			middleControlPos = Vector3(0.5, 1, 0);
			middleControlRect = Rect(-128, -768, 256, 128);
			rightControlPos = Vector3(1, 1, 0);
			rightControlRect = Rect(-384, -640, 384, 192);
			
			stereoParamsRect = Rect(-32, 164, 64, 64);
			interaxialRect = Rect(0, 92, 256, 64);
			zeroPrlxRect = Rect(-128, 92, 256, 64);
			hitRect = Rect(-256, 92, 256, 64);

			loadNewSceneRect = Rect(-32, 16, 64, 64);

			fpsTool01Rect = Rect(0, 256, 128, 128);
			fpsTool02Rect = Rect(-128, 256, 128, 128);
			break;
		case (phoneType.iPad3_LandLeft):
			camera3D.leftViewRect = Vector4(0, 0.33, 0.5, 0.67);
			camera3D.rightViewRect = Vector4(0.5, 0.33, 0.5, 0.67);
			setCameraRect(camera3D.leftViewRect,camera3D.rightViewRect);
			camera3D.H_I_T = -12;
			cursorSize = 64;
			
			leftControlPos = Vector3(0, 0, 0);
			leftControlRect = Rect(0, 640, 600, 300);
			middleControlPos = Vector3(0, 0, 0);
			middleControlRect = Rect(0, 256, 400, 200);
			rightControlPos = Vector3(1, 0, 0);
			rightControlRect = Rect(-600, 640, 600, 300);
			
			stereoParamsRect = Rect(-64, 280, 128, 128);
			interaxialRect = Rect(240, 180, 480, 80);
			zeroPrlxRect = Rect(-240, 180, 480, 80);
			hitRect = Rect(-720, 180, 480, 80);

			loadNewSceneRect = Rect(-64, 32, 128, 128);

			fpsTool01Rect = Rect(384, 512, 128, 128);
			fpsTool02Rect = Rect(-512, 512, 128, 128);
			
			break;
		case (phoneType.iPad3_Portrait):
			camera3D.leftViewRect = Vector4(0.08, 0.6875, 0.42, 0.3125);
			camera3D.rightViewRect = Vector4(0.5, 0.6875, 0.42, 0.3125);
			setCameraRect(camera3D.leftViewRect,camera3D.rightViewRect);
			camera3D.H_I_T = 0;
			cursorSize = 64;
			
			leftControlPos = Vector3(0, 1, 0);
			//leftControlRect = Rect(120, -1280, 512, 512);
			leftControlRect = Rect(0, -1280, 768, 384);
			middleControlPos = Vector3(0.5, 1, 0);
			middleControlRect = Rect(-256, -1536, 512, 256);
			rightControlPos = Vector3(1, 1, 0);
			rightControlRect = Rect(-768, -1280, 768, 384);
			
			stereoParamsRect = Rect(-64, 328, 128, 128);
			interaxialRect = Rect(0, 184, 512, 128);
			zeroPrlxRect = Rect(-256, 184, 512, 128);
			hitRect = Rect(-512, 184, 512, 128);

			loadNewSceneRect = Rect(-64, 32, 128, 128);

			fpsTool01Rect = Rect(0, 512, 256, 256);
			fpsTool02Rect = Rect(-256, 512, 256, 256);
			break;
		case (phoneType.iPhone4_LandLeft):
			camera3D.leftViewRect = Vector4(0.0, 0.27, 0.49, 0.73);
			camera3D.rightViewRect = Vector4(0.51, 0.27, 0.49, 0.73);
			setCameraRect(camera3D.leftViewRect,camera3D.rightViewRect);
			camera3D.H_I_T = 4;
			cursorSize = 64;
			
			leftControlPos = Vector3(0, 0, 0);
			leftControlRect = Rect(0, 0, 320, 160);
			middleControlPos = Vector3(0.5, 0, 0);
			middleControlRect = Rect(-160, 0, 320, 160);
			rightControlPos = Vector3(1, 0, 0);
			rightControlRect = Rect(-320, 0, 320, 160);
			
			stereoParamsRect = Rect(-50, 260, 100, 100);
			interaxialRect = Rect(120, 180, 240, 60);
			zeroPrlxRect = Rect(-120, 180, 240, 60);
			hitRect = Rect(-360, 180, 240, 60);

			loadNewSceneRect = Rect(-50, 380, 100, 100);

			fpsTool01Rect = Rect(0, 172, 100, 100);
			fpsTool02Rect = Rect(-100, 172, 100, 100);
			break;
		case (phoneType.OneS_LandLeft):
			camera3D.leftViewRect = Vector4(0.0, 0.18, 0.49, 0.82);
			camera3D.rightViewRect = Vector4(0.51, 0.18, 0.49, 0.82);
			setCameraRect(camera3D.leftViewRect,camera3D.rightViewRect);
			camera3D.H_I_T = 8;
			cursorSize = 64;
			
			leftControlPos = Vector3(0, 0, 0);
			leftControlRect = Rect(0, 0, 240, 120);
			middleControlPos = Vector3(0.5, 0, 0);
			middleControlRect = Rect(-120, 0, 240, 120);
			rightControlPos = Vector3(1, 0, 0);
			rightControlRect = Rect(-240, 0, 240, 120);
			
			stereoParamsRect = Rect(-50, 200, 100, 100);
			interaxialRect = Rect(100, 130, 240, 60);
			zeroPrlxRect = Rect(-120, 130, 240, 60);
			hitRect = Rect(-340, 130, 240, 60);

			loadNewSceneRect = Rect(-50, 320, 100, 100);

			fpsTool01Rect= Rect(0, 120, 100, 100);
			fpsTool02Rect = Rect(-100, 120, 100, 100);
			break;
		case (phoneType.Rezound_LandLeft):
			camera3D.leftViewRect = Vector4(0.0, 0.27, 0.49, 0.73);
			camera3D.rightViewRect = Vector4(0.51, 0.27, 0.49, 0.73);
			setCameraRect(camera3D.leftViewRect,camera3D.rightViewRect);
			camera3D.H_I_T = 2;
			cursorSize = 64;
			
			leftControlPos = Vector3(0, 0, 0);
			leftControlRect = Rect(0, 0, 400, 200);
			middleControlPos = Vector3(0.5, 0, 0);
			middleControlRect = Rect(-200, 0, 400, 200);
			rightControlPos = Vector3(1, 0, 0);
			rightControlRect = Rect(-400, 0, 400, 200);
			
			stereoParamsRect = Rect(-64, 280, 128, 128);
			interaxialRect = Rect(150, 200, 300, 80);
			zeroPrlxRect = Rect(-150, 200, 300, 80);
			hitRect = Rect(-450, 200, 300, 80);
			
			loadNewSceneRect = Rect(-64, 440, 128, 128);
			
			fpsTool01Rect = Rect(0, 200, 128, 128);
			fpsTool02Rect = Rect(-128, 200, 128, 128);
			break;
		case (phoneType.Thrill_LandLeft):
			camera3D.leftViewRect = Vector4(0.0, 0.19, 0.49, 0.81);
			camera3D.rightViewRect = Vector4(0.51, 0.19, 0.49, 0.81);
			setCameraRect(camera3D.leftViewRect,camera3D.rightViewRect);
			camera3D.H_I_T = 2;
			cursorSize = 40;
			
			leftControlPos = Vector3(0, 0, 0);
			leftControlRect = Rect(0, 0, 256, 128);
			middleControlPos = Vector3(0.5, 0, 0);
			middleControlRect = Rect(-128, 0, 256, 128);
			rightControlPos = Vector3(1, 0, 0);
			rightControlRect = Rect(-256, 0, 256, 128);
			
			stereoParamsRect = Rect(-32, 200, 64, 64);
			interaxialRect = Rect(100, 128, 200, 50);
			zeroPrlxRect = Rect(-100, 128, 200, 50);
			hitRect = Rect(-300, 128, 200, 50);

			loadNewSceneRect = Rect(-32, 300, 64, 64);

			fpsTool01Rect = Rect(0,128,64,64);
			fpsTool02Rect = Rect(-64,128,64,64);
			break;
		case (phoneType.my3D_LandLeft):
			camera3D.leftViewRect = Vector4(0.0, 0.075, 0.4675, 0.925);
			camera3D.rightViewRect = Vector4(0.529, 0.075, 0.4675, 0.925);
			setCameraRect(camera3D.leftViewRect,camera3D.rightViewRect);
			camera3D.H_I_T = 3;
			cursorSize = 64;
			
			leftControlPos = Vector3(0, 0, 0);
			leftControlRect = Rect(0, 0, 200, 100);
			middleControlPos = Vector3(0.5, 0, 0);
			middleControlRect = Rect(-80, 100, 160, 80);
			rightControlPos = Vector3(1, 0, 0);
			rightControlRect = Rect(-200, 0, 200, 100);
			
			stereoParamsRect = Rect(-40, 400, 80, 80);
			interaxialRect = Rect(120, 100, 240, 60);
			zeroPrlxRect = Rect(-120, 100, 240, 60);
			hitRect = Rect(-360, 100, 240, 60);

			loadNewSceneRect = Rect(-40, 500, 80, 80);

			fpsTool01Rect = Rect(440, 200, 80, 80);
			fpsTool02Rect = Rect(-520, 300, 80, 80);
			break;
	}

	if (moveTouchpad) {
		moveTouchpad.gameObject.active = true;
		moveTouchpad.enabled = true;
		if (movePadPosition == controlPos.left) {
			moveTouchpad.transform.position = leftControlPos;
			moveTouchpad.GetComponent(GUITexture).pixelInset = leftControlRect;
		} else if (movePadPosition == controlPos.center) {
			moveTouchpad.transform.position = middleControlPos;
			moveTouchpad.GetComponent(GUITexture).pixelInset = middleControlRect;
		} else if (movePadPosition == controlPos.right) {
			moveTouchpad.transform.position = rightControlPos;
			moveTouchpad.GetComponent(GUITexture).pixelInset = rightControlRect;
		} else if (movePadPosition == controlPos.off) {
			moveTouchpad.gameObject.active = false;
		}
		moveTouchpad.setUp();
	}
	if (turnTouchpad) {
		turnTouchpad.gameObject.active = true;
		turnTouchpad.enabled = true;
		if (turnPadPosition == controlPos.left) {
			turnTouchpad.transform.position = leftControlPos;
			turnTouchpad.GetComponent(GUITexture).pixelInset = leftControlRect;
		} else if (turnPadPosition == controlPos.center) {
			turnTouchpad.transform.position = middleControlPos;
			turnTouchpad.GetComponent(GUITexture).pixelInset = middleControlRect;
		} else if (turnPadPosition == controlPos.right) {
			turnTouchpad.transform.position = rightControlPos;
			turnTouchpad.GetComponent(GUITexture).pixelInset = rightControlRect;
		} else if (turnPadPosition == controlPos.off) {
			turnTouchpad.gameObject.active = false;
		}
		turnTouchpad.setUp();
	}
	if (pointTouchpad) {
		pointTouchpad.gameObject.active = true;
		pointTouchpad.enabled = true;
		if (pointPadPosition == controlPos.left) {
			pointTouchpad.transform.position = leftControlPos;
			pointTouchpad.GetComponent(GUITexture).pixelInset = leftControlRect;
		} else if (pointPadPosition == controlPos.center) {
			pointTouchpad.transform.position = middleControlPos;
			pointTouchpad.GetComponent(GUITexture).pixelInset = middleControlRect;
		} else if (pointPadPosition == controlPos.right) {
			pointTouchpad.transform.position = rightControlPos;
			pointTouchpad.GetComponent(GUITexture).pixelInset = rightControlRect;
		} else if (pointPadPosition == controlPos.off) {
			pointTouchpad.gameObject.active = false;
		}
		pointTouchpad.setUp();
	}
	
	if (stereoParamsTouchpad) {
		if (useStereoParamsTouchpad) {
			stereoParamsTouchpad.gameObject.active = true;
			stereoParamsTouchpad.transform.position = Vector3(0.5, 0, 0);
			stereoParamsTouchpad.GetComponent(GUITexture).pixelInset = stereoParamsRect;
			interaxialTouchpad.transform.position = Vector3(0, 0, 0);
			interaxialTouchpad.GetComponent(GUITexture).pixelInset = interaxialRect;
			zeroPrlxTouchpad.transform.position = Vector3(0.5, 0, 0);
			zeroPrlxTouchpad.GetComponent(GUITexture).pixelInset = zeroPrlxRect;
			hitTouchpad.transform.position = Vector3(1, 0, 0);
			hitTouchpad.GetComponent(GUITexture).pixelInset = hitRect;
			stereoParamsTouchpad.setUp();
			interaxialTouchpad.setUp();
			zeroPrlxTouchpad.setUp();
			hitTouchpad.setUp();
		} else {
			stereoParamsTouchpad.gameObject.active = false;
		}
	}
	
	if (loadNewSceneTouchpad) {
		if (showLoadNewScenePad) {
			loadNewSceneTouchpad.gameObject.active = true;
			loadNewSceneTouchpad.transform.position = Vector3(0.5, 0, 0);
			loadNewSceneTouchpad.GetComponent(GUITexture).pixelInset = loadNewSceneRect;
			loadNewSceneTouchpad.setUp();
		} else {
			loadNewSceneTouchpad.gameObject.active = false;
		}
	}
	
	if (fpsTool01) {
		if (showFpsTool01) {
			fpsTool01.gameObject.active = true;
			fpsTool01.transform.position = Vector3(0,0,0);
			fpsTool01.GetComponent(GUITexture).pixelInset = fpsTool01Rect;
			fpsTool01.setUp();
		} else {
			fpsTool01.gameObject.active = false;
		}
	}

	if (fpsTool02) {
		if (showFpsTool02) {
			fpsTool02.gameObject.active = true;
			fpsTool02.transform.position = Vector3(1,0,0);
			fpsTool02.GetComponent(GUITexture).pixelInset = fpsTool02Rect;
			fpsTool02.setUp();
		} else {
			fpsTool02.gameObject.active = false;
		}
	}
	
	if (cursor3D) {
		var texture3D : s3dGuiTexture;
		texture3D = cursor3D.GetComponent(s3dGuiTexture);
		if (use3dCursor) {
			texture3D.toggleVisible(true);
			cursor3D.transform.position = Vector3(0.5, 0.5, 0);
			cursor3D.GetComponent(GUITexture).pixelInset = Rect(cursorSize/-2,cursorSize/-2,cursorSize,cursorSize);
		} else {
			texture3D.toggleVisible(false);
		}
	}
}

function setCameraRect(l : Vector4, r : Vector4) {
	// don't mess with screen layout if stereoShader is being used for anaglyph, etc
	if (!camera3D.useStereoShader) {
		camera3D.leftCam.camera.rect = Rect(l.x, l.y, l.z, l.w);
		camera3D.rightCam.camera.rect = Rect(r.x,r.y,r.z,r.w);
		camera3D.leftViewRect = camera3D.RectToVector4(camera3D.leftCam.camera.rect);
		camera3D.rightViewRect = camera3D.RectToVector4(camera3D.rightCam.camera.rect);
	}
}

function Update () {
	#if UNITY_EDITOR
		checkForChanges();
	#endif
}

function checkForChanges() {
	// only run in edit mode
    if (prevPhoneLayout != phoneLayout) {
        setPhoneLayout();
    }
    prevPhoneLayout = phoneLayout;
}

// called from Editor script
function forceUpdate() {
	setPhoneLayout();
   	prevPhoneLayout = phoneLayout;
	camera3D.initStereoCamera();
}
