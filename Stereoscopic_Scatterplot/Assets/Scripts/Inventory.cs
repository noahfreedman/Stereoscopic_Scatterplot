using UnityEngine;
using System.Collections;

// display a list of objects that are a child of this component's transform.
// methods to select (??) 
// delete
// show edit sub menus (??)
public class Inventory : MonoBehaviour
{
		public bool ShowMenu = true;
		public float areaWidth = 130;
		public float areaHeight = 100;
		public float ScreenX = 800;
		public float ScreenY = 0;
		public bool doubleMenu = false;

		void Start ()
		{
				//ScreenX = ((Screen.width * 0.5f) - (areaWidth * 0.5f));
				//ScreenY = ((Screen.height) - 200f);
		}

		void Update ()
		{
				//TODO: menu should be correctly position on all resolutions & modes
				areaHeight = Screen.height;
		}
	
		void OnGUI ()
		{
				GUILayout.BeginArea (new Rect (Screen.width / 2 - areaWidth, 0, areaWidth, areaHeight));

				Display ();
				GUILayout.EndArea ();
			
				if (doubleMenu) {
						GUILayout.BeginArea (new Rect (Screen.width - areaWidth, 0, areaWidth, areaHeight));
					
						Display ();
						GUILayout.EndArea ();
				}
		}
		
		void Display ()
		{
		
				if (ShowMenu) {
				
						GUILayout.BeginVertical ("box");
						if (transform.childCount > 0) {
								foreach (Transform child in transform) {
						
										GUILayout.Label (child.name);
						
										if (GUILayout.Button ("Delete")) {
												Destroy (child.gameObject);
										}
								}
						} else {
								GUILayout.Label ("0 Objects");
						}
						GUILayout.EndVertical ();
				
				
				} 
		
		}
}
