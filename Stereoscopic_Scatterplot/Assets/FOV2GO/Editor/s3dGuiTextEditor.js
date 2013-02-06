/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

// This script goes in the Editor folder. It provides a custom inspector for s3DguiText.js.

@CustomEditor (s3dGuiText)
class s3dGuiTextEditor extends Editor {
	static var foldout1 : boolean = true;
	static var foldout2 : boolean = true;
	static var foldout3 : boolean = true;
    function OnInspectorGUI () {
  		var allowSceneObjects : boolean = !EditorUtility.IsPersistent (target);
 		EditorGUIUtility.LookLikeControls(150,50);
		foldout1 = EditorGUILayout.Foldout(foldout1, "Text"); 
		if (foldout1) {
	    	EditorGUILayout.BeginVertical("box");
	     	EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(GUIContent("Initial Text","Enter text to display at scene start"), "", GUILayout.MaxWidth(75));
			target.initString = EditorGUILayout.TextArea(target.initString, GUILayout.MinHeight(50));
	       	if (GUILayout.Button("Enter",GUILayout.MaxWidth(50))) {
	    		GUIUtility.keyboardControl = 0;
			}
	    	EditorGUILayout.EndHorizontal();
			target.beginVisible = EditorGUILayout.Toggle(GUIContent("Begin Visible","Display text at start"), target.beginVisible);
	        target.timeToDisplay = EditorGUILayout.Slider (GUIContent("Time to Display (sec)","Enter 0 to keep visible indefinitely"), target.timeToDisplay, 0, 30);
 
	    	EditorGUILayout.EndVertical();
		}
        
 		foldout2 = EditorGUILayout.Foldout(foldout2, "Style"); 
		if (foldout2) {
     		EditorGUILayout.BeginVertical("box");
       		target.TextColor = EditorGUILayout.ColorField(GUIContent("Text Color","Specify text color"), target.TextColor);
			target.shadowsOn = EditorGUILayout.Toggle(GUIContent("Shadow","Add drop shadow to text"), target.shadowsOn);
			if (target.shadowsOn) {
      			target.ShadowColor = EditorGUILayout.ColorField(GUIContent("Shadow Color","Specify shadow color"), target.ShadowColor);
       			target.shadowOffset = EditorGUILayout.Slider(GUIContent("Shadow Offset","Specify shadow offset"), target.shadowOffset, 1, 20);
			}
     		EditorGUILayout.EndVertical();
		}

 		foldout3 = EditorGUILayout.Foldout(foldout3, "Distance"); 
		if (foldout3) {
	      	EditorGUILayout.BeginVertical("box");
			target.objectDistance = EditorGUILayout.Slider (GUIContent("Initial Object Distance (M)","Distance in meters at start"), target.objectDistance, 0.25, 20);
	       	target.keepCloser = EditorGUILayout.Toggle(GUIContent("Track Depth","Use dynamic depth to keep text closer than scene objects"), target.keepCloser);
	       	if (target.keepCloser) {
				var distMin : float = target.minimumDistance;
	     		var distMax : float = target.maximumDistance;
	        	EditorGUILayout.MinMaxSlider(distMin, distMax, 0.25, 20.0);
	        	EditorGUILayout.BeginHorizontal();
	       		EditorGUILayout.LabelField(GUIContent("Min Distance (M) "+ Mathf.Round(target.minimumDistance*10)/10,"Minimum allowed distance"), "");
	        	EditorGUILayout.LabelField(GUIContent("Max Distance (M) "+ Mathf.Round(target.maximumDistance*10)/10,"Maximum allowed distance"), "");
	       		EditorGUILayout.EndHorizontal();
	        	target.minimumDistance = distMin;
	       	 	target.maximumDistance = distMax;
	       		target.nearPadding = EditorGUILayout.Slider (GUIContent("Near Padding (mm)","Padding between text and nearest object behind"), target.nearPadding, 0.5, 20);
	        	target.lagTime = EditorGUILayout.Slider (GUIContent("Smooth Depth Changes","Smooth out sudden shifts in depth"), target.lagTime, 0, 50);
     			target.trackMouseXYPosition = EditorGUILayout.Toggle(GUIContent("Track Mouse Position","Text follows mouse position"), target.trackMouseXYPosition);
        		target.onlyWhenMouseDown = EditorGUILayout.Toggle(GUIContent("Track Only When Down","Text follows mouse position only when mouse button down"), target.onlyWhenMouseDown);
	       	}
			EditorGUILayout.EndVertical();
		}        
        if (GUI.changed) {
            EditorUtility.SetDirty (target);
        }
	}
}

