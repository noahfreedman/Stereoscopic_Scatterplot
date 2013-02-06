/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* This is an s3d Interaction script.
 * Requires a s3dInteractor component
 * tap makes object jump in the air
 * requires rigidbody
 */

#pragma strict
var upForce : float = 100;
var randomForce : float = 20;
var torqueForce : float  = 20;
var tapType : int = 3; // 1 = short tap 2 = long tap 3 = any tap
@script RequireComponent( Rigidbody )
@script RequireComponent(s3dInteractor);

function Start () {
}

function Update () {
}

function NewTap(params: TapParams) {
	if (params.tap == tapType || tapType == 3) {
		var pushForce : Vector3 = new Vector3(Random.Range(-randomForce,randomForce), upForce, Random.Range(-randomForce,randomForce));
		rigidbody.AddForce (pushForce);
		var spinForce : Vector3 = new Vector3(Random.Range(-torqueForce,torqueForce), Random.Range(-torqueForce,torqueForce), Random.Range(-torqueForce,torqueForce));
		rigidbody.AddRelativeTorque (spinForce);
	}
}