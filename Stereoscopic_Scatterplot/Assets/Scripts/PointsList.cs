using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointsList : MonoBehaviour
{
	public GameObject CreatePoints(PointsData pointsData, GameObject pointPrefab) {
		GameObject group = new GameObject (pointsData.name); 
		group.transform.parent = transform;
		//group.transform.position = center;
		group.transform.rotation = transform.rotation;
		float scale = pointsData.scale;
		foreach (Vector3S pointS in pointsData.points) {
			Vector3 pos = pointS.ToVector3();
			GameObject preFabObj = Instantiate (pointPrefab, pos, Quaternion.identity) as GameObject;
			if (preFabObj) {			
				preFabObj.transform.parent = group.transform;
				preFabObj.transform.position = preFabObj.transform.position;
				preFabObj.transform.localScale = new Vector3(scale, scale, scale);
			}
		}
		DataHolder dh = group.AddComponent<DataHolder>();
		dh.StatsData = pointsData;
		return group;
	}
}