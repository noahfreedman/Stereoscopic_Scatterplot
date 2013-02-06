/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* Usage: attach this script to s3dTouchpad
 * toggle chosen object on/off (default short tap) and/or
 * trigger action on chosen object ("Go" function on object) (default long tap)
 * s3dTouchpad.js mimics touchpad input using mouse on desktop. 
 */

#pragma strict
var theObject : GameObject;
// 0 = ignore (don't trigger)
// 1 = trigger with short tap only 
// 2 = trigger with long tap only
// 3 = trigger with any tap
var toggleTapType : int = 1;
var actionTapType : int = 2;
var startOn : boolean = false;
private var touchpad : s3dTouchpad;
private var onOff : boolean = true;

function Start () {
	touchpad = gameObject.GetComponent(s3dTouchpad);
	toggleOnOff(startOn);
}

function Update() {
	if (touchpad.tap > 0) {
		if (touchpad.tap == toggleTapType || toggleTapType == 3) {
			onOff = !onOff;
			toggleOnOff(onOff);
		}
		if (touchpad.tap == actionTapType || actionTapType == 3) {
			triggerAction();
		}
	}
}

function toggleOnOff(onoff: boolean) {
	onOff = onoff;
	theObject.SetActiveRecursively(onOff);
	touchpad.reset();
}

function triggerAction() {
	theObject.SendMessage("Go", SendMessageOptions.DontRequireReceiver);
	touchpad.reset();
}
