/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* s3d GUI Text Script
 * Usage: Attach to GUIText
 * Creates left and right copies of GUIText for stereoscopic view
 * Can adjust GUIText parallax automatically to keep the GUIText closer than anything it occludes.
 * Assumes that the GUIText Pixel Offset is set to the default (0,0) position, and that the
 * x and y position (each set between 0.0 and 1.0) are used to place the GUIText.
 * Dependencies: requires one s3dCamera.js with useLeftRightOnlyLayers boolean set to TRUE (default) 
 */

#pragma strict

// should GUI Text track mouse position?
var trackMouseXYPosition : boolean = false;
// should GUI Text track mouse only when down?
var onlyWhenMouseDown : boolean = true;
// set initial GUIElement distance from camera
var objectDistance : float = 1.0;
// keep this GUIElement closer than anything behind it
var keepCloser : boolean = false;
// make GUIElement a bit closer than nearest object or object under mouse
var nearPadding : float = 1.0;
// minimum distance for this GUIELement
var minimumDistance : float = 1.0;
// maximum distance for this GUIElement, no matter what's behind it
var maximumDistance : float = 3.0;
// how gradually to change depth (bigger numbers are slower - more than 25 is very slow);
var lagTime : float = 0;
// start with guiText visible;
var beginVisible : boolean = true;
// timer to turn off text if visible (0 to leave on)
var timeToDisplay : float = 2.0;
// string to begin with
public var initString : String;
// text color
var TextColor : Color = Color.white;
// shadows?
var shadowsOn : boolean  = true;
// shadowColor
var ShadowColor : Color = Color.black;
// shadowOffset
var shadowOffset : float = 5.0;

private var camera3D : s3dCamera;
private var objectCopyR : GameObject;
private var shadowL : GameObject;
private var shadowR : GameObject;
private var screenWidth : float;
public var obPosition : Vector3;
private var scrnPrlx : float = 0;
private var curScrnPrlx : float = 0;
private var rays = new Array();
private var corners : Vector2[];
private var corner : Vector2;
private var textOn : boolean;
private var unitWidth : float;

@script AddComponentMenu ("stereoskopix/s3d GUI Text")

function Start() {
	findS3dCamera();
	corners = new Vector2[4];
	objectCopyR = Instantiate(gameObject, transform.position, transform.rotation);
	Destroy(objectCopyR.GetComponent(s3dGuiText));
	objectCopyR.name = gameObject.name+"_R";
	objectCopyR.transform.parent = gameObject.transform.parent;
	gameObject.name = gameObject.name+"_L";
	gameObject.layer = camera3D.leftOnlyLayer;
	gameObject.guiText.material.color = TextColor;
	objectCopyR.layer = camera3D.rightOnlyLayer;
	objectCopyR.gameObject.guiText.material.color = TextColor;
	if (shadowsOn) {
		shadowL = Instantiate(objectCopyR.gameObject, transform.position, transform.rotation);
		shadowL.name = gameObject.name+"_shadL";
		shadowL.gameObject.layer = camera3D.leftOnlyLayer;
		shadowL.guiText.material.color = ShadowColor; 
		shadowL.transform.parent = transform;
		shadowR = Instantiate(objectCopyR.gameObject, transform.position, transform.rotation);
		shadowR.name = gameObject.name+"_shadR";
		shadowR.gameObject.layer = camera3D.rightOnlyLayer;
		shadowR.guiText.material.color = ShadowColor; 
		shadowR.transform.parent = objectCopyR.transform;
	}
	obPosition = gameObject.transform.position;
	setText(initString);
	toggleVisible(beginVisible);
	
	var horizontalFOV : float = (2 * Mathf.Atan(Mathf.Tan((camera3D.camera.fieldOfView * Mathf.Deg2Rad) / 2) * camera3D.camera.aspect))*Mathf.Rad2Deg;
	unitWidth = Mathf.Tan(horizontalFOV/2*Mathf.Deg2Rad); // need unit width to calculate cursor depth when there's a HIT
	screenWidth = unitWidth*camera3D.zeroPrlxDist*2;
	
	if (timeToDisplay) {
		yield WaitForSeconds (timeToDisplay);
//toggleVisible(false);
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
	if (textOn) {
		var obPrlx : float;
		if (trackMouseXYPosition) {
			if (!onlyWhenMouseDown || (onlyWhenMouseDown && Input.GetMouseButton(0))) {
				obPosition = matchMousePos();
			}
		}
		if (keepCloser) {
			findDistanceUnderObject();
			objectDistance = Mathf.Max(objectDistance,camera3D.camera.nearClipPlane);
		}
		setScreenParallax();
		if (curScrnPrlx != scrnPrlx) {
			curScrnPrlx += (scrnPrlx-curScrnPrlx)/(lagTime+1);
		}
		gameObject.transform.position = Vector3(obPosition.x + (curScrnPrlx/2), obPosition.y, gameObject.transform.position.z+1);
		if (shadowsOn) shadowL.gameObject.transform.localPosition = Vector3(shadowOffset/1100, -shadowOffset/1000, 0);
		objectCopyR.transform.position = Vector3(obPosition.x - (curScrnPrlx/2), obPosition.y, objectCopyR.gameObject.transform.position.z+1);
		if (shadowsOn) shadowR.gameObject.transform.localPosition = Vector3(shadowOffset/900, -shadowOffset/1000, 0);
	}
}

function findDistanceUnderObject() {
	var nearDistance : float = Mathf.Infinity;
	var dPosition : Vector2;
	if (camera3D.format3D == 0 && !camera3D.sideBySideSqueezed) {
		dPosition = Vector2((obPosition.x/2)+0.25,obPosition.y); // 0 = left, 0.5 = center, 1 = right
	} else {
		dPosition = obPosition;
	}
	var hit: RaycastHit;
	var ray : Ray = camera3D.camera.ViewportPointToRay (dPosition);
	if (Physics.Raycast (ray, hit, 100.0)) {
		Debug.DrawRay (ray.origin, ray.direction*hit.distance, Color(0,1,0,1));
		var camPlane : Plane = Plane(camera3D.camera.transform.forward, camera3D.camera.transform.position);
		var thePoint = ray.GetPoint(hit.distance);
		nearDistance = camPlane.GetDistanceToPoint(thePoint);
	}
	if (nearDistance < Mathf.Infinity) {
		objectDistance = Mathf.Clamp(nearDistance,minimumDistance,maximumDistance);
	}
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
	textOn = t;
	gameObject.active = textOn;
	if (objectCopyR) objectCopyR.active = textOn;
	if (shadowsOn) {
		if (shadowL) shadowL.active = textOn;
		if (shadowR) shadowR.active = textOn;
	}
}

function setText(theText: String) {
	gameObject.guiText.text =  theText;
	if (objectCopyR) objectCopyR.guiText.text = theText;
	if (shadowsOn) {
		if (shadowL) shadowL.guiText.text = theText;
		if (shadowR) shadowR.guiText.text = theText;
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

// set position from another script (rollover text)
function setObPosition(obPos : Vector2) {
	obPosition.x = obPos.x;
	obPosition.y = obPos.y;
}
