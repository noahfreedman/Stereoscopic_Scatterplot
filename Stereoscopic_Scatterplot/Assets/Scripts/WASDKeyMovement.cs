using UnityEngine;
using System.IO;

public class WASDKeyMovement : MonoBehaviour
{
		public int mouseButtonWASD = 1;
		public bool requireMouseButton = false;
		private float CurrentSpeed = 1.0f;
		private float Speed = 1.0f;
		private float FastSpeed = 4.0f;

		void LateUpdate ()
		{
				if (Input.GetKey (KeyCode.LeftShift)) {
						CurrentSpeed = FastSpeed;
				} else {
						CurrentSpeed = Speed;
				}
				if (requireMouseButton & Input.GetMouseButton (mouseButtonWASD) || !requireMouseButton) {

						if (Input.GetKey ("w") || Input.GetKey (KeyCode.UpArrow)) {
								transform.Translate (Vector3.forward * CurrentSpeed);
						} else if (Input.GetKey ("s") || Input.GetKey (KeyCode.DownArrow)) { 
								transform.Translate (Vector3.back * CurrentSpeed);
						} 
						if (Input.GetKey ("a") || Input.GetKey (KeyCode.LeftArrow)) {
								transform.Translate (Vector3.left * CurrentSpeed);
						} else if (Input.GetKey ("d") || Input.GetKey (KeyCode.RightArrow)) { 
								transform.Translate (Vector3.right * CurrentSpeed);
						} 
				}
		
		}


}