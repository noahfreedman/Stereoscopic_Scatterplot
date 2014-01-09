using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectContainer : MonoBehaviour {

	protected List<GameObject> gameObjects;
	public bool saveMe = true;

	public ObjectContainer () {
		gameObjects = new List<GameObject> ();
	}

}


