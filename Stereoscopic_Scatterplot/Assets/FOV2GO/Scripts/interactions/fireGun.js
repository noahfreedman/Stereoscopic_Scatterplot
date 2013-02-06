/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

#pragma strict
@script RequireComponent(AudioSource);
@script RequireComponent(s3dInteractor);

private var lightGameObject : GameObject;

function Go() {
	audio.Play();
	yield WaitForSeconds(0.1);
	transform.Translate(Vector3.forward * -0.1);
	lightGameObject = new GameObject("gunshot");
	lightGameObject.transform.localPosition = transform.position+transform.forward;
	lightGameObject.AddComponent(Light);
    lightGameObject.light.intensity = 50;
	lightGameObject.light.range = 50;
	lightGameObject.light.type = LightType.Point;
	yield WaitForSeconds(0.1);
	lightGameObject.transform.localPosition = transform.position+transform.forward*5;
   	lightGameObject.light.intensity = 25;
	lightGameObject.light.range = 25;
	yield WaitForSeconds(0.1);
	Destroy(lightGameObject);
	transform.Translate(Vector3.forward * 0.1);
}
