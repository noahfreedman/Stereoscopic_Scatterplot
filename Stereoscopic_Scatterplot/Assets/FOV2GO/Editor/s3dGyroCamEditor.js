/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/*  This script goes in the Editor folder. It provides a custom inspector for s3dGyroCam.
 */

@CustomEditor (s3dGyroCam)
class s3dGyroCamEditor extends Editor {
    function OnInspectorGUI () {
	    EditorGUILayout.BeginVertical("box");
	   		target.touchRotatesHeading = EditorGUILayout.Toggle(GUIContent("Touch Rotates Heading","Horizontal Swipe Rotates Heading"), target.touchRotatesHeading);
	   		target.setZeroToNorth = EditorGUILayout.Toggle(GUIContent("Set Zero Heading to North","Face Compass North on Startup"), target.setZeroToNorth);
	    EditorGUILayout.EndVertical();
        if (GUI.changed) {
            EditorUtility.SetDirty (target);
		}
    }
}   		
