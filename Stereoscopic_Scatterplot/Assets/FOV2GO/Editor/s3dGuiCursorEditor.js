/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

// This script goes in the Editor folder. It provides a custom inspector for s3dGuiText.js.

@CustomEditor (s3dGuiCursor)
class s3dGuiCursorEditor extends Editor {
	var showFileFields : boolean;
    function OnInspectorGUI () {
		var allowSceneObjects : boolean = !EditorUtility.IsPersistent (target);  
		EditorGUIUtility.LookLikeControls(160,50);
	    EditorGUILayout.BeginVertical("box");
     	target.trackMouseXYPosition = EditorGUILayout.Toggle(GUIContent("Track Mouse Position","Follow mouse position [enable for desktop stereo cursor]"), target.trackMouseXYPosition);
     	if (target.trackMouseXYPosition) {
			EditorGUI.indentLevel = 1;
     		target.onlyWhenMouseDown = EditorGUILayout.Toggle(GUIContent("Track Only When Down","Follow mouse position only when mouse button down"), target.onlyWhenMouseDown);
			EditorGUI.indentLevel = 0;
     	}
     	target.useTouchpad = EditorGUILayout.Toggle(GUIContent("Use Touchpad","Control via s3dTouchpad"), target.useTouchpad);
     	if (target.useTouchpad) {
			EditorGUI.indentLevel = 1;
     		target.touchpad = EditorGUILayout.ObjectField(GUIContent("Touchpad","Assign s3d Touchpad"),target.touchpad,s3dTouchpad,allowSceneObjects);
     		target.touchpadSpeed = EditorGUILayout.Vector2Field("Touchpad Speed Factor", target.touchpadSpeed);
			EditorGUI.indentLevel = 0;
     	}
        target.clickDistance = EditorGUILayout.Slider (GUIContent("Maximum Click Distance","Ignore clicks beyond this distance"),target.clickDistance, 5, 100);
       	target.hidePointer = EditorGUILayout.Toggle(GUIContent("Hide Pointer","Hide Default Pointer"), target.hidePointer);
		target.interactiveLayer = EditorGUILayout.LayerField(GUIContent("Interactive Layer:","Layer for clickable objects."), target.interactiveLayer);
		showFileFields = EditorGUILayout.Foldout(showFileFields, "Textures & Sounds");
		if (showFileFields) {
			EditorGUI.indentLevel = 1;
      		target.defaultTexture = EditorGUILayout.ObjectField(GUIContent("Default Texture","Default Texture"),target.defaultTexture,Texture,allowSceneObjects);
      		target.clickSound = EditorGUILayout.ObjectField(GUIContent("Click Sound","Sound for click"),target.clickSound,AudioClip,allowSceneObjects);
    		target.clickTexture = EditorGUILayout.ObjectField(GUIContent("Click Texture","Texture for clicks"),target.clickTexture,Texture,allowSceneObjects);
     		target.pickSound = EditorGUILayout.ObjectField(GUIContent("Pick Sound","Sound for select"),target.pickSound,AudioClip,allowSceneObjects);
			target.pickTexture = EditorGUILayout.ObjectField(GUIContent("Pick Texture","Texture for select"),target.pickTexture,Texture,allowSceneObjects);
			EditorGUI.indentLevel = 0;
		}
      	EditorGUILayout.EndVertical();
        if (GUI.changed) {
            EditorUtility.SetDirty (target);
		}
    }
}
