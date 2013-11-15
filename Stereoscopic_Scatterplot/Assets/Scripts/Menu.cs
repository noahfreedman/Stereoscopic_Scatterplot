// ESC menu/ options

using UnityEngine;
using System.Collections;




public class Menu : MonoBehaviour {
	//public List<string> MenuItems = new List<string>();
	
    void OnGUI() {

	        if (GUI.Button(new Rect(0, 0, 100, 20), "item"))
	            Debug.Log("clicked");

	}
//    void Start () {
//			MenuItems 'help', 'about','controls', 'options', 'stats'
//		}
//    }
}