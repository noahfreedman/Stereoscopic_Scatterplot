/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* S3D Interactor Script
 * To make any object interactive:
  - put it in the Interactive layer
  - add this script
 * This script receives interaction messages from object with s3d_Gui_Pointer - with s3dGuiTexture.js script
  - add interaction scripts to object that implement functions sent from this script
 */

#pragma strict

// called from s3dGuiTexture.js
function tapAction(theHit: RaycastHit, theTap: int) { // 1 = short tap 2 = long tap
	gameObject.SendMessage ("NewTap", new TapParams(theHit,theTap), SendMessageOptions.DontRequireReceiver);
}

// called from: s3dGuiTexture.js, aimObject.js
function updatePosition(thePos: Vector3) {
	gameObject.SendMessage("NewPosition",thePos, SendMessageOptions.DontRequireReceiver);
}

// called from s3dGuiTexture.js
function deactivateObject() {
	gameObject.SendMessage("Deactivate", SendMessageOptions.DontRequireReceiver);
}

// called from s3dGuiTexture.js
function rolloverText(theHit: RaycastHit, onObject, obPosition : Vector2) {
	if (onObject) {
		gameObject.SendMessage ("ShowText",obPosition,SendMessageOptions.DontRequireReceiver);
	} else {
		gameObject.SendMessage ("HideText",obPosition,SendMessageOptions.DontRequireReceiver);
	}
}

public class TapParams {
     public var hit : RaycastHit;
     public var tap : int;
     //constructor
     function TapParams( hit : RaycastHit, tap : int) {  
     	this.hit = hit;
     	this.tap = tap;
     }
}
