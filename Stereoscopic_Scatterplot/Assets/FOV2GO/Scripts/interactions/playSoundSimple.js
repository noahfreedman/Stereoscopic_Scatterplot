/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* This is an s3d Interaction script.
 * Requires a s3dInteractor component
 * First tap starts audioClip playing
 * If toggleOnOff = true, next tap stops audioClip
 - If rewindOnStop = true, next tap starts audioClip from beginning
 - If rewindOnStop = false, next tap starts audioClip from where it stopped
 * If toggleOnOff = false, audioClip plays through to the end
 - additional taps have no effect until clip is finished, then next tap starts clip again
*/

#pragma strict

@script RequireComponent(AudioSource);
@script RequireComponent(s3dInteractor);

public var toggleOnOff : boolean = true;
public var rewindOnStop : boolean = true;
var tapType : int = 3; // 1 = short tap 2 = long tap 3 = any tap

function Start() {
}

function NewTap(params: TapParams) {
	if (params.tap == tapType || tapType == 3) {
		if (!audio.isPlaying){
	        audio.Play();
		} else {
			if (toggleOnOff) {
				if (rewindOnStop) {
					audio.Stop();
				} else {
					audio.Pause();
				}
			}
		}
	}
} 
