/**********************************************************************

Filename    :	SFMovie.cs
Content     :	Movie Wrapper
Created     :   
Authors     :   Ankur Mohan

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/

using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace Scaleform
{
namespace GFx
{

[StructLayout(LayoutKind.Explicit,CharSet=CharSet.Ansi)]
class test
{
    public test(int _a, int _b)
    {a = _a; b = _b;
    }
    [FieldOffset(0), MarshalAs( UnmanagedType.I4)] public int a;
    [FieldOffset(4), MarshalAs( UnmanagedType.I4)]public int b;
}

enum SFUnityErrorCodes : int
{
	Success = 0,
	IncompatibleRenderer,
	NullManager
};

public enum HitTestType : int
{
	HitTest_Bounds = 0,
	HitTest_Shapes = 1,
	HitTest_ButtonEvents = 2,
	HitTest_ShapesNoInvisible = 3
};

public partial class Movie: System.Object
{

    public    String        MovieName;
    public    long          MovieID;
    public    Boolean       MarkForRelease;
    public    SFManager     SFMgr;
    public    bool          IsFocused;
    public    bool          AdvanceWhenGamePaused;
    public    bool          IsAutoManageViewport;
	// Currently used on Android to read swf file data in an unmanaged memory buffer and pass it to Scaleform runtime
	public	  IntPtr		pDataUnmanaged;
	public    ScaleModeType TheScaleModeType;
    public    SFRTT         mRenderTexture;
    struct _ViewPort
    {
        public int OX;
        public int OY;
        public int Width;
        public int Height;
    } 
    _ViewPort ViewPort;

    public Movie()
    {
        MovieID = 0;
    }

    public Movie(SFManager sfmgr, SFMovieCreationParams creationParams)
    {
        if (sfmgr == null)
        {
            MovieID = 0;
            return;
        }
        
        MovieName                   = creationParams.MovieName;
        ViewPort.OX                 = creationParams.OX;
        ViewPort.OY                 = creationParams.OY;
        ViewPort.Width              = creationParams.Width;
        ViewPort.Height             = creationParams.Height;
		pDataUnmanaged				= creationParams.pData; 
        MovieID                     = 0; // Assigned when the C++ Movie is created. 
        MarkForRelease              = false;
        SFMgr                       = sfmgr;
        IsFocused                  	= false;
        AdvanceWhenGamePaused      	= false;
        IsAutoManageViewport        = creationParams.IsAutoManageViewport;
		TheScaleModeType			= creationParams.TheScaleModeType;

		int cpSize = Marshal.SizeOf(typeof(SFMovieCreationParams));

		IntPtr pdata = Marshal.AllocCoTaskMem(cpSize);
		Marshal.StructureToPtr(creationParams, pdata, false);

        MovieID = SF_CreateMovie(pdata);
		Marshal.DestroyStructure(pdata, typeof(SFMovieCreationParams));
		if (MovieID != -1)
		{
			SFMgr.AddMovie(this);
		}
    }
    
    // Important: We can't destroy the underlying C++ Movie object in finalize method, because this Finalize
    // can get called from the garbage collector thread, and we can only destroy C++ Movie objects from
    // the main thread. Therefore, we inform the SFManager that this movie is to be deleted, and it's put on a
    // release queue. This queue is cleared (see ReleaseMoviesMarkedForRelease method on the SFMAnager) during
    // the update function, which is invoked on the main thread. 
    ~Movie()
    {
        Console.WriteLine("In Movie Destructor. ID = " + MovieID);
        if (MovieID != 0)
        {
            MarkForRelease = true;
            if (SFMgr != null)
            {
                SFMgr.AddToReleaseList(MovieID);
            }
            MovieID = -1;
        }
		Marshal.FreeCoTaskMem(pDataUnmanaged);
    }

	public Value CreateValue(String t)
	{
		return new Value(t, MovieID);
	}

	public Value CreateValue(Boolean t)
	{
		return new Value(t, MovieID);
	}

	public Value CreateValue(Double t)
	{
		return new Value(t, MovieID);
	}

	public Value CreateValue(UInt32 t)
	{
		return new Value(t, MovieID);
	}

	public Value CreateValue(Int32 t)
	{
		return new Value(t, MovieID);
	}
			
	public Value CreateValue(object o)
	{
		if(o is int)
		{
			return new Value((int)o, MovieID);	
		}
		else if(o is float)
		{
			//Error: float => object X=> Double. 
			//Good: float => object => Single => Double
			Single s = (Single)o;
			return new Value((Double)s, MovieID);			
		}
		else if(o is double)
		{
			return new Value((Double)o, MovieID);		
		}
		else if(o is String)
		{
			return new Value((String)o, MovieID);			
		}
		else if(o is bool)
		{
			return new Value((bool)o, MovieID);			
		}
				
		return new Value(0, MovieID);
	}
		
    public long GetID() 
    { 
        return MovieID; 
    }
    
    public void SetID(long id)
    {
        MovieID = id; 
    }
    
    public void OnSFManagerDied()
    {
        // The SFManager just died, set our reference to null. 
        SFMgr = null;
    }
    
    public void SetViewport(int ox, int oy, int width, int height)
    {
        ViewPort.OX = ox; ViewPort.OY = oy; ViewPort.Width = width;
        ViewPort.Height = height;
    }
    
    public virtual void Update()
    {
        // Override in derived class for movie specific update actions
    }
    
    public virtual  void Advance(float deltaTime)
    {
        if (MovieID == -1) return;
        if (!MarkForRelease)
        {
			int errCode; 
            if (AdvanceWhenGamePaused)
            {
                // Advance the movie automatically.
               errCode = SF_Advance(MovieID, 0, true);
            }
            else
            {
                errCode = SF_Advance(MovieID, deltaTime);
            }
			if ((SFUnityErrorCodes)(errCode) == SFUnityErrorCodes.IncompatibleRenderer)
			{
				Debug.Log("Unity and Scaleform Plug-in are using incompatible renderer type");
			}
        }
    }

    public bool DoHitTest(float x, float y, HitTestType hitTestType)
    {
        if (MovieID == -1 || mRenderTexture == null)
        {
            return false;
        }

        // Adjust according to viewport
        if (mRenderTexture)
        {
            RaycastHit hit;
            if (Camera.main!=null && Physics.Raycast(Camera.main.ScreenPointToRay(new Vector2(x, y)), out hit))
            {
                Renderer hitRenderer = hit.collider.renderer;
                MeshCollider meshCollider = hit.collider as MeshCollider;
                if (!(hit.collider is MeshCollider) || hitRenderer == null || meshCollider == null)
                {
                    return false;
                }

                if (hit.collider.gameObject.GetComponent("SFMovie") == null)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        return SF_DoHitTest(MovieID, x, y, (int)hitTestType);
    }

    public void SetFocus(bool focus)
    {
        if (MovieID == -1)
        {
            return;
        }
        IsFocused = focus;
        SF_SetFocus(MovieID, focus);
    }

    public void SetRTT(SFRTT rtt)
    {
        mRenderTexture = rtt;
    }

    public SFRTT GetRTT() { return mRenderTexture; }

    public bool IsRTTMovie()
    {
        return mRenderTexture == null ? false : true;
    }
    public virtual bool AcceptKeyEvents()
    {
        // Can check for various conditions here. Derived classes can override this function as well
        return IsFocused;
    }

    public virtual bool AcceptCharEvents()
    {
        // Can check for various conditions here. Derived classes can override this function as well
        return IsFocused;
    }
    
    public virtual bool AcceptMouseEvents()
    {
        // Can check for various conditions here. Derived classes can override this function as well
        // Check if the mouse event is over the movie viewport..
        return true;
    }

	public virtual bool AcceptTouchEvents()
	{
		// Can check for various conditions here. Derived classes can override this function as well
		// Check if the mouse event is over the movie viewport..
		return true;
	}
    
    public bool HandleMouseEvent(float x, float y, int icase, int buttonType)
    {
        if (MovieID == -1)
        {
            return false;
        }
        
        if (AcceptMouseEvents())
        {
            // Adjust according to viewport
            float xx = x;
            float yy = y;
            if (mRenderTexture != null && Camera.main != null)
            {
				RaycastHit hit;
        		if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector2(x, Screen.height - y)), out hit))
				{
          			Renderer hitRenderer = hit.collider.gameObject.renderer;
                    MeshCollider meshCollider = hit.collider as MeshCollider;
                    if (!(hit.collider is MeshCollider) || hitRenderer == null || meshCollider == null)
                    {
                        return false;
                    }

                    SFRTT rtt = hit.collider.gameObject.GetComponent("SFRTT") as SFRTT;
                    if (rtt == null)
                    {
                        return false;
                    }
	                float rttWidth 	= hitRenderer.material.mainTexture.width;
	                float rttHeight = hitRenderer.material.mainTexture.height;
	                float hitCoordX = hit.textureCoord.x * rttWidth;
	                float hitCoordY = rttHeight - hit.textureCoord.y * rttHeight;
	                xx = hitCoordX;
	                yy = hitCoordY;
                    yy -= 32; // HACK
				}
            }
            else
            {
                xx -= ViewPort.OX;
                yy -= ViewPort.OY;
            }

            if (SF_HandleMouseEvent(MovieID, xx, yy, icase, buttonType))
            {
                UnityEngine.Debug.Log("Handle mouse event");
                return true;
            }
        }
        return false;
    }

    public bool HandleKeyEvent(SFKey.Code cd, SFKeyModifiers.Modifiers mod, UInt32 down, int keyboardIndex = 0)
    {
        if (MovieID == -1)
        {
            return false;
        }
        if (AcceptKeyEvents())
        {
            return SF_HandleKeyEvent(MovieID, cd, mod, down, keyboardIndex);
        }
        return false;
    }
    
    public bool HandleCharEvent(UInt32 wchar)
    {
        if (MovieID == -1)
        {
            return false;
        }
        if (AcceptCharEvents())
        {
			if (wchar == 10) wchar = 13;
            return SF_HandleCharEvent(MovieID, wchar);
        }
        return false;
    }
    
    public bool HandleTouchEvent(int fingerId, float x, float y, int icase)
    {
		if (AcceptTouchEvents())
        {
            // Adjust according to viewport
            float xx = x - ViewPort.OX;
    	   	float yy = y; 
            return SF_HandleTouchEvent(MovieID, fingerId, xx, yy, icase);
        }
        return false;
    }
    
    public void SetBackgroundAlpha(float alpha)
    {
        if (MovieID == -1)
        {
            return;
        }
        SF_SetBackgroundAlpha(MovieID, alpha);
    }

	public IntPtr Serialize(params object[] objs)
	{
		int valueSize = Marshal.SizeOf(typeof(Value));
		int IntPtrSize = Marshal.SizeOf(typeof(IntPtr));
		int IntSize = Marshal.SizeOf(typeof(int));
		int numElem = objs.Length;
		IntPtr ptr = IntPtr.Zero;

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

	public Value CreateObject(String className, params object[] objs)
    {
		Value pvalManaged = new Value();
		int numElem = objs.Length;
		IntPtr ptr = Serialize(objs);
        SF_CreateObject(MovieID, pvalManaged, className, numElem, ptr);
        Marshal.FreeCoTaskMem(ptr);
        return pvalManaged;
    }
			
	public Value CreateArray(String typeName)
	{
		Value pvalManaged = new Value();
        if(SF_CreateArray(MovieID, pvalManaged))
		{
			return pvalManaged;
		}
				
		return null;
	}
			
	public Value Invoke(String methodName, params object[] objs)
    {
		int numElem = objs.Length;
		IntPtr ptr = Serialize(objs);
		Value retVal = new Value();

        bool result = SF_Invoke3(MovieID, methodName, numElem, ptr, retVal);
        Marshal.FreeCoTaskMem(ptr);
		if (result)
			return retVal; // indicates that method was successfully invoked, but doesn't return anything
		else
			return null; // indicates there was a problem invoking the method.
    }		
}

} // namespace GFx;

} // namespace Scaleform;
