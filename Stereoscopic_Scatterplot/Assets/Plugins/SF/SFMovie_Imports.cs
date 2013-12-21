/**********************************************************************

Filename    :	SFMovie_Imports.cs
Content     :	
Created     :   
Authors     :   Ankur Mohan

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Scaleform
{
namespace GFx
{

// For a class or pointer to class to be passed to unmanaged code, it must have
// StructLayout Attribute.
[StructLayout(LayoutKind.Sequential)]
public partial class  Movie: System.Object
{
			
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
	[DllImport("libgfxunity3d")]
    private static extern int SF_Advance(long movieID, float deltaTime, bool advanceWhilePaused = false);
    
    [DllImport("libgfxunity3d")]
    private static extern void SF_SetFocus(long movieID, bool focus);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_Invoke3(long movieID, String methodName, int numVal, IntPtr iptr, [Out] Value retVal);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_CreateArray(long movieId, [Out] Value pvalManaged);

    [DllImport("libgfxunity3d")]
    private static extern int SF_CreateMovie(IntPtr param);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_CreateObject(long movieId, [Out] Value pvalManaged, String className, int numValues,
        IntPtr mvalArray);
			
    [DllImport("libgfxunity3d")]
    private static extern bool SF_HandleKeyEvent(long movieID, SFKey.Code cd, SFKeyModifiers.Modifiers mod, UInt32 down, int keyboardIndex);
    
    [DllImport("libgfxunity3d")]
    private static extern bool SF_HandleCharEvent(long movieID, UInt32 wchar);
    
    [DllImport("libgfxunity3d")]
    private static extern bool SF_HandleMouseEvent(long movieID, float x, float y, int icase, int buttonType);
    
    [DllImport("libgfxunity3d")]
    private static extern bool SF_HandleTouchEvent(long movieID, int fid, float x, float y, int icase);
    
    [DllImport("libgfxunity3d")]
    private static extern bool SF_DoHitTest(long movieID, float x, float y, int hitTestType);
    
    [DllImport("libgfxunity3d")]
    private static extern bool SF_SetBackgroundAlpha(long movieID, float bgAlpha);
    
    [DllImport("libgfxunity3d")]
    private static extern float SF_GetFrameRate(int MovieID);
			
#elif UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern int SF_Advance(long movieID, float deltaTime, bool advanceWhilePaused = false);
    
    [DllImport("__Internal")]
    private static extern void SF_SetFocus(long movieID, bool focus);
    
    [DllImport("__Internal")]
    private static extern bool SF_Invoke3(long movieID, String methodName, int numVal, IntPtr iptr, [Out] Value retVal);

    [DllImport("__Internal")]
    private static extern bool SF_CreateArray(long movieId, [Out] Value pvalManaged);
    
    [DllImport("__Internal")]
    private static extern int SF_CreateMovie(IntPtr param);
    
    [DllImport("__Internal")]
    private static extern bool SF_CreateObject(long movieId, [Out] Value pvalManaged, String className, int numValues, IntPtr mvalArray);
    
    [DllImport("__Internal")]
    private static extern bool SF_HandleKeyEvent(long movieID, SFKey.Code cd, SFKeyModifiers.Modifiers mod, UInt32 down, int keyboardIndex);
    
    [DllImport("__Internal")]
    private static extern bool SF_HandleCharEvent(long movieID, UInt32 wchar);
    
    [DllImport("__Internal")]
    private static extern bool SF_HandleMouseEvent(long movieID, float x, float y, int icase, int buttonType);
    
    [DllImport("__Internal")]
    private static extern bool SF_HandleTouchEvent(long movieID, int fid, float x, float y, int icase);
    
    [DllImport("__Internal")]
    private static extern bool SF_DoHitTest(long movieID, float x, float y, int hitTestType);
    
    [DllImport("__Internal")]
    private static extern bool SF_SetBackgroundAlpha(long movieID, float bgAlpha);
    
    [DllImport("__Internal")]
    private static extern float SF_GetFrameRate(int MovieID);

    
#elif UNITY_ANDROID
	[DllImport("gfxunity3d")]
    private static extern int SF_Advance(long movieID, float deltaTime, bool advanceWhilePaused = false);
    
    [DllImport("gfxunity3d")]
    private static extern void SF_SetFocus(long movieID, bool focus);

    [DllImport("gfxunity3d")]
    private static extern bool SF_Invoke3(long movieID, String methodName, int numVal, IntPtr iptr, [Out] Value retVal);

    [DllImport("gfxunity3d")]
    private static extern bool SF_CreateArray(long movieId, [Out] Value pvalManaged);

    [DllImport("gfxunity3d")]
    private static extern int SF_CreateMovie(IntPtr param);

    [DllImport("gfxunity3d")]
    private static extern bool SF_CreateObject(long movieId, [Out] Value pvalManaged, String className, int numValues, IntPtr mvalArray);    

    [DllImport("gfxunity3d")]
    private static extern bool SF_HandleKeyEvent(long movieID, SFKey.Code cd, SFKeyModifiers.Modifiers mod, UInt32 down, int keyboardIndex);
    
    [DllImport("gfxunity3d")]
    private static extern bool SF_HandleCharEvent(long movieID, UInt32 wchar);
    
    [DllImport("gfxunity3d")]
    private static extern bool SF_HandleMouseEvent(long movieID, float x, float y, int icase, int buttonType);
    
    [DllImport("gfxunity3d")]
    private static extern bool SF_HandleTouchEvent(long movieID, int fid, float x, float y, int icase);
    
    [DllImport("gfxunity3d")]
    private static extern bool SF_DoHitTest(long movieID, float x, float y, int hitTestType);
    
    [DllImport("gfxunity3d")]
    private static extern bool SF_SetBackgroundAlpha(long movieID, float bgAlpha);
    
    [DllImport("gfxunity3d")]
    private static extern float SF_GetFrameRate(int MovieID);
			
#endif
}

} // namespace GFx;

} // namespace Scaleform;