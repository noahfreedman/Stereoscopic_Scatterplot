/**********************************************************************

Filename    :	SFValue_Imports.cs
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
namespace GFx
{

// For a class or pointer to class to be passed to unmanaged code, it must have
// StructLayout Attribute.
[StructLayout(LayoutKind.Sequential)]
public partial class  Value: System.Object
{
			
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR

	[DllImport("libgfxunity3d")]
    private static extern Boolean SF_Equals(Value val1, Value val2);

    [DllImport("libgfxunity3d")]
    private static extern IntPtr SF_CreateNewValue(IntPtr src, long movieID);

    [DllImport("libgfxunity3d")]
    public static extern void SF_DecrementValRefCount(IntPtr val);
    
    [DllImport("libgfxunity3d")]
    private static extern bool SF_Invoke2(Value val, String methodName, int numVal, IntPtr iptr, [Out] Value retVal);

    [DllImport("libgfxunity3d")]
    private static extern IntPtr SF_AllocateString(String sval, long movieID);
    
    [DllImport("libgfxunity3d")]
    private static extern IntPtr SF_AllocateBoolean(Boolean bval, long movieID);
    
    [DllImport("libgfxunity3d")]
    private static extern IntPtr SF_AllocateDouble(Double nval, long movieID);

    [DllImport("libgfxunity3d")]
    private static extern double SF_GetNumber(Value val);

    [DllImport("libgfxunity3d")]
    private static extern UInt32 SF_GetUInt(Value val);

    [DllImport("libgfxunity3d")]
    private static extern Int32 SF_GetInt(Value val);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_GetBool(Value val);

    [DllImport("libgfxunity3d")]
    private static extern IntPtr SF_GetString(Value val);

	[DllImport("libgfxunity3d")]
	private static extern void SF_SetNumber(Value val, double num);

	[DllImport("libgfxunity3d")]
	private static extern void SF_SetUInt(Value val, UInt32 uival);

	[DllImport("libgfxunity3d")]
	private static extern void SF_SetInt(Value val, Int32 ival);

	[DllImport("libgfxunity3d")]
	private static extern void SF_SetBool(Value val, bool bval);

	[DllImport("libgfxunity3d")]
	private static extern void SF_SetString(Value val, IntPtr str);

    [DllImport("libgfxunity3d")]
    private static extern void SF_GetObject(Value val, IntPtr newval);
    
    [DllImport("libgfxunity3d")]
    private static extern bool SF_GetDisplayInfo(Value target, [Out] SFDisplayInfo dinfo, Int32 size);
    
    [DllImport("libgfxunity3d")]
    private static extern bool SF_SetDisplayInfo(Value target, SFDisplayInfo dinfo, Int32 size);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_GetDisplayMatrix(Value target, [Out] SFDisplayMatrix dmat, Int32 size);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_SetDisplayMatrix(Value target, SFDisplayMatrix dmat, Int32 size);
    
    [DllImport("libgfxunity3d")]
    private static extern bool SF_SetMember(Value target, String elemName, Value val);
    
    [DllImport ("libgfxunity3d")]
    private static extern bool SF_GetMember(Value target, String elemName, [Out] Value dest);

    [DllImport("libgfxunity3d")]
    private static extern int SF_GetArraySize(Value target);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_SetArraySize(Value target, UInt32 sz);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_GetElement(Value target, UInt32 idx, [Out] Value dest);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_SetElement(Value target, UInt32 idx, Value val);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_RemoveElement(Value target, UInt32 idx);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_ClearElements(Value target);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_SetColorTransform(Value target, SFCxForm cxform);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_GetColorTransform(Value target, [Out] SFCxForm cxform);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_SetText(Value target, String str);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_GetText(Value target, [Out] Value dst);
    
    [DllImport("libgfxunity3d")]
    private static extern bool SF_CreateEmptyMovieClip(Value target, [Out] Value dest, String instanceName, Int32 depth);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_AttachMovie(Value target, [Out] Value dest, String symbolName, String instanceName, Int32 depth);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_GotoAndPlayFrame(Value target, String frameName);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_GotoAndStopFrame(Value target, String frameName);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_GotoAndPlay(Value target, Int32 frameNum);

    [DllImport("libgfxunity3d")]
    private static extern bool SF_GotoAndStop(Value target, Int32 frameNum);

#elif UNITY_IPHONE
			
	[DllImport("__Internal")]
    private static extern Boolean SF_Equals(Value val1, Value val2);
			
    [DllImport("__Internal")]
    private static extern IntPtr SF_CreateNewValue(IntPtr src, long movieID);

    [DllImport("__Internal")]
    public static extern void SF_DecrementValRefCount(IntPtr val);

    [DllImport("__Internal")]
    private static extern bool SF_Invoke2(Value val, String methodName, int numVal, IntPtr iptr, [Out] Value retVal);

    [DllImport("__Internal")]
    private static extern IntPtr SF_AllocateString(String sval, long movieID);
    
    [DllImport("__Internal")]
    private static extern IntPtr SF_AllocateBoolean(Boolean bval, long movieID);
    
    [DllImport("__Internal")]
    private static extern IntPtr SF_AllocateDouble(Double nval, long movieID);

    [DllImport("__Internal")]
    private static extern double SF_GetNumber(Value val);

    [DllImport("__Internal")]
    private static extern UInt32 SF_GetUInt(Value val);

    [DllImport("__Internal")]
    private static extern Int32 SF_GetInt(Value val);

    [DllImport("__Internal")]
    private static extern bool SF_GetBool(Value val);

    [DllImport("__Internal")]
    private static extern IntPtr SF_GetString(Value val);

	[DllImport("__Internal")]
	private static extern void SF_SetNumber(Value val, double num);

	[DllImport("__Internal")]
	private static extern void SF_SetUInt(Value val, UInt32 uival);

	[DllImport("__Internal")]
	private static extern void SF_SetInt(Value val, Int32 ival);

	[DllImport("__Internal")]
	private static extern void SF_SetBool(Value val, bool bval);

	[DllImport("__Internal")]
	private static extern void SF_SetString(Value val, IntPtr str);

    [DllImport("__Internal")]
    private static extern void SF_GetObject(Value val, IntPtr newval);
    
    [DllImport("__Internal")]
    private static extern bool SF_GetDisplayInfo(Value target, [Out] SFDisplayInfo dinfo, Int32 size);
    
    [DllImport("__Internal")]
    private static extern bool SF_SetDisplayInfo(Value target, SFDisplayInfo dinfo, Int32 size);

    [DllImport("__Internal")]
    private static extern bool SF_GetDisplayMatrix(Value target, [Out] SFDisplayMatrix dmat, Int32 size);

    [DllImport("__Internal")]
    private static extern bool SF_SetDisplayMatrix(Value target, SFDisplayMatrix dmat, Int32 size);
    
    [DllImport("__Internal")]
    private static extern bool SF_SetMember(Value target, String elemName, Value val);
    
    [DllImport("__Internal")]
    private static extern bool SF_GetMember(Value target, String elemName, [Out] Value dest);

    [DllImport("__Internal")]
    private static extern int SF_GetArraySize(Value target);

    [DllImport("__Internal")]
    private static extern bool SF_SetArraySize(Value target, UInt32 sz);

    [DllImport("__Internal")]
    private static extern bool SF_GetElement(Value target, UInt32 idx, [Out] Value dest);

    [DllImport("__Internal")]
    private static extern bool SF_SetElement(Value target, UInt32 idx, Value val);

    [DllImport("__Internal")]
    private static extern bool SF_RemoveElement(Value target, UInt32 idx);

    [DllImport("__Internal")]
    private static extern bool SF_ClearElements(Value target);

    [DllImport("__Internal")]
    private static extern bool SF_SetColorTransform(Value target, SFCxForm cxform);

    [DllImport("__Internal")]
    private static extern bool SF_GetColorTransform(Value target, [Out] SFCxForm cxform);

    [DllImport("__Internal")]
    private static extern bool SF_SetText(Value target, String str);

    [DllImport("__Internal")]
    private static extern bool SF_GetText(Value target, [Out] Value dst);
    
    [DllImport("__Internal")]
    private static extern bool SF_CreateEmptyMovieClip(Value target, [Out] Value dest, String instanceName, Int32 depth);

    [DllImport("__Internal")]
    private static extern bool SF_AttachMovie(Value target, [Out] Value dest, String symbolName, String instanceName, Int32 depth);

    [DllImport("__Internal")]
    private static extern bool SF_GotoAndPlayFrame(Value target, String frameName);

    [DllImport("__Internal")]
    private static extern bool SF_GotoAndStopFrame(Value target, String frameName);

    [DllImport("__Internal")]
    private static extern bool SF_GotoAndPlay(Value target, Int32 frameNum);

    [DllImport("__Internal")]
    private static extern bool SF_GotoAndStop(Value target, Int32 frameNum);


#elif UNITY_ANDROID
			
	[DllImport("gfxunity3d")]
    private static extern Boolean SF_Equals(Value val1, Value val2);

    [DllImport("gfxunity3d")]
    private static extern IntPtr SF_CreateNewValue(IntPtr src, long movieID);

    [DllImport("gfxunity3d")]
    public static extern void SF_DecrementValRefCount(IntPtr val);
    
    [DllImport("gfxunity3d")]
    private static extern bool SF_Invoke2(Value val, String methodName, int numVal, IntPtr iptr, [Out] Value retVal);

    [DllImport("gfxunity3d")]
    private static extern IntPtr SF_AllocateString(String sval, long movieID);
    
    [DllImport("gfxunity3d")]
    private static extern IntPtr SF_AllocateBoolean(Boolean bval, long movieID);
    
    [DllImport("gfxunity3d")]
    private static extern IntPtr SF_AllocateDouble(Double nval, long movieID);

    [DllImport("gfxunity3d")]
    private static extern double SF_GetNumber(Value val);

    [DllImport("gfxunity3d")]
    private static extern UInt32 SF_GetUInt(Value val);

    [DllImport("gfxunity3d")]
    private static extern Int32 SF_GetInt(Value val);

    [DllImport("gfxunity3d")]
    private static extern bool SF_GetBool(Value val);

    [DllImport("gfxunity3d")]
    private static extern IntPtr SF_GetString(Value val);

	[DllImport("gfxunity3d")]
	private static extern void SF_SetNumber(Value val, double num);

	[DllImport("gfxunity3d")]
	private static extern void SF_SetUInt(Value val, UInt32 uival);

	[DllImport("gfxunity3d")]
	private static extern void SF_SetInt(Value val, Int32 ival);

	[DllImport("gfxunity3d")]
	private static extern void SF_SetBool(Value val, bool bval);

	[DllImport("gfxunity3d")]
	private static extern void SF_SetString(Value val, IntPtr str);

    [DllImport("gfxunity3d")]
    private static extern void SF_GetObject(Value val, IntPtr newval);
    
    [DllImport("gfxunity3d")]
    private static extern bool SF_GetDisplayInfo(Value target, [Out] SFDisplayInfo dinfo, Int32 size);
    
    [DllImport("gfxunity3d")]
    private static extern bool SF_SetDisplayInfo(Value target, SFDisplayInfo dinfo, Int32 size);

    [DllImport("gfxunity3d")]
    private static extern bool SF_GetDisplayMatrix(Value target, [Out] SFDisplayMatrix dmat, Int32 size);

    [DllImport("gfxunity3d")]
    private static extern bool SF_SetDisplayMatrix(Value target, SFDisplayMatrix dmat, Int32 size);
    
    [DllImport("gfxunity3d")]
    private static extern bool SF_SetMember(Value target, String elemName, Value val);
    
    [DllImport ("gfxunity3d")]
    private static extern bool SF_GetMember(Value target, String elemName, [Out] Value dest);

    [DllImport("gfxunity3d")]
    private static extern int SF_GetArraySize(Value target);

    [DllImport("gfxunity3d")]
    private static extern bool SF_SetArraySize(Value target, UInt32 sz);

    [DllImport("gfxunity3d")]
    private static extern bool SF_GetElement(Value target, UInt32 idx, [Out] Value dest);

    [DllImport("gfxunity3d")]
    private static extern bool SF_SetElement(Value target, UInt32 idx, Value val);

    [DllImport("gfxunity3d")]
    private static extern bool SF_RemoveElement(Value target, UInt32 idx);

    [DllImport("gfxunity3d")]
    private static extern bool SF_ClearElements(Value target);

    [DllImport("gfxunity3d")]
    private static extern bool SF_SetColorTransform(Value target, SFCxForm cxform);

    [DllImport("gfxunity3d")]
    private static extern bool SF_GetColorTransform(Value target, [Out] SFCxForm cxform);

    [DllImport("gfxunity3d")]
    private static extern bool SF_SetText(Value target, String str);

    [DllImport("gfxunity3d")]
    private static extern bool SF_GetText(Value target, [Out] Value dst);
    
    [DllImport("gfxunity3d")]
    private static extern bool SF_CreateEmptyMovieClip(Value target, [Out] Value dest, String instanceName, Int32 depth);

    [DllImport("gfxunity3d")]
    private static extern bool SF_AttachMovie(Value target, [Out] Value dest, String symbolName, String instanceName, Int32 depth);

    [DllImport("gfxunity3d")]
    private static extern bool SF_GotoAndPlayFrame(Value target, String frameName);

    [DllImport("gfxunity3d")]
    private static extern bool SF_GotoAndStopFrame(Value target, String frameName);

    [DllImport("gfxunity3d")]
    private static extern bool SF_GotoAndPlay(Value target, Int32 frameNum);

    [DllImport("gfxunity3d")]
    private static extern bool SF_GotoAndStop(Value target, Int32 frameNum);
#endif
}

} // namespace GFx;

} // namespace Scaleform;
    