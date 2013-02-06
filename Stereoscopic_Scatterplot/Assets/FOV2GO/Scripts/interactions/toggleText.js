/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* This is an s3d Interaction script.
 * Requires a s3dInteractor component
 * simple script to toggle s3dGuiText on/off
 */

#pragma strict
var textObject : GUIText;
private var textScript: s3dGuiText;
var textOn : boolean = false;
var tapType : int = 2; // 1 = short tap 2 = long tap 3 = any tap
@script RequireComponent(s3dInteractor);

function Start() {
	textScript = textObject.GetComponent(s3dGuiText);
}

function NewTap(params: TapParams) {
	if (params.tap == tapType || tapType == 3) {
		textOn = !textOn;
    	textScript.toggleVisible(textOn);
	}
} 
