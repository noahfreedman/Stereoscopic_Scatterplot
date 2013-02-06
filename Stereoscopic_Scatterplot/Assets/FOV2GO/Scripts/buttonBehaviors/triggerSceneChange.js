/* This file is part of Stereoskopix FOV2GO for Unity. * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu */
/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* Usage: attach this script to s3dTouchpad
 * tap on touchpad to load next level
 * s3dTouchpad mimics touchpad input with mouse on desktop.
 */

#pragma strict
private var touchpad : s3dTouchpad;
// 0 = ignore (don't trigger)
// 1 = trigger with short tap only 
// 2 = trigger with long tap only
// 3 = trigger with any tap
var tapType : int = 3;
// seconds to pause before loading next level
// GUI Text for loading message
public var loadingText : s3dGuiText;
public var loadingMessage : String = "Loading...";
public var pauseBeforeLoad : float = 1;
private var ready : boolean = false;

function Start () {
	touchpad = gameObject.GetComponent(s3dTouchpad);
	waitABit();
}

function waitABit() {
	yield WaitForSeconds(0.5);
	ready = true;
}	

function Update() {
	if (ready) {
		if (touchpad.tap > 0) {
			if (touchpad.tap == tapType || tapType == 3) {
				loadNewScene();
			}
		}
	}
}

function loadNewScene() {
	touchpad.reset();
	if (loadingText) {
		loadingText.setText(loadingMessage);
		loadingText.toggleVisible(true);
	}
	yield WaitForSeconds (pauseBeforeLoad);
	if (loadingText) loadingText.toggleVisible(false);
	
	var nextLevel : int = Application.loadedLevel+1;
	if (nextLevel > Application.levelCount-1) {
		nextLevel = 0;
	}
	Application.LoadLevel(nextLevel);
}
