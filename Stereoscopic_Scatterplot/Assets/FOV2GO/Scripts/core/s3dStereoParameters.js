/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* s3d Stereo Parameters.
 * Usage: Provides an interface to adjust stereo parameters (interaxial, zero parallax distance, and horizontal image transform (HIT)
 * Option to use in conjunction with s3dDeviceManager.js or on its own 
 * Has companion Editor script to create custom inspector 
 */

#pragma strict

private var camera3D : s3dCamera;
private var s3dDeviceMan : s3dDeviceManager;
public var stereoParamsTouchpad : s3dTouchpad;
public var saveStereoParamsToDisk : boolean;
public var showStereoParamsTexture : Texture2D;
public var dismissStereoParamsTexture : Texture2D;

private var showParamGui : boolean = false;
public var stereoReadoutText : s3dGuiText;

public var interaxialTouchpad : s3dTouchpad;
private var interaxialStart : float;
private var interaxialInc : float;

public var zeroPrlxTouchpad : s3dTouchpad;
private var zeroPrlxStart : float;
private var zeroPrlxInc : float;

public var hitTouchpad : s3dTouchpad;
private var hitStart : float;
private var hitInc : float;

@script ExecuteInEditMode()

function Awake () {
	s3dDeviceMan = gameObject.GetComponent(s3dDeviceManager);
	// if object has s3dDeviceManager.js, use that script's camera & touchpads
	if (s3dDeviceMan) {
		stereoParamsTouchpad = s3dDeviceMan.stereoParamsTouchpad;
		interaxialTouchpad = s3dDeviceMan.interaxialTouchpad;
		zeroPrlxTouchpad = s3dDeviceMan.zeroPrlxTouchpad;
		hitTouchpad = s3dDeviceMan.hitTouchpad;
	}
}

function Start() {
	findS3dCamera();
	if (saveStereoParamsToDisk) {
		if (PlayerPrefs.GetFloat(Application.loadedLevelName+"_interaxial")) {
			camera3D.interaxial = PlayerPrefs.GetFloat(Application.loadedLevelName+"_interaxial");
		}
		if (PlayerPrefs.GetFloat(Application.loadedLevelName+"_zeroPrlxDistance")) {
			camera3D.zeroPrlxDist = PlayerPrefs.GetFloat(Application.loadedLevelName+"_zeroPrlxDist");
		}
		if (PlayerPrefs.GetFloat(Application.loadedLevelName+"_H_I_T")) {
			camera3D.H_I_T = PlayerPrefs.GetFloat(Application.loadedLevelName+"_H_I_T");
		}
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

function Update () {
	if (stereoParamsTouchpad && stereoParamsTouchpad.tap > 0) {
		showParamGui = !showParamGui;
		toggleStereoParamGui(showParamGui);
		stereoParamsTouchpad.reset();
		if (showParamGui) {
			interaxialStart = camera3D.interaxial;
			zeroPrlxStart = camera3D.zeroPrlxDist;
			hitStart = camera3D.H_I_T;
		} else { // showParamGui has just been dismissed, write new values to disk
			if (saveStereoParamsToDisk) {
				PlayerPrefs.SetFloat(Application.loadedLevelName+"_interaxial",camera3D.interaxial);
				PlayerPrefs.SetFloat(Application.loadedLevelName+"_zeroPrlxDist",camera3D.zeroPrlxDist);
				PlayerPrefs.SetFloat(Application.loadedLevelName+"_H_I_T",camera3D.H_I_T);
			}
			interaxialStart = camera3D.interaxial;
			zeroPrlxStart = camera3D.zeroPrlxDist;
			hitStart = camera3D.H_I_T;
		}
	}
	if (showParamGui) {
		// touchpad should be set to moveLikeJoystick = false, actLikeJoystick = true
		// so that a tapdown generates a position change
		// grab position while touchpad is being dragged - because when we actually get the tap (at TouchPhase.Ended) 
		// or the click (on Input.GetMouseButtonUp), position has already been reset to 0
		if (interaxialTouchpad.position.x) {
			// position values are between -1.0 and 1.0 - values < 1.0 are converted to -1.0, values > 1.0 are converted to 1.0
			interaxialInc = Mathf.Round(interaxialTouchpad.position.x+(0.49*Mathf.Sign(interaxialTouchpad.position.x)));
		}
		if (interaxialTouchpad.tap > 0) {
			camera3D.interaxial += interaxialInc;
			camera3D.interaxial = Mathf.Max(camera3D.interaxial,0);
			interaxialTouchpad.reset();
		}
		if (zeroPrlxTouchpad.position.x) {
			zeroPrlxInc = Mathf.Round(zeroPrlxTouchpad.position.x+(0.49*Mathf.Sign(zeroPrlxTouchpad.position.x)))*0.25;
		}
		if (zeroPrlxTouchpad.tap > 0) {
			camera3D.zeroPrlxDist += zeroPrlxInc;
			camera3D.zeroPrlxDist = Mathf.Max(camera3D.zeroPrlxDist,1.0);
			zeroPrlxTouchpad.reset();
		}
		if (hitTouchpad.position.x) {
			hitInc = Mathf.Round(hitTouchpad.position.x+(0.49*Mathf.Sign(hitTouchpad.position.x)))*0.1;
		}
		if (hitTouchpad.tap > 0) {
			camera3D.H_I_T += hitInc;
			hitTouchpad.reset();
		}
		stereoReadoutText.setText("Interaxial: "+Mathf.Round(camera3D.interaxial*10)/10+"mm \nZero Prlx: "+Mathf.Round(camera3D.zeroPrlxDist*10)/10+"M \nH.I.T.: "+Mathf.Round(camera3D.H_I_T*10)/10);
	}
}

function toggleStereoParamGui(on : boolean) {
	if (on) {
		interaxialTouchpad.gameObject.active = true;
		zeroPrlxTouchpad.gameObject.active = true;
		hitTouchpad.gameObject.active = true;
		stereoParamsTouchpad.gameObject.GetComponent(GUITexture).texture = dismissStereoParamsTexture;
		stereoReadoutText.toggleVisible(true);
	} else {
		interaxialTouchpad.gameObject.active = false;
		zeroPrlxTouchpad.gameObject.active = false;
		hitTouchpad.gameObject.active = false;
		stereoParamsTouchpad.gameObject.GetComponent(GUITexture).texture = showStereoParamsTexture;
		stereoReadoutText.toggleVisible(false);
	}
}
