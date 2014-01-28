


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
		                     
public class ScatterplotList : PointsList {
	public GameObject  pointPrefab;
	public int numberPoints = 100;
			
	public void createSinglePoint(Vector3 center) {
		PointsData pointsData = new PointsData ();
		pointsData.scale = .01f;
		pointsData.AddPoint (center);
		pointsData.name = "Point";
		CreatePoints (pointsData);
	}
			
	public void createRandomPoints(int number) {
		createRandomPoints (Vector3.one, number, 1.0f);
	}
			
	public void createRandomPoints(Vector3 center, int number, float range = 1.0f) {
		float scale = range * scaleSizeFactor;
		PointsData pointsData = new PointsData ();
		pointsData.scale = scale;
				
		for (int i = 0; i < number; i++) {
			pointsData.AddPoint (center + Random.insideUnitSphere * range);
		}
		pointsData.name = "Random Points";
		CreatePoints (pointsData);
	}
	public GameObject CreatePoints(PointsData pointsData) {
		return CreatePoints (pointsData, pointPrefab);
	}
}
