using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectContainer : MonoBehaviour {

	protected List<string> persistedProperties;
	protected List<GameObject> gameObjects;

	public ObjectContainer () {
		gameObjects = new List<GameObject> ();
	}

}


