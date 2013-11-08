using UnityEngine;

[RequireComponent (typeof (LineRenderer))]
public class TickMark: MonoBehaviour {
 
	void Start () {
		
	}
 
    void Update() {
		Debug.Log (((LineRenderer) this.GetComponent(typeof(LineRenderer))).isVisible);
    }
}