using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlanesList : ObjectContainer {

	public Material planeMaterial;

	public PlanesList() {

	}

	public void AddPlane (Vector3 point1, Vector3 point2) {
		GameObject plane = GameObject.CreatePrimitive (PrimitiveType.Plane);

		plane.transform.position = point1;

		plane.transform.parent = transform;
		plane.renderer.material = planeMaterial;
		PlaneData data = (PlaneData) plane.AddComponent("PlaneData");
		data.point1 = point1;
		data.point2 = point2;
		gameObjects.Add(plane);
	}
}
