using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

//this one
public class LoadPoints : MonoBehaviour
{
	public List<GameObject> CreatedPoints;
	public GameObject PointPrefab;
	public string _filePath = "";
	public List<string> recentFiles;
	private ArrayList points;
	private string notificationMessage = "";
	private float minX = Mathf.Infinity;
	private float minY = Mathf.Infinity;
	private float minZ = Mathf.Infinity;
	private float maxX = Mathf.NegativeInfinity;
	private float maxY = Mathf.NegativeInfinity;
	private float maxZ = Mathf.NegativeInfinity;
	private float scale;
		
	void pointsArrayListToObjects (ArrayList pointarray)
	{
		//point size to 1% of longest range
		float longestRange = maxX - minX;
		if (maxY - minY > longestRange) longestRange = maxY - minY;
		if (maxZ - minZ > longestRange) longestRange = maxZ - minZ;
		scale = longestRange / 100f;
				
		GameObject group = new GameObject ();
		string fileName = System.IO.Path.GetFileName (_filePath);
		group.name = fileName;
		group.transform.parent = transform;
		//group.transform.rotation = transform.rotation;

		foreach (Vector3 point in pointarray) {

			GameObject preFabObj = Instantiate (PointPrefab, point, Quaternion.identity) as GameObject;
			if (preFabObj) {			
				preFabObj.transform.parent = group.transform;
				preFabObj.transform.localScale = new Vector3(scale, scale, scale);
			}
			
		}
		CreatedPoints.Add (group);
	}

	public void showNotification (string s)
	{
		//notificationMessage = s;
		Debug.Log (s);
	}

	public void closeNotification ()
	{
		notificationMessage = "";
	}

	public void LoadPointsFile ()
	{
		LoadPointsFile (_filePath);	
	}
	//(csvDataFileName, csvDataPath);
	//ToDo: all these object factories need better names
	public void LoadPointsFile (string pointFilePath)
	{	
				
		//fileName = pointFilename;

		pointFilePath = System.IO.Path.GetFullPath (_filePath);
		//_filePath = pointFilePath;
		points = new ArrayList ();	

		bool hasHeaders = false;

		//load data from file path
		try {
			string[] lines = File.ReadAllLines (pointFilePath);
			
			if (lines.Length > 0) {
				//check for headers
				Regex rgx = new Regex (@"[^\d\.,]"); //search for any non-digit or . , chars
				hasHeaders = rgx.IsMatch (lines [0]);

				//assume that data is in x,y,z format
				int ii = hasHeaders ? 1 : 0;
				for (int i = ii; i < lines.Length; i++) {
					string s = lines [i];
					string[] values = s.Split (',');
					//add data point
					float x = float.Parse (values [0]);
					float y = float.Parse (values [1]);
					float z = float.Parse (values [2]);
					//add to point ArrayList
					points.Add (new Vector3 (x, y, z));
					if (x < minX)
						minX = x;	
					if (y < minY)
						minY = y;
					if (z < minZ)
						minZ = z;
					if (x > maxX)
						maxX = x;	
					if (y > maxY)
						maxY = y;	
					if (z > maxZ)
						maxZ = z;		
				}
				
				pointsArrayListToObjects (points);
			} else {
				showNotification ("Error loading file. No lines detected.");
			}
		} catch (FileNotFoundException f) {
			showNotification (f.Message);
		}

	}
}