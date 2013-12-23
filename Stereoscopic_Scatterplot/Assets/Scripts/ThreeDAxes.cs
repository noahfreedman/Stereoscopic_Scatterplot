using UnityEngine;
using System.Collections;
using System;

public class ThreeDAxes : MonoBehaviour
{
    public GameObject AxesTextPrefab;
    public Transform Camera;
    public Shader shader;
    public Color xColor = new Color(1, 0, 0, 1f);
    public Color xTickColor = new Color(1, 0, 0, 1f);
    public Color yColor = new Color(0, 1, 0, 1f);
    public Color yTickColor = new Color(0, 1, 0, 1f);
    public Color zColor = new Color(0, 0, 1, 1f);
    public Color zTickColor = new Color(0, 0, 1, 1f);
    public int axis_range = 1000;
    private float LabelInterval = 10.0f;
    private float LabelRangeMin = 1f;// these get overwritten
    private float LabelRangeMax = 1f;
    private float LabelHeight = 0.05f;
    public float LabelHeightMultiplier = 0.05f;
    public float LabelSize = 0.003f;
    public bool showTicks = true;
    public bool showAxes = true;

    private int canvasIndex = 0;
    private float lineSize = 0.006f;
    private float LastUpdatedDistance = 0.1f; // the last mesured distance to the camera
    private float LineThicknessMultiplier = 0.01f;//
    private float TextSizeMultiplier = 0.5f;//
    public float LabelIntervalDivisor = 5.0f;
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

        // when the cam's relative position changes 
        if ((LastUpdatedDistance < currentDistance) || (LastUpdatedDistance > currentDistance) || (NearXOld != nearX))
        {
            DrawAxes();
        }

        LastUpdatedDistance = currentDistance;
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

            LabelInterval = LastUpdatedDistance / LabelIntervalDivisor;
            LabelHeight = LastUpdatedDistance * LabelHeightMultiplier;
            LabelRangeMax = LastUpdatedDistance * 0.9f;
            LabelRangeMin = LastUpdatedDistance * -0.9f;
            LabelSize = LastUpdatedDistance * LineThicknessMultiplier / 5;
            for (float i = LabelRangeMin; i <= LabelRangeMax; i += LabelInterval)
            {
                // Y
                Vector3 start = new Vector3(-LabelHeight / 2, i, 0);
                Vector3 end = new Vector3(LabelHeight / 2, i, 0);
                GameObject line = createLine(start, end, LabelSize, yTickColor);
                //attachObjectLabel(line, SigFigs(i), yColor);
                attachObjectLabel(line, SigFifths(i), yColor);
                

            }
  
            for (float i = LabelRangeMin ; i <= LabelRangeMax ; i += LabelInterval)
            {
                // z 
                Vector3 start = new Vector3(0, -LabelHeight / 2, i);
                Vector3 end = new Vector3(0, LabelHeight / 2, i);
                GameObject line = createLine(start, end, LabelSize, zTickColor);
                //attachObjectLabel(line, SigFigs(i), zColor);
                attachObjectLabel(line, SigFifths(i), zColor);
            }
            for (float i = LabelRangeMin; i <= LabelRangeMax; i += LabelInterval)
            {
                // x
                Vector3 start = new Vector3(i, -LabelHeight / 2, 0);
                Vector3 end = new Vector3(i, LabelHeight / 2, 0);
                GameObject line = createLine(start, end, LabelSize, xTickColor);

                attachObjectLabel(line, SigFifths(i), xColor);
                //attachObjectLabel(line, SigFigs(i), xColor);
            }



        }

        if (showAxes)
        {
            lineSize = LineThicknessMultiplier * LastUpdatedDistance;
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
    //AxesText prefab instead of GUIText...
    private void attachObjectLabel(GameObject target, string text, Color? color = null)
    {

        GameObject preFabObj = Instantiate(AxesTextPrefab, target.transform.position, Quaternion.identity) as GameObject;
        if (preFabObj)
        {
            preFabObj.transform.parent = target.transform;
            preFabObj.transform.position = preFabObj.transform.position + target.transform.position;
            preFabObj.GetComponent<TextMesh>().text = text;
            preFabObj.GetComponent<TextMesh>().fontSize = (int)(LastUpdatedDistance * TextSizeMultiplier);
        }
    }
    private string SigFigs(float i)
    {
        return SignificantDigits.ToString(System.Convert.ToDouble(i), 2);
    }
    private string SigFifths(float i)
    {
        i = (float)Math.Round(i / 5.0) * 5;
        return SignificantDigits.ToString(System.Convert.ToDouble(i), 2);
    }
}







