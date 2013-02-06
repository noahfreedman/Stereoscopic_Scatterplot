/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* This is an s3d Interaction script.
 * Requires a s3dInteractor component
 * tap pushes object away from camera
 * requires rigidbody
 */

#pragma strict
var pushForce : float = 5;
var spinForce : float  = 20;
var tapType : int = 3; // 1 = short tap 2 = long tap 3 = any tap
@script RequireComponent( Rigidbody )
@script RequireComponent(s3dInteractor);

function NewTap(params: TapParams) {
	if (params.tap == tapType || tapType == 3) {
		var vec = transform.position - params.hit.point; // the direction from clickPoint to the rigidbody
		vec = vec.normalized;
		rigidbody.AddForce(vec*pushForce*100);
		var vec2 : Vector3 = new Vector3(Random.Range(-spinForce,spinForce), Random.Range(-spinForce,spinForce), Random.Range(-spinForce,spinForce));
		rigidbody.AddRelativeTorque (vec2*100);
	}
}
