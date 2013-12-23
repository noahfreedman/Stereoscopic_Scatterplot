using UnityEngine;
using System.Collections;
using System;

public class ScalingAxes : MonoBehaviour
{
	public Transform Camera;
	public Shader shader;
	public Color xColor = new Color(1, 0, 0, 1f);
	public Color xTickColor = new Color(1, 0, 0, 1f);
	public Color yColor = new Color(0, 1, 0, 1f);
	public Color yTickColor = new Color(0, 1, 0, 1f);
	public Color zColor = new Color(0, 0, 1, 1f);
	public Color zTickColor = new Color(0, 0, 1, 1f);
	public int axis_range = 1000;
	private float LabelInterval = 0.2f;
	private float LabelRangeMin = 2f;
	private float LabelRangeMax = 2f;
	public float LabelHeight = 0.05f;
	public float LabelSize = 0.003f;
	
	public Font axisLabelFont;
	public Color defaultLabelColor = new Color(0, 0, 0);
	
	public bool showTicks = true;
	public bool showAxes = true;
	
	private int canvasIndex = 0;
	private float lineSize = 0.006f;
	private float RoundedDistance = 0.1f; // keeps the last mesured modulo of the distance to the camera
	private float LineThicknessMultiplier = 0.01f;//
	private float MarkerDepthCameraGap = 0.9f; // prevents the floating label problem. 0.9 is 90% the distance from origin to camera.
	private bool NearXOld;
	void Start()
	{
		
		float roundedDistance = (float)System.Math.Round(Vector3.Distance(transform.position, Camera.transform.position));
		DrawAxes();
	}
	void Update()
	{
		// Only recreate axis when distance from origin changes 
		float currentDistance = (float)System.Math.Round(Vector3.Distance(transform.position, Camera.transform.position));
		bool nearX = Math.Abs(Camera.transform.position.x) > Math.Abs(Camera.transform.position.z);
		
		if ((RoundedDistance < currentDistance) || (RoundedDistance > currentDistance))
		{
			DrawAxes();
		}
		if (NearXOld != nearX)
		{
			DrawAxes();
		}
		RoundedDistance = currentDistance;
		NearXOld = nearX;
	}
	private void DrawAxes()
	{
		
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		if (showTicks)
		{
			
			LabelInterval = RoundedDistance / 5;
			LabelRangeMax = RoundedDistance * 0.9f;
			LabelRangeMin = RoundedDistance * -0.9f;
			for (float i = LabelRangeMin; i <= LabelRangeMax; i += LabelInterval)
			{
				// Y
				Vector3 start = new Vector3(-LabelHeight / 2, i, 0);
				Vector3 end = new Vector3(LabelHeight / 2, i, 0);
				GameObject line = createLine(start, end, LabelSize, yTickColor);
				attachObjectLabel(line, SigFigs(i), yColor);
			}
			// y is actually z
			bool nearX = Math.Abs(Camera.transform.position.x) > Math.Abs(Camera.transform.position.z);
			
			if (nearX)
			{
				Debug.Log(nearX);
				for (float i = LabelRangeMin * 2; i <= LabelRangeMax * 2; i += LabelInterval)
				{
					// z 
					Vector3 start = new Vector3(0, -LabelHeight / 2, i);
					Vector3 end = new Vector3(0, LabelHeight / 2, i);
					GameObject line = createLine(start, end, LabelSize, zTickColor);
					attachObjectLabel(line, SigFigs(i), zColor);
				}
				// cut off distant labels
				if (Camera.transform.position.x > 0)
				{
					LabelRangeMin = 0;
				}
				else
				{
					LabelRangeMax = 0;
				}
				for (float i = LabelRangeMin; i <= LabelRangeMax; i += LabelInterval)
				{
					// x
					Vector3 start = new Vector3(i, -LabelHeight / 2, 0);
					Vector3 end = new Vector3(i, LabelHeight / 2, 0);
					GameObject line = createLine(start, end, LabelSize, xTickColor);
					attachObjectLabel(line, SigFigs(i), xColor);
				}
				
			}
			else // near y axis
			{
				for (float i = LabelRangeMin * 2; i <= LabelRangeMax * 2; i += LabelInterval)
				{
					// X
					Vector3 start = new Vector3(i, -LabelHeight / 2, 0);
					Vector3 end = new Vector3(i, LabelHeight / 2, 0);
					GameObject line = createLine(start, end, LabelSize, xTickColor);
					attachObjectLabel(line, SigFigs(i), xColor);
				}
				// cut off distant labels
				if (Camera.transform.position.z > 0)
				{
					LabelRangeMin = 0;
				}
				else
				{
					LabelRangeMax = 0;
				}
				for (float i = LabelRangeMin; i <= LabelRangeMax; i += LabelInterval)
				{
					// z 
					Vector3 start = new Vector3(0, -LabelHeight / 2, i);
					Vector3 end = new Vector3(0, LabelHeight / 2, i);
					GameObject line = createLine(start, end, LabelSize, zTickColor);
					attachObjectLabel(line, SigFigs(i), zColor);
				}
				
			}
			
		}
		
		if (showAxes)
		{
			lineSize = LineThicknessMultiplier * RoundedDistance;
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
		
	}
	
	private GameObject createLine(Vector3 start, Vector3 end, float lineSize, Color c)
	{
		return createLine(start, end, lineSize, c, shader);
	}
	
	private GameObject createLine(Vector3 start, Vector3 end, float lineSize, Color c, Shader s)
	{
		GameObject canvas = new GameObject("line" + canvasIndex);
		canvas.transform.parent = transform;
		canvas.transform.rotation = transform.rotation;
		LineRenderer lines = (LineRenderer)canvas.AddComponent<LineRenderer>();
		lines.material = new Material(s);
		lines.material.color = c;
		lines.useWorldSpace = false;
		lines.SetWidth(lineSize, lineSize);
		lines.SetVertexCount(2);
		lines.SetPosition(0, new Vector3(0, 0, 0));
		end = end - start;
		lines.SetPosition(1, end);
		canvas.transform.position = start;
		canvasIndex++;
		return canvas;
	}
	
	private void attachObjectLabel(GameObject target, string text, Color? color = null)
	{
		if (color == null)
			color = defaultLabelColor;
		GameObject go = new GameObject("Axis Label");
		GUIText gt = (GUIText)go.AddComponent(typeof(GUIText));
		//TextMesh gt = (TextMesh)go.AddComponent(typeof(TextMesh));
		gt.font = axisLabelFont;
		gt.text = text;
		gt.alignment = TextAlignment.Center;
		gt.material.color = (Color)color;
		
		((ObjectLabel)go.AddComponent("ObjectLabel")).target = target.transform;
		go.transform.parent = this.transform;
	}
	
	private string SigFigs(float i)
	{
		return SignificantDigits.ToString(System.Convert.ToDouble(i), 2);
	}
}







