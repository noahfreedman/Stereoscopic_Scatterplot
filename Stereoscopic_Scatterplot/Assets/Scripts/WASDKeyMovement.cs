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
								transform.Translate (Vector3.forward);
						} else if (Input.GetKey ("s")) { 
								transform.Translate (Vector3.back);
						} 
						if (Input.GetKey ("a")) {
								transform.Translate (Vector3.right);
						} else if (Input.GetKey ("d")) { 
								transform.Translate (Vector3.left);
						} 
				}
		
		}


}