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
		public string fileName = "test0.csv";
		public List<string> recentFiles;
		public bool autoDetectDataRange = true;
		public bool normalizeAxesIndividually = true;
		public float minX = 0;
		public float maxX = 1;
		public float minY = 0;
		public float maxY = 1;
		public float minZ = 0;
		public float maxZ = 1;
		private ArrayList points;
		private string notificationMessage = "";

		void pointsArrayListToObjects (ArrayList pointarray)
		{


				GameObject group = new GameObject ();
				group.name = fileName;
				group.transform.parent = transform;
				//group.transform.rotation = transform.rotation;

				foreach (Vector3 point in pointarray) {

						GameObject pointObj = Instantiate (PointPrefab, point, Quaternion.identity) as GameObject;
						if (pointObj) {

								pointObj.transform.parent = group.transform;


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
				LoadPointsFile (_filePath, fileName);	
		}
		//(csvDataFileName, csvDataPath);
		//ToDo: all these object factories need better names
		public void LoadPointsFile (string pointFilePath, string pointFilename)
		{	
				
				//fileName = pointFilename;

				pointFilePath = System.IO.Path.Combine (_filePath, fileName);
				//_filePath = pointFilePath;
				points = new ArrayList ();	

				bool hasHeaders = false;
				float autoMinX = float.PositiveInfinity;
				float autoMaxX = float.NegativeInfinity;
				float autoMinY = float.PositiveInfinity;
				float autoMaxY = float.NegativeInfinity;
				float autoMinZ = float.PositiveInfinity;
				float autoMaxZ = float.NegativeInfinity;

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
										float z = float.Parse (values [2]) / 3;
										//update min and max values
										if (x < autoMinX)
												autoMinX = x;
										if (x > autoMaxX)
												autoMaxX = x;
										if (y < autoMinY)
												autoMinY = y;
										if (y > autoMaxY)
												autoMaxY = y;
										if (z < autoMinZ)
												autoMinZ = z;
										if (z > autoMaxZ)
												autoMaxZ = z;
										//add to point ArrayList
										points.Add (new Vector3 (x, y, z));
										
								}
								
								//set range
								if (autoDetectDataRange) {
										minX = autoMinX;
										maxX = autoMaxX;
										minY = autoMinY;
										maxY = autoMaxY;
										minZ = autoMinZ;
										maxZ = autoMaxZ;
										//set all ranges to largest min and max if normalize individually is off
										if (!normalizeAxesIndividually) {
												float min = minX;
												if (minY < min)
														min = minY;
												if (minZ < min)
														min = minZ;
												float max = maxX;
												if (maxY > max)
														max = maxY;
												if (maxZ > max)
														max = maxZ;
												minX = min;
												minY = min;
												minZ = min;
												maxX = max;
												maxY = max;
												maxZ = max;
										}
										//notify of computed range
										string ss = "Points were normalized to the following ranges (disable Auto Detect Data Range to override)\n";
										ss += "x: (" + minX + ", " + maxX + ")\n";
										ss += "y: (" + minY + ", " + maxY + ")\n";
										ss += "z: (" + minZ + ", " + maxZ + ")\n";
										//showNotification(ss);

										
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