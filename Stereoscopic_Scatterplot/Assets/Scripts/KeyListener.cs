using UnityEngine;
using System.IO;

public class KeyListener : MonoBehaviour {
	//state vars
	private Vector3? oldAV = null;
	
	// Update is called once per frame
	void Update () {
		//listen for key 
		if (Input.GetKeyDown ("x")) {
			if (oldAV.HasValue) {
				this.rigidbody.angularVelocity = (Vector3) oldAV;
				oldAV = null;
			} else {
				oldAV = rigidbody.angularVelocity;
				this.rigidbody.angularVelocity = new Vector3(0,0,0);
			}
		} else if (Input.GetKeyDown ("r")) { 
			this.rigidbody.rotation = new Quaternion(0,0,0,1);
			this.rigidbody.angularVelocity = new Vector3(0,0,0);
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			float f = rigidbody.angularVelocity.y;
			this.rigidbody.angularVelocity = new Vector3(0, f + .2f,0);
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			float f = rigidbody.angularVelocity.y;
			this.rigidbody.angularVelocity = new Vector3(0,f-.2f,0);
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			float f = rigidbody.angularVelocity.x;
			this.rigidbody.angularVelocity = new Vector3(f - .2f,0,0);
		} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			float f = rigidbody.angularVelocity.x;
			this.rigidbody.angularVelocity = new Vector3(f + .2f,0,0);
		}
	}
}
