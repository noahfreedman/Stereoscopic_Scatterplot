/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* This script goes in the Editor folder. It provides a custom inspector for s3dCamera.
 */

@CustomEditor (s3dCamera)
class s3dCameraEditor extends Editor {
	static var foldout1 : boolean = true;
	static var foldout2 : boolean = true;
	static var foldout3 : boolean = true;
    function OnInspectorGUI () {
	    EditorGUIUtility.LookLikeControls(140,30);
		var allowSceneObjects : boolean = !EditorUtility.IsPersistent (target);
		foldout1 = EditorGUILayout.Foldout(foldout1, GUIContent("Stereo Parameters","Configure stereo camera")); 
		if (foldout1) {
	      	EditorGUILayout.BeginVertical("box");
	        target.interaxial = EditorGUILayout.IntSlider (GUIContent("Interaxial (mm)","Distance (in millimeters) between cameras."),target.interaxial, 0, 1000);
	        target.zeroPrlxDist = EditorGUILayout.Slider (GUIContent("Zero Prlx Dist (M)","Distance (in meters) at which left and right images overlap exactly."),target.zeroPrlxDist, 0.1, 100);
	        target.toedIn = EditorGUILayout.Toggle(GUIContent("Toed-In ","Angle cameras inward to converge. Bad idea!"), target.toedIn);
	 		target.cameraSelect = EditorGUILayout.EnumPopup(GUIContent("Camera Order","Swap cameras for cross-eyed free-viewing."), target.cameraSelect);
			target.H_I_T = EditorGUILayout.Slider (GUIContent("H I T","horizontal image transform (default 0)"),target.H_I_T, -25, 25);
	 		EditorGUILayout.EndVertical();
		}
		foldout2 = EditorGUILayout.Foldout(foldout2, GUIContent("Stereo Render","Configure display format")); 
		if (foldout2) {
	       	EditorGUILayout.BeginVertical("box");
	    	target.useStereoShader = EditorGUILayout.Toggle(GUIContent("Stereo Shader (Pro)","Enable for anaglyph and other modes. Unity Pro required. Not necessary for side-by-side."), target.useStereoShader);
			target.format3D = EditorGUILayout.EnumPopup(GUIContent("Stereo Format","Select 3D render format."), target.format3D); 
	    	if (target.format3D == 0) {	// side by side
	    		target.sideBySideSqueezed = EditorGUILayout.Toggle(GUIContent("Squeezed","For 3D TV frame-compatible format"), target.sideBySideSqueezed);
	    		if (!target.useStereoShader) {
	    			target.usePhoneMask = EditorGUILayout.Toggle(GUIContent("Use Phone Mask","Mask for side-by-side mobile phone formats"), target.usePhoneMask);
	    			if (target.usePhoneMask) {
	 					EditorGUI.indentLevel = 1;
						target.leftViewRect = EditorGUILayout.Vector4Field("Left View Rect (x y width height)", target.leftViewRect);
						target.rightViewRect = EditorGUILayout.Vector4Field("Right View Rect (x y width height)", target.rightViewRect);
	 					EditorGUI.indentLevel = 0;
	    			}
	    		} else {
	    			//target.usePhoneMask = false;
	    		}
	    	} else if (target.format3D == 1) { // anaglyph
	    		target.anaglyphOptions = EditorGUILayout.EnumPopup(GUIContent("Options","Anaglyph color formats"), target.anaglyphOptions);
	    	} else if (target.format3D == 3) { // interlace
				target.interlaceRows = EditorGUILayout.IntSlider (GUIContent("Rows","Vertical resolution for interlace format"),target.interlaceRows, 1, 1080);
	    	} else if (target.format3D == 4) { // checkerboard
				target.checkerboardColumns = EditorGUILayout.IntSlider (GUIContent("Columns", "Horizontal resolution for checkerboard format"),target.checkerboardColumns, 1, 1920);
				target.checkerboardRows = EditorGUILayout.IntSlider (GUIContent("Rows","Vertical resolution for checkerboard format"),target.checkerboardRows, 1, 1080);
	    	} else if (target.format3D == 5) { // scene screens
	    		target.useStereoShader = false;
				target.sceneScreenL = EditorGUILayout.ObjectField(GUIContent("Scene Screen L","Assign RenderTexture for Left Scene Screen"),target.sceneScreenL,RenderTexture,allowSceneObjects);
				target.sceneScreenR = EditorGUILayout.ObjectField(GUIContent("Scene Screen R","Assign RenderTexture for Right Scene Screen"),target.sceneScreenR,RenderTexture,allowSceneObjects);
	    	}
			if (target.useStereoShader) {
 				target.stereoMaterial = EditorGUILayout.ObjectField(GUIContent("Stereo Material","Assign stereoMat material (included)."),target.stereoMaterial,Material,allowSceneObjects);
				target.leftCamRT = EditorGUILayout.ObjectField(GUIContent("Render Texture L","Assign left view render texture"),target.leftCamRT,RenderTexture,allowSceneObjects);
				target.rightCamRT = EditorGUILayout.ObjectField(GUIContent("Render Texture R","Assign right view render texture"),target.rightCamRT,RenderTexture,allowSceneObjects);
	    	} else { // not using stereo shader
	     		if (target.format3D != 5) { // if not rendering to scene screens
	     			target.format3D = 0; // only format available is side-by-side
	     		}
	    	}

	       	EditorGUILayout.BeginHorizontal();
	    	GUILayout.FlexibleSpace();
			if (GUILayout.Button(GUIContent("Update","Clicking to update Game View"),EditorStyles.miniButton,GUILayout.MaxWidth(50))) {
				EditorUtility.SetDirty (target);
				target.initStereoCamera();
			}
	      	EditorGUILayout.EndHorizontal();
	      	EditorGUILayout.EndVertical();
			/*
			*** This section can be uncommented to expose various esoteric settings that are only needed for specialized applications ***
			foldout3 = EditorGUILayout.Foldout(foldout3, GUIContent("Advanced Options","Customize render for special fx")); 
			if (foldout3) {
	      		EditorGUILayout.BeginVertical("box");
 				target.offAxisFrustum = EditorGUILayout.Slider (GUIContent("Off Axis Frustum", "assymetrical frustum (default 0)"),target.offAxisFrustum, -10, 10);

				target.useCustomStereoLayer = EditorGUILayout.Toggle(GUIContent("Use Custom Stereo Layer","Set a custom layer to use multiple stereo cameras."), target.useCustomStereoLayer);  
	 			if (target.useCustomStereoLayer) {
	 				EditorGUI.indentLevel = 1;
	 				target.stereoLayer = EditorGUILayout.LayerField(GUIContent("Stereo Layer:","Camera will render this layer only."), target.stereoLayer);
	 				target.renderOrderDepth = EditorGUILayout.IntField(GUIContent("Render Order Depth:","For multiple stereo cameras. Higher layers are rendered on top of lower layers."), target.renderOrderDepth);  
	 				EditorGUI.indentLevel = 0;
	 			} else {
					target.stereoLayer = 0;
	 			}
	 			target.useLeftRightOnlyLayers = EditorGUILayout.Toggle(GUIContent("Use Left Right Only Layers","Enable layers seen by only one camera.") , target.useLeftRightOnlyLayers);  
		 		if (target.useLeftRightOnlyLayers) {
		 			EditorGUI.indentLevel = 1;
		 			target.leftOnlyLayer = EditorGUILayout.LayerField(GUIContent("Left Only Layer:","Layer seen by left camera only."), target.leftOnlyLayer);
		 			target.rightOnlyLayer = EditorGUILayout.LayerField(GUIContent("Right Only Layer:","Layer seen by right camera only."), target.rightOnlyLayer);  
		 			target.guiOnlyLayer = EditorGUILayout.LayerField(GUIContent("Gui Only Layer:","Layer seen by center camera (guiCam) only."), target.guiOnlyLayer);  
		 			EditorGUI.indentLevel = 0;
		 		} else {
		 			target.leftOnlyLayer = 20;
		 			target.rightOnlyLayer = 21;
		 			target.maskOnlyLayer = 22;
		 		}
		 		EditorGUILayout.EndVertical();
			}*/
	
		}    	
        if (GUI.changed) {
             EditorUtility.SetDirty (target);
    	}
	}
}
