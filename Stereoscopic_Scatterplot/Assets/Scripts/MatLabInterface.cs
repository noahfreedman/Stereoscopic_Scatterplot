using UnityEngine;
using System;
using System.Collections;

public class MatLabInterface : MonoBehaviour {
	
	private static string API_root = "http://171.64.185.236:8080/";
	private string[] deliminator = {"  "};
	
	public float xMin, xInterval, xMax, yMin, yInterval, yMax;
	
	public void formulaToMeshgrid(string formula, double[] xRange, double[] yRange) {
		this.xMin = (float) xRange[0];
		this.xInterval = (float) xRange[1];
		this.xMax = (float) xRange[2];
		this.yMin = (float) yRange[0];
		this.yInterval = (float) yRange[1];
		this.yMax = (float) yRange[2];
		
		string url = API_root + "formulaToMeshgrid";
		WWWForm form = new WWWForm();
		form.AddField("formula", formula);
		form.AddField("xMin", xRange[0].ToString ());
		form.AddField("xInterval", xRange[1].ToString ());
		form.AddField("xMax", xRange[2].ToString ());
		form.AddField("yMin", yRange[0].ToString ());
		form.AddField("yInterval", yRange[1].ToString ());
		form.AddField("yMax", yRange[2].ToString ());
		
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
	}
	
	IEnumerator WaitForRequest(WWW www) {
		yield return www;
		
		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.data);
			Debug.Log(www.text);
			ProcessMeshgrid(www.text);
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}
	private void ProcessMeshgrid(string data) {
		//generate full point data based on range. Data is returned in rows of increasing x. Columns increase by Y.
		string[] lines = data.Split ("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		int rowLength = 0;
		ArrayList pointsList = new ArrayList();
		//record length of x row, so then can process points in grid
		for (int i = 0, l = lines.Length; i < l; i++) {
			float x = xMin + xInterval * i;
			string line = lines[i];
			string[] columns = line.Split(deliminator, System.StringSplitOptions.RemoveEmptyEntries);
			if (rowLength < 1) rowLength = columns.Length;
			for (int ii = 0, ll = columns.Length; ii < ll; ii++) {
				float y = yMin + yInterval * ii;
				Debug.Log(columns);
				Debug.Log(columns[ii]);
				float z = float.Parse (columns[ii]);
				Vector3 v = new Vector3(x, y, z);
				Debug.Log(v);
				pointsList.Add(v);
			}
		}
		//draw mesh
		int height = lines.Length;
		GameObject stage = GameObject.Find("Inventory");
		stage.GetComponent<PlanarFunctionsList> ().AddPlanar (pointsList, rowLength, height);
	}
}