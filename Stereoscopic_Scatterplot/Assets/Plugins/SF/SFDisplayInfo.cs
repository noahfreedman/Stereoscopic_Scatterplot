/**********************************************************************

Filename    :	SFDisplayInfo.cs
Content     :	Definition of DisplayInfo Wrapper
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
using System.Runtime.InteropServices;

namespace Scaleform
{

public class Matrix3F: System.Object
{
    //float [,] =  mat[3][3];
}

public class Matrix4F: System.Object
{
    //float mat[4][3];
}

public enum EdgeAAMode
{
    EdgeAA_Inherit = 0x0000,    // Take EdgeAA setting from parent; On by default.
    EdgeAA_On      = 0x0004,    // Use EdgeAA for this node and its children, unless disabled.
    EdgeAA_Off     = 0x0008,    // Do not use EdgeAA for this node or its children.
    EdgeAA_Disable = 0x000C     // Disable EdgeAA for this node and subtree, overriding On settings.
}

// For a class or pointer to class to be passed to unmanaged code, it must have
// StructLayout Attribute.
[StructLayout(LayoutKind.Sequential)]
public class  SFDisplayInfo: System.Object
{
    
    public double              	X;
    public double              	Y;
    public double           	Rotation;
    public double           	XScale;
    public double           	YScale;
    public double           	Alpha;
    public double           	Z;
    public double           	XRotation;
    public double           	YRotation;
    public double           	ZScale;
    public double           	FOV;
    public IntPtr				ViewMatrix3DPtr;
    public IntPtr				ProjectionMatrix3DPtr;
    public EdgeAAMode  			EdgeAAmode;
    public bool                	Visible;

    public SFDisplayInfo()
    {
        // Allocate space on the COM heap. Should also be able to use AllocHGlobal in order to 
        // allocate space on process heap.
        int floatSize = Marshal.SizeOf(typeof(float));
        ViewMatrix3DPtr         = Marshal.AllocCoTaskMem(4 * 3 * floatSize);
        ProjectionMatrix3DPtr   = Marshal.AllocCoTaskMem(4 * 4 * floatSize);
    }

    ~SFDisplayInfo()
    {
        Marshal.FreeCoTaskMem(ViewMatrix3DPtr);
        Marshal.FreeCoTaskMem(ProjectionMatrix3DPtr);
    }
    
    public void Print()
    {
        Debug.Log("X = " + X);
        Debug.Log("Y = " + Y);
        Debug.Log("Rotation = " + Rotation);
        Debug.Log("XScale = " + XScale);
        Debug.Log("YScale = " + YScale);
        Debug.Log("Alpha = " + Alpha);
        Debug.Log("Z = " + Z);
        Debug.Log("FOV = " + FOV);
        Debug.Log("Visible = " + Visible);
        
        PrintViewMatrix();
        PrintProjectionMatrix();
    }
    
    public void PrintViewMatrix()
    {
        Debug.Log("ViewMatrix:");
        float m00 = GetViewMatrixElem(0,0); float m01 = GetViewMatrixElem(0,1);
        float m02 = GetViewMatrixElem(0,2); float m03 = GetViewMatrixElem(0,3);
        float m10 = GetViewMatrixElem(1,0); float m11 = GetViewMatrixElem(1,1);
        float m12 = GetViewMatrixElem(1,2); float m13 = GetViewMatrixElem(1,3);
        float m20 = GetViewMatrixElem(2,0); float m21 = GetViewMatrixElem(2,1);
        float m22 = GetViewMatrixElem(2,2); float m23 = GetViewMatrixElem(2,3);
        
        Debug.Log(m00 + " " + m01 + " " + m02 + " " + m03);
        Debug.Log(m10 + " " + m11 + " " + m12 + " " + m13);
        Debug.Log(m20 + " " + m21 + " " + m22 + " " + m23);
    }
    
    public void PrintProjectionMatrix()
    {
        Debug.Log("Projection" + "Matrix:");
        float m00 = GetProjectionMatrixElem(0,0); float m01 = GetProjectionMatrixElem(0,1);
        float m02 = GetProjectionMatrixElem(0,2); float m03 = GetProjectionMatrixElem(0,3);
        float m10 = GetProjectionMatrixElem(1,0); float m11 = GetProjectionMatrixElem(1,1);
        float m12 = GetProjectionMatrixElem(1,2); float m13 = GetProjectionMatrixElem(1,3);
        float m20 = GetProjectionMatrixElem(2,0); float m21 = GetProjectionMatrixElem(2,1);
        float m22 = GetProjectionMatrixElem(2,2); float m23 = GetProjectionMatrixElem(2,3);
        float m30 = GetProjectionMatrixElem(2,0); float m31 = GetProjectionMatrixElem(2,1);
        float m32 = GetProjectionMatrixElem(2,2); float m33 = GetProjectionMatrixElem(2,3);
        
        Debug.Log(m00 + " " + m01 + " " + m02 + " " + m03);
        Debug.Log(m10 + " " + m11 + " " + m12 + " " + m13);
        Debug.Log(m20 + " " + m21 + " " + m22 + " " + m23);
        Debug.Log(m30 + " " + m31 + " " + m32 + " " + m33);
    }
    
    // Accessors for view matrix and projection matrices
    public float GetViewMatrixElem(int row, int col)
    {
        int numRows     = 3;
        int numCols     = 4;
        if (row < 0 || row >= numRows || col < 0 || col >= numCols) return -1;
        
        float[] retVal = new float[1];
        IntPtr vmptr    = ViewMatrix3DPtr;
        int floatSize   = Marshal.SizeOf(typeof(float));
        IntPtr psrc     = new IntPtr(vmptr.ToInt32() + floatSize*(row*numCols + col));
        Marshal.Copy(psrc, retVal, 0, 1);
        return retVal[0];
    }
    
    public float GetProjectionMatrixElem(int row, int col)
    {
        int numRows     = 4;
        int numCols     = 4;
        if (row < 0 || row >= numRows || col < 0 || col >= numCols) return -1;
        
        float[] retVal = new float[1];
    
        IntPtr pmptr    = ProjectionMatrix3DPtr;
        int floatSize   = Marshal.SizeOf(typeof(float));
        IntPtr psrc     = new IntPtr(pmptr.ToInt32() + floatSize*(row*numCols + col));
        Marshal.Copy(psrc, retVal, 0, 1);
        return retVal[0];
    }
}
}


 