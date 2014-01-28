/**********************************************************************

Filename    :	SFValue.cs
Content     :	Wrapper for Value
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

[StructLayout(LayoutKind.Sequential)]
public class SFCommand: System.Object
{
    public long       MovieID;
    public int        ArgCount;
    public IntPtr     pMethodName;
}

public class SFDllName
{
    public static string DllName = "libgfxunity3d";
}

// For a class or pointer to class to be passed to unmanaged code, it must have
// StructLayout Attribute.
public partial class  Value: System.Object
{
    
    public enum ValueType: int
    {
        // ** Type identifiers
        // Basic types
        VT_Undefined        = 0x00,
        VT_Null             = 0x01,
        VT_Boolean          = 0x02,
        VT_Int              = 0x03,
        VT_UInt             = 0x04,
        VT_Number           = 0x05,
        VT_String           = 0x06,

        // StringW can be passed as an argument type, but it will only be returned 
        // if VT_ConvertStringW was specified, as it is not a native type.
        VT_StringW          = 0x07,  // wchar_t* string
        
        // ActionScript VM objects
        VT_Object           = 0x08,
        VT_Array            = 0x09,
        // Special type for stage instances (MovieClips, Buttons, TextFields)
        VT_DisplayObject    = 0x0a,

    }

    enum ValueTypeControl:int
    {
        // ** Special Bit Flags
        // Explicit coercion to type requested 
        VTC_ConvertBit = 0x80,
        // Flag that denotes managed resources
        VTC_ManagedBit = 0x40,
        // Set if value was referencing managed value and the owner Movie/VM
        // was destroyed.
        VTC_OrphanedBit = 0x20,

        // ** Type mask
        VTC_TypeMask = VTC_ConvertBit | 0x0F,
    };
    public IntPtr   	pInternalData;
    public ValueType    Type;
	public long         MovieId;

    public Value()
    {
        MovieId            = 0;
        pInternalData    = IntPtr.Zero;
    }

    new public ValueType    GetType()       { return (ValueType)(((int)(Type)) & ((int)(ValueTypeControl.VTC_TypeMask))); }
    
    // Check types
    public Boolean      IsUndefined()       { return GetType() == ValueType.VT_Undefined; }
    public Boolean      IsNull()            { return GetType() == ValueType.VT_Null; }
    public Boolean      IsBoolean()         { return GetType() == ValueType.VT_Boolean; }
    public Boolean      IsInt()             { return GetType() == ValueType.VT_Int; }
    public Boolean      IsUInt()            { return GetType() == ValueType.VT_UInt; }
    public Boolean      IsNumber()          { return GetType() == ValueType.VT_Number; }
    public Boolean      IsNumeric()         { return IsInt() || IsUInt() || IsNumber(); }
    public Boolean      IsString()          { return GetType() == ValueType.VT_String; }
    public Boolean      IsStringW()         { return GetType() == ValueType.VT_StringW; }
    public Boolean      IsObject()
    {
        return (GetType() == ValueType.VT_Object || GetType() == ValueType.VT_Array ||
                        GetType() == ValueType.VT_DisplayObject);
    }
    public Boolean        IsArray()         { return GetType() == ValueType.VT_Array; }
    public Boolean        IsDisplayObject() { return GetType() == ValueType.VT_DisplayObject; }
    
	// override equals
	public bool Equals(Value obj)
	{
	/*
	    Can't do this: 
		if (obj.MovieId == MovieId && obj.pInternalData == pInternalData && obj.type == type) return true;
		return false;
	    Need to compare the underlying GFx Data, which can only be done in C++.
	 */ 
		return SF_Equals(this, obj);
	}

    public Value(String sval, long movieID)
    {
        pInternalData   = SF_AllocateString(sval, movieID);
        Type            = ValueType.VT_String;
        MovieId         = movieID;
    }
    public Value(Boolean bval, long movieID)
    {
        pInternalData   = SF_AllocateBoolean(bval, movieID);
        Type            = ValueType.VT_Boolean;
        MovieId         = movieID;
    }
    public Value(Double nval, long movieID)
    {
        pInternalData   = SF_AllocateDouble(nval, movieID);
        Type            = ValueType.VT_Number;
        MovieId         = movieID;
    }
    
    // Copy Constructor
    public Value(Value val)
    {
        if (val == null) 
        {
            return;
        }
        
		pInternalData = IntPtr.Zero;
        if (val.pInternalData != IntPtr.Zero)
        {
            pInternalData    = SF_CreateNewValue(val.pInternalData, val.MovieId);
        }
       
        Type = val.Type;
        MovieId = val.MovieId;
    }

	public bool IsValid()
	{
		if (pInternalData == IntPtr.Zero) return false;
		if (IsUndefined()) return false;
		return true;
	}

	// Implicit casts..

	public static implicit operator String(Value instance)
	{
		//implicit cast logic
		return instance.GetString();
	}

	public static implicit operator Int32(Value instance)
	{
		//implicit cast logic
		return instance.GetInt();
	}

	public static implicit operator UInt32(Value instance)
	{
		//implicit cast logic
		return instance.GetUInt();
	}

	public static implicit operator Double(Value instance)
	{
		//implicit cast logic
		return instance.GetNumber();
	}

	public static implicit operator Boolean(Value instance)
	{
		//implicit cast logic
		return instance.GetBool();
	}

    public override string ToString()
    {
        if (IsBoolean())    return "Boolean: " + GetBool() + "\n";
        if (IsString())        return "String: " + GetString() + "\n";
        if (IsUInt())        return "UInt: " + GetUInt() + "\n";
        if (IsInt())        return "Int: " + GetInt() + "\n";
        if (IsNumber())        return "Number: " + GetNumber() + "\n";
        if (IsObject())        return "Object" + "\n";
        return "Unknown";
    }
    
    public double GetNumber()
    {
		if (IsNumber())
		{ 
			return SF_GetNumber(this);
		}
		UnityEngine.Debug.Log("Trying to call GetNumber on a Non-Number value");
		return 0;
    }

    public UInt32 GetUInt()
    {
		if (IsNumeric())
		{
			return SF_GetUInt(this);
		}
		UnityEngine.Debug.Log("Trying to call GetUInt on a Non-Numeric value");
		return 0;
    }

    public Int32 GetInt()
    {
		if (IsNumeric())
		{
			return SF_GetInt(this);
		}
		UnityEngine.Debug.Log("Trying to call GetInt on a Non-Numeric value");
		return 0;
    }

    public  Boolean GetBool()
    {
		if (IsBoolean())
		{
			return SF_GetBool(this);
		}
		UnityEngine.Debug.Log("Trying to call GetBoolean on a Non-Boolean value");
		return false;
    }

    public String GetString()
    {
		if (IsString())
		{
			String str = Marshal.PtrToStringAnsi(SF_GetString(this));
			return str;
		}
		UnityEngine.Debug.Log("Trying to call GetString on a Non-String value");
		return String.Empty;
    }

	public void SetNumber(double num)
	{
		if (IsNumber())
		{
			SF_SetNumber(this, num);
		}
		else
		{
			UnityEngine.Debug.Log("Trying to call SetNumber on a Non-Number value");
		}
	}

	public void SetUInt(UInt32 uival)
	{
		if (IsNumeric())
		{
			SF_SetUInt(this, uival);
		}
		else
		{
			UnityEngine.Debug.Log("Trying to call SetUInt on a Non-Numeric value");
		}
		
	}

	public void SetInt(Int32 ival)
	{
		if (IsNumeric())
		{
			SF_SetInt(this, ival);
		}
		else
		{
			UnityEngine.Debug.Log("Trying to call SetInt on a Non-Numeric value");
		}
	}

	public void SetBool(Boolean bval)
	{
		if (IsBoolean())
		{
			SF_SetBool(this, bval);
		}
		else
		{
			UnityEngine.Debug.Log("Trying to call SetBool on a Non-Boolean value");
		}
	}

	public void SetString(String str)
	{
		if (IsString())
		{
			SF_SetString(this, Marshal.StringToCoTaskMemAnsi(str));
		}
		else
		{
			UnityEngine.Debug.Log("Trying to call SetString on a Non-String value");
		}
		
	}
			
    public Value GetObject()
    {
        int SFValueSize = Marshal.SizeOf(typeof(Value));
        IntPtr ptr2 = Marshal.AllocCoTaskMem(SFValueSize);
        SF_GetObject(this, ptr2);
        
        // This value will be garbage collected as well.
        Value val = SFManager.GetValueData(ptr2);
        Marshal.FreeCoTaskMem(ptr2);
        return val;
    }
    
    public bool SetMember(String elemName, Value val)
    {
        return SF_SetMember(this, elemName, val);
    }
    
	public bool SetMember(String elemName, String str)
    {
        return SF_SetMember(this, elemName, new Value(str, this.MovieId));
    }
			
	public bool SetMember(String elemName, Boolean bval)
    {
        return SF_SetMember(this, elemName, new Value(bval, this.MovieId));
    }
			
	public bool SetMember(String elemName, int num)
    {
        return SF_SetMember(this, elemName, new Value(num, this.MovieId));
    }
		
	public bool SetMember(String elemName, uint num)
    {
        return SF_SetMember(this, elemName, new Value(num, this.MovieId));
    }
	
	public bool SetMember(String elemName, double num)
    {
        return SF_SetMember(this, elemName, new Value(num, this.MovieId));
    }
			
    public bool GetMember(String elemName, ref Value dest)
    {
        return SF_GetMember(this, elemName, dest);
    }
			
	public Value GetMember(String elemName)
    {
		Value dest = new Value();
        bool res = SF_GetMember(this, elemName, dest);
		if (res)
		{
			return dest;
		}
		else
		{
			return null;
		}
    }
			 				
    public int GetArraySize()
    {
        if (!IsArray()) 
        {
            return -1;
        }
        return SF_GetArraySize(this);
    }

    public bool SetArraySize(UInt32 sz)
    {
        if (!IsArray())
        {
            return false;
        }
        return SF_SetArraySize(this, sz);
    }

    public Value GetElement(UInt32 idx)
    {
		Value dest = new Value();
        if (!IsArray())
        {
			return null;
        }
        bool res = SF_GetElement(this, idx, dest);
		if (res)
		{
			return dest;
		}
		return null;
    }

    public bool SetElement(UInt32 idx, Value val)
    {
        if (!IsArray())
        {
            return false;
        }
        return SF_SetElement(this, idx, val);
    }

    public bool RemoveElement(UInt32 idx)
    {
        if (!IsArray())
        {
            return false;
        }
        return SF_RemoveElement(this, idx);
    }

    public bool ClearElements()
    {
        if (!IsArray())
        {
            return false;
        }
        return SF_ClearElements(this);
    }

    public SFDisplayInfo GetDisplayInfo()
    {
        SFDisplayInfo dinfo = new SFDisplayInfo();
        Int32 size          = Marshal.SizeOf(typeof(SFDisplayInfo));
        bool retVal         = SF_GetDisplayInfo(this, dinfo, size);
        return retVal ? dinfo : null;
    }
    
    public bool SetDisplayInfo(SFDisplayInfo dinfo)
    {
        if (!IsDisplayObject())
        {
            return false;
        }
        Int32 size = Marshal.SizeOf(typeof(SFDisplayInfo));
        return SF_SetDisplayInfo(this, dinfo, size);
    }


    public SFDisplayMatrix GetDisplayMatrix()
    {
        SFDisplayMatrix dmat = new SFDisplayMatrix();
        Int32 size = Marshal.SizeOf(typeof(SFDisplayMatrix));
        bool retVal = SF_GetDisplayMatrix(this, dmat, size);
        return retVal ? dmat : null;
    }

    public bool SetDisplayMatrix(SFDisplayMatrix dmat)
    {
        if (!IsDisplayObject())
        {
            return false;
        }
        Int32 size = Marshal.SizeOf(typeof(SFDisplayMatrix));
        return SF_SetDisplayMatrix(this, dmat, size);
    }
    
    public bool SetColorTransform(SFCxForm cxform)
    {
        if (!IsDisplayObject())
        {
            return false;
        }
        return SF_SetColorTransform(this,  cxform);
    }

    public bool GetColorTransform(ref SFCxForm cxform)
    {
        if (!IsDisplayObject())
        {
            return false;
        }
        return SF_GetColorTransform(this, cxform);
    }

    public bool SetText(String str)
    {
        if (!IsDisplayObject())
        {
            return false;
        }
        return SF_SetText(this, str);
    }

    public bool GetText(ref Value txt)
    {
        if (!IsDisplayObject())
        {
            return false;
        }
        return SF_GetText(this, txt);
    }

    public bool CreateEmptyMovieClip(ref Value dest, String instanceName, Int32 depth)
    {

        return SF_CreateEmptyMovieClip(this, dest, instanceName, depth);
    }
    
    public bool AttachMovie(ref Value dest, String symbolName, String instanceName, Int32 depth)
    {
        return SF_AttachMovie(this, dest, symbolName, instanceName, depth);
    }

	public bool RemoveMovieAS3(Value movieRemoved)
	{
		if (movieRemoved == null) return false;
		Value[] valArray = new Value[] { movieRemoved };
		if (Invoke("removeChild", valArray) == null)
			return false; // problem invoking removeChild
		return true; // all good
	}

	public bool RemoveMovieAS2(Value movieRemoved)
	{
		if (movieRemoved == null) return false;
		if (movieRemoved.Invoke("removeMovie", null) == null)
			return false; // problem invoking removeChild
		return true; // all good
	}

    public bool GotoAndPlayFrame(String frameName)
    {
        if (!IsDisplayObject()) 
        {
            return false;
        }
        return SF_GotoAndPlayFrame(this, frameName);
    }

    public bool GotoAndStopFrame(String frameName)
    {
        if (!IsDisplayObject())
        {
            return false;
        }
        return SF_GotoAndStopFrame(this, frameName);
    }

    public bool GotoAndPlay(Int32 frameNum)
    {
        if (!IsDisplayObject()) 
        {
            return false;
        }
        return SF_GotoAndPlay(this, frameNum);
    }

    public bool GotoAndStop(Int32 frameNum)
    {
        if (!IsDisplayObject()) 
        {
            return false;
        }
        return SF_GotoAndStop(this, frameNum);
    }	
			
	public Value CreateValue(object o)
	{
		if(o is int)
		{
			return new Value((int)o, MovieId);	
		}
		else if((o is float) || (o is double))
		{
			return new Value((double)o, MovieId);			
		}
		else if(o is String)
		{
			return new Value((String)o, MovieId);			
		}
		else if(o is bool)
		{
			return new Value((bool)o, MovieId);			
		}
				
		return new Value(0, MovieId);
	}

	public IntPtr Serialize(params object[] objs)
	{
		int valueSize	= Marshal.SizeOf(typeof(Value));
		int IntPtrSize	= Marshal.SizeOf(typeof(IntPtr));
		int IntSize		= Marshal.SizeOf(typeof(int));
		int numElem		= objs.Length;
		IntPtr ptr		= IntPtr.Zero;

		if (numElem > 0)
		{
			ptr = Marshal.AllocCoTaskMem(valueSize * numElem);
			for (int i = 0; i < numElem; i++)
			{
				Value val;

				object currentObj = objs[i];
				if (!(currentObj is Value))
				{
					val = CreateValue(currentObj);
				}
				else
				{
					val = (Value)currentObj;
				}

				// Can't add an integer offset to IntPtr as you would with C/C++ pointer 
				IntPtr data = new IntPtr(ptr.ToInt32() + valueSize * i);
				Marshal.WriteIntPtr(data, val.pInternalData);
				data = new IntPtr(data.ToInt32() + IntPtrSize);
				Marshal.WriteInt32(data, (int)val.Type);
				data = new IntPtr(data.ToInt32() + IntSize);
				Marshal.WriteInt64(data, (long)val.MovieId);
			}
		}
		return ptr;
	}
		
	public Value Invoke(String methodName, params object[] objs)
    {
        
		int numElem		= objs.Length;
		Value retVal	= new Value();
		IntPtr ptr = Serialize(objs);
        bool result = SF_Invoke2(this, methodName, numElem, ptr, retVal);
        Marshal.FreeCoTaskMem(ptr);
		if (result)
			return retVal; // indicates that method was successfully invoked, but doesn't return anything
		else
			return null; // indicates there was a problem invoking the method. 
    
    }		

	public object ConvertFromASObject(Type objectType)
	{
		if (!IsObject())
			return null;

		// Create a new C# object of the desired type
		object result = Activator.CreateInstance(objectType);
		if(null == result)
		{
			return null;
		}

		// Convert AS object members based on C# type definition
		foreach (var propInfo in objectType.GetProperties())
		{
			if (!propInfo.CanWrite)
			{
				continue;
			}
			UnityEngine.Debug.Log(propInfo.PropertyType);
			// Get the corresponding object member from ActionScript
			Value propertyValue = GetMember(propInfo.Name);
			if (propertyValue == null)
			{
				continue;
			}

			object csValue = null;
			if (propInfo.PropertyType == typeof(System.Int32))
			{
				csValue = propertyValue.GetInt();
			}
			else if(propInfo.PropertyType == typeof(String))
			{
				csValue = propertyValue.GetString();
			}
			else if (propInfo.PropertyType == typeof(System.UInt32))
			{
				csValue = propertyValue.GetUInt();
			}
			else if (propInfo.PropertyType == typeof(Double))
			{
				csValue = propertyValue.GetNumber();
			}
			else if (propInfo.PropertyType == typeof(System.Single)) // float
			{
				csValue = (float)propertyValue.GetNumber();
			}
			else if (propInfo.PropertyType == typeof(System.Boolean))
			{
				csValue = propertyValue.GetBool();
			}
			else
			{
				if(!propInfo.PropertyType.IsPrimitive )
				{
					Value objectVal = propertyValue.GetObject();
					csValue = objectVal.ConvertFromASObject(propInfo.PropertyType);
				}
				else
				{
					String info = String.Format("Trying to convert a not handled managed type{0}!", propInfo.PropertyType.Name); 
					UnityEngine.Debug.Log(info);
				}
			}
					
			propInfo.SetValue(result, csValue, null);

		}
		return result; 
	}

	public static Value ConvertToASObject(object obj, Movie mv)
	{
		// Step1: Get object type
		System.Type tp = obj.GetType();
		UnityEngine.Debug.Log(String.Format ("Type of CS object to convert: {0} \n", tp.Name));
		// Step 2: Create AS object of the correct type using CreateObject (defined in SFMovie for now)
		// The ActionScript class must have a default constructor that accepts no arguments, otherwise we will
		// get error# 1063
		Value result = mv.CreateObject(tp.Name);
		if(null == result || !result.IsValid())
		{
			return null;
		}
		// Step 3: Iterate over the properties of obj and fill in the corresponding members of the ASObject, if 
		// the members exist
		// Also think about nested objects etc. 
		foreach (var propInfo in tp.GetProperties())
		{
			UnityEngine.Debug.Log(propInfo.PropertyType);
					
			Value propertyValue = result.GetMember(propInfo.Name);
			if (propertyValue == null)
			{
				continue;
			}
					
			if (propInfo.PropertyType == typeof(System.Int32))
			{
				result.SetMember(propInfo.Name, (Int32)propInfo.GetValue(obj, null));
			}
			else if (propInfo.PropertyType == typeof(String))
			{
				result.SetMember(propInfo.Name, (String)propInfo.GetValue(obj, null));
			}
			else if (propInfo.PropertyType == typeof(System.UInt32))
			{
				result.SetMember(propInfo.Name, propertyValue);		
			}
			else if (propInfo.PropertyType == typeof(Double))
			{
				result.SetMember(propInfo.Name, (Double)propInfo.GetValue(obj, null));
			}
			else if (propInfo.PropertyType == typeof(System.Single)) // float
			{
				result.SetMember(propInfo.Name, (Single)propInfo.GetValue(obj, null));
			}
			else if (propInfo.PropertyType == typeof(System.Boolean))
			{
				result.SetMember(propInfo.Name, (Boolean)propInfo.GetValue(obj, null));						
			}
			else
			{
				if(!propInfo.PropertyType.IsPrimitive )
				{
					propertyValue = ConvertToASObject(propInfo.GetValue(obj, null), mv);
					result.SetMember(propInfo.Name, propertyValue);
				}
				else
				{
					UnityEngine.Debug.Log(String.Format("Trying to convert a not handled managed type{0}!", propInfo.PropertyType.Name));
				}
			}
		}
		
		return result;
	}

						
	/*		
    public Value Invoke(String methodName, Value[] valArray)
    {

        int valueSize   = Marshal.SizeOf(typeof(Value));
        int IntPtrSize  = Marshal.SizeOf(typeof(IntPtr));
        int IntSize     = Marshal.SizeOf(typeof(int));
		int numElem		= 0;
		IntPtr	ptr		= IntPtr.Zero;
		Value retVal	= new Value();
		if (valArray != null)
		{
			numElem = valArray.GetLength(0);
			ptr = Marshal.AllocCoTaskMem(valueSize * numElem);
			for (int i = 0; i < numElem; i++)
			{
				// Can't add an integer offset to IntPtr as you would with C/C++ pointer 
				IntPtr data = new IntPtr(ptr.ToInt32() + valueSize * i);
				Marshal.WriteIntPtr(data, valArray[i].pInternalData);
				data = new IntPtr(data.ToInt32() + IntPtrSize);
				Marshal.WriteInt32(data, (int)valArray[i].Type);
				data = new IntPtr(data.ToInt32() + IntSize);
				Marshal.WriteInt64(data, (long)valArray[i].MovieId);
			}
		}
		
        
        bool result = SF_Invoke2(this, methodName, numElem, ptr, retVal);
        Marshal.FreeCoTaskMem(ptr);
		if (result)
			return retVal; // indicates that method was successfully invoked, but doesn't return anything
		else
			return null; // indicates there was a problem invoking the method. 
    
    }*/
    
    ~Value()
    {
        if (pInternalData != IntPtr.Zero)
        {
			SFManager.AddValueToReleaseList(pInternalData);
        //    SF_DecrementValRefCount(pInternalData);
			
        }
    }
}

} // namespace GFx;

} // namespace Scaleform;
 