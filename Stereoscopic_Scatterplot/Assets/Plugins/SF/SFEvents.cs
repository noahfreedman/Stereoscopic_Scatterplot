/**********************************************************************

Filename    :	SFEvents.cs
Content     :	Definition of SFEvent Wrapper
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
    
[StructLayout(LayoutKind.Sequential)]
public class SFEvent
{
    public SFEvent()
    {
        Type = SFEventType.Unknown;
    }
    public SFEvent(SFEventType eventType)
    {
        Type = eventType;
    }
    
    public enum SFEventType
    {
        Unknown = 0,       
        // Informative events sent to the player.
        MouseMove,
        MouseDown,
        MouseUp,
        MouseWheel,
        KeyDown,
        KeyUp,
        SceneResize,
        SetFocus,
        KillFocus,
        Char,
        IME
    };
    
    // What kind of event this is.
    public SFEventType          Type;
    // State of special keys
    public SFKeyModifiers       EvModifiers;
    
    public UInt32   Dummy;
    
}

public class MouseEvent:SFEvent
{
    public float	X;
	public float	Y;
    public float	ScrollDelta;
    public int		Button;
    public int		MouseIndex;
    
    public MouseEvent(): base()
    {
        Button = 0; X = 0; Y = 0; ScrollDelta = 0.0f; MouseIndex = 0;
    }
    
    public MouseEvent(SFEvent.SFEventType eventType, float xpos, float ypos, float scrollVal = 0.0f, int mouseIdx = 0, int button = 0)
        :base(eventType)
    {
        Button = button; X = xpos; Y = ypos; ScrollDelta = scrollVal;
        MouseIndex = mouseIdx;
    }
}

[StructLayout(LayoutKind.Sequential)]
public class KeyEvent:SFEvent
{
    public SFKey.Code    KeyCode;
    public Byte          AsciiCode;
    public UInt32        WcharCode;
    public Byte          KeyboardIndex; 
    
    public KeyEvent(Event ev, UInt32 down): base()
    {
        AsciiCode        = 0;
        WcharCode        = 0;
        KeyboardIndex    = 0;
        if (down == 1)
        {
            Type            = SFEvent.SFEventType.KeyDown;
        }
        else
        {
            Type            = SFEvent.SFEventType.KeyUp;
        }
        if (ev.keyCode == UnityEngine.KeyCode.A)
        {
            KeyCode         = SFKey.Code.A;    
        }
    }
}

[StructLayout(LayoutKind.Sequential)]
public class CharEvent:SFEvent
{
    public UInt32           WcharCode;
    public Byte             KeyboardIndex; // The index of the physical keyboard controller.

    public CharEvent(Event ev): base()
    {
        KeyboardIndex    = 0;
        WcharCode        = ev.character; 
    }
}

} // namespace Scaleform;