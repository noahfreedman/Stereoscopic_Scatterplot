/**********************************************************************

Filename    :   SFRTT.cs
Content     :   Inherits from MonoBehaviour
Created     :   
Authors     :   Ryan Holtz, Ankur Mohan

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
using Scaleform.GFx;

public class SFRTT : MonoBehaviour
{
    // Publicly-accessible values (settable in the Unity editor)
    public string SwfName;
	public string MovieClassName;
    public Color32 ClearColor;
    public int RenderTextureWidth;
    public int RenderTextureHeight;
	public int OriginX;
	public int OriginY;
    
    public RenderTexture RenderTexture;
    protected Movie RTTMovie;
    protected Collider TheCollider;

    public virtual void Start()
    {
    	Debug.Log("Start: Initializing RTT: " + SwfName);
        // The idea is to use the existing RTT if the user has already set one in the Unity editor. If not, we'll go ahead 
        // and create one.
        if (RenderTexture)
        {
            RenderTextureHeight = RenderTexture.height;
            RenderTextureWidth  = RenderTexture.width;
        }
        if (RenderTextureWidth == 0 || RenderTextureHeight == 0)
        {
            Debug.Log("RenderTexture width or height is zero. Can't instantiate RTT.");
            return;
        }

		if (RenderTextureWidth != RenderTextureHeight)
		{
			Debug.Log("RenderTexture width must be the same as the height. This is a Unity limitation");
			RenderTexture = null;
			return;
		}
		
		if (OriginX < 0 || OriginY < 0 || OriginX > RenderTextureWidth || OriginY > RenderTextureHeight)
		{
			Debug.Log("RenderTexture Origin coordinates must be set properly");
			RenderTexture = null;
			return;
		}
        if (!RenderTexture)
        {
            RenderTexture = new RenderTexture(RenderTextureWidth, RenderTextureHeight, 16, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
            RenderTexture.Create();
            renderer.material.mainTexture = RenderTexture;
        }
        TheCollider = GetComponent("Collider") as Collider;
        Debug.Log(TheCollider);

    }

    /*
    public virtual void OnDestroy()
    {
        Debug.Log("In Destroy");
        renderer.material.mainTexture = null;
        RenderTexture = null;
        RTTMovie = null;
    }
    */
    public virtual void Update()
    {
        
    }

    public Collider GetCollider()
    {
        return TheCollider;
    }

    public bool CreateRenderMovie(SFCamera camera, Type movieClassType)
    {
#if !(UNITY_4_0 || UNITY_4_1) && UNITY_STANDALONE_WIN
		if (SystemInfo.graphicsDeviceVersion[0] == 'D')
		{
			return false;
		}
#endif
        if (RenderTexture == null)
        {
            Debug.Log("RenderTexture is null, failure to create RenderMovie. Check if you specified all the parameters properly.");
            return false;
        }
        if (RTTMovie == null || RTTMovie.MovieID == 0)
        {
            if (camera)
            {
                Debug.Log("Creating movie");
                SFMovieCreationParams creationParams = SFCamera.CreateRTTMovieCreationParams(SwfName, OriginX, OriginY, RenderTexture, ClearColor);
                creationParams.TheScaleModeType      = ScaleModeType.SM_ExactFit;
                creationParams.IsInitFirstFrame      = true;
                creationParams.IsAutoManageViewport  = false;
                // Has the user specified a movie subclass?
                if (movieClassType != null)
                {
                    SFManager sfMgr = camera.GetSFManager();
                    RTTMovie = Activator.CreateInstance(movieClassType, sfMgr, creationParams) as Movie;
                    if (RTTMovie != null)
                    {
                        RTTMovie.SetRTT(this);
                    }
                    return true;
                }
            }
        }
        return false;
    }
}