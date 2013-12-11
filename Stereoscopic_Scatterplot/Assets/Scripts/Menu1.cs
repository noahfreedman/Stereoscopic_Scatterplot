using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Menu1 : MonoBehaviour
{
		public bool doubleMenu = false;
		public bool doubleMenuInSteroMode = true;
		public GameObject Stage;//TODO:It's oddly named
		public Camera mainCamera;
		public GameObject AxisObject;
		public Texture OnButtonTexture;
		public Texture OffButtonTexture;
		private bool ShowMenu = false;
		private bool ShowCreateLineMenu = false;
		private bool ShowCreatePlaneMenu = false;
		private bool ShowLoadPointsMenu = false;
		private bool ShowGenPointsMenu = false;
		private bool ShowOptionsMenu = false;
		private bool ShowCreateSinglePointMenu = false;
		private Rect fullScreenRect = new Rect (0, 0, Screen.width, Screen.height);
		private Rect MenuRect = new Rect (0, 30, 130, 300);
		// Settings
		private int CameraModeSetting = 0;
		private bool DisableMenu = false;
		//TODO:rename refactor

		private string string_X_0 = "0.0";
		private string string_Y_0 = "0.0";
		private string string_Z_0 = "0.0";
		private string string_X_1 = "0.0";
		private string string_Y_1 = "0.0";
		private string string_Z_1 = "0.0";
		private Vector3 centerPosition;
		private string menuFileName = "default.csv";
		private string menuFilePath = "";
		private string LoadPath = "";
		private int menuWidth = 400;

		void Start ()
		{
				centerPosition = Vector3.zero;
				menuFileName = Stage.GetComponent<LoadPoints> ().fileName.ToString ();
				menuFilePath = Stage.GetComponent<LoadPoints> ()._filePath.ToString ();
		}

		void Update ()
		{
				// esc opens the option menu, closes submenus.
				if (Input.GetKeyDown (KeyCode.Escape)) {
						BackButton ();
				}
		}

		void BackButton ()
		{
				ShowMenu = !ShowMenu;
				ShowCreateLineMenu = false;
				ShowCreatePlaneMenu = false;
				ShowLoadPointsMenu = false;
				ShowGenPointsMenu = false;
				ShowOptionsMenu = false;
				ShowCreateSinglePointMenu = false;
		}

		void OnGUI ()
		{

				GUILayout.BeginArea (new Rect (2, 0, menuWidth, 700));
				DisplayMenu ();
				GUILayout.EndArea ();
				if (doubleMenu) {
						GUILayout.BeginArea (new Rect (Screen.width / 2, 0, menuWidth, 700));
						DisplayMenu ();
						GUILayout.EndArea ();
				}
		}

		void MainMenuButton ()
		{
				if (OnButtonTexture) {
						if (GUI.Button (new Rect (0, 0, OnButtonTexture.height + 8, OnButtonTexture.width + 8), OnButtonTexture)) {

								ShowMenu = true;
						}
				} else {
						if (GUI.Button (new Rect (0, 0, 100, 20), "menu"))

								ShowMenu = true;
				}

		}

		public Vector3 GUIVectorFromStrings (string x, string y, string z)
		{
				float float_X;
				float float_Y;
				float float_Z;
				GUILayout.BeginHorizontal ();
				float.TryParse (GUILayout.TextField (x), out float_X);
				float.TryParse (GUILayout.TextField (y), out float_Y);
				float.TryParse (GUILayout.TextField (z), out float_Z);
				GUILayout.EndHorizontal ();
				return new Vector3 (float_X, float_Y, float_Z);

		}

		public Vector3 VectorFromStrings (string x, string y, string z)
		{
				float float_X;
				float float_Y;
				float float_Z;
				float.TryParse (x, out float_X);
				float.TryParse (y, out float_Y);
				float.TryParse (z, out float_Z);
				return new Vector3 (float_X, float_Y, float_Z);
		
		}

		void MenuCreatePlanes ()
		{
				GUI.enabled = true;
				GUILayout.BeginVertical ("box");
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Point 1");
				string_X_0 = GUILayout.TextField (string_X_0);
				string_Y_0 = GUILayout.TextField (string_Y_0);
				string_Z_0 = GUILayout.TextField (string_Z_0);
				GUILayout.EndHorizontal ();

				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Point 2");
				string_X_1 = GUILayout.TextField (string_X_1);
				string_Y_1 = GUILayout.TextField (string_Y_1);
				string_Z_1 = GUILayout.TextField (string_Z_1);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				
				if (GUILayout.Button ("Create Plane")) {
						Vector3 startPosition = GUIVectorFromStrings (string_X_0, string_Y_0, string_Z_0);
						Vector3 endPosition = VectorFromStrings (string_X_1, string_Y_1, string_Z_1);
						Stage.GetComponent<PlanesList> ().AddPlane (startPosition, endPosition);
				}
				if (GUILayout.Button ("Back")) {
						BackButton ();
				}
				GUILayout.EndHorizontal ();
				GUILayout.EndVertical ();
		}
	
		void MenuCreateSinglePoint ()
		{
				GUI.enabled = true;
				GUILayout.BeginVertical ("box");
		
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Point X Y Z:");
				string_X_0 = GUILayout.TextField (string_X_0);
				string_Y_0 = GUILayout.TextField (string_Y_0);
				string_Z_0 = GUILayout.TextField (string_Z_0);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button ("Create Point")) {
			
						Vector3 point = GUIVectorFromStrings (string_X_0, string_Y_0, string_Z_0);
			
						Stage.GetComponent<RandomPoints> ().createSinglePoint (point);
				}
				if (GUILayout.Button ("Back")) {
						BackButton ();
				}
				GUILayout.EndHorizontal ();
				GUILayout.EndVertical ();
		}

		void MenuGeneratePoints ()
		{
				GUILayout.BeginVertical ("box");
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Point 1");
				string_X_0 = GUILayout.TextField (string_X_0);
				string_Y_0 = GUILayout.TextField (string_Y_0);
				string_Z_0 = GUILayout.TextField (string_Z_0);
				GUILayout.EndHorizontal ();
		
				centerPosition = VectorFromStrings (string_X_0, string_Y_0, string_Z_0);
	
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button ("Generate")) {
						Stage.GetComponent<RandomPoints> ().createRandomPoints (centerPosition, 333);
				}
				if (GUILayout.Button ("Back")) {
						BackButton ();
				}
				
				GUILayout.EndHorizontal ();
		}

		void MenuLoadPoints ()
		{
				string fp = Stage.GetComponent<LoadPoints> ()._filePath;
				string fn = Stage.GetComponent<LoadPoints> ().fileName;
				string labelName = System.IO.Path.Combine (fp, fn);

				GUILayout.BeginVertical ("box");
				GUILayout.Label (labelName);
				
				menuFileName = GUILayout.TextField (menuFileName);
				
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button ("Set")) {
						Stage.GetComponent<LoadPoints> ().fileName = menuFileName;

				}
				if (GUILayout.Button ("Load")) {
						Stage.GetComponent<LoadPoints> ().LoadPointsFile ();
				}
				if (GUILayout.Button ("Back")) {
						BackButton ();
				}
				GUILayout.EndHorizontal ();
				
				GUILayout.EndVertical ();
				
				GUILayout.BeginVertical ("box");
				List<string> foundRecentFilenames = Stage.GetComponent<LoadPoints> ().recentFiles;
				if (foundRecentFilenames.Count > 0) {
						foreach (string recentFile in foundRecentFilenames) {
								GUILayout.Label (recentFile);
						}
						GUILayout.EndVertical ();
				}
			
		}

		void MenuCreateLine ()
		{
				GUILayout.BeginVertical ("box");
				// vertex coord entry grid
				GUILayout.BeginVertical ();
				GUILayout.BeginHorizontal ();
				GUI.enabled = true;
				GUILayout.Label ("Point 1");
				// TODO: rename sLine_0_X
				string_X_0 = GUILayout.TextField (string_X_0);
				string_Y_0 = GUILayout.TextField (string_Y_0);
				string_Z_0 = GUILayout.TextField (string_Z_0);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Point 2");
				string_X_1 = GUILayout.TextField (string_X_1);
				string_Y_1 = GUILayout.TextField (string_Y_1);
				string_Z_1 = GUILayout.TextField (string_Z_1);
				GUILayout.EndHorizontal ();
				GUILayout.EndVertical ();
				if (GUILayout.Button ("Create Line")) {
						Vector3 startPosition = GUIVectorFromStrings (string_X_0, string_Y_0, string_Z_0);
						Vector3 endPosition = GUIVectorFromStrings (string_X_1, string_Y_1, string_Z_1);
						Stage.GetComponent<LinesList> ().AddALine (startPosition, endPosition);
				}
				if (GUILayout.Button ("Back")) {
						BackButton ();
				}
				GUILayout.EndVertical ();
		}

		void MenuOptions ()
		{
				string csvPath = Stage.GetComponent<LoadPoints> ()._filePath.ToString ();
				string csvFilename = Stage.GetComponent<LoadPoints> ().fileName.ToString ();

				GUI.enabled = true;
				GUILayout.BeginVertical ("box");

				AxisObject.active = GUILayout.Toggle (AxisObject.active, "Show Axis?");
				doubleMenu = GUILayout.Toggle (doubleMenu, "Show Double Menu?");
				Stage.GetComponent<Inventory> ().doubleMenu = doubleMenu;

				if (GUILayout.Button ("Back")) {
						BackButton ();
				}
				GUILayout.EndVertical ();
		}
	
		void DisplayMenu ()
		{
				if (ShowMenu) {
			
						GUILayout.BeginHorizontal ();
						GUILayout.BeginVertical ();  // column one - the open/close menu button
						MenuMain ();
						/////////// SUBMENUS
			
						if (ShowCreateLineMenu) {
								MenuCreateLine ();
						}
			
						if (ShowLoadPointsMenu) {
								MenuLoadPoints ();
						}
			
						if (ShowGenPointsMenu) {
								MenuGeneratePoints ();
						}
			
						if (ShowCreatePlaneMenu) {
								MenuCreatePlanes ();
						}
						if (ShowOptionsMenu) {
								MenuOptions ();
						}
						if (ShowCreateSinglePointMenu) {
								MenuCreateSinglePoint ();
						}
						GUILayout.EndVertical ();
						GUILayout.EndHorizontal ();
			
				} else {
						MainMenuButton ();
				}
		}
	
		void MenuMain ()
		{
				//Menu Hides itself here
				if (ShowCreateLineMenu 
					|| ShowCreatePlaneMenu 
					|| ShowLoadPointsMenu 
					|| ShowGenPointsMenu 
					|| ShowOptionsMenu 
					|| ShowCreateSinglePointMenu) {
				
				} else {
						GUILayout.BeginVertical ("box");
			
						if (GUILayout.Button ("Create Line..")) {
								ShowCreateLineMenu = true;
						}
						if (GUILayout.Button ("Create Plane..")) {
								ShowCreatePlaneMenu = true;
						}
						if (GUILayout.Button ("Create Point..")) {
								ShowCreateSinglePointMenu = true;
						}
						if (GUILayout.Button ("Load Point Data..")) {
								ShowLoadPointsMenu = true;
						}
						if (GUILayout.Button ("Generate Scatter..")) {
								ShowGenPointsMenu = true;
						}
						if (GUILayout.Button ("Options")) {
								ShowOptionsMenu = true;
						}
						if (GUILayout.Button ("Quit.")) {
								ShowMenu = false;
								// TODO: Application.Quit isn't working in debug... be sure to try in a build
								Application.Quit ();
						}
						GUILayout.EndVertical ();
						GUI.enabled = true;
				}
		}
	
}
