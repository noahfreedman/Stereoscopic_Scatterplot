/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* This script goes in the Editor folder. It provides a custom inspector for s3dTouchpad.
 */

@CustomEditor (s3dTouchpad)
class s3dTouchpadEditor extends Editor {
    function OnInspectorGUI () {
	    EditorGUILayout.BeginVertical("box");
	   		target.moveLikeJoystick = EditorGUILayout.Toggle(GUIContent("Move Like Joystick","Move Graphic With Touch"), target.moveLikeJoystick);
	   		target.actLikeJoystick = EditorGUILayout.Toggle(GUIContent("Act Like Joystick","Jump to Touch Down Position"), target.actLikeJoystick);
        	target.shortTapTimeMax = EditorGUILayout.Slider (GUIContent("Short Tap Time Max","Maximum Touch Time for Short Tap"),target.shortTapTimeMax, 0.1, 0.5);
        	target.longTapTimeMax = EditorGUILayout.Slider (GUIContent("Long Tap Time Max","Maximum Touch Time for Long Tap"),target.longTapTimeMax, 0.2, 1.0);
       		target.tapDistanceLimit = EditorGUILayout.Slider (GUIContent("Tap Distance Limit","Maximum Travel Distance for Tap"),target.tapDistanceLimit, 1.0, 20.0);
	    EditorGUILayout.EndVertical();
    	if (GUI.changed) {
			EditorUtility.SetDirty(target);
		}
    }
}		
