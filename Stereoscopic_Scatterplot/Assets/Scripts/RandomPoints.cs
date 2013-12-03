using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomPoints : MonoBehaviour
{
		public GameObject  pointPrefab;
		public int numberPoints = 100;
		public bool showPointsAtStart = false;
		public List<GameObject> CreatedPointGroups;

		void Start ()
		{
				if (showPointsAtStart) {
						if (pointPrefab) {
								this.createRandomPoints (pointPrefab, gameObject.transform.position, numberPoints);
						}
				}
		}

		public void createRandomPoints (int number)
		{
				createRandomPoints (pointPrefab, Vector3.one, number);
		}
		public void createRandomPoints ( Vector3 center, int number)
		{
			createRandomPoints (pointPrefab, center, number);
		}
		public void createRandomPoints (GameObject pointPrefab, Vector3 center, int count)
		{
				GameObject group = new GameObject ("Random Points Group"); 
				group.transform.parent = transform;
				group.transform.position = center;
				group.transform.rotation = transform.rotation;

				for (int i = 0; i < count; i++) {

						GameObject preFabObj = Instantiate (pointPrefab, Random.insideUnitSphere, Quaternion.identity) as GameObject;
						if (preFabObj) {			
								//prefabGroupRandomPoints.transform.parent = this.gameObject.transform;
								preFabObj.transform.parent = group.transform;
								preFabObj.transform.position = preFabObj.transform.position  + center;

						}

				}
				CreatedPointGroups.Add (group);
		}


}
