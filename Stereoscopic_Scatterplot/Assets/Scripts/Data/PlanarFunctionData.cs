using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class PlanarFunctionData : StatsData {
	public List<Vector3S> points;
	public int width;
	public int height;

	public PlanarFunctionData (List<Vector3> points, int width, int height) {
		List<Vector3S> pointsS = new List<Vector3S>();
		for (int i = 0, l = points.Count; i < l; i++) {
			pointsS.Add(new Vector3S(points[i]));
		}
		this.points = pointsS;
		this.width = width;
		this.height = height;
	}

}


