using UnityEngine;
using System.Collections;
using System.IO;

public class MatLabInterface : MonoBehaviour {

	private static string API_root = "http://localhost:1337/";

	public void formulaToMeshgrid(string formula, double[] xRange, double[] yRange) {
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
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}
	private void ProcessFile() {
		try {
			string planeFile = System.IO.Path.Combine(planePath, planeName);
			string[] lines = File.ReadAllLines(planeFile);
			ArrayList pointsList = new ArrayList();
			if (lines.Length > 0) {
				for (int i = 0; i < lines.Length; i++) {
					string s = lines[i];
					string[] values = s.Split(',');
					//add data point
					float x = float.Parse(values[0]);
					float y = float.Parse(values[1]);
					float z = float.Parse(values[2]);
					pointsList.Add(new Vector3(x,y,z));
				}
			}
			//sort all items by x, then by y in one list
			pointsList.Sort(new VectorComparer());
			//record length of x row, so then can process points in grid
			int rowLength = 0;
			float firstX = ((Vector3) pointsList[0]).x;
			for (int i = 0; i < pointsList.Count; i++) {
				if (((Vector3) pointsList[i]).x != firstX) {
					break;        
				}
				rowLength++;
			}
			//draw mesh
			int height = pointsList.Count / rowLength;
			GameObject plane = GeneratePlane.GeneratePlane(this.gameObject, pointsList, rowLength, height);
			plane.transform.parent = transform;
			plane.renderer.material = planeMaterial;
		} catch (FileNotFoundException f) {
			showNotification(f.Message);                
		}
	}
}