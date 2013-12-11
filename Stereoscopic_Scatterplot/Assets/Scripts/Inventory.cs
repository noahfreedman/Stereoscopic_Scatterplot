using UnityEngine;
using System.Collections;

// display a list of objects that are a child of this component's transform.
// methods to select (??) 
// delete
// show edit sub menus (??)
public class Inventory : MonoBehaviour
{	
		public bool ShowMenu = true;
		public float MenuAreaWidth = 200;
		private float areaHeight = 100;
		public float ScreenXOffset = 10;
		public bool doubleMenu = false; // Main menu controls this
		private GameObject CurrentSelection;
		private Rect MenuAreaRect;
		private Rect MenuAreaRightScreen;
		private Rect fullScreenRect = new Rect (0, 0, Screen.width, Screen.height);
		private Rect MenuRect = new Rect (0, 30, 130, 300);
		private Color MenuDefaultGUIColor;
		public Color SelectedColor = Color.green;

		void Start ()
		{
				MenuDefaultGUIColor = GUI.contentColor;

				ScreenXOffset = (Screen.width / 2) - ScreenXOffset;

				areaHeight = Screen.height;
				MenuAreaRect = new Rect (ScreenXOffset, 0, MenuAreaWidth, areaHeight);
				MenuAreaRightScreen = new Rect (Screen.width, 0, MenuAreaWidth, areaHeight);

		}

		void OnGUI ()
		{

				GUILayout.BeginArea (MenuAreaRect);

				DisplayGUI ();
				GUILayout.EndArea ();
				if (doubleMenu) {

						GUILayout.BeginArea (MenuAreaRightScreen);
						DisplayGUI ();
						GUILayout.EndArea ();
				}
		}

		void SelectObject (GameObject obj)
		{
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

		Color CurrentButtonColor (GameObject obj)
		{
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

		void DisplayGUI ()
		{
				if (ShowMenu) {
						GUILayout.BeginVertical ("box");
						if (transform.childCount > 0) {
								foreach (Transform child in transform) {
										GUILayout.BeginHorizontal ();
										GUI.contentColor = CurrentButtonColor (child.gameObject);
										if (GUILayout.Button (child.name)) {
												SelectObject (child.gameObject);
										}
										GUI.contentColor = MenuDefaultGUIColor;
										GUILayout.EndHorizontal ();
								}
						} else {
								GUILayout.Label ("0 Objects");
						}
						GUILayout.EndVertical ();
				} 
		}
}
