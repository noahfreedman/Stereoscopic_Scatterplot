using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LOD : MonoBehaviour {
	public GameObject CameraTransform;
	public List<GameObject> LODs;
	public List<Vector2> LODRanges;
	// Use this for initialization
	public float runtimeCameraDistance;
	public float cameraDistance 
	{
		get 
		{
			return Vector3.Distance(CameraTransform.transform.position, transform.position); 
		}
	}
	
	void Start () {
		
	}
	void Update () {
		runtimeCameraDistance = cameraDistance;
		for (int i = 0; i < LODs.Count; i++) 
		{
		
			if ((runtimeCameraDistance >= LODRanges[i].x) & (runtimeCameraDistance < LODRanges[i].y))
			{
				
				LODs[i].SetActive(true);
			}
			else
			{
			
				LODs[i].SetActive(false);
			}
		}
	}
}
