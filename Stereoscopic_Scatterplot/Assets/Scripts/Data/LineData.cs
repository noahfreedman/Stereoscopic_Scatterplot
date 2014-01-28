using System;
using UnityEngine;

[Serializable]
public class LineData : StatsData {
	public Vector3S point1;
	public Vector3S point2;
	
	public LineData (Vector3S point1, Vector3S point2) {
		this.point1 = point1;
		this.point2 = point2;
	}
	public LineData (Vector3 point1, Vector3 point2) {
		this.point1 = new Vector3S(point1);
		this.point2 = new Vector3S(point2);
	}
	public void SetData(Vector3 point1, Vector3 point2) {
		this.point1 = new Vector3S(point1);
		this.point2 = new Vector3S(point2);
	}
}


