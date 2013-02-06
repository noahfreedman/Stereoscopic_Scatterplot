/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* This is an s3d Interaction script.
 * Requires a s3dInteractor component
// attach this script to an object (example gun or flashlight) to make it point at the current s3d_GUI_Pointer position
*/

#pragma strict
var smooth : boolean = true;
var damp : float = 6.0;
@script RequireComponent(s3dInteractor);

function Start () {
}

function Update () {
}

// aim at active object position OR at hit point
function NewPosition (pos: Vector3) {
	if (renderer.enabled) {
		if (smooth) {
			// Look at and dampen the rotation
			var rot : Quaternion = Quaternion.LookRotation(pos - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damp);
		} else {
			// Just lookat
		    transform.LookAt(pos);
		}
	}
}
