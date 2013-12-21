/**********************************************************************

Filename    :	SFManagerImports.cs
Content     :	
Created     :   
Authors     :   Ankur Mohan

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Scaleform
{

// For a class or pointer to class to be passed to unmanaged code, it must have
// StructLayout Attribute.
[StructLayout(LayoutKind.Sequential)]
public partial class SFManager
{
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
	[DllImport("libgfxunity3d")]
    private static extern bool SF_DestroyMovie(long movieID);

    [DllImport("libgfxunity3d")]
    private static extern void SF_NotifyNativeManager(long movieID, MovieLifeCycleEvents ev);

    // Delegates..
    [DllImport("libgfxunity3d")]
    private static extern void SF_SetExternalInterfaceDelegate(SF_ExternalInterfaceDelegate sf_eiDelegate);

    [DllImport("libgfxunity3d")]
    private static extern void SF_SetDisplayInfoDelegate(SF_DisplayInfoDelegate sf_doDelegate);

    [DllImport("libgfxunity3d")]
    private static extern void SF_SetAllocateValues(SF_AllocateDelegate sf_allocDelegate);

    [DllImport("libgfxunity3d")]
    private static extern void SF_SetSharedData(IntPtr pCommandOffset, IntPtr pCommandQueue, Int32 id);

    [DllImport("libgfxunity3d")]
    private static extern void SF_SetLogDelegate(SF_LogDelegate sf_logDelegate);

    [DllImport("libgfxunity3d")]
    private static extern void SF_ClearCommandBuffer(int numCommands);

    [DllImport("libgfxunity3d")]
    private static extern int SF_Init(IntPtr pdata, int size, String version);

    [DllImport("libgfxunity3d")]
    private static extern void SF_DestroyManager();

    [DllImport("libgfxunity3d")]
    private static extern bool SF_Display();
		
	[DllImport("libgfxunity3d")]
    private static extern void SF_Destroy();


	[DllImport("libgfxunity3d")]
    private static extern bool SF_ProcessMarkedForDeleteMovies();

	[DllImport("libgfxunity3d")]
	private static extern void SF_SetNewViewport(int ox, int oy, int Width, int Height);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_ReplaceTexture(long movieId, String textureName, int textureId, int RTWidth, int RTHeight);

	[DllImport("libgfxunity3d")]
	private static extern void SF_EnableIME();
	
	[DllImport("libgfxunity3d")]
	private static extern void SF_ApplyLanguage(String langName);

	[DllImport("libgfxunity3d")]
	private static extern void SF_LoadFontConfig(String fontConfigPath);
	
#elif UNITY_IPHONE
    [DllImport ("__Internal")]
    private static extern bool SF_DestroyMovie(long movieID);
    
    [DllImport ("__Internal")]
    private static extern void SF_NotifyNativeManager(long movieID, MovieLifeCycleEvents ev);
    
    // Delegates..
    [DllImport ("__Internal")]
    private static extern void SF_SetExternalInterfaceDelegate(SF_ExternalInterfaceDelegate sf_eiDelegate);
    
    [DllImport ("__Internal")]
    private static extern void SF_SetDisplayInfoDelegate(SF_DisplayInfoDelegate  sf_doDelegate);
    
    [DllImport ("__Internal")]
    private static extern void SF_SetAllocateValues(SF_AllocateDelegate sf_allocDelegate);
    
    [DllImport ("__Internal")]
    private static extern void SF_SetSharedData(IntPtr pCommandOffset, IntPtr pCommandQueue, Int32 id);
    
    [DllImport ("__Internal")]
    private static extern void SF_SetLogDelegate(SF_LogDelegate sf_logDelegate);

    [DllImport ("__Internal")]
    private static extern void SF_ClearCommandBuffer(int numCommands);
    
    [DllImport("__Internal")]
    private static extern int SF_Init(IntPtr pdata, int size, String version);

    [DllImport("__Internal")]
    private static extern void SF_DestroyManager();
		
	[DllImport("__Internal")]
    private static extern void SF_Destroy();	
    
    [DllImport ("__Internal")]
    private static extern bool SF_Display();

	[DllImport("__Internal")]
    private static extern bool SF_ProcessMarkedForDeleteMovies();

	[DllImport("__Internal")]
    private static extern void SF_SetNewViewport(int ox, int oy, int Width, int Height);
		
	[DllImport("__Internal")]
	private static extern bool SF_ReplaceTexture(long movieId, String textureName, int textureId, int RTWidth, int RTHeight);
	
	[DllImport("__Internal")]
	private static extern void SF_ApplyLanguage(String langName);

	[DllImport("__Internal")]
	private static extern void SF_LoadFontConfig(String fontConfigPath);
    

#elif UNITY_ANDROID

	[DllImport("gfxunity3d")]
    private static extern bool SF_DestroyMovie(long movieID);

    [DllImport("gfxunity3d")]
    private static extern void SF_NotifyNativeManager(long movieID, MovieLifeCycleEvents ev);

    // Delegates..
    [DllImport("gfxunity3d")]
    private static extern void SF_SetExternalInterfaceDelegate(SF_ExternalInterfaceDelegate sf_eiDelegate);

    [DllImport("gfxunity3d")]
    private static extern void SF_SetDisplayInfoDelegate(SF_DisplayInfoDelegate sf_doDelegate);

    [DllImport("gfxunity3d")]
    private static extern void SF_SetAllocateValues(SF_AllocateDelegate sf_allocDelegate);

    [DllImport("gfxunity3d")]
    private static extern void SF_SetSharedData(IntPtr pCommandOffset, IntPtr pCommandQueue, Int32 id);

    [DllImport("gfxunity3d")]
    private static extern void SF_SetLogDelegate(SF_LogDelegate sf_logDelegate);

    [DllImport("gfxunity3d")]
    private static extern void SF_ClearCommandBuffer(int numCommands);

    [DllImport("gfxunity3d")]
    private static extern int SF_Init(IntPtr pdata, int size, String version);

    [DllImport("gfxunity3d")]
    private static extern void SF_DestroyManager();

	[DllImport("gfxunity3d")]
    private static extern void SF_Destroy();

    [DllImport("gfxunity3d")]
    private static extern bool SF_Display();

	[DllImport("gfxunity3d")]
    private static extern bool SF_ProcessMarkedForDeleteMovies();

	[DllImport("gfxunity3d")]
	private static extern void SF_SetNewViewport(int ox, int oy, int Width, int Height);
	
	[DllImport("gfxunity3d")]
	private static extern bool SF_ReplaceTexture(long movieId, String textureName, int textureId, int RTWidth, int RTHeight);
	
	[DllImport("gfxunity3d")]
	private static extern void SF_ApplyLanguage(String langName);

	[DllImport("gfxunity3d")]
	private static extern void SF_LoadFontConfig(String fontConfigPath);
	
#endif
}

} // namespace Scaleform;