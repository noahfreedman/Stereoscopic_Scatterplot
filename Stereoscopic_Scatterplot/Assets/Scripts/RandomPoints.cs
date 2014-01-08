using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomPoints : MonoBehaviour
{
	public GameObject  pointPrefab;
	public int numberPoints = 100;
	public List<GameObject> CreatedPointGroups;

	public void createSinglePoint (Vector3 center)
	{
		string pointName = "Point " + center.x.ToString () + ", " + center.y.ToString () + ", " + center.z.ToString ();
		GameObject group = new GameObject (pointName); 
		group.transform.parent = transform;
		group.transform.position = center;
		group.transform.rotation = transform.rotation;
		GameObject preFabObj = Instantiate (pointPrefab, center, Quaternion.identity) as GameObject;
		if (preFabObj) {			
			preFabObj.transform.parent = group.transform;
			preFabObj.transform.position = preFabObj.transform.position + center;
		}
		
	}

	public void createRandomPoints (int number)
	{
		createRandomPoints (pointPrefab, Vector3.one, number, 1.0f);
	}

	public void createRandomPoints (Vector3 center, int number, float range = 1.0f)
	{
		createRandomPoints (pointPrefab, center, number, range);
	}

	public void createRandomPoints (GameObject pointPrefab, Vector3 center, int count, float range)
	{
		GameObject group = new GameObject ("Random Points"); 
		float scale = range / 100;
		group.transform.parent = transform;
		group.transform.position = center;
		group.transform.rotation = transform.rotation;

		for (int i = 0; i < count; i++) {

			GameObject preFabObj = Instantiate (pointPrefab, (Random.insideUnitSphere * range), Quaternion.identity) as GameObject;
			if (preFabObj) {			
				preFabObj.transform.parent = group.transform;
				preFabObj.transform.position = preFabObj.transform.position + center;
				preFabObj.transform.localScale = new Vector3(scale, scale, scale);
			}

		}
		CreatedPointGroups.Add (group);
	}


}
