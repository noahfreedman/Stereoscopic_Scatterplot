using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LinesList : MonoBehaviour {

	public Material lineMaterial;
	private float scaleFactor = .01f; // todo; monodevelop has lost the ability to shift1
	void Start () {
	}
	public GameObject AddALine(Vector3 point1, Vector3 point2, Vector3 cameraPos) {
		LineData lineData = new LineData(point1, point2);
		lineData.scale = Vector3.Distance((point1 + point2) / 2, cameraPos) * scaleFactor;
		return AddALine(lineData);
	}
	public GameObject AddALine (LineData lineData) {
		GameObject line = new GameObject ("Line");
		LineRenderer lineRenderer = line.AddComponent ("LineRenderer") as LineRenderer;
		lineRenderer.SetWidth (lineData.scale, lineData.scale);
		lineRenderer.SetPosition (0, lineData.point1.ToVector3());
		lineRenderer.SetPosition (1, lineData.point2.ToVector3());
		lineRenderer.transform.parent = transform;
		lineRenderer.renderer.material = lineMaterial;
		DataHolder holder = (DataHolder) line.AddComponent("DataHolder");
		holder.StatsData = lineData;
		return line;
	}
}
