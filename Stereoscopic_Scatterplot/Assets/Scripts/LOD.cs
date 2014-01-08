using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Level of Detail
///	Switches objects from enabled to disabled based on distance from a transform (The Camera)
///	Vector 2 list represents a list of float ranges - the distances where each object is enabled. 
/// </summary>
public class LOD : MonoBehaviour
{
	public GameObject CameraTransform;
	public List<GameObject> LODs;
	public List<Vector2> LODRanges;
	public float runtimeCameraDistance;

	public float cameraDistance {
		get {
			return Vector3.Distance (CameraTransform.transform.position, transform.position); 
		}
	}
	
	void Start ()
	{
		
	}

	void Update ()
	{
 
		runtimeCameraDistance = cameraDistance;
		for (int i = 0; i < LODs.Count; i++) {

			if ((runtimeCameraDistance >= LODRanges [i].x) & (runtimeCameraDistance < LODRanges [i].y)) {

				LODs [i].SetActive (true);

			} else {

				LODs [i].SetActive (false);
			}
		}
	}
}
