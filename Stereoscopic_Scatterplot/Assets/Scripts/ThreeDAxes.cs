using UnityEngine;
using System.Collections;
using System;

public class ThreeDAxes : MonoBehaviour
{
    # region
    public GameObject AxesTextPrefab;   // use in horizontal axes
    public GameObject AxesTextOffsetPrefab; // use in vertical axes 
    public Transform Camera;
    public Shader shader;
    public Color xColor = new Color(1, 0, 0, 1f);
    public Color yColor = new Color(0, 1, 0, 1f);
    public Color zColor = new Color(0, 0, 1, 1f);
    public float LabelHeightMultiplier = 0.05f;
    public bool showTicks = true;
    public bool showAxes = true;
    public int axis_range = 1000;
    private float LabelRangeMax = 1f;
    private float LabelRangeMin = 1f;// these get overwritten
    private float LabelInterval = 100.0f;
    private float TickLineHeight = 0.05f;
    private float TickLineSize = 0.003f;
    private int canvasIndex = 0;
    private float lineSize = 0.006f;
    private float LastDistance = 0.1f; // the last mesured distance to the camera
    private float LineThicknessMultiplier = 0.01f;//
    private float CharacterSizeMultiplier = 0.01f;//
    public float LabelIntervalDivisor = 5.0f;
    public Vector2[] CameraZones;
    # endregion
    void Start()
    {
        LastDistance = (float)System.Math.Round(Vector3.Distance(transform.position, Camera.transform.position));
        DrawAxes();
    }
    void Update()
    {
        Debug.Log(Vector3.Distance(transform.position, Camera.transform.position).ToString());
        // Only recreate axis when distance from origin changes 
        float currentDistance = (float)System.Math.Round(Vector3.Distance(transform.position, Camera.transform.position));
        bool nearX = Math.Abs(Camera.transform.position.x) > Math.Abs(Camera.transform.position.z);

        // when the cam's relative position changes 
        //if ((LastUpdatedDistance < currentDistance) || (LastUpdatedDistance > currentDistance))
        if ((LastDistance < currentDistance) || (LastDistance > currentDistance))
        {
            DrawAxes();
        }

        LastDistance = currentDistance;
    }
    private void DrawAxes()
    {

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        if (showTicks)
        {
            TickLineHeight = LastDistance * LabelHeightMultiplier;
            LabelRangeMax = axis_range;
            LabelRangeMin = -axis_range;
            TickLineSize = LastDistance * LineThicknessMultiplier;
            for (float i = LabelRangeMin; i <= LabelRangeMax; i += LabelInterval )
            {
                // Y Vertical!!
                Vector3 start = new Vector3(-TickLineHeight * 2, i, 0);
                Vector3 end = new Vector3(TickLineHeight * 2, i, 0);
                GameObject line = createLine(start, end, TickLineSize, yColor);

                Vector3 start2 = new Vector3(0, i, -TickLineHeight* 2);
                Vector3 end2 = new Vector3(0, i, TickLineHeight * 2);
                GameObject line2 = createLine(start2, end2, TickLineSize, yColor);

                attachObjectLabelOffset(line, SigFigs(i), yColor);
            }
  
            for (float i = LabelRangeMin ; i <= LabelRangeMax ; i += LabelInterval)
            {
                // z 
                Vector3 start = new Vector3(0, -TickLineHeight, i);
                Vector3 end = new Vector3(0, TickLineHeight, i);
                GameObject line = createLine(start, end, TickLineSize, zColor);
                attachObjectLabel(line, SigFigs(i), zColor);
            }
            for (float i = LabelRangeMin; i <= LabelRangeMax; i += LabelInterval)
            {
                // x
                Vector3 start = new Vector3(i, -TickLineHeight, 0);
                Vector3 end = new Vector3(i, TickLineHeight , 0);
                GameObject line = createLine(start, end, TickLineSize, xColor);
                attachObjectLabel(line, SigFigs(i), xColor);
            }
        }
        if (showAxes)
        {
            lineSize = LineThicknessMultiplier * LastDistance;
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

        GameObject preFabObj = Instantiate(AxesTextPrefab, target.transform.position, Quaternion.identity) as GameObject;
        if (preFabObj)
        {
            preFabObj.transform.parent = target.transform;
            preFabObj.transform.position = preFabObj.transform.position + target.transform.position;
            preFabObj.GetComponent<TextMesh>().text = text;
            preFabObj.GetComponent<TextMesh>().characterSize = (int)(LastDistance * CharacterSizeMultiplier);
        }
    }
    private void attachObjectLabelOffset(GameObject target, string text, Color? color = null)
    {

        GameObject preFabObj = Instantiate(AxesTextOffsetPrefab, target.transform.position, Quaternion.identity) as GameObject;
        if (preFabObj)
        {
            preFabObj.transform.parent = target.transform;
            preFabObj.transform.position = preFabObj.transform.position + target.transform.position;
            Component[] textMeshes = GetComponentsInChildren<TextMesh>();
            foreach (TextMesh tm in textMeshes)
            {
                tm.text = text;
                tm.characterSize = (int)(LastDistance * CharacterSizeMultiplier);
            }

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







