/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* This is an s3d Interaction script.
 * Requires a s3dInteractor component
 * first tap turns object along chosen axis
 * if toggleTurn = true, next tap returns object to original rotation
 * if toggleTurn = false, each tap rotates object further
 * plays audioClip if one is assigned
 */

#pragma strict
var turnAmount : Vector3;
var turnTime : float = 1;
var toggleTurn : boolean = true;
var turned : boolean = false;
var tapType : int = 3; // 1 = short tap 2 = long tap 3 = any tap
private var startRotation : Quaternion;
private var endRotation : Quaternion;
private var goalRotation : Quaternion;
private var turning : boolean = false;
// audio clip to play when object pivots
public var pivotSound: AudioClip;

@script RequireComponent(s3dInteractor);
@script RequireComponent(AudioSource);

function Start () {
	startRotation = transform.rotation;
	endRotation = startRotation*Quaternion.Euler(turnAmount);
}

function NewTap(params: TapParams) {
	if (params.tap == tapType || tapType == 3) {
		if (!turning) {
			if (!turned || !toggleTurn) {
				goalRotation = endRotation;
				rotate(transform.rotation,endRotation);
				if (pivotSound) audio.PlayOneShot(pivotSound);
				turned = true;
			} else {
				goalRotation = startRotation;
				rotate(transform.rotation,startRotation);
				if (pivotSound) audio.PlayOneShot(pivotSound);
				turned = false;
			}
		}
	}
}

function rotate(from : Quaternion, to: Quaternion) {
	var rate = 1.0/turnTime;
	var t = 0.0;
	while (t < 1.0) {
		t += Time.deltaTime * rate;
		transform.rotation = Quaternion.Slerp(from, to, t);
		turning = true;
		yield;
	}
	turning = false;
	if (!toggleTurn) {
		startRotation = endRotation;
		endRotation = startRotation*Quaternion.Euler(turnAmount);
	}
}
