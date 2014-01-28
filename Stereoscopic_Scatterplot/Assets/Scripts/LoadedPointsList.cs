using UnityEngine;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

//this one
public class LoadedPointsList : PointsList
{
	public GameObject pointPrefab;
	public List<string> recentFiles;
	private List<Vector3S> points;
	private string notificationMessage = "";
	private float minX = Mathf.Infinity;
	private float minY = Mathf.Infinity;
	private float minZ = Mathf.Infinity;
	private float maxX = Mathf.NegativeInfinity;
	private float maxY = Mathf.NegativeInfinity;
	private float maxZ = Mathf.NegativeInfinity;
	private float scale;
		
	void pointsArrayListToObjects (List<Vector3S> pointarray, string fileName = "Points")
	{
		//point size to 1% of longest range
		float longestRange = maxX - minX;
		if (maxY - minY > longestRange) longestRange = maxY - minY;
		if (maxZ - minZ > longestRange) longestRange = maxZ - minZ;
		scale = longestRange * scaleSizeFactor;
				
		PointsData pointsData = new PointsData();
		pointsData.name = fileName;
		pointsData.points = pointarray;
		CreatePoints(pointsData);
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

	//(csvDataFileName, csvDataPath);
	//ToDo: all these object factories need better names
	public void LoadPointsFile (string pointFilePath, string pointFileName = "Points")
	{	
				


		pointFilePath = System.IO.Path.GetFullPath (pointFilePath);

		points = new List<Vector3S> ();	

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
					points.Add (new Vector3S (x, y, z));
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
				
				pointsArrayListToObjects (points, pointFileName);
			} else {
				showNotification ("Error loading file. No lines detected.");
			}
		} catch (Exception e) {
			showNotification (e.Message);
		}

	}
	public GameObject CreatePoints(PointsData pointsData) {
		return CreatePoints (pointsData, pointPrefab);
	}
}