// Menu and options
// TODO: This is going to require a refactoring. 

// Warning: lots of messy stuff in here / work & experiments in progress
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
	public Transform PointPrefab;
	public Material objectMaterial;
	public List<LineRenderer> CreatedLines;
	public List<GameObject> CreatedGameObjects;
	public List<GameObject> DataPointCollections;
	public Texture MenuButtonTexture;
	public Texture OptionButtonTexture;
	public Texture CloseButtonTexture;
	public Transform TargetTransform;
	private bool ShowMenu = false;
	private bool ShowCreateLineMenu = false;
	private bool ShowCreatePlaneMenu = false;
	private bool ShowLoadPointsMenu = false;
	private bool ShowGenPointsMenu = false;
	private Rect fullScreenRect = new Rect (0, 0, Screen.width, Screen.height);
	private string LoadPath = "";
	private Rect MenuRect = new Rect (0, 30, 130, 300);
	// Settings
	private int CameraModeSetting = 0;

	// Temporary mess ...
	private bool DisableMenu = false;
	private string sLine_0_X = "0.0";
	private string sLine_0_Y = "0.0";
	private string sLine_0_Z = "0.0";
	private string sLine_1_X = "0.0";
	private string sLine_1_Y = "0.0";
	private string sLine_1_Z = "0.0";

	void Start ()
	{

		CreatedLines = new List<LineRenderer> ();
		CreatedGameObjects = new List<GameObject> ();
		DataPointCollections = new List<GameObject> ();
	}

	void Update ()
	{

		// esc opens the option menu, closes submenus.
		if (Input.GetKeyDown (KeyCode.Escape)) {

			ToggleMenus ();
		}

	}

	void ToggleMenus ()
	{
		ShowMenu = !ShowMenu;
		ShowCreateLineMenu = false;
		ShowCreatePlaneMenu = false;
		ShowLoadPointsMenu = false;
		ShowGenPointsMenu = false;
	}
	// TODO: Disable buttons in the first col of options when second col active
	void OnGUI ()
	{
		if (ShowMenu) {
			GUILayout.BeginHorizontal ();

			GUILayout.BeginVertical ();
			if (GUILayout.Button ("X")) {
				ToggleMenus ();
			}
			GUILayout.EndVertical ();

			GUILayout.BeginVertical ("box");
			// disable this menu when it's submenus are up
			if (ShowCreateLineMenu ||ShowCreatePlaneMenu || ShowLoadPointsMenu ||ShowGenPointsMenu) {
			
				GUI.enabled = false;
			} else {
				GUI.enabled = true;
			}
			 
			if (GUILayout.Button ("Create Line..")) {
				ShowCreateLineMenu = true;

			}
			if (GUILayout.Button ("Create Plane..")) {
				ShowCreatePlaneMenu = true;
			}
			if (GUILayout.Button ("Load Point Data..")) {
				ShowLoadPointsMenu = true;
			}
			if (GUILayout.Button ("Generate Point Data..")) {
				ShowGenPointsMenu = true;
			}
			if (GUILayout.Button ("Quit.")) {
				ShowMenu = false;
				// TODO: This isn't working - or doesnt work in debug...
				Application.Quit ();
			}
			GUI.enabled = true;
			GUILayout.EndVertical ();
			//////////////////////////////////////    Lines 
			if (ShowCreateLineMenu) {
				GUILayout.BeginVertical ("box");

				// vertor coord entry grid
				GUILayout.BeginVertical ();
				GUILayout.BeginHorizontal ();
				GUI.enabled = true;
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

				if (GUILayout.Button ("Create Line")) {
					if (TargetTransform) {

						LineRenderer line = new GameObject ().AddComponent ("LineRenderer") as LineRenderer;
						line.SetWidth (0.1F, 0.1F);
						line.SetPosition (0, VectorFromStrings (sLine_0_X, sLine_0_Y, sLine_0_Z));
						line.SetPosition (1, VectorFromStrings (sLine_1_X, sLine_1_Y, sLine_1_Z));
						CreatedLines.Add (line);
						line.renderer.material = objectMaterial;
					}
				}
				if (GUILayout.Button ("Back")) {
					ShowCreateLineMenu = false;
				}
				GUILayout.EndVertical ();
			}

			//////////////////////////////////////    Load Points Data 
			if (ShowLoadPointsMenu) {
				GUILayout.BeginVertical ("box");


				if (GUILayout.Button ("Back")) {
					ShowLoadPointsMenu = false;
				}
				GUILayout.EndVertical ();
			}
			//////////////////////////////////////    Generate Points Data 
			if (ShowGenPointsMenu) {
				GUILayout.BeginVertical ("box");
				if (GUILayout.Button ("Generate")) {
					
					//					GameObject pointsObject = new GameObject();
					//					pointsObject.AddComponent<RandomPoints>();
					// GOOD SYNTAX : Stage.GetComponent<RandomPoints>().PointPrefab = PointPrefab;
					//Stage.GetComponent<RandomPoints>().AddRandomPoints(100);
					//Stage.AddComponent("RandomPoints");
					//Stage.GetComponent<Lines>().AddALine(Vector3.zero, Vector3.down);
					//DataPointCollections.Add (pointsObject);
					//Stage.GameObject<RandomPoints>().AddRandomPoints(100);
					//Stage.GetComponent<RandomPointsList>().AddRandomPoints(100);
				}
				if (GUILayout.Button ("Back")) {
					ShowGenPointsMenu = false;
				}
				GUILayout.EndVertical ();
			}

			///////////////////////////////////////////////// 
			if (ShowCreatePlaneMenu) {
				GUILayout.BeginVertical ("box");

				// vertor coord entry grid
				GUILayout.BeginVertical ();
				GUILayout.BeginHorizontal ();
				GUI.enabled = true;
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

				if (GUILayout.Button ("Create Plane")) {
					if (TargetTransform) {
						GameObject plane = GameObject.CreatePrimitive (PrimitiveType.Plane);
						plane.transform.position = VectorFromStrings (sLine_0_X, sLine_0_Y, sLine_0_Z);
						plane.renderer.material = objectMaterial;
						CreatedGameObjects.Add (plane);
					}
				}
				if (GUILayout.Button ("Back")) {
					ShowCreatePlaneMenu = false;
				}
				GUILayout.EndVertical ();
			}
			///////////////////////////////////////////// Created Objects menu
			if (CreatedGameObjects.Count > 0 || CreatedLines.Count > 0 || DataPointCollections.Count > 0) {
				GUILayout.BeginVertical ("box");

				foreach (GameObject o in CreatedGameObjects) {
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button ("x")) {
					}
					GUILayout.Label ("Plane 01");
					GUILayout.EndHorizontal ();
				}

				foreach (LineRenderer l in CreatedLines) {
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button ("x")) {
					}
					GUILayout.Label ("Line 01");
					GUILayout.EndHorizontal ();
				}
				foreach (GameObject v in DataPointCollections) {
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button ("x")) {
					}
					GUILayout.Label ("Data 01");
					GUILayout.EndHorizontal ();
				}
				GUILayout.EndVertical ();
			}
			GUILayout.EndHorizontal ();

		} else {
			if (MenuButtonTexture) {
				if (GUI.Button (new Rect (0, 0, MenuButtonTexture.height + 8, MenuButtonTexture.width + 8), MenuButtonTexture)) {

					ShowMenu = true;
				}
			} else {
				if (GUI.Button (new Rect (0, 0, 100, 20), "menu"))

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
