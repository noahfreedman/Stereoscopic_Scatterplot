/**********************************************************************

Filename    :	UI_Scene_Demo1.cs
Content     :  
Created     :   
Authors     :   Ankur Mohan

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/

using System;
using System.Collections;
using UnityEngine;
using Scaleform;
using Scaleform.GFx;

public class RenderTextureDemo : Movie
{
    protected Value	theMovie = null;
	public static RenderTextureDemo instance;
    
    public RenderTextureDemo(SFManager sfmgr, SFMovieCreationParams cp) :
        base(sfmgr, cp)
    {
        SFMgr = sfmgr;
        this.SetFocus(true);
		instance = this;
    }
	
    public void OnRegisterSWFCallback(Value movieRef)
    {
        theMovie = movieRef;
    }
	
	public void OpenGate()
	{
		theMovie.Invoke("openGate");
	}

	public void CloseGate()
	{
		theMovie.Invoke("closeGate");
	}
}
	
	