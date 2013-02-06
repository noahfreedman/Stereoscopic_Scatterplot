/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* This is an s3d Interaction script.
 * Requires a s3dInteractor component
 * tap spins object around random axis
 * when not spinning, object can be set to turn constantly around a random or specified axis
 */

#pragma strict
public var tapType : int = 3; // 1 = short tap 2 = long tap 3 = any tap

// spin on tap
public var spinTime : float = 1.0;

// rotate constantly
public var rotateOnRest : boolean = false;
public var rotateAxis : Vector3;
public var randomAxis : boolean = true;
public var rotateSpeed : float = 1;
public var randomSpeed : boolean = true;
public var minSpeed : float = 1.0;
public var maxSpeed : float = 5.0;

private var spinAmount : Vector3;
private var spinning : boolean = false;
private var spinTapTime : float;
private var goalRotation : Quaternion;
@script RequireComponent(s3dInteractor);

function Start() {
	if (randomAxis) {
		rotateAxis = Vector3(Random.Range(-1,1),Random.Range(-1,1),Random.Range(-1,1));
	}
	if (randomSpeed) {
		rotateSpeed = Random.Range(minSpeed,maxSpeed);
	}
}

function Update () {
	if (!spinning) {
    	transform.Rotate(Vector3(Time.deltaTime*rotateSpeed*rotateAxis.x,Time.deltaTime*rotateSpeed*rotateAxis.y,Time.deltaTime*rotateSpeed*rotateAxis.z));
	}
}

function NewTap(params: TapParams) {
	if (params.tap == tapType || tapType == 3) {
		if (!spinning) {
			spinTapTime = Time.time;
			spinAmount =  Vector3(Random.Range(-30, 30), Random.Range(-30, 30),Random.Range(-30, 30));
			goalRotation = transform.rotation*Quaternion.Euler(spinAmount);
			spinning = true;
			rotate(transform.rotation,goalRotation);
		}
	}
}

function rotate(from : Quaternion, to: Quaternion) {
	var rate = 1.0/spinTime;
	var t = 1.0;
	while (t > 0.0) {
		t -= Time.deltaTime * rate;
		transform.Rotate(spinAmount*t);
		spinning = true;
		yield;
	}
	spinning = false;
}



