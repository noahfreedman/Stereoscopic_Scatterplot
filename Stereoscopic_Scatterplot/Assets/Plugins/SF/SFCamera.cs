/**********************************************************************

Filename    :   SFCamera.cs
Content     :   Creates ScaleformManager, Hooks into Unity event handling system
Created     :   
Authors     :   Ankur Mohan

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/
using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using Scaleform;

// Note that SFCamera is currently a Singleton as it creates a new SFMgr in Start().
public class SFCamera : MonoBehaviour
{
    public enum RenderTime
    {
        EndOfFrame = 0,
        PreCamera,
        PostCamera
    };
    
    // When to trigger Scaleform rendering during the frame.
    public RenderTime WhenToRender = RenderTime.EndOfFrame;
    
    // true if the SFCamera has been initialized; false otherwise.
    public bool Initialized = false;
	public RenderTexture RenderableTexture = null;
	public bool UseBackgroundColor = false;
	public Color32 BackgroundColor = new Color32(0, 0, 0, 255);
    
    // The mouse position during the last onGUI() call. Used for MouseMove tracking.
    protected Vector2 LastMousePos;
    // Reference to the SFManager that manages all SFMovies.
    protected static SFManager SFMgr;

    protected static bool InitOnce = false;

    protected SFGamepad GamePad;

    protected bool      GamepadConnected;

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
    [DllImport("libgfxunity3d")]
    private static extern void SF_Uninit();
    
    [DllImport("libgfxunity3d")]
    public static extern void SF_SetKey(String key);
#elif UNITY_IPHONE
    [DllImport("__Internal")]
    public static extern void UnityRenderEvent(int id);

    [DllImport("__Internal")]
    private static extern void SF_Uninit();
    
    [DllImport("__Internal")]
    public static extern void SF_SetKey(String key);
#elif UNITY_ANDROID
    [DllImport("gfxunity3d")]
    public static extern void UnityRenderEvent(int id);

    [DllImport("gfxunity3d")]
    private static extern void SF_Uninit();
    
    [DllImport("gfxunity3d")]
    public static extern void SF_SetKey(String key);    
#endif

    public SFInitParams InitParams;
    public void Awake()
    {
        //  Mark DontDestroyOnload so that this object is not destroyed when MainMenu level is unloaded and Game Level is loaded.

        //  if (InitOnce == false)
        {

            //      InitOnce = true;
        }
    }
    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SFMgr = new SFManager(InitParams);
        if (SFMgr.IsSFInitialized())
        {
            GamePad = new SFGamepad(SFMgr);
            GamePad.Init();
            //SFMgr.InstallDelegates();
            InitParams.Print();
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
            GL.IssuePluginEvent(0);
#elif UNITY_IPHONE  || UNITY_ANDROID
            UnityRenderEvent(0);
#endif
            GL.InvalidateState();
        }
        
        // Figure out if gamepad is connected.
        GamepadConnected = false;
        if (Input.GetJoystickNames().Length != 0)
        {
            GamepadConnected = true;
        }
    }

    // Issues a call to perform Scaleform rendering. Rendering is multithreaded on windows and single threaded on iOS/Android
    private void PumpPluginRender()
    {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
        GL.IssuePluginEvent(1);
#elif UNITY_IPHONE  || UNITY_ANDROID
        UnityRenderEvent(1);
#endif
    }
    
    // Used with EndOfFrame render layers, pumps Scaleform once per frame at the end of the frame
    protected IEnumerator CallPluginAtEndOfFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            PumpPluginRender();
        }
    }
    
    // Used with PreCamera render layers, pumps Scaleform once prior to the Camera instance rendering its contents
    public void OnPreRender()
    {
        if (WhenToRender != RenderTime.PreCamera)
        {
            return;
        }
        PumpPluginRender();
    }
    
    // Used with PostCamera render layers, pumps Scaleform once after the Camera instance renders its contents
    public void OnPostRender()
    {
        if (WhenToRender != RenderTime.PostCamera)
        {
            return;
        }
        PumpPluginRender();
    }
    
    void OnDestroy()
    {
        // This is used to initiate RenderHALShutdown, which must take place on the render thread. 
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
        GL.IssuePluginEvent(2);
#endif
        SF_Uninit();
    }

    void OnApplicationQuit()
    {
/*
        // This is used to initiate RenderHALShutdown, which must take place on the render thread. 
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
        GL.IssuePluginEvent(2);
#endif
        SF_Uninit();
 */
    }

    public SFManager GetSFManager()
    {
        return SFMgr;
    }

    // Hook into Unity Events
    public virtual void OnGUI()
    {
        if (SFMgr == null) return;

        
        // Process touch events:
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                SFMgr.HandleTouchEvent(touch);
            }
        }

        Vector2 mousePos;

        // Get Time since last frame: keep in mind the following from Unity Doc
        // Note that you should not rely on Time.deltaTime from inside OnGUI since 
        // OnGUI can be called multiple times per frame and deltaTime would hold the 
        // same value each call, until next frame where it would be updated again.
        // float deltaTime = Time.deltaTime;
        // Check if the mouse moved:

        mousePos = Event.current.mousePosition;
        if ((mousePos[0] != LastMousePos[0]) || (mousePos[1] != LastMousePos[1]))
        {
            SFMgr.HandleMouseMoveEvent(mousePos[0], mousePos[1]);
            LastMousePos[0] = mousePos[0]; LastMousePos[1] = mousePos[1];
        }

        switch (Event.current.type)
        {
            case EventType.MouseMove:
                break;
            case EventType.MouseDown:
                SFMgr.HandleMouseEvent(Event.current);
                break;
            case EventType.MouseUp:
                SFMgr.HandleMouseEvent(Event.current);
                break;
            case EventType.KeyDown:

                if (Event.current.isKey && Event.current.keyCode != KeyCode.None)
                {
                    SFMgr.HandleKeyDownEvent(Event.current);
                }
                
                if (Event.current.isKey && Event.current.character != 0)
                {
                    SFMgr.HandleCharEvent(Event.current);
                }
                break;
            case EventType.KeyUp:
                if (Event.current.isKey && Event.current.keyCode != KeyCode.None)
                {
                    SFMgr.HandleKeyUpEvent(Event.current);
                }
                break;
                
            case EventType.Repaint:
                break;
        }
    }

    public void Update()
    {
        if (SFMgr != null)
        {
            // Update gamepad only if it's connected, otherwise we might get duplicate key presses for WASD, etc. 
            if (GamepadConnected)
            {
                GamePad.Update(); 
            }
            SFMgr.ProcessCommands();
            SFMgr.Update();
            SFMgr.Advance(Time.deltaTime);
        }
    }

    public static void GetViewport(ref int ox, ref int oy, ref int width, ref int height)
    {
        width = Screen.width;
        height = Screen.height;
        ox = 0;
        oy = 0;
        // Note that while using D3D renderer, the tool bar (that contains "Maximize on Play" text) is part of 
        // the viewport, while using GL renderer, it's not. So there should be a further condition here depending on 
        // what type of renderer is being used, however I couldn't find a macro for achieving that. 
#if UNITY_EDITOR 
        oy = 24;
#endif
    }

    public static SFMovieCreationParams CreateRTTMovieCreationParams(string swfName, int RTToX, int RTToY, RenderTexture texture, Color32 clearColor)
    {
        // Used for Android only
        Int32 length = 0;
		IntPtr pDataUnManaged = IntPtr.Zero;
        String SwfPath = SFManager.GetScaleformContentPath() + swfName;

		return new SFMovieCreationParams(SwfPath, RTToX, RTToY, texture.width, texture.height, pDataUnManaged, length, false, texture, clearColor, true, ScaleModeType.SM_ShowAll, true);
    }

    public static SFMovieCreationParams CreateMovieCreationParams(string swfName)
    {
        return CreateMovieCreationParams(swfName, new Color32(0, 0, 0, 0), false);
    }

    public static SFMovieCreationParams CreateMovieCreationParams(string swfName, byte bgAlpha)
    {
        return CreateMovieCreationParams(swfName, new Color32(0, 0, 0, bgAlpha), false);
    }

    public static SFMovieCreationParams CreateMovieCreationParams(string swfName, Color32 bgColor, bool overrideBackgroundColor)
    {
    
        Int32 length = 0;

        int ox = 0;
        int oy = 0;
        int width = 0;
        int height = 0;
        
        IntPtr pDataUnManaged = IntPtr.Zero;
        String swfPath = SFManager.GetScaleformContentPath() + swfName;
        GetViewport(ref ox, ref oy, ref width, ref height);
		return new SFMovieCreationParams(swfPath, ox, oy, width, height, pDataUnManaged, length, false, bgColor, overrideBackgroundColor, ScaleModeType.SM_ShowAll, true);
    }

    public SFMovieCreationParams CreateMovieCreationParams(string swfName, Byte[] swfBytes, Color32 bgColor, bool overrideBackgroundColor)
    {
        int ox = 0;
        int oy = 0;
        int width = 0;
        int height = 0;
        
        GetViewport(ref ox, ref oy, ref width, ref height);
        
        Int32 length = 0;
		IntPtr pDataUnManaged = IntPtr.Zero;

		if (swfBytes != null)
			 length = swfBytes.Length;

		if (length > 0)
		{
			pDataUnManaged = new IntPtr();
			pDataUnManaged = Marshal.AllocCoTaskMem((int)length);
			Marshal.Copy(swfBytes, 0, pDataUnManaged, (int)length);	
		}

		String swfPath = SFManager.GetScaleformContentPath() + swfName;

		return new SFMovieCreationParams(swfPath, ox, oy, width, height, pDataUnManaged, length, false, bgColor, overrideBackgroundColor, ScaleModeType.SM_ShowAll, true);
    }

    protected static void GetFileInformation(String name, ref long length, ref IntPtr pDataUnManaged)
    {
#if UNITY_ANDROID
		
		long start;
		Int32 fd;
        IntPtr cls_Activity = (IntPtr)AndroidJNI.FindClass("com/unity3d/player/UnityPlayer");
        IntPtr fid_Activity = AndroidJNI.GetStaticFieldID(cls_Activity, "currentActivity", "Landroid/app/Activity;");
        IntPtr obj_Activity = AndroidJNI.GetStaticObjectField(cls_Activity, fid_Activity);

        IntPtr obj_cls = AndroidJNI.GetObjectClass(obj_Activity);
        IntPtr asset_func = AndroidJNI.GetMethodID(obj_cls, "getAssets", "()Landroid/content/res/AssetManager;");

        IntPtr assetManager = AndroidJNI.CallObjectMethod(obj_Activity, asset_func, new jvalue[2]);

        IntPtr assetManagerClass = AndroidJNI.GetObjectClass(assetManager);
        IntPtr openFd = AndroidJNI.GetMethodID(assetManagerClass, "openFd", "(Ljava/lang/String;)Landroid/content/res/AssetFileDescriptor;");
        jvalue[] param_array2 = new jvalue[2];
        jvalue param = new jvalue();
        param.l = AndroidJNI.NewStringUTF(name);
        param_array2[0] = param;
        IntPtr jfd;
        try
        {
            jfd = AndroidJNI.CallObjectMethod(assetManager, openFd, param_array2);
            IntPtr assetFdClass = AndroidJNI.GetObjectClass(jfd);
            IntPtr getParcelFd = AndroidJNI.GetMethodID(assetFdClass, "getParcelFileDescriptor", "()Landroid/os/ParcelFileDescriptor;");
            IntPtr getStartOffset = AndroidJNI.GetMethodID(assetFdClass, "getStartOffset", "()J");
            IntPtr getLength = AndroidJNI.GetMethodID(assetFdClass, "getLength", "()J");
            start = AndroidJNI.CallLongMethod(jfd, getStartOffset, new jvalue[2]);
            length = AndroidJNI.CallLongMethod(jfd, getLength, new jvalue[2]);

            IntPtr fileInputStreamId = AndroidJNI.GetMethodID(assetFdClass, "createInputStream", "()Ljava/io/FileInputStream;");
            IntPtr fileInputStream = AndroidJNI.CallObjectMethod(jfd, fileInputStreamId, new jvalue[2]);
            IntPtr fileInputStreamClass = AndroidJNI.GetObjectClass(fileInputStream);
            // Method signatures:newbytear B: byte, Z: boolean
            IntPtr read = AndroidJNI.GetMethodID(fileInputStreamClass, "read", "([BII)I");
            jvalue[] param_array = new jvalue[3];
            jvalue param1 = new jvalue();
            IntPtr pData = AndroidJNI.NewByteArray((int)(length));
            param1.l = pData;
            jvalue param2 = new jvalue();
            param2.i = 0;
            jvalue param3 = new jvalue();
            param3.i = (int)(length);
            param_array[0] = param1;
            param_array[1] = param2;
            param_array[2] = param3;
            int numBytesRead = AndroidJNI.CallIntMethod(fileInputStream, read, param_array);
            UnityEngine.Debug.Log("Bytes Read = " + numBytesRead);

            Byte[] pDataManaged = AndroidJNI.FromByteArray(pData);
            pDataUnManaged = Marshal.AllocCoTaskMem((int)length);
            Marshal.Copy(pDataManaged, 0, pDataUnManaged, (int)length);

            jfd = AndroidJNI.CallObjectMethod(jfd, getParcelFd, new jvalue[2]);

            IntPtr parcelFdClass = AndroidJNI.GetObjectClass(jfd);
            jvalue[] param_array3 = new jvalue[2];
            IntPtr getFd = AndroidJNI.GetMethodID(parcelFdClass, "getFileDescriptor", "()Ljava/io/FileDescriptor;");
            jfd = AndroidJNI.CallObjectMethod(jfd, getFd, param_array3);
            IntPtr fdClass = AndroidJNI.GetObjectClass(jfd);

            IntPtr descriptor = AndroidJNI.GetFieldID(fdClass, "descriptor", "I");
            fd = AndroidJNI.GetIntField(jfd, descriptor);

        }
        catch (IOException ex)
        {
            UnityEngine.Debug.Log("IO Exception: Failed to load swf file");
        }
#endif
        return;
    }
}