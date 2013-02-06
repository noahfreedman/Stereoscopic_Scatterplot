/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* s3d GUI Texture Script
 * Usage: Attach to a GUI Texture.
 * Creates left and right copies of GUITexture for stereoscopic view. 
 * Can adjust GUI Texture parallax automatically to keep the GUITexture closer than anything it occludes.
 * GUITextures are placed in leftOnlyLayer and rightOnlyLayer (layers are set in s3dCamera.js).
 * Assumes that the GUI Texture Pixel Inset is set to the default (center) position, and that the
   x and y position (each set between 0.0 and 1.0) are used to place the GUITexture.
 * Dependencies: 
 * s3dCamera.js (on main camera) with useLeftRightOnlyLayers boolean set to TRUE (default)  
 */
  
#pragma strict

// *** Z Depth ***
// set initial GUIElement distance from camera
var objectDistance : float = 1.0;
// keep this GUIElement closer than anything behind it
var keepCloser : boolean = true;
// make GUIElement this a bit closer than nearest object or object under mouse
var nearPadding : float = 0.0;
// minimum distance for this GUIELement
var minimumDistance : float = 0.01;
// maximum distance for this GUIElement, no matter what's behind it
var maximumDistance : float = 100.0;

// *** Display & Movement ***
// start with guiText visible?
var beginVisible : boolean = true;
// timer to turn off texture if visible (0 = stays on forever)
var timeToDisplay : float = 0.0;
// how gradually to change depth (bigger numbers are slower - more than 25 is very slow);
var lagTime : float = 0;

private var camera3D : s3dCamera;
var objectCopyR : GameObject;
private var screenWidth : float;
public var obPosition : Vector3;
private var scrnPrlx : float = 0;
private var curScrnPrlx : float = 0;
private var checkpoints : Vector2[];
private var corner : Vector2;
public var on : boolean;
private var unitWidth : float;
private var s3dCursor : s3dGuiCursor;

@script AddComponentMenu ("stereoskopix/s3d GUI Texture")

function Start() {
	findS3dCamera();
	checkpoints = new Vector2[5];
	objectCopyR = Instantiate(gameObject, transform.position, transform.rotation);
	Destroy(objectCopyR.GetComponent(s3dGuiTexture));
	objectCopyR.name = gameObject.name+"_R";
	gameObject.name = gameObject.name+"_L";
	gameObject.layer = camera3D.leftOnlyLayer;
	objectCopyR.layer = camera3D.rightOnlyLayer;
	obPosition = gameObject.transform.position;
	// if using stereo shader + side-by-side + not squeezed, double width of guiTexture
	if (camera3D.useStereoShader && camera3D.format3D == 0 && !camera3D.sideBySideSqueezed) {
		var xWidth : float = guiTexture.pixelInset.width * 2;
		gameObject.guiTexture.pixelInset.width = xWidth;
		objectCopyR.guiTexture.pixelInset.width = xWidth;
		var xInset : float = gameObject.guiTexture.pixelInset.width/-2;
		gameObject.guiTexture.pixelInset.x = xInset;
		objectCopyR.guiTexture.pixelInset.x = xInset;
	// if not using stereo shader + squeezed, halve width of guiTexture
	} else if (!camera3D.useStereoShader && camera3D.sideBySideSqueezed) {
		xWidth = gameObject.guiTexture.pixelInset.width * 0.5;
		gameObject.guiTexture.pixelInset.width = xWidth;
		objectCopyR.guiTexture.pixelInset.width = xWidth;
		xInset = gameObject.guiTexture.pixelInset.width/-2;
		gameObject.guiTexture.pixelInset.x = xInset;
		objectCopyR.guiTexture.pixelInset.x = xInset;
	}
	// find corner offset
	corner = Vector2(gameObject.guiTexture.pixelInset.width/2/Screen.width,gameObject.guiTexture.pixelInset.height/2/Screen.height);
	toggleVisible(beginVisible);
		
	var horizontalFOV : float = (2 * Mathf.Atan(Mathf.Tan((camera3D.camera.fieldOfView * Mathf.Deg2Rad) / 2) * camera3D.camera.aspect))*Mathf.Rad2Deg;
	unitWidth = Mathf.Tan(horizontalFOV/2*Mathf.Deg2Rad); // need unit width to calculate cursor depth when there's a HIT (horizontal image transform)
	screenWidth = unitWidth*camera3D.zeroPrlxDist*2;
	setScreenParallax();
	
	if (timeToDisplay) {
		yield WaitForSeconds (timeToDisplay);
		toggleVisible(false);
	}
	s3dCursor = gameObject.GetComponent(s3dGuiCursor);
	if (s3dCursor) {
		s3dCursor.initialize();
	}

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

function Update() {
	if (on) {
		if (keepCloser) {
			findDistanceUnderObject();
		}
		if (curScrnPrlx != scrnPrlx) {
			curScrnPrlx += (scrnPrlx-curScrnPrlx)/(lagTime+1);
		}
		gameObject.transform.position = Vector3(obPosition.x + (curScrnPrlx/2), obPosition.y, gameObject.transform.position.z);
		objectCopyR.transform.position = Vector3(obPosition.x - (curScrnPrlx/2), obPosition.y, objectCopyR.gameObject.transform.position.z);
	}
}

function matchMousePos() {
	var mousePos : Vector2 = Input.mousePosition;
	if (camera3D.format3D == 0) { // side by side
		mousePos.x /= Screen.width/2;
	} else {
		mousePos.x /= Screen.width;
	}
	mousePos.y /= Screen.height;
	return mousePos;
}

function findDistanceUnderObject() {
	var nearDistance : float = Mathf.Infinity;
	var dPosition : Vector2;
	var actScript : s3dInteractor;
	if (camera3D.format3D == 0 && !camera3D.sideBySideSqueezed) {
		dPosition = Vector2((obPosition.x/2)+0.25,obPosition.y); // 0 = left, 0.5 = center, 1 = right
	} else {
		dPosition = obPosition;
	}
	checkpoints[0] = dPosition; // raycast against object center
	checkpoints[1] = dPosition + Vector2(-corner.x,-corner.y); // raycast against object corners
	checkpoints[2] = dPosition + Vector2(corner.x,-corner.y);
	checkpoints[3] = dPosition + Vector2(corner.x,corner.y);
	checkpoints[4] = dPosition + Vector2(-corner.x,corner.y);
	
	// raycast against all objects
	for (var cor=0; cor<5; cor++) {
		var hit: RaycastHit;
		var ray : Ray = camera3D.camera.ViewportPointToRay (checkpoints[cor]);
		if (Physics.Raycast (ray, hit, 100.0)) {
			Debug.DrawRay (ray.origin, ray.direction*hit.distance, Color(0,1,0,1));
			var camPlane : Plane = Plane(camera3D.camera.transform.forward, camera3D.camera.transform.position);
			var thePoint = ray.GetPoint(hit.distance);
			var currentDistance = camPlane.GetDistanceToPoint(thePoint);
			if (currentDistance < nearDistance) {
				nearDistance = currentDistance;
			}
		}
	}
	if (nearDistance < Mathf.Infinity) {
		objectDistance = Mathf.Clamp(nearDistance,minimumDistance,maximumDistance);
	}
	objectDistance = Mathf.Max(objectDistance,camera3D.camera.nearClipPlane);
	setScreenParallax();
}

function setScreenParallax() {
	var obPrlx : float = (camera3D.interaxial/1000)*(camera3D.zeroPrlxDist-objectDistance)/objectDistance;
	
	if (camera3D.format3D == 0 && !camera3D.sideBySideSqueezed) {
		scrnPrlx = (obPrlx/screenWidth*2) + nearPadding/1000 - (camera3D.H_I_T/(unitWidth*15)); // why 15? no idea.
	} else {
		scrnPrlx = (obPrlx/screenWidth*1) + nearPadding/1000 - (camera3D.H_I_T/(unitWidth*15));
	}
}

function toggleVisible(t : boolean) {
	on = t;
	if (t) {
		gameObject.active = true;
		if (objectCopyR) objectCopyR.active = true;
	} else {
		gameObject.active = false;
		if (objectCopyR) objectCopyR.active = false;
	}
}
