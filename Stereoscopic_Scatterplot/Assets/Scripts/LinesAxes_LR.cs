using UnityEngine;
using System.Collections;

public class LinesAxes_LR : MonoBehaviour {

	public Shader shader;
	public Color xColor = new Color(1, 0, 0, 1f);
	public Color yColor = new Color(0, 1, 0, 1f);
	public Color zColor = new Color(0, 0, 1, 1f);
		
	public int axis_range = 1000;
	public float marker_interval = 0.2f;
	public float marker_depth = 2f;
	public float marker_height = 0.05f;
	public float marker_size = 0.003f;
	
	public Shader grid_shader;
	public int grid_depth = 2;
	public int grid_range = 1;
	public Color grid_color = new Color(1, 1, 1, 0.5f);
	public float grid_size = 0.002f;
	public float grid_interval = 0.2f;
	
	public bool showTicks = true;
	public bool showAxes = true;
	public bool showGrid = true;
	
	private int canvasIndex = 0;
	private float lineSize = 0.006f;
	
	void Start () {
		init();
	}
	
	private void init() {
		if (showTicks) { 
			for (float i = -marker_depth; i <= marker_depth; i += marker_interval) {
				//add x marker
				Vector3 start = new Vector3(i, -marker_height / 2, 0);
				Vector3 end = new Vector3(i, marker_height / 2, 0);
				createLine(start, end, marker_size, xColor);
				
				//add y marker
				start = new Vector3(-marker_height / 2, i, 0);
				end = new Vector3(marker_height / 2, i, 0);
				createLine(start, end, marker_size, yColor);
				
				//add z marker
				start = new Vector3(0, -marker_height / 2, i);
				end = new Vector3(0, marker_height / 2, i);
				createLine(start, end, marker_size, zColor);
			}
		}
		
		if (showAxes) {
			Vector3 x_start = new Vector3(-axis_range, 0, 0);
			Vector3 x_end = new Vector3(axis_range, 0, 0);
			Vector3 y_start = new Vector3(0, -axis_range, 0);
			Vector3 y_end = new Vector3(0, axis_range, 0);
			Vector3 z_start = new Vector3(0, 0, -axis_range);
			Vector3 z_end = new Vector3(0, 0, axis_range);
			createLine(x_start, x_end, lineSize, xColor);
			createLine(y_start, y_end, lineSize, yColor);
			createLine(z_start, z_end, lineSize, zColor);
		}
		
		if (showGrid) {
			for (float i = -grid_depth; i <= grid_depth; i += grid_interval) {
				for (float j = -grid_depth; j <= grid_depth; j += grid_interval) {
					//add x grid
					Vector3 start = new Vector3(i, -grid_range, j);
					Vector3 end = new Vector3(i, grid_range, j);
					createLine(start, end, marker_size, grid_color, grid_shader);
					
					//add y grid
					start = new Vector3(-grid_range, i, j);
					end = new Vector3(grid_range, i, j);
					createLine(start, end, marker_size, grid_color, grid_shader);
					
					//add y grid
					start = new Vector3(i, j, -grid_range);
					end = new Vector3(i, j, grid_range);
					createLine(start, end, marker_size, grid_color, grid_shader);
				}
			}
		}
	}
	
	private void createLine(Vector3 start, Vector3 end, float lineSize, Color c) {
		createLine (start, end, lineSize, c, shader);
	}
	
	private void createLine(Vector3 start, Vector3 end, float lineSize, Color c, Shader s) {
		GameObject canvas = new GameObject("canvas" + canvasIndex); 
		canvas.transform.parent = transform;
		canvas.transform.rotation = transform.rotation;
		LineRenderer lines = (LineRenderer) canvas.AddComponent<LineRenderer>();
		lines.material = new Material(s);
		lines.material.color = c;
		lines.useWorldSpace = false;
		lines.SetWidth(lineSize, lineSize);
		lines.SetVertexCount(2);
		lines.SetPosition(0, start);
		lines.SetPosition(1, end);
		canvasIndex++;
	}
}







