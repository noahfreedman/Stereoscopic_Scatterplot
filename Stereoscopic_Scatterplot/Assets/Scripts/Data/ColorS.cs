using System;
using UnityEngine;

[Serializable]
public class ColorS {
	private float r,g,b,a;

	public ColorS (float r, float g, float b, float a = 1f) {
		this.r = r;
		this.g = g;
		this.b = b;
		this.a = a;
	}
	public ColorS(Color c) {
		this.r = c.r;
		this.g = c.g;
		this.b = c.b;
		this.a = c.a;
	}
	public ColorS(Color? cc) {
		if (cc != null) {
			Color c = (Color) cc;
			this.r = c.r;
			this.g = c.g;
			this.b = c.b;
			this.a = c.a;
		}
	}

	public Color ToColor() {
		return new Color(r, g, b, a);
	}
}

