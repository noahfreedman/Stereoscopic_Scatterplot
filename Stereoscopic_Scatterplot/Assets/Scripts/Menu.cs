// Menu and options

// Warning: lots of slop in here / work & eperiments in progress
using UnityEngine;
using System.Collections;
using System.IO;

public class Menu : MonoBehaviour
{


		public Texture MenuButtonTexture;
		public Texture OptionButtonTexture;
		public Texture CloseButtonTexture;
		public Transform TargetTransform;
		private bool ShowMenu = false;
		private bool ShowCreateLineMenu = false;
		private Rect fullScreenRect = new Rect (0, 0, Screen.width, Screen.height);
		private string LoadPath = "";
		private Rect MenuRect = new Rect (0, 30, 130, 300);
		// Settings
		private int CameraModeSetting = 0;
		
		// Temporary crap while I figure out how to clean up numerical input 

		private string sLine_0_X = "0.0";
		private string sLine_0_Y = "0.0";
		private string sLine_0_Z = "0.0";
		private string sLine_1_X = "0.0";
		private string sLine_1_Y = "0.0";
		private string sLine_1_Z = "0.0";
		
		void Update ()
		{

				// esc opens the option menu
				if (Input.GetKeyDown (KeyCode.Escape)) {
						Debug.Log ("ESC");
						ShowMenu = !ShowMenu;
						ShowCreateLineMenu = false;
				}
		}
		// TODO: Disable buttons in the first row of menu options 
		void OnGUI ()
		{
				if (ShowMenu) {
						GUILayout.BeginHorizontal ();
						GUILayout.BeginVertical ();
						if (GUILayout.Button ("X")) {
								Debug.Log ("X clicked");
								ShowMenu = false;
						}
						GUILayout.EndVertical ();
						GUILayout.BeginVertical ();

						if (GUILayout.Button ("Create Line...")) {
								ShowCreateLineMenu = true;
						}
						GUI.enabled = false;
						if (GUILayout.Button ("Two")) {
								Debug.Log ("Two clicked");
								ShowMenu = false;
						}
						GUI.enabled = true;
						if (GUILayout.Button ("Three")) {
								Debug.Log ("Three clicked");
								ShowMenu = false;
						}
						if (GUILayout.Button ("Four")) {
								Debug.Log ("Four clicked");
								ShowMenu = false;
						}
						GUILayout.Box ("Path:   ");
						GUILayout.TextField (LoadPath);
						if (GUILayout.Button ("Load")) {
								Debug.Log ("Load clicked");
								ShowMenu = false;
						}
						if (GUILayout.Button ("Save...")) {
								Debug.Log ("Save clicked");
								ShowMenu = false;
						}

						if (GUILayout.Button ("Quit.")) {
								ShowMenu = false;
								Application.Quit ();
						}
						GUILayout.EndVertical ();
						GUILayout.BeginVertical ("box");
						if (ShowCreateLineMenu) {
								// vertor coord entry grid
								GUILayout.BeginVertical ();
								GUILayout.BeginHorizontal ();
								GUILayout.Label ("Point 1");
								sLine_0_X = GUILayout.TextField (sLine_0_X);
								sLine_0_Y = GUILayout.TextField (sLine_0_Y);
								sLine_0_Z = GUILayout.TextField (sLine_0_Z);
								GUILayout.EndHorizontal ();
								GUILayout.BeginHorizontal ();
								GUILayout.Label ("Point 2");
								sLine_1_X = GUILayout.TextField (sLine_1_X);
								sLine_1_Y = GUILayout.TextField (sLine_1_Y);
								sLine_1_Z = GUILayout.TextField (sLine_1_Z);
				
								GUILayout.EndHorizontal ();
								GUILayout.EndVertical ();				
								if (GUILayout.Button ("Create")) {
										if (TargetTransform) {
												LineRenderer line = TargetTransform.gameObject.AddComponent<LineRenderer> ();
												line.SetWidth (0.1F, 0.1F);
												line.SetPosition (0, VectorFromStrings(sLine_0_X, sLine_0_Y, sLine_0_Z));
												line.SetPosition (1, VectorFromStrings(sLine_1_X, sLine_1_Y, sLine_1_Z));
										}
								}
								if (GUILayout.Button ("Delete")) {
									Destroy(TargetTransform.gameObject.GetComponent("LineRenderer"));
								}
								if (GUILayout.Button ("Back")) {
									ShowCreateLineMenu = false;
								}
						}
						GUILayout.EndVertical ();

						GUILayout.EndHorizontal ();

				} else {
						if (MenuButtonTexture) {
								if (GUI.Button (new Rect (0, 0, MenuButtonTexture.height + 8, MenuButtonTexture.width + 8), MenuButtonTexture)) {
										Debug.Log ("One clicked");
										ShowMenu = true;
								}
						} else {
								if (GUI.Button (new Rect (0, 0, 100, 20), "menu"))
										Debug.Log ("One clicked");
								ShowMenu = true;
						}
				}
		}
		
		public Vector3 VectorFromStrings (string x, string y, string z)
		{
			float float_X;
			float float_Y;
			float float_Z;
			float.TryParse (GUILayout.TextField (x), out float_X);
			float.TryParse (GUILayout.TextField (y), out float_Y);
			float.TryParse (GUILayout.TextField (z), out float_Z);
			return new Vector3 (float_X, float_Y, float_Z);
		
		}
}
