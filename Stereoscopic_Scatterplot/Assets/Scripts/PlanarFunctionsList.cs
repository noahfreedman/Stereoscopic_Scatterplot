using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlanarFunctionsList : MonoBehaviour {

	public List<GameObject> createdPlanars;
	public Material planarMaterial;
	void Start () {
		createdPlanars = new List<GameObject> ();
	}

	public void AddPlanar(ArrayList points, int width, int height) {
		GameObject planar = GeneratePlane.GeneratePlane(GameObject.Find("Inventory"), points, width, height);

		planar.transform.parent = transform;
		createdPlanars.Add (planar);
		planar.renderer.material = planarMaterial;
	}

}
