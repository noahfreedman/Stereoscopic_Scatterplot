using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlanesList : MonoBehaviour {

	public List<GameObject> createdPlanes;
	public Material planeMaterial;
	void Start () {
		createdPlanes = new List<GameObject> ();
	}

	public void AddPlane (Vector3 point1, Vector3 point2) {
		//LineRenderer plane = new GameObject ("Line").AddComponent ("LineRenderer") as LineRenderer;
		GameObject plane = GameObject.CreatePrimitive (PrimitiveType.Plane);

		plane.transform.position = point1;

		plane.transform.parent = transform;
		createdPlanes.Add (plane);
		plane.renderer.material = planeMaterial;
	}

}
