using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Class for storing all saved data
[Serializable]
public class FileData {
	public List<StatsData> objList;

	public FileData () {
		objList = new List<StatsData>();
	}
	public int AddStatsData(StatsData sd) {
		objList.Add(sd);
		return objList.Count;
	}
}

