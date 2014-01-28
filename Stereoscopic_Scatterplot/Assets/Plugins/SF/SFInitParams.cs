/**********************************************************************

Filename    :	SFInitParams.cs
Content     :	Inititialization parameters for Scaleform runtime
Created     :   
Authors     :   Ankur Mohan

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

namespace Scaleform
{
// Need Serializable and class attributes to make InitParms visible in the editor.
[StructLayout(LayoutKind.Explicit, Pack=4)]
[Serializable]
public class SFInitParams
{

    // static int sizeInt = Marshal.SizeOf(typeof(int));
    public SFInitParams()
    {
        SetFontCacheParams 	= true;
        SetFontPackParams 	= true;
        IsInitIME			= false;
    }
    
    public enum ASVersion:int
    {
        AS2 = 0,
        AS3,
        Both
    }
    
    [FieldOffset(0)]
    public ASVersion TheASVersion = SFInitParams.ASVersion.AS3;
    [FieldOffset(4)]
    public bool InitVideo = false;
    [FieldOffset(5)]
    public bool InitSound = true;
    
    public enum VideoSoundSystem : int
    {
        SystemSound = 0,
        FMod,
        WWise,
        Default
    }
    
    [FieldOffset(8)]
    public VideoSoundSystem TheVideoSoundSystem = VideoSoundSystem.SystemSound;
    
    public enum InitIME:int
    {
        Yes = 0,
        No
    }
    [FieldOffset(12)]
    public bool IsInitIME = false;
    
    public enum EnableAmpProfiling : int
    {
        Yes = 0,
        No
    }
    [FieldOffset(16)]
    public EnableAmpProfiling IsProgLoading;
    public enum EnableProgressiveLoading : int
    {
        Yes = 0,
        No
    }
    [FieldOffset(20)]
    public EnableProgressiveLoading ProgLoading;
    
    [StructLayout(LayoutKind.Explicit, Pack=4)]
    [Serializable]
    public class FontCacheConfig
    {
		[FieldOffset(0)]
        public int TextureHeight    = 1024;
		[FieldOffset(4)]
        public int TextureWidth        = 512;
		[FieldOffset(8)]
        public int MaxNumTextures    = 1;
		[FieldOffset(12)]
        public int MaxSlotHeight    = 48;
    }
    
    [FieldOffset(24)]
    public FontCacheConfig TheFontCacheConfig;
    [FieldOffset(40)]
    public bool SetFontCacheParams = true;
    [FieldOffset(41)]
    public bool IsEnableDynamicCache = true;

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class FontPackParams
    {
        public int NominalSize    = 32;
        public int PadPixels    = 2;
        public int TextureWidth = 512;
        public int TextureHeight= 512;
    }
    
    [FieldOffset(44)]
    public FontPackParams TheFontPackParams;
    [FieldOffset(60)]
    public bool SetFontPackParams = true;
    [FieldOffset(64)]
    public int    GlyphCountLimit = 1000;
    [FieldOffset(68)]
    public float SoundVolume = 10;
    [FieldOffset(72)]
    public bool IsMute = false;
    [FieldOffset(76)]
    public UInt32 Sentinal = SFSentinal.Sentinal;
    
    public void Print()
    {
        Console.WriteLine("ASVersion:");
        switch(TheASVersion)
        {
            case ASVersion.AS2:
                Console.WriteLine("AS2");
                break;
            case ASVersion.AS3:
                Console.WriteLine("AS3");
                break;
            case ASVersion.Both:
                Console.WriteLine("Both");
                break;
            default:
                Console.WriteLine("Unknown");
                break;
        }
        
        // Console.Write("SetFontPackParams = " + SetFontPackParams);
        // Console.Write(TheFontCacheConfig.TextureHeight); 
        
        // Complete the rest..
    }
}


// Need struct definition in order to prevent AOT compilation error on iOS
[StructLayout(LayoutKind.Explicit, Pack = 4)]
[Serializable]
public struct SFInitParams2
{
    public SFInitParams2(int dummy)
    {
        TheASVersion              = ASVersion.AS3;
        InitVideo              = false;
        InitSound              = true;
        TheVideoSoundSystem       = VideoSoundSystem.SystemSound;
        IsInitIME                = false;
        IsProgLoading     = EnableAmpProfiling.Yes;
        IsSetFontCacheParams     = true;
        IsEnableDynamicCache     = true;
        SetFontPackParams      = true;
        IsInitIME                = false;
        ProgLoading            = EnableProgressiveLoading.Yes;
        
        TheFontCacheConfig.TextureHeight      = 1024;
        TheFontCacheConfig.TextureWidth       = 1024;
        TheFontCacheConfig.MaxNumTextures     = 1;
        TheFontCacheConfig.MaxSlotHeight      = 48;
        
        TheFontPackParams.NominalSize     = 32;
        TheFontPackParams.PadPixels       = 2;
        TheFontPackParams.TextureWidth    = 512;
        TheFontPackParams.TextureHeight   = 512;
        
        SetFontPackParams      = true;
        GlyphCountLimit        = 1000;
        SoundVolume             = 10;
        IsMute                  = false;
        Sentinal                = SFSentinal.Sentinal;
    }
    
    public SFInitParams2(SFInitParams initParams)
    {
        TheASVersion    = (ASVersion)initParams.TheASVersion;
        InitVideo    = initParams.InitVideo;
        InitSound    = initParams.InitSound;
        TheVideoSoundSystem       = (VideoSoundSystem) initParams.TheVideoSoundSystem;
        IsInitIME                = initParams.IsInitIME;
        IsProgLoading     = (EnableAmpProfiling) initParams.IsProgLoading;
        IsSetFontCacheParams     = initParams.SetFontCacheParams;
        IsEnableDynamicCache     = initParams.IsEnableDynamicCache;
        SetFontPackParams      = initParams.SetFontPackParams;
        IsInitIME                = initParams.IsInitIME;
        ProgLoading            = (EnableProgressiveLoading) initParams.ProgLoading;
        
        TheFontCacheConfig.TextureHeight      = initParams.TheFontCacheConfig.TextureHeight;
        TheFontCacheConfig.TextureWidth       = initParams.TheFontCacheConfig.TextureWidth;
        TheFontCacheConfig.MaxNumTextures     = initParams.TheFontCacheConfig.MaxNumTextures;
        TheFontCacheConfig.MaxSlotHeight      = initParams.TheFontCacheConfig.MaxSlotHeight;

        TheFontPackParams.NominalSize         = initParams.TheFontPackParams.NominalSize;
        TheFontPackParams.PadPixels           = initParams.TheFontPackParams.PadPixels;
        TheFontPackParams.TextureWidth        = initParams.TheFontPackParams.TextureWidth;
        TheFontPackParams.TextureHeight       = initParams.TheFontPackParams.TextureHeight;

        SetFontPackParams      = initParams.SetFontCacheParams;
        GlyphCountLimit        = initParams.GlyphCountLimit;
        SoundVolume             = initParams.SoundVolume;
        IsMute                  = initParams.IsMute;
        Sentinal                = SFSentinal.Sentinal;
    }
    
    public enum ASVersion : int
    {
        AS2 = 0,
        AS3,
        Both
    }
    
    [FieldOffset(0)]
    public ASVersion TheASVersion;
    [FieldOffset(4)]
    public bool InitVideo;
    [FieldOffset(5)]
    public bool InitSound;

    public enum VideoSoundSystem : int
    {
        SystemSound = 0,
        FMod,
        WWise,
        Default
    }
    [FieldOffset(8)]
    public VideoSoundSystem TheVideoSoundSystem;

    public enum InitIME : int
    {
        Yes = 0,
        No
    }
    [FieldOffset(12)]
    public bool IsInitIME;

    public enum EnableAmpProfiling : int
    {
        Yes = 0,
        No
    }
    
    [FieldOffset(16)]
    public EnableAmpProfiling IsProgLoading; // Unused.
    public enum EnableProgressiveLoading : int
    {
        Yes = 0,
        No
    }
    [FieldOffset(20)]
    public EnableProgressiveLoading ProgLoading;

	[StructLayout(LayoutKind.Explicit, Pack = 4)]
    [Serializable]
    public struct FontCacheConfig
    {
		[FieldOffset(0)]
        public int TextureHeight;
		[FieldOffset(4)]
        public int TextureWidth;
		[FieldOffset(8)]
        public int MaxNumTextures;
		[FieldOffset(12)]
        public int MaxSlotHeight;
    }
    
    [FieldOffset(24)]
    public FontCacheConfig TheFontCacheConfig;
    [FieldOffset(40)]
    public bool IsSetFontCacheParams;
    [FieldOffset(41)]
    public bool IsEnableDynamicCache;

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct FontPackParams
    {
        public int NominalSize;
        public int PadPixels;
        public int TextureWidth;
        public int TextureHeight;
    }
    
    [FieldOffset(44)]
    public FontPackParams TheFontPackParams;
    [FieldOffset(60)]
    public bool SetFontPackParams;
    [FieldOffset(64)]
    public int GlyphCountLimit;
    [FieldOffset(68)]
    public float SoundVolume;
    [FieldOffset(72)]
    public bool IsMute;
    [FieldOffset(76)]
    public int Sentinal;

    public void Print()
    {
        Console.WriteLine("ASVersion:");
        switch (TheASVersion)
        {
            case ASVersion.AS2:
                Console.WriteLine("AS2");
                break;
            case ASVersion.AS3:
                Console.WriteLine("AS3");
                break;
            case ASVersion.Both:
                Console.WriteLine("Both");
                break;
            default:
                Console.WriteLine("Unknown");
                break;
        }

        // Console.Write("SetFontPackParams = " + SetFontPackParams);
        // Console.Write(TheFontCacheConfig.TextureHeight);

        // Complete the rest..
    }
}
}