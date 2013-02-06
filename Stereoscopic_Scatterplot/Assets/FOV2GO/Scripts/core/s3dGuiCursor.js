/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* s3d GUI Cursor Script
 * Usage: Attach to a GUI Texture, along with  s3dGuiTexture.js
 * Creates a stereoscopic cursor (s3d_Gui_Pointer)
 * Assign 3 textures (default, click and pick) and 2 sounds (click and pick).
 * assign s3dTouchpad for cursor input on mobile devices
 * s3dTouchpad mimics touchpad input with mouse on desktop (leave trackMouseXYPosition OFF)
 * cursor triggers clickAction function in Interactor.js (attached to interactive objects)
 * interactive objects need to be placed in interactiveLayer (default is layer 9) 
 */
  
#pragma strict
// hide default pointer so it doesn't compete with 3D cursor
var hidePointer : boolean = false;

private var camera3D : s3dCamera;
private var s3dTexture : s3dGuiTexture;

// *** interaction ***
// layer for clickable objects
public var interactiveLayer : int = 23; // interactive layer
// main texture
var defaultTexture : Texture;
// texture for click
var clickTexture : Texture;
// texture for grab
var pickTexture : Texture;
// maximum distance for object clicks
var clickDistance : float = 20;
// click sound
public var clickSound: AudioClip;
// pick sound
public var pickSound: AudioClip;

// *** Mouse Input ***
// track mouse position - leave off if using touchpad input
var trackMouseXYPosition : boolean = false;
// track mouse position only when held down
var onlyWhenMouseDown : boolean = false;

// *** Touchscreen Joystick ***
// select joystick
public var touchpad : s3dTouchpad;
// toggle joystick
var useTouchpad : boolean = false;
// touchpad tracking speed
var touchpadSpeed : Vector2 = Vector2(1.0,1.0);
// make joystick area like a trackpad
var uniformTouchpadMovement : boolean = true;

private var touchpadPrevPosition : Vector2 = Vector2(0,0);
private var interactiveLayerMask : LayerMask;
public var activeObj : GameObject;
private var readyForTap : boolean = true;
private var unitWidth : float;
var followActiveObject : boolean = true; // can be set to false so flashlight, gun etc don't follow certain objects - working?
private var prevRolloverObject : GameObject; // most recent rollover object
private var initialized : boolean;

@script RequireComponent(AudioSource);
// @script RequireComponent(s3dGuiTexture); 
// generates this error: "Can't remove s3dGuiTexture (Script) because s3dGuiCursor (Script) depends on it"
// which is odd because it's exactly the opposite of what I'm trying to do!

function Start () {
	findS3dCamera();
	if (hidePointer) {
		Screen.showCursor = false;
	} else {
		Screen.showCursor = true;
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

function initialize() { // called from s3dGuiTexture on startup, after it's been initialized;
	s3dTexture = gameObject.GetComponent(s3dGuiTexture);
	if (trackMouseXYPosition || useTouchpad) {
		var xInset : float = s3dTexture.guiTexture.pixelInset.width/-2;
		var yInset : float = s3dTexture.guiTexture.pixelInset.height/-2;
		s3dTexture.guiTexture.pixelInset.x = xInset;
		s3dTexture.guiTexture.pixelInset.y = yInset;
		s3dTexture.objectCopyR.guiTexture.pixelInset.x = xInset;
		s3dTexture.objectCopyR.guiTexture.pixelInset.y = yInset;
	}
	
	if (defaultTexture) setTexture(defaultTexture);
	interactiveLayerMask = 1 << interactiveLayer;
	initialized = true;
}

function Update () {
	if (initialized) {
		if (s3dTexture.on) {
			#if UNITY_EDITOR
			if (trackMouseXYPosition) {
				if (!onlyWhenMouseDown || (onlyWhenMouseDown && Input.GetMouseButton(0))) {
					s3dTexture.obPosition = matchMousePos();
				}
			}
			#endif
			if (useTouchpad) {
				doTouchpad();
			}
			castForObjects();
		}
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

function castForObjects() {
	var dPosition : Vector2;
	var actScript : s3dInteractor;
	if (camera3D.format3D == 0 && !camera3D.sideBySideSqueezed) {
		dPosition = Vector2((s3dTexture.obPosition.x/2)+0.25,s3dTexture.obPosition.y); // 0 = left, 0.5 = center, 1 = right
	} else {
		dPosition = s3dTexture.obPosition;
	}
	var hit: RaycastHit;
	var ray : Ray = camera3D.camera.ViewportPointToRay (dPosition);
	if (Physics.Raycast (ray, hit, 100.0)) {
		Debug.DrawRay (ray.origin, ray.direction*hit.distance, Color(0,1,0,0));
		// if there's currently an activeObj, notify it of hit position
		if (activeObj && followActiveObject) {
			actScript = activeObj.GetComponent(s3dInteractor);
			if (actScript) {
				actScript.updatePosition(hit.point);
			}
			// if activeObj, tell any attached aimObject.js scripts to point at it
			gameObject.SendMessage ("PointAt",activeObj.transform.position, SendMessageOptions.DontRequireReceiver);
		} else {
			// if no activeObj, tell any attached aimObject.js scripts to point at hitpoint
			gameObject.SendMessage ("PointAt",hit.point, SendMessageOptions.DontRequireReceiver);
		}	
	}
	// next, raycast against objects in interactive layer (for taps)
	ray = camera3D.camera.ViewportPointToRay (dPosition);
	
	#if UNITY_EDITOR // if in editor & mouse button down
	if (trackMouseXYPosition) {
		if (Physics.Raycast (ray, hit, clickDistance,interactiveLayerMask)) {
			if (Input.GetMouseButtonDown(0)) {
				processTap(hit,true,1); // tapped on object
			} else {
				processRollover(hit,true); // rolled over object
			}
		} else {
			if (Input.GetMouseButtonDown(0)) {
				processTap(hit, false,1); // if there's no hit, process clicks anyway to deactivate active objects
			} else {
				if (prevRolloverObject != null) {
					processRollover(hit, false); // lost rolled over object
				}
			}
		}
	}
	#endif
	
	if (useTouchpad && readyForTap) {
		if (Physics.Raycast (ray, hit, clickDistance,interactiveLayerMask)) {
			if (touchpad.tap > 0) {
				processTap(hit,true, touchpad.tap); // tapped on object
			} else {
				processRollover(hit,true); // rolled over object
			}
		} else {
			if (touchpad.tap > 0) {
				processTap(hit,false, touchpad.tap); // if there's no hit, process clicks anyway to deactivate active objects
			} else {
				if (prevRolloverObject != null) {
					processRollover(hit,false); // lost rolled over object
				}
			}
		}
		touchpad.reset();
		readyForTap = false;
		pauseAfterTap();
	}
}

// update parallax
function doTouchpad() {
	if (uniformTouchpadMovement) {
		if (touchpad.position != Vector2.zero) {
			s3dTexture.obPosition.x = Mathf.Clamp(s3dTexture.obPosition.x + ((touchpad.position.x-touchpadPrevPosition.x)*touchpadSpeed.x),0.05,0.95);
			s3dTexture.obPosition.y = Mathf.Clamp(s3dTexture.obPosition.y + ((touchpad.position.y-touchpadPrevPosition.y)*touchpadSpeed.y),0.05,0.95);
			touchpadPrevPosition = touchpad.position;
		} else {
			touchpadPrevPosition = Vector2.zero;
		}
	} else {
		s3dTexture.obPosition.x = Mathf.Clamp(s3dTexture.obPosition.x + (touchpad.position.x*touchpadSpeed.x),0.05,0.95);
		s3dTexture.obPosition.y = Mathf.Clamp(s3dTexture.obPosition.y + (touchpad.position.y*touchpadSpeed.y),0.05,0.95);
	}
}

function processTap(theHit : RaycastHit, gotHit: boolean, tapType : int) {
	setTexture(clickTexture);
	if (clickSound) audio.PlayOneShot(clickSound);
	if (activeObj && (!gotHit || (activeObj != theHit.transform.gameObject))) { // if there's currently an active object and there was a tap but no hit - then deactivate this object
		var actScript : s3dInteractor = activeObj.GetComponent(s3dInteractor); // or if there's currently an active object and there was a tap that hit another object - then deactivate this object
		if (actScript) {
			actScript.deactivateObject();
		}
	} else {
		if (gotHit) { // if there's not a currently active object and there was a tap with a hit on an active object - then activate it
			actScript = theHit.transform.gameObject.GetComponent(s3dInteractor);
			if (actScript) {
				actScript.tapAction(theHit,tapType);
			}
		}
	}
	unclickTexture();
}

function processRollover (theHit : RaycastHit, onObject : boolean) {
	if (onObject) {
		var actScript = theHit.transform.gameObject.GetComponent(s3dInteractor);
		actScript.rolloverText(theHit,true,s3dTexture.obPosition);
		prevRolloverObject = theHit.transform.gameObject;
	} else {
		actScript = prevRolloverObject.transform.gameObject.GetComponent(s3dInteractor);
		actScript.rolloverText(theHit,false,s3dTexture.obPosition);
		prevRolloverObject = null;
	}
}

function setTexture(tex : Texture) {
	guiTexture.texture = tex;
	if (s3dTexture.objectCopyR) s3dTexture.objectCopyR.guiTexture.texture = tex;
}

function unclickTexture() {
	yield WaitForSeconds (0.2);
	if (!activeObj) {
		setTexture(defaultTexture);
	} else {
		setTexture(pickTexture);
		if (pickSound) audio.PlayOneShot(pickSound);
	}
}	

function pauseAfterTap() {
	yield WaitForSeconds (0.25);
	readyForTap = true;
}

