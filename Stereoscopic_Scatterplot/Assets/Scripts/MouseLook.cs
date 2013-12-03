using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{
	
		public enum RotationAxes
		{
				MouseXAndY = 0,
				MouseX = 1,
				MouseY = 2
		}
		public RotationAxes axes = RotationAxes.MouseXAndY;
		public float sensitivityX = 15f;
		public float sensitivityY = 15f;
		public float minimumX = -360f;
		public float maximumX = 360f;
		public float minimumY = -100f;
		public float maximumY = 100f;
		float rotationY = 0f;
		public int mouseButton = 1;
	
		void Update ()
		{
		
		
				if (Input.GetMouseButton (mouseButton)) {
						if (axes == RotationAxes.MouseXAndY) {
								float rotationX = transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * sensitivityX;
			
								rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
								rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
								transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0);
						} else if (axes == RotationAxes.MouseX) {
								transform.Rotate (0, Input.GetAxis ("Mouse X") * sensitivityX, 0);
						} else {
								rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
								rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
								transform.localEulerAngles = new Vector3 (-rotationY, transform.localEulerAngles.y, 0);
						}
				}
		}

}