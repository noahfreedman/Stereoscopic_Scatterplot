/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* automatically set FOV2GO layout for Android devices
 * works together with s3dDeviceManager.js
 * checks resolution of device and - if resolution matches any supported Android device -
 * switches to that screen layout
 * so a single app can run on all supported devices
 * checks both orientations - layouts are for landscape orientation, but checks for portrait
 * just in case app is starting up in portrait, assuming it will be rotated to landscape in a moment
 * (s3dGyroCam.js monitors AutoRotation & updates display when it changes) 
 */
 
#pragma strict
private var s3dDeviceMan : s3dDeviceManager;
@script RequireComponent(s3dDeviceManager);

function Start () {
	checkResolutionToDetermineDevice();
}	

function checkResolutionToDetermineDevice() {
	yield WaitForSeconds(1.0);
	s3dDeviceMan = gameObject.GetComponent(s3dDeviceManager);
	if (s3dDeviceMan) {
		if (Screen.width == 800 && Screen.height == 480 || Screen.width == 480 && Screen.height == 800) {
			s3dDeviceMan.phoneLayout = phoneType.Thrill_LandLeft;
		} else if (Screen.width == 960 && Screen.height == 540 || Screen.width == 540 && Screen.height == 960) {
			s3dDeviceMan.phoneLayout = phoneType.OneS_LandLeft;
		} else if (Screen.width == 1280 && Screen.height == 720 || Screen.width == 720 && Screen.height == 1280) {
			s3dDeviceMan.phoneLayout = phoneType.GalaxyNexus_LandLeft;
		} else if (Screen.width == 1280 && Screen.height == 800 || Screen.width == 800 && Screen.height == 1280) {
			s3dDeviceMan.phoneLayout = phoneType.GalaxyNote_LandLeft;
		} else {
			// (resolution doesn't match any iOS device, so leave it alone)
		}
		s3dDeviceMan.setPhoneLayout();
		s3dDeviceMan.camera3D.initStereoCamera();
	}
}
