/**********************************************************************

Filename    :   RTTObject.cs
Content     :   Inherits from MonoBehaviour
Created     :   
Authors     :   Ryan Holtz

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Collections;

using Scaleform;
using Scaleform.GFx;

public class MyRTT: SFRTT
{
    bool bSucceeded = false;

    new public virtual void Start()
    {
        base.Start();
    }

    new public virtual void Update()
    {
        if (!bSucceeded)
        {
            // Try to get the SFManager from the Camera
            SFCamera camera = Component.FindObjectOfType(typeof(SFCamera)) as SFCamera;
            if (camera == null)
            {
                return;
            }

			if (MovieClassName != null)
            {
                // Is this a class?
                Type classType = Type.GetType(MovieClassName);
                // GetType with the type name only will look for the Type in the caller's assembly and then in the System assembly.
                // GetType with the assembly qualified type name will look for the Type in any assembly.
                
                // Also: GetType() is resolved at runtime and can be used to get the run time type of an object.
                // typeof is resolved at compile time.
                Type movieType = Type.GetType(typeof(Scaleform.GFx.Movie).AssemblyQualifiedName);
                if (classType != null && classType.IsSubclassOf(movieType))
                {
                    if (CreateRenderMovie(camera, classType))
                    {
                        bSucceeded = true;
                    }
                }
                else
                {
                    // If subclass is not specified, just create a default Movie.
                    if (CreateRenderMovie(camera, movieType))
                    {
                        bSucceeded = true;
                    }
                }   
            }
        }
    }
}