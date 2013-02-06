/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* s3d First Person Controller for phones + tablets
 * Usage: Replace standard Unity FPS controller script with this script.
 * requires s3dGyroCam.js script attached to camera (child)
 * Assign s3dTouchpad for navigation (translation) input
 * default: x movement sidesteps (rotateHeading = false);
 * option: x movement rotates heading (rotateHeading = true);
 * s3dTouchpad mimics touchpad input with mouse on desktop. 
 */

#pragma strict
private var motor : CharacterMotor;
public var touchpad : s3dTouchpad;

// touchpad speed
public var touchSpeed : Vector2 = Vector2(1.0,1.0);
public var horizontalControlsHeading : boolean = false;
private var directionVector : Vector3;
private var gyroCam : s3dGyroCam;

@script RequireComponent (CharacterMotor)

function Awake () {
	motor = GetComponent(CharacterMotor);
}

function Start() {
	gyroCam = gameObject.GetComponentInChildren(s3dGyroCam);
}

function Update () {
	if (horizontalControlsHeading) {
		directionVector = new Vector3(0, 0, touchpad.position.y*touchSpeed.y);
		gyroCam.heading += touchpad.position.x*touchSpeed.x;
		gyroCam.heading = gyroCam.heading%360;
	} else {
		directionVector = new Vector3(touchpad.position.x*touchSpeed.x, 0, touchpad.position.y*touchSpeed.y);
	}
	motor.inputMoveDirection = gyroCam.transform.rotation * directionVector;
	if (touchpad.tap > 0) {
		motor.inputJump = true;
		touchpad.reset();
	} else {
		motor.inputJump = false;
	}
}
