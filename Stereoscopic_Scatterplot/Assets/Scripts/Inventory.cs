using UnityEngine;
using System;
using System.IO;

//using System.IO.Stream;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class Inventory : MonoBehaviour {
	#region
	private int Version = 1;
	public GUISkin MenuSkin;
	public Transform MainCamera;
	public Menu1 MainMenu;
	public Texture ColorButtonTexture;
	public bool StereoMenu = true;
	public bool DoubleMenu = false; // Main menu controls this
	public float MenuWidth = 200;
	private float MenuHeight = 200;
	private string filename = "/Users/Noah/Documents/TestSave.3D";
	private float ScaleMultiplier = 1.05f;
	private GameObject CurrentSelection;
	private Color MenuDefaultGUIColor;
	private Color SelectedColor = Color.green;
	private Color[] AssignableColors;
	#endregion

	void Start() {
		MenuDefaultGUIColor = GUI.contentColor;
		MenuHeight = Screen.height;
		AssignableColors = new Color[5];
		AssignableColors [0] = Color.white;
		AssignableColors [1] = Color.red;
		AssignableColors [2] = Color.green;
		AssignableColors [3] = Color.blue;
		AssignableColors [4] = Color.yellow;
	}
	
	void Update() {
		MenuDefaultGUIColor = GUI.contentColor;
		MenuHeight = Screen.height;
	}
	
	void OnGUI() {
		if (MenuSkin) {
			
			GUI.skin = MenuSkin;
			if (MenuSkin.box.fixedWidth != 0) {
				MenuWidth = MenuSkin.box.fixedWidth;
				// dont override skin set width.
			}
		}
		
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
	
		DisplayMenus ();
		GUILayout.EndArea ();
		
		if (DoubleMenu) {
			GUILayout.BeginArea (new Rect (Screen.width / 2, 0, Screen.width, Screen.height));
			DisplayMenus ();
			GUILayout.EndArea ();
		}
	}
	
	void DisplayMenus() {
		GUI.enabled = true;
		GUILayout.BeginHorizontal ();
		if (StereoMenu) {
			GUILayout.Space((Screen.width / 2) - (MenuWidth * 2));
		} else {
			GUILayout.Space((Screen.width) - (MenuWidth * 2));
		}
		
		if (CurrentSelection == null) {
			
			GUILayout.Space (MenuWidth);
		} else {
			//Close MainMenu if Inventory is open
			MainMenu.CloseMenu();

			GUILayout.BeginVertical ("box", GUILayout.Width (MenuWidth));
			SubMenuObjectProperties (CurrentSelection);
			GUILayout.EndVertical ();
			
		}
		
		GUILayout.BeginVertical ("box", GUILayout.Width (MenuWidth));
		
		SelectableObjectsMenu ();
		
		GUILayout.EndVertical ();
		GUILayout.EndHorizontal ();
		GUI.enabled = true;
		//Debug.Log("DisplayMenus exit");
	}
	
	void SelectableObjectsMenu() {
		if (transform.childCount > 0) {
			foreach (Transform child in transform) {
				
				GUI.color = CurrentButtonColor (child.gameObject);
				if (GUILayout.Button (child.name)) {
					SelectObject (child.gameObject);
				}
				GUI.contentColor = MenuDefaultGUIColor;
				GUI.color = MenuDefaultGUIColor;
				
			}
		} else {
			GUILayout.Label ("0 Objects");
		}
		
		//Debug.Log("SelectableObjectsMenu exit");
		
	}
	
	void SubMenuObjectProperties(GameObject obj) {
		if (CurrentSelection != null) {
			
			GUILayout.Label (obj.name);
			
			if (MainCamera) {
				if (GUILayout.Button ("Look At")) {
					MainCamera.LookAt (obj.transform);
				}
			}
			
			GUILayout.BeginHorizontal ();
			
			if (obj.GetComponent<LineRenderer> ()) {
				GUILayout.Label ("Thickness");
				if (GUILayout.Button (" + ")) {
					SetScaleLine(obj, GetScaleLine(obj) * ScaleMultiplier);
				}
				if (GUILayout.Button (" - ")) {
					SetScaleLine(obj, GetScaleLine(obj) * 1 / ScaleMultiplier);
				}
			} else {
				if ((obj.GetComponent<MeshFilter> ()) && (obj.GetComponent<MeshFilter> ().name == "Plane")) {
					GUILayout.Label ("Plane Size");
					if (GUILayout.Button (" + ")) {
						SetScalePlane (obj, GetScalePlane (obj) * ScaleMultiplier);
					}
					if (GUILayout.Button (" - ")) {
						SetScalePlane (obj, GetScalePlane (obj) * 1f / ScaleMultiplier);
					}
				} else {
					GUILayout.Label ("Point Size");
					if (GUILayout.Button (" + ")) {
						SetScalePoints (obj, GetScalePoints (obj) * ScaleMultiplier);
					}
					if (GUILayout.Button (" - ")) {
						SetScalePoints (obj, GetScalePoints (obj) * 1f / ScaleMultiplier);
					}
					
				}
			}
			GUILayout.EndHorizontal ();
			
			ColorButtons (obj);
			
			
			if (GUILayout.Button ("Delete")) {
				Destroy (obj); // note this makes SelectedObject null, making it unselected.
			}
			
		}
		
	}
	
	void SelectObject(GameObject obj) {
		if (CurrentSelection) {
			if (CurrentSelection.GetInstanceID () == obj.GetInstanceID ()) {
				//toggle selection off when selected twice.
				CurrentSelection = null;
			} else {
				CurrentSelection = obj;
			}
		} else {
			CurrentSelection = obj;
		}
	}

	public void CloseInventory() {
		CurrentSelection = null;
	}
	
	Color CurrentButtonColor(GameObject obj) {
		if (CurrentSelection) {
			if (CurrentSelection.GetInstanceID () == obj.GetInstanceID ()) {
				return SelectedColor;
			} else {
				return MenuDefaultGUIColor;
			}
		} else {
			return MenuDefaultGUIColor;
		}
	}
	
	void TexturedButton(Texture texture) {
		if (texture) {
			if (GUI.Button (new Rect (0, 0, texture.height + 8, texture.width + 8), texture)) {
				
			}
		} else {
			if (GUI.Button (new Rect (0, 0, 100, 20), "x")) {
				
			}
			
			
		}
		
	}

	public void ClearScene() {
		foreach (Transform child in transform) {
			if (child.gameObject) {
				Destroy (child.gameObject); 
			}
		}
	}

	public void OpenScene(string pathToFile) {
		Stream serializationStream;
		FileStream fs = File.Open (pathToFile, FileMode.Open);
		FileData fileData;
		PlaneData planeData;
		try {
			BinaryFormatter formatter = new BinaryFormatter ();
			fileData = (FileData)formatter.Deserialize (fs);
			foreach (StatsData statsData in fileData.objList) {
				string type = statsData.GetType ().Name;
				GameObject obj = null;
				switch (type) {
					case "LineData":
						obj = GetComponent<LinesList> ().AddALine ((LineData) statsData);
						SetScaleLine(obj, statsData.scale);
					break;
					case "PlaneData":
						obj = GetComponent<PlanesList> ().AddPlane ((PlaneData) statsData);
						SetScalePlane(obj, statsData.scale);
						break;
					case "PointsData":
						obj = GetComponent<ScatterplotList> ().CreatePoints ((PointsData) statsData);
						break;
				}
				if (obj != null && statsData.color != null) {
					SetColor (obj, (Color)statsData.color);
				}
			}
		} catch (SerializationException e) {
			Debug.LogWarning ("Failed to deserialize. Reason: " + e.Message);
			throw;
		} finally {
			fs.Close ();
		}
	}

	public void SaveScene(string pathToFile) {
		//Load all StatsData objects from each GameObject in Inventory
		//Persist them via the FileData object
		FileData fileData = new FileData ();
		StatsData statsData;
		if (transform.childCount > 0) {
			foreach (Transform child in transform) {
				GameObject obj = transform.gameObject;
				string type = obj.GetType ().Name;
				statsData = ((DataHolder)child.GetComponent<DataHolder> ()).StatsData;
				fileData.AddStatsData (statsData);
			}
		}
		Debug.Log ("saving scene");

		FileStream fileStream = File.Open (pathToFile, FileMode.Create);
		try {
			BinaryFormatter formatter = new BinaryFormatter ();
			formatter.Serialize (fileStream, fileData);
		} catch (Exception e) {
			Debug.LogWarning ("Save.SaveScene(): Failed to serialize object to a file " + filename + " (Reason: " + e.ToString () + ")");
		} finally {
			fileStream.Close ();
		}
	}
	
	public static void SaveFile(string filename, System.Object obj) {
		try {
			Stream fileStream = File.Open (filename, FileMode.Create);
			BinaryFormatter formatter = new BinaryFormatter ();
			formatter.Serialize (fileStream, obj);
			fileStream.Close ();
		} catch (Exception e) {
			Debug.LogWarning ("Save.SaveFile(): Failed to serialize object to a file " + filename + " (Reason: " + e.ToString () + ")");
		}
	}
	
	public static System.Object LoadFile(String filename) {
		try {
			Debug.Log ("LoadFile...");
			Stream fileStream = File.Open (filename, FileMode.Open, FileAccess.Read);
			BinaryFormatter formatter = new BinaryFormatter ();
			System.Object obj = formatter.Deserialize (fileStream);
			fileStream.Close ();
			return obj;
		} catch (Exception e) {
			Debug.LogWarning ("SaveLoad.LoadFile(): Failed to deserialize a file " + filename + " (Reason: " + e.ToString () + ")");
			return null;
		}
	}
	
	void ColorButtons(GameObject obj) {
		GUILayout.BeginHorizontal ();
		if (AssignableColors.Length > 0) {
			
			foreach (Color c in AssignableColors) {
				GUI.contentColor = c;
				
				if (ColorButtonTexture) {
					if (GUILayout.Button (ColorButtonTexture)) {
						SetColor (obj, c);
					}
				} else {
					if (GUILayout.Button ("x")) {
						SetColor (obj, c);
					}
					
					
				}
				
			}
			GUI.contentColor = MenuDefaultGUIColor;
			GUI.color = MenuDefaultGUIColor;
		}
		GUILayout.EndHorizontal ();
	}
	
	void SetColor(GameObject obj, Color color) {
		if (obj.renderer) {
			obj.renderer.material.color = color;
		}
		//color data
		if (obj.GetComponent<DataHolder> ()) {
			StatsData statsData = obj.GetComponent<DataHolder> ().StatsData;
			statsData.color = color;
		}
		if (obj.transform.childCount > 0) {
			foreach (Transform child in obj.transform) {
				if (child.renderer) {
					child.renderer.material.color = color;
				}
			}
			
		}
		
	}
	float GetScaleLine(GameObject obj) {
		if (obj.GetComponent<DataHolder> ()) {
			StatsData statsData = obj.GetComponent<DataHolder> ().StatsData;
			return statsData.scale;
		} else {
			return 1f;
		}
	}
	float GetScalePlane(GameObject obj) {
		return obj.transform.localScale.x;
	}

	float GetScalePoints(GameObject obj) {
		if (obj.transform.childCount > 0) {
			foreach (Transform child in obj.transform) {
				return child.transform.localScale.x;
				break;
			}
			return 1f;
		} else {
			return 1f;
		}
	}

	void SetScaleLine(GameObject obj, float scale) {
		obj.GetComponent<LineRenderer> ().SetWidth (scale, scale);
		if (obj.GetComponent<DataHolder> ()) {
			StatsData statsData = obj.GetComponent<DataHolder> ().StatsData;
			statsData.scale = scale;
		}
	}

	void SetScalePlane(GameObject obj, float scale) {
		obj.transform.localScale = new Vector3 (scale, scale, scale);
		if (obj.GetComponent<DataHolder> ()) {
			StatsData statsData = obj.GetComponent<DataHolder> ().StatsData;
			statsData.scale = scale;
		}
	}

	void SetScalePoints(GameObject obj, float scale) {
		if (obj.transform.childCount > 0) {
			foreach (Transform child in obj.transform) {
				child.transform.localScale = new Vector3 (scale, scale, scale);
			} 
		}
		if (obj.GetComponent<DataHolder> ()) {
			StatsData statsData = obj.GetComponent<DataHolder> ().StatsData;
			statsData.scale = scale;
		}
	}
}
