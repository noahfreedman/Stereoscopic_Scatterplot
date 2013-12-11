using UnityEngine;
using System.Collections;

public class NetTest : MonoBehaviour {
	public string baseAPI = "http://localhost/";

	void Start() {
		callMatLab();
	}

	void callMatLab() {
		MatLabInterface matlab = gameObject.AddComponent( typeof(MatLabInterface)) as MatLabInterface;
		double[] xRange = {-2, .2, 2};
		double[] yRange = {-2, .2, 2};
		matlab.formulaToMeshgrid("Z = Y.^2 + X.^2", xRange, yRange);
	}
}