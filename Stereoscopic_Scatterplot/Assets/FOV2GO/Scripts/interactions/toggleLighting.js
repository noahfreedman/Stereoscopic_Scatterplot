/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* This is an s3d Interaction script.
 * Requires a s3dInteractor component
 * toggles between lighting on and off states
 * option to swap out skyboxes along with lighting states
 * supports multiple switches, all toggling same light
 * designate one switch as the masterSwitch, make sure it's the same on all instances of the script
*/

#pragma strict
var masterSwitch: GameObject;
var roomlight : Light;
var roomlightIntensityOn : float = 1.5;
var roomlightIntensityOff : float = 0.5;
var useSkyBox : boolean = false;
var skyboxDay : GameObject;
var skyboxNight : GameObject;
var tapType : int = 3; // 1 = short tap 2 = long tap 3 = any tap
private var thisScript : toggleLighting;

var roomLightOn : boolean = true;
// audio clip to play for switch
public var clickSound: AudioClip;

@script RequireComponent(AudioSource);
@script RequireComponent(s3dInteractor);

function Start() {
	if (gameObject == masterSwitch) {
		if (!skyboxDay || !skyboxNight) {
			useSkyBox = false;
		}
		roomLightOn = !roomLightOn; // initial double swap
		switchLighting();
	}
	thisScript = masterSwitch.GetComponent(toggleLighting);
}

function NewTap(params: TapParams) {
	if (params.tap == tapType || tapType == 3) {
		if (clickSound) audio.PlayOneShot(clickSound);
		thisScript.switchLighting();
	}
} 

function switchLighting() {
	roomLightOn = !roomLightOn;
	if (roomLightOn) {
        roomlight.light.intensity = roomlightIntensityOn;
        if (useSkyBox) {
        	skyboxDay.active = true;
        	skyboxNight.active = false;
        }
	} else {
        roomlight.light.intensity = roomlightIntensityOff;
        if (useSkyBox) {
        	skyboxDay.active = false;
        	skyboxNight.active = true;
        }
	}
}