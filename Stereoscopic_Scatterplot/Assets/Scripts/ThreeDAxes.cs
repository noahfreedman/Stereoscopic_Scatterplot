using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ThreeDAxes : MonoBehaviour
{
    # region
    public GameObject AxesTextPrefab;   // use in horizontal axes
    public GameObject AxesTextOffsetPrefab; // use in vertical axes 
    public Transform MainCamera;
    public Shader Shader;
    public Color xColor = new Color(1, 0, 0, 1f);
    public Color yColor = new Color(0, 1, 0, 1f);
    public Color zColor = new Color(0, 0, 1, 1f);

    public bool showHalfTicks = false;
    public bool showQuarterTicks = false;
    public bool showTicks = true;
    public bool showAxes = true;
    public int axis_range = 1000;
    public int LabelRange = 1000;

    public float LabelInterval = 100;
    public float TickLineHeight = 1.0f;
    public float LineThickness = 0.45f;
    public float VerticalLabelsOffset = 1.0f;
    public float CharacterSize = 2.0f;//
 

    private int canvasIndex = 0;
    private int LabelRangeMax = 1;
    private int LabelRangeMin = 1;// these get overwritten

    # endregion
    void Start()
    {

        BuildAxes();
    }

    private void DestroyChildren()
    {
        //TODO: a nice fade bewteen would be nice
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    private void BuildAxes()
    {

        DestroyChildren(); // this is slow
        if (showTicks)
        {

            LabelRangeMax = LabelRange;
            LabelRangeMin = -LabelRange;


            for (float i = LabelRangeMin; i <= LabelRangeMax; i += LabelInterval)
            {

                // Y Vertical
                // The origin is nearly always far from camera so this will be fatter 3X
                if (Math.Abs(i) >= LabelInterval)
                {
                    Vector3 start = new Vector3(-TickLineHeight * 3, i, 0);
                    Vector3 end = new Vector3(TickLineHeight * 3, i, 0);
                    GameObject line = createLine(start, end, LineThickness, yColor);
                    line.name = "yTick ";
                    Vector3 start2 = new Vector3(0, i, TickLineHeight * 3);
                    Vector3 end2 = new Vector3(0, i, -TickLineHeight * 3);
                    GameObject line2 = createLine(start2, end2, LineThickness, yColor);

                    line2.name = "yTick " + i.ToString();
                    line.transform.parent = transform;
                    line2.transform.parent = transform;

                    LabelVerticalAxes(line2, SigFigs(i), yColor);
                }
            }

            for (float i = LabelRangeMin; i <= LabelRangeMax; i += LabelInterval)
            {
                // z
                if (Math.Abs(i) >= LabelInterval)
                {
                    Vector3 start = new Vector3(0, -TickLineHeight, i);
                    Vector3 end = new Vector3(0, TickLineHeight, i);
                    GameObject line = createLine(start, end, LineThickness, zColor);
                    line.name = "zTick " + i.ToString();
                    line.transform.parent = transform;
                    LabelAxes(line, SigFigs(i), zColor);
                }
            }
            for (float i = LabelRangeMin; i <= LabelRangeMax; i += LabelInterval)
            {
                // x
                if (Math.Abs(i) >= LabelInterval)
                {
                    Vector3 start = new Vector3(i, -TickLineHeight, 0);
                    Vector3 end = new Vector3(i, TickLineHeight, 0);
                    GameObject line = createLine(start, end, LineThickness, xColor);
                    line.name = "xTick " + i.ToString();
                    line.transform.parent = transform;
                    LabelAxes(line, SigFigs(i), xColor);
                }
            }
        }
        if (showAxes)
        {

            Vector3 x_start = new Vector3(-axis_range, 0, 0);
            Vector3 x_end = new Vector3(axis_range, 0, 0);
            Vector3 y_start = new Vector3(0, -axis_range, 0);
            Vector3 y_end = new Vector3(0, axis_range, 0);
            Vector3 z_start = new Vector3(0, 0, -axis_range);
            Vector3 z_end = new Vector3(0, 0, axis_range);
            GameObject xLine = createLine(x_start, x_end, LineThickness, xColor);
            GameObject yLine = createLine(y_start, y_end, LineThickness * 3, yColor);
            GameObject zLine = createLine(z_start, z_end, LineThickness, zColor);

            xLine.transform.parent = transform;
            yLine.transform.parent = transform;
            zLine.transform.parent = transform;

        }
    }
    private GameObject createLine(Vector3 start, Vector3 end, float lineSize, Color c)
    {
        return createLine(start, end, lineSize, c, Shader);
    }
    private GameObject createLine(Vector3 start, Vector3 end, float lineSize, Color c, Shader s)
    {

        GameObject returnLineGameObject = new GameObject("line" + canvasIndex);

        returnLineGameObject.transform.rotation = transform.rotation;

        LineRenderer lines = (LineRenderer)returnLineGameObject.AddComponent<LineRenderer>();
        lines.material = new Material(s);
        lines.material.color = c;
        lines.useWorldSpace = false;
        lines.SetWidth(lineSize, lineSize);
        lines.SetVertexCount(2);
        end = end - start;
        lines.SetPosition(1, end);
        returnLineGameObject.transform.position = start;
        canvasIndex++;
        return returnLineGameObject;
    }
    private void LabelAxes(GameObject target, string text, Color? color = null)
    {

        GameObject preFabObj = Instantiate(AxesTextPrefab, target.transform.position, Quaternion.identity) as GameObject;
        if (preFabObj)
        {

            //preFabObj.transform.position = preFabObj.transform.position + target.transform.position;
            preFabObj.transform.parent = target.transform;
            preFabObj.GetComponent<TextMesh>().text = text;
            preFabObj.GetComponent<AlwaysFacing>().Target = MainCamera;
            preFabObj.GetComponent<TextMesh>().characterSize = CharacterSize;
        }
    }
    private void LabelVerticalAxes(GameObject target, string text, Color? color = null)
    {
        Vector3 targetPositionXY = target.transform.position;
        targetPositionXY.z = 0;
        GameObject preFabObj = Instantiate(AxesTextOffsetPrefab, targetPositionXY, Quaternion.identity) as GameObject;
        if (preFabObj)
        {
            preFabObj.transform.parent = target.transform;
            Component[] textMeshes = preFabObj.GetComponentsInChildren<TextMesh>();


            foreach (TextMesh component in textMeshes)
            {

                component.text = text;
                component.characterSize = CharacterSize;



                Vector3 labelPosition = component.transform.position;
                labelPosition.x = VerticalLabelsOffset;
                component.transform.position = labelPosition;

            }
            preFabObj.transform.GetComponentInChildren<TextMesh>().text = text;
            Component[] components = GetComponentsInChildren<AlwaysFacing>();

            foreach (AlwaysFacing component in components)
            {
                component.Target = MainCamera;
            }
        }
    }
    private string SigFigs(float i)
    {
        return SignificantDigits.ToString(System.Convert.ToDouble(i), 2);
    }

 
}







