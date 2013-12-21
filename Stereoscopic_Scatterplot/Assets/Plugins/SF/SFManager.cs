/**********************************************************************

Filename    :   SFManager.cs
Content     :   SFManager implementation
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

using Scaleform.GFx;

namespace Scaleform
{
    
public class DisplayInfo
{
    float X;
    float Y;
    float Z;
    float Rotation;
    float XScale;
    float YScale;
    float ZScale;
    float Alpha;
    bool  Visible;
    float XRotation;
    float YRotation;
}

public enum ScaleModeType
{
    SM_NoScale,
    SM_ShowAll,
    SM_ExactFit,
    SM_NoBorder
};

// Note: Must use struct and not class, otherwise iOS will throw AOT compilation errors
[StructLayout(LayoutKind.Explicit, Pack = 4)]
public struct SFMovieCreationParams
{
// Allows a rendertexture to be passed in
    public SFMovieCreationParams(String name, int ox, int oy, int width, int height,
        IntPtr pdata, int length, bool initFirstFrame, RenderTexture texture, Color32 bgColor, 
		bool useBackgroundColor = false, ScaleModeType scaleModeType = ScaleModeType.SM_ShowAll, bool bAutoManageVP = true):
       this(name, ox, oy, width, height, pdata, length, initFirstFrame, bgColor, useBackgroundColor, scaleModeType, bAutoManageVP)
	   {
        IsRenderToTexture    = (texture != null);
        TexWidth            = ((texture != null) ? (UInt32)texture.width : 0);
        TexHeight           = ((texture != null) ? (UInt32)texture.height : 0);

#if (UNITY_4_0) || (UNITY_4_1)
        if (texture)
        {
#if UNITY_IPHONE
            TextureId = (uint)texture.GetNativeTextureID();
#else
            IntPtr texPtr = texture.GetNativeTexturePtr();
            TextureId = (uint)(texPtr);
#endif //UNITY_IPHONE
        }
        else
        {
            TextureId = 0;
        }
#else
        TextureId = ((texture != null) ? (UInt32)texture.GetNativeTextureID() : 0);
#if UNITY_STANDALONE_WIN
        if (SystemInfo.graphicsDeviceVersion[0] == 'D')
        {
            // SystemInfo.graphicsDeviceVersion starts with either "Direct3D" or "OpenGL".
            // We need to disable RTT on D3D+Windows because GetNativeTextureID() returns 
            // a garbage value in D3D mode instead of zero, even though 
            // GetNativeTextureID() is only supported in OpenGL mode.
            TextureId = 0;
            IsRenderToTexture = false;
            TexWidth = 0;
            TexHeight = 0;
        }
#endif //UNITY_STANDALONE_WIN
#endif //(UNITY_4_0) || (UNITY_4_1)

    //  pData = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(System.IntPtr)));
    //  Marshal.WriteIntPtr(pData, pdata);
    }

    public SFMovieCreationParams(String name, int ox, int oy, int width, int height,
        IntPtr pdata, int length, bool initFirstFrame, Color32 bgColor, bool useBackgroundColor = false,
        ScaleModeType scaleModeType = ScaleModeType.SM_ShowAll, bool bautoManageVP = true)
    {
		MovieName = name;
		OX = ox;
		OY = oy;
		Width = width;
		Height = height;
		IsInitFirstFrame = initFirstFrame;
		IsAutoManageViewport = bautoManageVP;
		pData = pdata;
		Length = length;
		TheScaleModeType = scaleModeType;
		IsUseBackgroundColor = useBackgroundColor;
		IsRenderToTexture = false;
		TextureId = 0;
		TexWidth = 0;
		TexHeight = 0;
		Red = bgColor.r;
		Green = bgColor.g;
		Blue = bgColor.b;
		Alpha = bgColor.a;
		IsMemoryFile = (pdata != IntPtr.Zero);
		Sentinal = SFSentinal.Sentinal;
		IsPad0 = IsPad1 = IsPad2 = false;
    }

    [FieldOffset(0)]
    public String   MovieName;
    [FieldOffset(4)]
    public int      OX;
    [FieldOffset(8)]
    public int      OY;
    [FieldOffset(12)]
    public int      Width;
    [FieldOffset(16)]
    public int      Height;
    // The song and dance below is necessary because OSX's compiler does not align Start to 8 bytes on the C++ side of things
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(20)]
#else
    [FieldOffset(20)]
#endif
    public IntPtr   pData;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(24)]
#else
    [FieldOffset(24)]
#endif
	public int	Length;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(28)]
#else
	[FieldOffset(28)]
#endif
    public ScaleModeType TheScaleModeType;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(32)]
#else
    [FieldOffset(32)]
#endif
    // Determines if the movie is advanced right after creation. 
    public bool     IsInitFirstFrame;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(33)]
#else
    [FieldOffset(33)]
#endif
    public bool     IsAutoManageViewport;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(34)]
#else
    [FieldOffset(34)]
#endif
    public bool     IsUseBackgroundColor;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(35)]
#else
    [FieldOffset(35)]
#endif
    public bool     IsRenderToTexture;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(36)]
#else
    [FieldOffset(36)]
#endif
    public UInt32   TexWidth;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(40)]
#else
    [FieldOffset(40)]
#endif
    public UInt32   TexHeight;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(44)]
#else
    [FieldOffset(44)]
#endif
    public Byte     Red;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(45)]
#else
    [FieldOffset(45)]
#endif
    public Byte     Green;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(46)]
#else
    [FieldOffset(46)]
#endif
    public Byte     Blue;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(47)]
#else
    [FieldOffset(47)]
#endif
    public Byte     Alpha;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(48)]
#else
    [FieldOffset(48)]
#endif
    public UInt32   TextureId;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(52)]
#else
    [FieldOffset(52)]
#endif
    public bool     IsMemoryFile;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(53)]
#else
    [FieldOffset(53)]
#endif
    public bool     IsPad0;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(54)]
#else
    [FieldOffset(54)]
#endif
    public bool     IsPad1;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(55)]
#else
    [FieldOffset(55)]
#endif
    public bool     IsPad2;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
    [FieldOffset(56)]
#else
    [FieldOffset(56)]
#endif
    public UInt32 Sentinal; // Used to make sure the structure was passed through to C++ without any marshaling/alignment issues
}



// Purpose of this class is to manage Movie creation/destruction etc.
public partial class SFManager
{
    public event SF_ExternalInterfaceDelegate EIEvent
    {
        add
        {
            EIDelegate += value;
        }
        remove
        {
            EIDelegate -= value;
        }
    }
    
    public enum MovieLifeCycleEvents:int 
    {
        Movie_Created = 0,
        Movie_Destroyed, 
    }  
    
    // Maintains a list of all SFMovies created in the game
    public List<Movie>              SFMovieList;
    List<long>                      MarkForReleaseIDs;
    static List<IntPtr>             MarkForReleaseValues;    
    List<SFLifecycleEvent>          LifecycleEventsList;
    bool                            SFInitialized;
    
    SF_ExternalInterfaceDelegate    EIDelegate;
    SF_DisplayInfoDelegate          DisplayInfoDelegate;
    SF_AllocateDelegate             AllocDelegate;
    SF_LogDelegate                  LogDelegate;
    
    IntPtr                          pValues_PreAllocated;
    IntPtr                          pValueQueue;
    IntPtr                          pCommandQueue;
    IntPtr                          pASOutput;
    Int32                           NumPreAllocatedValues;
    IntPtr                          pCommandOffset;
    IntPtr                          pValueOffset;
    IntPtr                          pASOutputOffset;
    Int32                           MaxLogBufferMessageSize; // corresponds to the buffer size in GFx.
    int                             ScreenWidth;
    int                             ScreenHeight;
    
    // Delegate Declarations
    public delegate void SF_ExternalInterfaceDelegate( long MovieID, String command, IntPtr pValue, int numArgs, int valueSize);
    public delegate void SF_LogDelegate(String message);
    public delegate IntPtr SF_AllocateDelegate( int numVal);
    public delegate IntPtr SF_DisplayInfoDelegate(IntPtr ptr);

    

    public SFManager(SFInitParams initParams)
    {
        SFInitParams2 initParams2   = new SFInitParams2(initParams);
        int initParamsSize          = Marshal.SizeOf(typeof(SFInitParams2));
        int sfValueSize             = Marshal.SizeOf(typeof(Value));
        
        // initParams2.Print();
        IntPtr pdata = Marshal.AllocCoTaskMem(initParamsSize);
        Marshal.StructureToPtr(initParams2, pdata, false);
        String version = Application.unityVersion;
        SFInitialized = false;
        if (SF_Init(pdata, initParamsSize, version) == -1)
        {
            UnityEngine.Debug.Log("Error in Scaleform Manager Initialization. There could be a problem with marshaling structure members");
            return;
        }
        SF_LoadFontConfig(GetScaleformContentPath() + "FontConfig/");
        AllocateSharedData();
        SF_SetSharedData(pCommandOffset, pCommandQueue, 0);
        SF_SetSharedData(pValueOffset, pValueQueue, 1);  
        SF_SetSharedData(pASOutputOffset, pASOutput, 2);
        
        Marshal.DestroyStructure(pdata, typeof(SFInitParams));
        
        SFMovieList = new List<Movie>();
        
        MarkForReleaseIDs           = new List<long>();
        MarkForReleaseValues        = new List<IntPtr>();
        LifecycleEventsList         = new List<SFLifecycleEvent>();
        pValues_PreAllocated        = Marshal.AllocCoTaskMem(sfValueSize * NumPreAllocatedValues);
        
        SFKey.CreateKeyDictionary();
        SFInitialized = true;
    }

    public void Init()
    {
        SFInitialized = true;
    }

    public void Destroy()
    {
        // Clear out ReleaseList
        MarkForReleaseIDs.Clear();
        SFMovieList.Clear();
        GC.Collect();
#if UNITY_ANDROID || UNITY_IPHONE   
        Console.WriteLine("In SFManager::OnDestroy");   
        SF_DestroyManager();
        SF_Destroy();
#endif
        SFInitialized = false;
    }

    ~SFManager()
    {
        Marshal.FreeCoTaskMem(pValues_PreAllocated);
        Marshal.FreeCoTaskMem(pValueQueue);
        Marshal.FreeCoTaskMem(pCommandQueue);
        Marshal.FreeCoTaskMem(pCommandOffset);
        Marshal.FreeCoTaskMem(pValueOffset);
        // Notify all movies that the Manager has been destroyed.
        for (int i = 0; i < SFMovieList.Count; i++)
        {
            SFMovieList[i].OnSFManagerDied();
        }
    }
    
    private void AllocateSharedData()
    {
        int SFCommandSize           = Marshal.SizeOf(typeof(SFCommand));
        int sfValueSize             = Marshal.SizeOf(typeof(Value));
        int SFCharSize              = Marshal.SizeOf(typeof(char));
        NumPreAllocatedValues       = 10;
        MaxLogBufferMessageSize     = 2048; 
        pValueQueue                 = Marshal.AllocCoTaskMem(sfValueSize * NumPreAllocatedValues*10);
        pCommandQueue               = Marshal.AllocCoTaskMem(SFCommandSize * NumPreAllocatedValues);
        pASOutput                   = Marshal.AllocCoTaskMem(10*MaxLogBufferMessageSize*SFCharSize);
        pCommandOffset              = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(System.Int32)));
        pValueOffset                = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(System.Int32)));
        pASOutputOffset             = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(System.Int32)));
        
        Marshal.WriteInt32(pCommandOffset, 0);
        Marshal.WriteInt32(pValueOffset, 0);
        Marshal.WriteInt32(pASOutputOffset, 0);
        
        for (int i = 0; i < (10*MaxLogBufferMessageSize); i++)
        {
            Marshal.WriteByte(pASOutput, i, 0);
        }
    }
    
    private Int32 RandomNumber()
    {
        System.Random random = new System.Random();
        return random.Next();
    }
    
    public void OnLogMessage(String message)
    {
        LogMessage(message);
    }
    
    public void LogMessage(String message)
    {
        Console.WriteLine(message);
    }
    
    private void HandleASTraces()
    {
        int numTraces = Marshal.ReadInt32(pASOutputOffset);
        if (numTraces == 0)
        {
            return;
        }
        
        IntPtr ptr = new IntPtr(pASOutput.ToInt32());
        String str = Marshal.PtrToStringAnsi(ptr);
        Console.Write(str); // Write trace() to Console.
        UnityEngine.Debug.Log( str ); // Write trace() to Debug Log.
        
        Marshal.WriteInt32(pASOutputOffset, 0);
        
        // Clear the buffer
        for (int i = 0; i < 10*MaxLogBufferMessageSize; i++)
        {
            Marshal.WriteByte(pASOutput, i, 0);
        }
    }
    
    public SFCommand GetCommandData(IntPtr pqueue)
    {    
        SFCommand command = new SFCommand();
        // Marshal.PtrToStructure(pqueue, command); // Works on Windows, unsupported on iOS due to aot compilation.
        IntPtr  ptr = new IntPtr(pqueue.ToInt32()); // Workaround for iOS.
        command.MovieID = Marshal.ReadInt64(ptr);
        ptr = new IntPtr(ptr.ToInt32() + Marshal.SizeOf(typeof(long)));
        command.ArgCount = Marshal.ReadInt32(ptr);  
        ptr = new IntPtr(ptr.ToInt32() + Marshal.SizeOf(typeof(int)));
        command.pMethodName = Marshal.ReadIntPtr(ptr);
        return command;
    }

    public static String GetScaleformContentPath()
    {
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR)
        return Application.dataPath + "/StreamingAssets/";          
#elif UNITY_IPHONE
        return (Application.dataPath + "/Raw/");

#elif UNITY_STANDALONE_OSX 
        return Application.dataPath + "/Data/StreamingAssets/";

#elif UNITY_ANDROID
    return "";
#endif
    }

    // Delegates don't work in an iOS environment. In order to get around this limitation, we put 
    // ExternalInterface notifications in a queue which is preallocated in the constructor of SFManager.
    public void ProcessCommands()
    {
        // Deal with AS Traces:
        HandleASTraces();
            
        int numCommands = Marshal.ReadInt32(pCommandOffset);
        if (numCommands == 0) return;
        
        int commandSize = Marshal.SizeOf(typeof(SFCommand));
        int sfValueSize = Marshal.SizeOf(typeof(Value));
        IntPtr pqueue = new IntPtr(pCommandQueue.ToInt32());
        IntPtr pargs = new IntPtr(pValueQueue.ToInt32());
        
        int cumNumArgs = 0;
        for (int i = 0; i < numCommands; i++)
        {
            pqueue = new IntPtr(pCommandQueue.ToInt32() + commandSize*i);
            SFCommand command       = GetCommandData(pqueue);
            int numArgs             = command.ArgCount;
            long MovieID             = command.MovieID;
            
            String methodName = Marshal.PtrToStringAnsi(command.pMethodName);
            OnExternalInterface(MovieID, methodName, pargs, numArgs, sfValueSize);
            
            cumNumArgs += numArgs;
            pargs = new IntPtr(pValueQueue.ToInt32() + sfValueSize*cumNumArgs);
        }
        
        SF_ClearCommandBuffer(numCommands);
        Marshal.WriteInt32(pCommandOffset, 0);
        Marshal.WriteInt32(pValueOffset, 0);
    }
    
    IntPtr AllocateImpl(int numVal)
    {
        if (numVal < NumPreAllocatedValues)
        {
            return pValues_PreAllocated;
        }
        else
        {
            int sfValueSize = Marshal.SizeOf(typeof(Value));
            // Allocate space on the COM heap. Should also be able to use AllocHGlobal in order to 
            // allocate space on process heap.
            IntPtr ptr = Marshal.AllocCoTaskMem(sfValueSize * numVal);
            return ptr;
        }
    }
    
    IntPtr AllocateDisplayInfo(IntPtr sz)
    {
        int dInfoSize       = Marshal.SizeOf(typeof(SFDisplayInfo));
        int floatSize       = Marshal.SizeOf(typeof(float));
        int doubleSize      = Marshal.SizeOf(typeof(double));
        int intPtrSize      = Marshal.SizeOf(typeof(IntPtr));
        
        // Allocate space on the COM heap. Should also be able to use AllocHGlobal in order to 
        // allocate space on process heap.
        IntPtr dInfoPtr     = Marshal.AllocCoTaskMem(dInfoSize);
        IntPtr vmptr        = Marshal.AllocCoTaskMem(4*3*floatSize);
        IntPtr projPtr      = Marshal.AllocCoTaskMem(4*4*floatSize);
        
        IntPtr pdata1       = new IntPtr(dInfoPtr.ToInt32() + doubleSize * 11);
        Marshal.WriteIntPtr(pdata1, vmptr);
        
        IntPtr pdata2       = new IntPtr(pdata1.ToInt32() + intPtrSize);
        Marshal.WriteIntPtr(pdata2, projPtr);
        
        // Return size of managed DisplayInfo struct to C++ so that it can be compared with
        // native dinfo size in order for marshalling to work correctly. 
        Marshal.WriteInt32(sz, dInfoSize);
        return dInfoPtr;
    }
    
   public static Value GetValueData(IntPtr pqueue)
    {
        Value pvalue = new Value();
        
        // Marshal.PtrToStructure(pqueue, pvalue); // The easy way to do this (supported on Windows).
        IntPtr ptr = new IntPtr(pqueue.ToInt32()); // Workaround for Mono on iOS.
        pvalue.pInternalData = Marshal.ReadIntPtr(ptr);
        
        ptr = new IntPtr(ptr.ToInt32() + Marshal.SizeOf(typeof(IntPtr)));
        pvalue.Type = (Value.ValueType)Marshal.ReadInt32(ptr);
            
        ptr = new IntPtr(ptr.ToInt32() + Marshal.SizeOf(typeof(Int32)));
        pvalue.MovieId = Marshal.ReadInt64(ptr);
        
        return pvalue;
    }
        
    void OnExternalInterface(long MovieID, String command, IntPtr ptr, int numArgs, int valueSize)
    {
        int sfValueSize = Marshal.SizeOf(typeof(Value));
        int count = 0;
        
        // Array of types passed to ExternalInterface
        Type[] typeArray;
        System.Object[] args;
        
        // Note that we can't preallocate typeArray and args since we have to pass them to the 
        // GetMethod function below and there is no way to pass the size of the array, so the
        // array can't contain any null values.
        typeArray = new Type[numArgs];
        args = new System.Object[numArgs];
      
        for (int i = 0; i < numArgs; i++)
        {
            // Can't add an integer offset to IntPtr as you would with C/C++ pointer 
            IntPtr data = new IntPtr(ptr.ToInt32() + sfValueSize * i);
            
            // This Value makes a copy of the data and will be garbage collected.
            Value val = GetValueData(data);
            // Value val = (Value)Marshal.PtrToStructure(data, typeof(Value)); // Unsupported on iOS.
            
            if (val.IsString())
            {
                String str = val.GetString();
                typeArray.SetValue(typeof(String), count);
                args.SetValue(str, count);
                count++;
            }
            else if (val.IsNumber())
            {
                double num = val.GetNumber();
                Console.Write(num);
                typeArray.SetValue(typeof(double), count);
                args.SetValue(num, count);
                count++;
            }    
            else if (val.IsBoolean())
            {
                Boolean boolVal = val.GetBool();
                typeArray.SetValue(typeof(Boolean), count);
                args.SetValue(boolVal, count);
                count++;
            }    
            else if (val.IsUInt())
            {
                UInt32 uintVal = val.GetUInt();
                typeArray.SetValue(typeof(UInt32), count);
                args.SetValue(uintVal, count);
                count++;
            }    
            else if (val.IsInt())
            {
                Int32 intVal = val.GetInt();
                typeArray.SetValue(typeof(Int32), count);
                args.SetValue(intVal, count);
                count++;
            }    
            else if (val.IsObject())
            {
                Value newval = val.GetObject();
                typeArray.SetValue(typeof(Value), count);
                args.SetValue(newval, count);
                count++;
            }
        }
        
        // Preallocated memory is destroyed in the destructor
        /* This code can be uncommented if delegates are being used for EI implementation.
        if (ptr != pValues_PreAllocated)
        {
            Console.WriteLine("Destroying Value Array");
            Marshal.FreeCoTaskMem(ptr);
        }
        */
        
        // At this point, count must be = numArgs, since we should be able to determine the type of each Value. If not,
        // there is some problem.
        if (count != numArgs)
        {
            LogMessage("Invalid Type passed in ExternalInterface!");
            return;
        }
        
        for (int j = 0; j < SFMovieList.Count; j++) // Loop through List with for
        {
            Movie movie = SFMovieList[j];
            long mId = movie.GetID();
            if (MovieID != mId) continue;
            
            Type SFMovieClass = movie.GetType();
            UnityEngine.Debug.Log("ExternalInterface arrived");
            // Command passed as an argument is the method we want to call
            MethodInfo methodInfo;
            if (typeArray == null)
            {
                methodInfo = SFMovieClass.GetMethod(command);
            }
            else
            {
                methodInfo = SFMovieClass.GetMethod(command, typeArray);
            }
            
            if( methodInfo != null )
            {
                // LogMessage("Method Implementing " + command + " found, Invoking method");
                methodInfo.Invoke(movie, args); // Call the method
            }
            else
            {
                UnityEngine.Debug.Log("Handler for command: " + command + " not found!");
            }
            return;
        }
    }

    public void EnableIME()
    {
#if UNITY_STANDALONE_WIN
        SF_EnableIME();
#endif
    }
    
    public void QueuedDestroy()
    {
        SFInitialized        = false;
    }
    
    public bool IsSFInitialized()
    {
        return SFInitialized; 
    }

    public void AddToLifecycleEventList(SFLifecycleEvent ev)
    {
        LifecycleEventsList.Add(ev);
    }
    
    public void AddToReleaseList(long movieId)
    {
        MarkForReleaseIDs.Add(movieId);
    }
    
    static public void AddValueToReleaseList(IntPtr valIntPtr)
    {
        MarkForReleaseValues.Add(valIntPtr);    
    }
    
    public void AddMovie(Movie movie)
    {
        SFMovieList.Add(movie);
    }
    
    public void ReleaseMoviesMarkedForRelease()
    {
        if (MarkForReleaseIDs.Count == 0)
        {
            return;
        }
        for (int i = 0; i < MarkForReleaseIDs.Count; i++) 
        {
            SF_DestroyMovie(MarkForReleaseIDs[i]);
        }
        MarkForReleaseIDs.Clear();
    }
    
    static public void ReleaseValuesMarkedForRelease()
    {
        if (MarkForReleaseValues.Count == 0)
        {
            return;
        }
        for (int i = 0; i < MarkForReleaseValues.Count; i++) 
        {
            // Console.WriteLine("Finalizing Object");
            IntPtr pInternalData = MarkForReleaseValues[i];
            if (pInternalData != IntPtr.Zero)
            {
                // Console.WriteLine("Releasing Internal Data");
                Value.SF_DecrementValRefCount(pInternalData);
            }
        }
        MarkForReleaseValues.Clear();
    }
    
    public void DestroyMovie(Movie movie)
    {    
        SFMovieList.Remove(movie);
        SF_NotifyNativeManager(movie.MovieID, MovieLifeCycleEvents.Movie_Destroyed);
    }
    /*
    public T CreateMovie <T> (SFMovieCreationParams params) where T : Movie
    {
        Activator.CreateInstance (typeof (T), argslist);
    }
    */
        
    public Movie CreateMovie(SFMovieCreationParams creationParams, Type MovieClassType)
    {
        Type[] argTypes = new Type[]{typeof(SFManager), typeof(SFMovieCreationParams)};
        object[] argVals = new object[] {this, creationParams};
        ConstructorInfo cInfo = MovieClassType.GetConstructor(argTypes);
        
        // Console.WriteLine(MovieClassType);
        // Console.WriteLine(cInfo);
        
        if (cInfo == null)
        {
            return null;
        }
        Movie movie = (Movie)cInfo.Invoke(argVals);
        return movie;
    }

    public Movie GetTopMovie()
    {
        if (SFMovieList != null && SFMovieList.Count != 0)
            return SFMovieList[0];
        return null;
    }

    public Movie GetBottomMovie()
    {
        if (SFMovieList != null)
        {
            int numMovies = SFMovieList.Count;
            if (numMovies != 0)
                return SFMovieList[numMovies - 1];
        }
        return null;
    }

    public int GetNumMovies()
    {
        if (SFMovieList != null)
        {
            return SFMovieList.Count;
        }
        return -1;
    }

	public List<Movie> GetMovies()
	{
		return SFMovieList;
	}

    public void Update()
    {
        if (!IsSFInitialized())
        {
            return;
        }
        SF_ProcessMarkedForDeleteMovies();
        ReleaseMoviesMarkedForRelease();
        ReleaseValuesMarkedForRelease();
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            SFMovieList[i].Update();
        }
    }

    public bool DoHitTest(float x, float y)
    {
        if (!IsSFInitialized())
        {
            return false;
        }
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].DoHitTest(x, y, HitTestType.HitTest_ShapesNoInvisible))
            {
                return true; 
            }
        }
        return false;
    }

    public bool ReplaceTexture(long movieId, String textureName, Texture texture)
    {
        int texId;
        int rt_with     = texture.width;
        int rt_height    = texture.height;

#if (UNITY_4_0) || (UNITY_4_1)
#if UNITY_IPHONE
        texId = texture.GetNativeTextureID();
#else
        IntPtr texPtr = texture.GetNativeTexturePtr();
        texId = (int)(texPtr);
#endif
#else
        texId = texture.GetNativeTextureID();
#endif

        return SF_ReplaceTexture(movieId, textureName, texId, rt_with, rt_height);
    }

    public void ApplyLanguage(String langName)
    {
        SF_ApplyLanguage(langName);
    }
    
    public void Advance(float deltaTime)
    {
        if (!IsSFInitialized())
        {
            return;
        }

        // Check if viewport coordinates have changed
        int newScreenWidth  = Screen.width;
        int newScreenHeight = Screen.height;
        int ox = 0;
        int oy = 0;
        #if UNITY_IPHONE
        oy = 0;
        #else
        oy = 24; // Note that while using D3D renderer, the tool bar (that contains "Maximize on Play" text) is part of 
        // the viewport, while using GL renderer, it's not. So there should be a further condition here depending on 
        // what type of renderer is being used, however I couldn't find a macro for achieving that. 
        #endif 

        if (ScreenWidth != newScreenWidth || ScreenHeight != newScreenHeight)
        {
            UnityEngine.Debug.Log("ScreenWidth = " + newScreenWidth + "ScreenHeight = " + newScreenHeight);
            ScreenHeight = newScreenHeight;
            ScreenWidth  = newScreenWidth; 
            SF_SetNewViewport(ox, oy, ScreenWidth, ScreenHeight);
        }
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            SFMovieList[i].Advance(deltaTime);
        }
        if (LifecycleEventsList != null)
        {
            for (int i = 0; i < LifecycleEventsList.Count; i++) // Loop through List with for
            {
                LifecycleEventsList[i].Execute();
            }
        }
        ReleaseMoviesMarkedForRelease();
        ReleaseValuesMarkedForRelease();
    }
    
    public void Display()
    {
        if (!IsSFInitialized())
        {
            return;
        }
        
        SF_Display();
        // This indicates to Unity that render states might have changed and it can't assume anything about previous render states
        GL.InvalidateState();
    }
    
    public void InstallDelegates()
    {
        EIDelegate = this.OnExternalInterface;
        SF_SetExternalInterfaceDelegate(EIDelegate);
        
        AllocDelegate = new SF_AllocateDelegate(this.AllocateImpl);
        SF_SetAllocateValues(AllocDelegate);
        
        LogDelegate = new SF_LogDelegate(this.OnLogMessage);
        SF_SetLogDelegate(LogDelegate);
        
        DisplayInfoDelegate = new SF_DisplayInfoDelegate(this.AllocateDisplayInfo);
        SF_SetDisplayInfoDelegate(DisplayInfoDelegate);
    }

    public bool HandleMouseMoveEvent(float x, float y)
    {
        if (!IsSFInitialized())
        {
            return false;
        }
        
        int icase = 3;
#if UNITY_EDITOR
        y = y + 24; // Need to offset by height of title bar
#endif
        bool handled = false;
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleMouseEvent(x, y, icase, 0))
            {
                handled = true;
            }
        }
        return handled;
    }

    public bool HandleMouseEvent(UnityEngine.Event ev)
    {
        if (!IsSFInitialized())
        {
            return false;
        }
        
        int icase = 0;
        Vector2 mousePos = ev.mousePosition;
        switch (ev.type)
        {
            case EventType.MouseDown:
                icase = 1;
                break;
            case EventType.MouseUp:
                icase = 2;
                break;
            case EventType.MouseMove:
                icase = 3;
                break;
        }

#if UNITY_EDITOR
        mousePos[1] = mousePos[1] + 24; // Need to offset by height of title bar
#endif
        bool handled = false;
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleMouseEvent(mousePos[0], mousePos[1], icase, ev.button))
            {
                handled = true;
            }
        }
        return handled;
    }
    
    public bool HandleTouchEvent(UnityEngine.Touch touch)
    {
        if (!IsSFInitialized())
        {
            return false;
        }

        int icase = 0;
        switch (touch.phase)
        {
            case TouchPhase.Began:
                icase = 1; 
                break;
            case TouchPhase.Moved:
                icase = 2; 
                break;
            case TouchPhase.Ended:
                icase = 3; 
                break;              
        }
        bool handled = false;
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleTouchEvent(touch.fingerId, touch.position[0], touch.position[1], icase))
            {
                handled = true;
            }
        }
        return handled;
    }

    // Overload for handling keydown event from GamePad
    public bool HandleKeyDownEvent(SFKey.Code code, SFKeyModifiers.Modifiers mod = 0, int keyboardIndex = 0)
    {
        if (!IsSFInitialized())
        {
            return false;
        }

        bool handled = false;
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleKeyEvent(code, mod, 1, keyboardIndex))
            {
                handled = true;
            }
        }
        return handled;
    }

    // Overload for handling keyup event from GamePad
    public bool HandleKeyUpEvent(SFKey.Code code, SFKeyModifiers.Modifiers mod = 0, int keyboardIndex = 0)
    {
        if (!IsSFInitialized())
        {
            return false;
        }

        bool handled = false;
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleKeyEvent(code, mod, 0, keyboardIndex))
            {
                handled = true;
            }
        }

        return handled;
    }


    public bool HandleKeyDownEvent(UnityEngine.Event ev)
    {
        if (!IsSFInitialized())
        {
            return false;
        }

        bool handled = false;
        SFKey.Code cd = SFKey.GetSFKeyCode(ev.keyCode);
        SFKeyModifiers.Modifiers modifiers = SFKey.GetSFModifiers(ev.modifiers);
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleKeyEvent(cd, modifiers, 1))
            {
                handled = true;
            }
        }
        return handled;
    }

    public bool HandleKeyUpEvent(UnityEngine.Event ev)
    {
        if (!IsSFInitialized())
        {
            return false;
        }

        bool handled = false;
        SFKey.Code cd = SFKey.GetSFKeyCode(ev.keyCode);
        SFKeyModifiers.Modifiers modifiers = SFKey.GetSFModifiers(ev.modifiers);
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleKeyEvent(cd, modifiers, 0))
            {
                handled = true;
            }
        }
        return handled;
    }

    public bool HandleCharEvent(UnityEngine.Event ev)
    {
        if (!IsSFInitialized())
        {
            return false;
        }

        bool handled = false;
        UInt32 wchar = ev.character;
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleCharEvent(wchar))
            {
                handled = true;
            }
        }
        return handled;
    }

    private void PrintAddress(System.Object o)
    {
        GCHandle h      = GCHandle.Alloc(o, GCHandleType.Pinned);
        IntPtr addr     = h.AddrOfPinnedObject();
        Console.WriteLine(addr.ToString("x"));
        h.Free();
    } 
}

} // namespace Scaleform;
