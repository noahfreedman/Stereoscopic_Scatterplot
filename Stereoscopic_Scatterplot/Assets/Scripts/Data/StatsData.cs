using UnityEngine;
using System;

//Base class for data stored on 3D stats objects
[Serializable()]
public class StatsData {

	public ColorS _color = null;
	public float scale = 1f;

	public StatsData () {

	}

	public Color? color {
		get { 
			if (_color == null) { 
				return null; 
			} else { 
				return (Color?) _color.ToColor(); 
			}
		}
		set { 
			_color = new ColorS(value);
		}
	}
}

