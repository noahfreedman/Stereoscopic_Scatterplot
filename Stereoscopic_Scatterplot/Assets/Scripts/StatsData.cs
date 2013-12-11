public class StatsData {
	private string _data;

	public StatsData(string data) {
		_data = data;
	}

	public string Data {
		get { return _data; }
		set { _data = value; }
	}

	public string ToString() {
		return _data;
	}
}