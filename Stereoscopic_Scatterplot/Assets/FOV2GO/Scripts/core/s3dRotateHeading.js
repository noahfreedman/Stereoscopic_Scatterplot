/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* s3d Rotate Heading
 * Usage: Provides manual heading (rotation) control for mobile devices
 * Assign s3dTouchpad for input on mobile devices 
 * s3dTouchpad mimics touchpad input with mouse on desktop.
 * controlPitchInEditor = true: y movement controls look up/down in editor
 */

public var touchpad : s3dTouchpad;
// touchpad speed
public var touchSpeed : Vector2 = Vector2(1,1);
public var controlPitchInEditor : boolean = true;
private var gyroScript : s3dGyroCam;

function Awake () {
}

function Start() {
	gyroScript = gameObject.GetComponentInChildren(s3dGyroCam);
}

function Update () {
	gyroScript.heading += touchpad.position.x*touchSpeed.x;
	gyroScript.heading = gyroScript.heading%360;
	if (controlPitchInEditor) {
		gyroScript.Pitch -= touchpad.position.y*touchSpeed.y;
		gyroScript.Pitch = Mathf.Clamp(gyroScript.Pitch%360, -60, 60);
	}
}