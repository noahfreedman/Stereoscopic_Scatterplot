/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* This is an s3d Interaction script.
 * Requires a s3dInteractor component
 * first tap picks up object, object can be dragged around
 * next tap drops object
 * requires rigidbody
 */

#pragma strict
// DragRigidBody
var spring : float = 100.0; //50
var damper = 100.0; //5
var drag = 10.0;
var angularDrag = 5.0;
var distance = 0.1; //0.2  
var tapType : int = 1; // 1 = short tap 2 = long tap 3 = any tap
var attachToCenterOfMass = false;
private var springJoint : SpringJoint;

var customCenterOfMass : Vector3 = Vector3.zero;

// if we move forward, object stays where it is 
var moveTowardsObject : boolean = true;
var minDist : float = 1;
var maxDist : float = 10;

public var grabOffset : Vector3 = Vector3(0,0.5,0);

private var mainCamObj : GameObject;
private var cursorObj : GameObject;
private var cursorScript : s3dGuiCursor;
private var activated : boolean = false;
private var startPos : Vector3;
private var startRot : Quaternion;
private var readyForStateChange : boolean = true;
private var newPosition : Vector3;
// position of the object when clicked on
private var clickPosition : Vector3;
private var hitDistance : float;
@script RequireComponent(s3dInteractor);
@script RequireComponent(Rigidbody);

function Start () {
	mainCamObj = GameObject.FindWithTag("MainCamera");	// Main Camera
	cursorObj = GameObject.FindWithTag("cursor");
	cursorScript = cursorObj.GetComponent(s3dGuiCursor);	// Main Stereo Camera Script
	startPos = transform.position; 
	startRot = transform.rotation;
	rigidbody.centerOfMass = customCenterOfMass;
}

function NewTap(params: TapParams) {
	if (readyForStateChange) {
		if (!activated) {
			if (params.tap == tapType || tapType == 3) {
				activated = true;
			}
		} else {
			activated = false;
		}
		//activated = !activated;
		hitDistance = params.hit.distance;
		readyForStateChange = false;
		pauseAfterStateChange();
	}
	if (activated) {
		clickPosition = params.hit.point;
		cursorScript.activeObj = gameObject; // tell cursorScript that we have an active object
		if (!springJoint) {
			var go = new GameObject("Rigidbody dragger");
			var body : Rigidbody = go.AddComponent (Rigidbody) as Rigidbody;
			springJoint = go.AddComponent (SpringJoint);
			body.isKinematic = true;
		}
		springJoint.transform.position = params.hit.point;
		if (attachToCenterOfMass) {
			var anchor = transform.TransformDirection(rigidbody.centerOfMass) + rigidbody.transform.position;
			anchor = springJoint.transform.InverseTransformPoint(anchor);
			springJoint.anchor = anchor;
		} else {
			springJoint.anchor = Vector3.zero;
		}
		springJoint.spring = spring;
		springJoint.damper = damper;
		springJoint.maxDistance = distance;
		springJoint.connectedBody = rigidbody;
		increaseSpringAfterPickup();
		StartCoroutine ("DragObject");
	} else {
		cursorScript.activeObj = null;
	}
}

function DragObject () {
	var oldDrag = springJoint.connectedBody.drag;
	var oldAngularDrag = springJoint.connectedBody.angularDrag;
	springJoint.connectedBody.drag = drag;
	springJoint.connectedBody.angularDrag = angularDrag;
	while (activated) { // end when receive another double-click touch
		springJoint.transform.position = newPosition+grabOffset;
		yield;
	}
	if (springJoint.connectedBody) {
		springJoint.connectedBody.drag = oldDrag;
		springJoint.connectedBody.angularDrag = oldAngularDrag;
		springJoint.connectedBody = null;
	}
}

function Deactivate() {
	activated = false;
	cursorScript.activeObj = null;
	readyForStateChange = false;
	springJoint.spring /= 10;
	pauseAfterStateChange();
}

function pauseAfterStateChange() {
	yield WaitForSeconds (0.25);
	readyForStateChange = true;
}	

function increaseSpringAfterPickup() {
	yield WaitForSeconds(1);
	springJoint.spring *= 10;
}

function NewPosition (pos: Vector3) {
	var currentDistance : float;
	if (activated) {
		var viewPos : Vector3 = mainCamObj.camera.WorldToViewportPoint (pos);
		var ray : Ray = mainCamObj.camera.ViewportPointToRay (viewPos);
		if (moveTowardsObject) {
		 	currentDistance = Vector3.Distance(mainCamObj.transform.position, clickPosition);
		 	currentDistance = Mathf.Clamp(currentDistance,minDist,maxDist);
		} else {
			currentDistance = hitDistance;
		}
		newPosition = ray.GetPoint(currentDistance);
	}
}
