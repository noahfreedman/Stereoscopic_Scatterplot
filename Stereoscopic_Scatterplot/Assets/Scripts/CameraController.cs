using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{		
		private bool _freezeCamera = false;
		public bool FreezeCamera	{
			get { return _freezeCamera; }
			set {
				if (_freezeCamera != value) {
					_freezeCamera = value;
					if (_freezeCamera) {
						((MonoBehaviour) GetComponent("MouseOrbitClicked")).enabled = false;
						GetComponent<MouseLook>().enabled = false;
						GetComponent<WASDKeyMovement>().enabled = false;
						GetComponent<DemoOrbitCamera>().enabled = false;
					} else {
						((MonoBehaviour) GetComponent("MouseOrbitClicked")).enabled = true;
						GetComponent<MouseLook>().enabled = true;
						GetComponent<WASDKeyMovement>().enabled = true;
						GetComponent<DemoOrbitCamera>().enabled = true;
					}
				}
			}
		}
		public CameraController ()
		{
		}
}


