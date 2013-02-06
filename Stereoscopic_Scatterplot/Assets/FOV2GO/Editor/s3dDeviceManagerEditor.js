/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* This script goes in the Editor folder. It provides a custom inspector for s3dDeviceManager.
 */

@CustomEditor (s3dDeviceManager)
class s3dDeviceManagerEditor extends Editor {
    function OnInspectorGUI () {
	    EditorGUIUtility.LookLikeControls(170,30);
		var allowSceneObjects : boolean = !EditorUtility.IsPersistent (target);
	    EditorGUILayout.BeginVertical("box");
		target.phoneLayout = EditorGUILayout.EnumPopup(GUIContent("Phone Layout","Select Phone Layout"), target.phoneLayout);
	    EditorGUILayout.BeginHorizontal();
	  	GUILayout.FlexibleSpace();
	    EditorGUILayout.EndHorizontal();
		target.use3dCursor = EditorGUILayout.Toggle(GUIContent("Use 3D Cursor","Use 3D Cursor"), target.use3dCursor);
		if (target.use3dCursor) {
			EditorGUI.indentLevel = 1;
			target.cursor3D = EditorGUILayout.ObjectField(GUIContent("3D Cursor","Select s3dGuiTexture for Cursor"),target.cursor3D,s3dGuiCursor,allowSceneObjects);
			EditorGUI.indentLevel = 0;
		}
		target.movePadPosition = EditorGUILayout.EnumPopup(GUIContent("Move Pad Position","Select Move Pad Position"), target.movePadPosition);
		if (target.movePadPosition != 0) {
			EditorGUI.indentLevel = 1;
			target.moveTouchpad = EditorGUILayout.ObjectField(GUIContent("Move Touchpad","Select s3dTouchpad for Move"),target.moveTouchpad,s3dTouchpad,allowSceneObjects);
			EditorGUI.indentLevel = 0;
		}
		target.turnPadPosition = EditorGUILayout.EnumPopup(GUIContent("Turn Pad Position","Select Turn Pad Position"), target.turnPadPosition); 
		if (target.turnPadPosition != 0) {
			EditorGUI.indentLevel = 1;
			target.turnTouchpad = EditorGUILayout.ObjectField(GUIContent("Turn Touchpad","Select s3dTouchpad for Turn"),target.turnTouchpad,s3dTouchpad,allowSceneObjects);
			EditorGUI.indentLevel = 0;
		}
		target.pointPadPosition = EditorGUILayout.EnumPopup(GUIContent("Cursor Pad Position","Select Cursor Pad Position"), target.pointPadPosition); 
		if (target.pointPadPosition != 0) {
			EditorGUI.indentLevel = 1;
			target.pointTouchpad = EditorGUILayout.ObjectField(GUIContent("Cursor Touchpad","Select s3dTouchpad for Cursor"),target.pointTouchpad,s3dTouchpad,allowSceneObjects);
			EditorGUI.indentLevel = 0;
		}
		target.useStereoParamsTouchpad = EditorGUILayout.Toggle(GUIContent("Show 3D Params Touchpad","Use Stereo Params Touchpad"), target.useStereoParamsTouchpad);
		if (target.useStereoParamsTouchpad) {
			EditorGUI.indentLevel = 1;
			target.stereoParamsTouchpad = EditorGUILayout.ObjectField(GUIContent("Stereo Params Touchpad","Select s3dTouchpad for Stereo Params"),target.stereoParamsTouchpad,s3dTouchpad,allowSceneObjects);
			EditorGUI.indentLevel = 2;
			target.interaxialTouchpad = EditorGUILayout.ObjectField(GUIContent("Interaxial Touchpad","Select s3dTouchpad for Interaxial"),target.interaxialTouchpad,s3dTouchpad,allowSceneObjects);
			target.zeroPrlxTouchpad = EditorGUILayout.ObjectField(GUIContent("Zero Prlx Touchpad","Select s3dTouchpad for Zero Prlx"),target.zeroPrlxTouchpad,s3dTouchpad,allowSceneObjects);
			target.hitTouchpad = EditorGUILayout.ObjectField(GUIContent("H I T Touchpad","Select s3dTouchpad for H I T"),target.hitTouchpad,s3dTouchpad,allowSceneObjects);
			EditorGUI.indentLevel = 0;
		}
		target.showLoadNewScenePad = EditorGUILayout.Toggle(GUIContent("Show Load Scene Touchpad","Show Load New Scene Touchpad"), target.showLoadNewScenePad);
		if (target.showLoadNewScenePad) {
			EditorGUI.indentLevel = 1;
			target.loadNewSceneTouchpad = EditorGUILayout.ObjectField(GUIContent("Load New Scene Touchpad","Select s3dTouchpad for Load New Scene"),target.loadNewSceneTouchpad,s3dTouchpad,allowSceneObjects);
			EditorGUI.indentLevel = 0;
		}
		target.showFpsTool01 = EditorGUILayout.Toggle(GUIContent("Show FPS Tool 01 Touchpad","Show FPS Tool 01 Touchpad"), target.showFpsTool01);
		if (target.showFpsTool01) {
			EditorGUI.indentLevel = 1;
			target.fpsTool01 = EditorGUILayout.ObjectField(GUIContent("FPS Tool 01 Touchpad","Select s3dTouchpad for FPS Tool 01"),target.fpsTool01,s3dTouchpad,allowSceneObjects);
			EditorGUI.indentLevel = 0;
		}
		target.showFpsTool02 = EditorGUILayout.Toggle(GUIContent("Show FPS Tool 02 Touchpad","Show FPS Tool 02 Touchpad"), target.showFpsTool02);
		if (target.showFpsTool02) {
			EditorGUI.indentLevel = 1;
			target.fpsTool02 = EditorGUILayout.ObjectField(GUIContent("FPS Tool 02 Touchpad","Select s3dTouchpad for FPS Tool 02"),target.fpsTool02,s3dTouchpad,allowSceneObjects);
			EditorGUI.indentLevel = 0;
		}
      	EditorGUILayout.EndVertical();

		if (GUI.changed) {
			EditorUtility.SetDirty (target);
		}
    }
}
