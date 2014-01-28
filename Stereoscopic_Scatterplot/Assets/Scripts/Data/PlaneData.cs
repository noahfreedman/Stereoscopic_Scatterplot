using System;
using UnityEngine;

[Serializable]
public class PlaneData : StatsData {
	public Vector3S _point1;
	public Vector3S _point2;

	public PlaneData (Vector3 point1, Vector3 point2) {
		this.point1 = point1;
		this.point2 = point2;
	}
	public Vector3 point1 {
		get { return _point1.ToVector3(); }
		set { 
			_point1 = new Vector3S(value);
		}
	}
	public Vector3 point2 {
		get { return _point2.ToVector3(); }
		set { 
			_point2 = new Vector3S(value);
		}
	}
}


