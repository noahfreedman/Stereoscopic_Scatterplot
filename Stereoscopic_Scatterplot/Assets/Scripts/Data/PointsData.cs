using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PointsData : StatsData {
	public List<Vector3S> points;
	public string name = "Points";
	
	public PointsData () {
		this.points = new List<Vector3S>();
	}
	public PointsData (List<Vector3S> points) {
		this.points = points;
	}
	public PointsData (List<Vector3> vectors) {
		this.points = new List<Vector3S>();
		foreach (Vector3 vector in vectors) {
			this.points.Add(new Vector3S(vector));
		}
	}
	public void AddPoint (Vector3 point) {
		points.Add(new Vector3S(point));
	}
}


