/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* This is an s3d Interaction script.
 * Requires a s3dInteractor component
 * add to an interactive object, assign a s3dGuiText
 * set rollover message to appear when s3d Gui Pointer rolls over object
 */

#pragma strict
var theText : s3dGuiText;
var offset : Vector2 = Vector2(0,0.15);
var message : String = "rollover message";
@script RequireComponent(s3dInteractor);

function ShowText(obPosition: Vector2) {
	var textPos : Vector2 = obPosition + offset;
	theText.setObPosition(textPos);
	theText.setText(message);
	theText.toggleVisible(true);
}

function HideText(obPosition : Vector2) {
	theText.toggleVisible(false);
}