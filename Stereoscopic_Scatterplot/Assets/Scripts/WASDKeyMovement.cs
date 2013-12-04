using UnityEngine;
using System.IO;

public class WASDKeyMovement : MonoBehaviour
{
		public int mouseButtonWASD = 1;
		public bool requireMouseButton = false;
		public bool arrowKeysEnabled = false;
		private Vector3 Velocity;
		private float Mass = 1;
		
		void LateUpdate ()
		{
				//		if (Input.GetKey(KeyCode.LeftShift)) {
				//			currentScrollSpeed = scrollSpeedFast;
				//		}   	
				if (requireMouseButton & Input.GetMouseButton (mouseButtonWASD) || !requireMouseButton) {

						if (Input.GetKey ("w")) {
								Move (1);
						} else if (Input.GetKey ("s")) { 
								Move (-1);
						} 
						if (Input.GetKey ("a")) {
								Strafe (1);
								Debug.Log('a');
						} else if (Input.GetKey ("d")) { 
								Strafe (-1);
						} 
				}
		
		}

		void Strafe (int direction)
		{
			transform.position = transform.position + Vector3.left * direction;
		}

		void Move (int direction)
		{
			transform.position = transform.position + Vector3.forward * direction;
		}
}