/**********************************************************************

Filename    :	SFCxForm.cs
Content     :	Definition of ColorTransform Wrapper
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
    
// For a class or pointer to class to be passed to unmanaged code, it must have
// StructLayout Attribute.
[StructLayout(LayoutKind.Sequential)]
public class  SFCxForm: System.Object
{
    public IntPtr M;
    
    public SFCxForm()
    {
        int doubleSize = Marshal.SizeOf(typeof(Double));
        M = Marshal.AllocCoTaskMem(8 * doubleSize);
    }

    public SFCxForm(Color color)
    {
        SetElem(0, 0); 
        SetElem(1, 0); 
        SetElem(2, 0); 
        SetElem(3, 0);
        SetElem(4, color.r / 255); 
        SetElem(5, color.g / 255);
        SetElem(6, color.b / 255); 
        SetElem(7, color.a / 255); 
    }
    
    ~SFCxForm()
    {
        Marshal.FreeCoTaskMem(M);
    }
    
   public void SetColor(Color color)
   {
        SetElem(0, 0); 
        SetElem(1, 0); 
        SetElem(2, 0); 
        SetElem(3, 0);
        SetElem(4, color.r); 
        SetElem(5, color.g);
        SetElem(6, color.b); 
        SetElem(7, color.a); 
   }

    public Double GetElem(int idx)
    {
        if (idx < 0 || idx > 7)
        {
            return -1;
        }

        Double[] retVal = new Double[1];
        int doubleSize = Marshal.SizeOf(typeof(Double));
        IntPtr psrc = new IntPtr(M.ToInt32() + doubleSize * idx);
        Marshal.Copy(psrc, retVal, 0, 1);
        
        return retVal[0];
    }

    public bool SetElem(int idx, Double val)
    {
        if (idx < 0 || idx > 7)
        {
            return false;
        }

        double[] dval = new double[1];
        dval[0] = val;
        
        int doubleSize = Marshal.SizeOf(typeof(Double));
        IntPtr psrc = new IntPtr(M.ToInt32() + doubleSize * idx);
        
        Marshal.Copy(dval, 0, psrc, 1);
        return true;
    }
    
    public void Print()
    {
        Console.WriteLine("CxForm = " + GetElem(0) + " " + GetElem(1) + " " + GetElem(2) + " " + GetElem(3)
                                    + " " + GetElem(4) + " " + GetElem(5) + " " + GetElem(6) + " " + GetElem(7));
    }
}

} // namespace Scaleform;


 