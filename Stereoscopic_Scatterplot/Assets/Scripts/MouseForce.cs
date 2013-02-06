using System.Collections.Generic;
using UnityEngine;

public class MouseForce : MonoBehaviour
{
	public float acceleration = 500.0f;
	
	private Rigidbody grabBody;
	private Vector3 grabPoint;
	private float grabDistance;
	
	public void Update()
	{
		ReleaseBody();
	}
	
	public void FixedUpdate()
	{
		GrabBody();
		
		MoveBody();
	}
	
	private void GrabBody()
	{
		if (grabBody == null)
		{
			// Let the player grab an object
			if (Input.GetMouseButton(0))
			{
				RaycastHit hit;
				
				if (Physics.Raycast(Camera.mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
				{
					if (hit.rigidbody != null)
					{
						grabBody = hit.rigidbody;
						grabPoint = grabBody.transform.InverseTransformPoint(hit.point);
						grabDistance = hit.distance;
					}
				}
			}
		}
	}
	
	private void ReleaseBody()
	{
		if (grabBody != null)
		{
			// Let the player release the object
			if (Input.GetMouseButtonUp(0))
			{
				grabBody = null;
			}
		}
	}
	
	private void MoveBody()
	{
		if (grabBody != null)
		{
			// Move grabbed object
			Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, grabDistance);
			
			Vector3 targetPoint = Camera.mainCamera.ScreenToWorldPoint(screenPoint);
			Vector3 anchorPoint = grabBody.transform.TransformPoint(grabPoint);
			
			Vector3 impulse = (targetPoint - anchorPoint) * (acceleration * Time.fixedDeltaTime);
			
			grabBody.AddForceAtPosition(impulse, anchorPoint, ForceMode.Acceleration);
		}
	}
}