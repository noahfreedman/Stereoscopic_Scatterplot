using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlanarFunctionsList : MonoBehaviour {

	public Material planarMaterial;
	void Start () {
	}
	
	public GameObject AddPlanar(List<Vector3> points, int width, int height) {
		//GeneratePlane.js is in Standard Assets folder
		GameObject planar = GeneratePlane.GeneratePlane(GameObject.Find("Inventory"), new ArrayList(points), width, height);
		
		planar.transform.parent = transform;
		planar.renderer.material = planarMaterial;
		
		DataHolder holder = (DataHolder) planar.AddComponent("DataHolder");
		holder.StatsData = new PlanarFunctionData(points, width, height);

		return planar;
	}
	public GameObject AddPlanar(PlanarFunctionData data) {
		List<Vector3S> pointsS = data.points;
		int width = data.width;
		int height = data.height;
		List<Vector3> points = new List<Vector3>();
		for (int i = 0, l = pointsS.Count; i < l; i++) {
			points.Add(pointsS[i].ToVector3());
		}
		return AddPlanar (points, width, height);
	}
}
