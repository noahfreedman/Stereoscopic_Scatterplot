
/**********************************************************************

Filename    :   MyCamera.cs
Content     :   Inherits from SFCamera
Created     :   
Authors     :   Ankur Mohan

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Collections;
using Scaleform;

/* The user should override SFCamera and add methods for creating movies whenever specific events take place in the game.
*/
public class MyCamera : SFCamera {
	
    public MainMenu demo1 = null;
	public RenderTextureDemo rttDemo = null;

	public String swfFile = "MainMenu3D.swf";

	////////////////
	public const int STATE_BEGIN = 1;
	public const int STATE_MAIN_MENU = 2;
	public const int STATE_RTT = 3;
	public const int STATE_INTERACTION = 4;
	public const int STATE_FONTS = 5;
	public const int STATE_CLIK = 6;
	
	public bool freeLook = true;
	public bool stageMouseDown = false;
	public int currentState;
	public float camSpeed;
	public GameObject textureInSWFCamera;
	public GameObject rttScreen;
	
	public Quaternion originalRotation;
	
	public float sensitivity = 1f;
	
	public float rotationX = 0F;
    public float rotationY = 0F;
	
	public float sensitivityX = .0002F;
    public float sensitivityY = .0001F;
 
    public float minimumX = -360F;
    public float maximumX = 360F;
 
    public float minimumY = -60F;
    public float maximumY = 60F;

	public Vector3 originalMousePos;
	public Vector3 firstMousePos;
    
	//
	new public void Awake()
	{
		
		currentState = STATE_BEGIN;	
		camSpeed = 2.5f;
		
		originalRotation = transform.localRotation;
		
	}
	
    // Hides the Start function in the base SFCamera. Will be called every time the ScaleformCamera (Main Camera game object)
    // is created. Use new and not override, since return type is different from that of base::Start()
    new public  IEnumerator Start()
    {
        // The eval key must be set before any Scaleform related classes are loaded, other Scaleform Initialization will not 
        // take place.
#if (UNITY_STANDALONE_WIN && UNITY_EDITOR) || (UNITY_STANDALONE_WIN && !UNITY_EDITOR)         
        SF_SetKey("");
#elif (UNITY_STANDALONE_OSX && UNITY_EDITOR) || (UNITY_STANDALONE_OSX && !UNITY_EDITOR)
		SF_SetKey("");
#elif UNITY_IPHONE
		SF_SetKey("");
#elif UNITY_ANDROID
		SF_SetKey("");
#endif
        base.Start();
        if (WhenToRender == RenderTime.EndOfFrame)
        {
            yield return StartCoroutine("CallPluginAtEndOfFrame");      
        }
    }

    // Application specific code goes here
    new public void Update()
    {
        CreateGameHud();
			
		
		if(Input.GetMouseButtonDown(0))
		{
			rotationX=0;
			rotationY=0;
			originalRotation = transform.localRotation;
		}
		
		if (Input.GetMouseButtonDown(0))
	    {
			RaycastHit hit;
			Ray ray;
	        ray = camera.ScreenPointToRay(Input.mousePosition);
	
	        /*if (Physics.Raycast(ray, out hit, 100.0f, 1)) 
	        {
				if(hit.collider.name=="Joystick")
				{
	            	Debug.Log (hit.collider.name);
					controllingJoystick = true;
					originalMousePos = Input.mousePosition;
					firstMousePos = originalMousePos;
				}
	        }*/
	    }
		
        base.Update ();
    }
	
    private void CreateGameHud()
    {
        if (demo1 == null)
        {
            SFMovieCreationParams creationParams = CreateMovieCreationParams(swfFile);
            creationParams.TheScaleModeType  = ScaleModeType.SM_ShowAll;
            creationParams.IsInitFirstFrame = false;
            demo1 = new MainMenu(this, SFMgr, creationParams);
        }
    }
	
	public static float ClampAngle (float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp (angle, min, max);
    }
}