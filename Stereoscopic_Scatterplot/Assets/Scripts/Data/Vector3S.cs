using System;
using UnityEngine;

[Serializable]
public class Vector3S {
	private float x, y, z;

	public Vector3S (float x, float y, float z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}
	public Vector3S(Vector3 v) {
		this.x = v.x;
		this.y = v.y;
		this.z = v.z;
	}

	public String ToString() {
		return x.ToString() + ", " + y.ToString() + ", " + z.ToString();
	}

	public Vector3 ToVector3() {
		return new Vector3(x, y, z);
	}
}

