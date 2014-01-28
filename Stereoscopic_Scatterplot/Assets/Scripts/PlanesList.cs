using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlanesList : MonoBehaviour {

	public Material planeMaterial;

	public PlanesList() {

	}
	public GameObject AddPlane(Vector3 point1, Vector3 point2) {
		PlaneData planeData = new PlaneData(point1, point2);
		return AddPlane(planeData);
	}
	public GameObject AddPlane (PlaneData planeData) {
		GameObject plane = GameObject.CreatePrimitive (PrimitiveType.Plane);

		plane.transform.position = planeData.point1;

		plane.transform.parent = transform;
		plane.renderer.material = planeMaterial;
		DataHolder holder = (DataHolder) plane.AddComponent("DataHolder");
		holder.StatsData = planeData;

		return plane;
	}
}
