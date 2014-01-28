using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PointsData : StatsData {
	public List<Vector3S> points;
	public string name = "Points";
	
	public PointsData (string name = "Points") {
		this.points = new List<Vector3S>();
		this.name = name;
	}
	public PointsData (List<Vector3S> points, string name = "Points") {
		this.points = points;
		this.name = name;
	}
	public PointsData (List<Vector3> vectors, string name = "Points") {
		this.points = new List<Vector3S>();
		foreach (Vector3 vector in vectors) {
			this.points.Add(new Vector3S(vector));
		}
		this.name = name;
	}
	public void AddPoint (Vector3 point) {
		points.Add(new Vector3S(point));
	}
}


