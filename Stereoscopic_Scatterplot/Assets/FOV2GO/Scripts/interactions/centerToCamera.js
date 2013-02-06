/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* This is an s3d Interaction script.
 * Requires a s3dInteractor component
 * first tap centers object to camera
 * next tap releases and returns object to original position
 * works with or without rigidbody
 */

#pragma strict
// distance from camera when floating
public var floatDistance : float = 2;
// speed
public var objectSpeed : float = 1;
public var returnToOriginalRotation : boolean = true;
public var floatPosOffset : Vector3;
public var floatRotOffset : Vector3;
public var returnToOrigin : boolean = true;
var tapType : int = 3; // 1 = short tap 2 = long tap 3 = any tap
private var mainCamObj : GameObject;
private var floating : boolean = false;
private var foundStartPos : boolean = false;
private var startPos : Vector3;
private var startRot : Quaternion;
private var ready : boolean = true;
private var cursorObj : GameObject;
private var cursorScript : s3dGuiCursor;
private var originalParent : Transform;
private var tempParent : GameObject;
var followWhenActive : boolean = true;

@script RequireComponent(s3dInteractor);

function Start () {
	mainCamObj = GameObject.FindWithTag("MainCamera");	// Main Camera
	cursorObj = GameObject.FindWithTag("cursor");
	cursorScript = cursorObj.GetComponent(s3dGuiCursor);
	if (!rigidbody) {
		startPos = transform.position;
		startRot = transform.rotation;
		foundStartPos = true;
	}
}

function OnCollisionEnter(collision : Collision) {
	if (!foundStartPos) {
		registerStartPosition();
	}
}

function registerStartPosition() {
	yield WaitForSeconds(0.5); // give object a chance to settle
	startPos = transform.position;
	startRot = transform.rotation;
	foundStartPos = true;
}

function NewTap(params: TapParams) {
	if (params.tap == tapType || tapType == 3) {
		if (ready) {
			floating = !floating;
			ready = false;
			takeBreather();
		}	
		if (floating) {
			if (rigidbody) {
				rigidbody.isKinematic = true;
			}
			originalParent = transform.parent;
			tempParent = new GameObject ("tempParent");
			tempParent.transform.position = transform.localPosition;
			tempParent.transform.rotation = transform.localRotation;
			transform.parent = tempParent.transform;
			tempParent.transform.parent = originalParent;
			cursorScript.followActiveObject = followWhenActive; // tell cursorScript whether it should follow this object
			cursorScript.activeObj = gameObject; // tell cursorScript that we have an active object
		} else {
			cursorScript.activeObj = null;
			transform.parent = originalParent;
			Destroy(tempParent);
		}
	}
}

function takeBreather() {
	yield WaitForSeconds (0.25);
	ready = true;
}	

function Deactivate() {
	floating = false;
	cursorScript.activeObj = null;
	transform.parent = originalParent;
	Destroy(tempParent);
}

function Update () {
	if (floating) {
		var ray = new Ray (mainCamObj.transform.position, mainCamObj.transform.forward);
		var floatPos : Vector3 = ray.GetPoint(floatDistance);
  		var obRot : Vector3 = (mainCamObj.transform.position - floatPos);
		var floatRot : Quaternion = Quaternion.LookRotation(obRot,mainCamObj.transform.up);
		if (tempParent.transform.position != floatPos) {
			tempParent.transform.position = Vector3.Lerp(tempParent.transform.position, floatPos,  Time.deltaTime*objectSpeed);
			tempParent.transform.rotation = Quaternion.Lerp(tempParent.transform.rotation, floatRot, Time.deltaTime*objectSpeed);
			transform.localPosition = Vector3.Lerp(transform.localPosition, floatPosOffset,  Time.deltaTime*objectSpeed);
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(floatRotOffset), Time.deltaTime*objectSpeed);
		}
	} else {
		if (!rigidbody || (rigidbody && returnToOrigin && foundStartPos)) {
			if (transform.position != startPos) {
				transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime*objectSpeed);
				if (returnToOriginalRotation) {
					transform.rotation = Quaternion.Lerp(transform.rotation, startRot, Time.deltaTime*objectSpeed);
				}
			} else {
				if (rigidbody) {
					rigidbody.isKinematic = false;
				}
			}
		} else {
			if (rigidbody.isKinematic) {
				rigidbody.WakeUp();
				rigidbody.isKinematic = false;
			}
		}
	}
}

