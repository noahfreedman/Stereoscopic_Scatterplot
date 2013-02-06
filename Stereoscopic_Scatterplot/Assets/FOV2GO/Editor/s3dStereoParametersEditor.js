/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* This script goes in the Editor folder. It provides a custom inspector for s3dStereoParameters. */

@CustomEditor (s3dStereoParameters)
class s3dStereoParametersEditor extends Editor {
	static var foldout1 : boolean = false;
    function OnInspectorGUI () {
	    EditorGUIUtility.LookLikeControls(170,30);
		var allowSceneObjects : boolean = !EditorUtility.IsPersistent (target);
	    EditorGUILayout.BeginVertical("box");
		if (!target.s3dDeviceMan) {
			target.stereoParamsTouchpad = EditorGUILayout.ObjectField(GUIContent("Stereo Params Touchpad","Select s3dTouchpad for Stereo Params"),target.stereoParamsTouchpad,s3dTouchpad,allowSceneObjects);
			target.interaxialTouchpad = EditorGUILayout.ObjectField(GUIContent("Interaxial Touchpad","Select s3dTouchpad for Interaxial"),target.interaxialTouchpad,s3dTouchpad,allowSceneObjects);
			target.zeroPrlxTouchpad = EditorGUILayout.ObjectField(GUIContent("Zero Prlx Touchpad","Select s3dTouchpad for Zero Prlx"),target.zeroPrlxTouchpad,s3dTouchpad,allowSceneObjects);
			target.hitTouchpad = EditorGUILayout.ObjectField(GUIContent("H I T Touchpad","Select s3dTouchpad for H I T"),target.hitTouchpad,s3dTouchpad,allowSceneObjects);
		}
		target.saveStereoParamsToDisk = EditorGUILayout.Toggle(GUIContent("Save 3D Params To Disk","Save 3D Params To Disk"), target.saveStereoParamsToDisk);
		target.stereoReadoutText = EditorGUILayout.ObjectField(GUIContent("Stereo Readout Text","Select s3dGuiText for Stereo Readout"),target.stereoReadoutText,s3dGuiText,allowSceneObjects);
		foldout1 = EditorGUILayout.Foldout(foldout1, GUIContent("3D Params Touchpad Textures","Select 3D Params Touchpad Textures")); 
		if (foldout1) {
			target.showStereoParamsTexture = EditorGUILayout.ObjectField(GUIContent("Show Params Texture","Select Texture for Stereo Params Toggle"),target.showStereoParamsTexture,Texture,allowSceneObjects);
			target.dismissStereoParamsTexture = EditorGUILayout.ObjectField(GUIContent("Dismiss Params Texture","Select Texture for Dismiss Stereo Params Toggle"),target.dismissStereoParamsTexture,Texture,allowSceneObjects);
		}
		EditorGUILayout.EndVertical();

		if (GUI.changed) {
			EditorUtility.SetDirty (target);
		}
    }
}

