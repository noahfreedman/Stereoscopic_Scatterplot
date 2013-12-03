using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LinesList : MonoBehaviour {

	public List<LineRenderer> createdPlanes;
	public Material lineMaterial;
	public float linewidth = 0.03F; // todo; monodevelop has lost the ability to shift1
	void Start () {
		createdPlanes = new List<LineRenderer> ();
	}

	public void AddALine (Vector3 point1, Vector3 point2) {
		LineRenderer line = new GameObject ("Line").AddComponent ("LineRenderer") as LineRenderer;
		line.SetWidth (linewidth, linewidth);
		line.SetPosition (0, point1);
		line.SetPosition (1, point2);
		line.transform.parent = transform;
		createdPlanes.Add (line);
		line.renderer.material = lineMaterial;
	}

}
