/**********************************************************************

Filename    :   RTTObject.cs
Content     :   Inherits from MonoBehaviour
Created     :   
Authors     :   Ryan Holtz

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

public class TextureInSwf: MonoBehaviour
{

    public void Start()
    {
    	Debug.Log("Start");
    }

    public void Update()
    {
        RenderTexture rtt = (GetComponent("Camera") as Camera).targetTexture;
        SFCamera camera = Component.FindObjectOfType(typeof(SFCamera)) as SFCamera;
        if (camera)
        {
            SFManager sfManager = camera.GetSFManager();
            Movie movie = sfManager.GetTopMovie();
            if (movie != null)
            {
#if !(UNITY_3_5)
				Debug.Log("GetNativeTexturePtr: " + rtt.GetNativeTexturePtr());
                sfManager.ReplaceTexture(movie.GetID(), "texture1", rtt);
#endif
            }
        }
    }
}