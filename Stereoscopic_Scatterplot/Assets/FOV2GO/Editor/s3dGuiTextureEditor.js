/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

// This script goes in the Editor folder. It provides a custom inspector for s3dGuiTexture.js.

@CustomEditor (s3dGuiTexture)
class s3dGuiTextureEditor extends Editor {
	var showFileFields : boolean;
    function OnInspectorGUI () {
		var allowSceneObjects : boolean = !EditorUtility.IsPersistent (target);  
		EditorGUIUtility.LookLikeControls(160,50);
	    EditorGUILayout.BeginVertical("box");
        target.beginVisible = EditorGUILayout.Toggle(GUIContent("Begin Visible","Display texture at start"), target.beginVisible);
        target.timeToDisplay = EditorGUILayout.Slider (GUIContent("Time to Display (sec)","Enter 0 to keep visible indefinitely"),target.timeToDisplay, 0, 30);
		target.objectDistance = EditorGUILayout.Slider (GUIContent("Initial Object Distance (M)","Distance in meters at start"),target.objectDistance, 0.25, 20);
       	target.keepCloser = EditorGUILayout.Toggle(GUIContent("Track Depth","Use dynamic depth to keep texture closer than scene objects"), target.keepCloser);
       	if (target.keepCloser) {
	       	var distMin : float = Mathf.Clamp(target.minimumDistance,0.01,100);
	     	var distMax : float = Mathf.Clamp(target.maximumDistance,0.01,100);
       		EditorGUILayout.MinMaxSlider(distMin, distMax, 0.01, 100.0);
        	EditorGUILayout.BeginHorizontal();
       		EditorGUILayout.LabelField(GUIContent("Min Distance (M) "+ Mathf.Round(target.minimumDistance*100)/100,"Minimum allowed distance"), "");
        	EditorGUILayout.LabelField(GUIContent("Max Distance (M) "+ Mathf.Round(target.maximumDistance*10)/10,"Maximum allowed distance"), "");
       		EditorGUILayout.EndHorizontal();
        	target.minimumDistance = Mathf.Clamp(distMin,0.01,100);
        	target.maximumDistance = Mathf.Clamp(distMax,0.01,100);
       		target.nearPadding = EditorGUILayout.Slider (GUIContent("Near Padding (mm)","Padding between texture and nearest object behind"),target.nearPadding, 0, 20);
        	target.lagTime = EditorGUILayout.Slider (GUIContent("Smooth Depth Changes","Smooth out sudden shifts in depth"),target.lagTime, 0, 50);
         	}
      	EditorGUILayout.EndVertical();
        if (GUI.changed) {
            EditorUtility.SetDirty (target);
		}
    }
}
