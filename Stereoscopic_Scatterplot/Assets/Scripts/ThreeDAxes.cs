using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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

    public bool showTicks = true;
    public bool showAxes = true;
    public int axis_range = 1000;
    private int LabelRangeMax = 1;
    private int LabelRangeMin = 1;// these get overwritten
    public int LabelInterval = 100;
    public float TickLineHeight = 1.0f;
    public float LineThickness = 0.45f;

    private int canvasIndex = 0;

    private float LastDistance = 0.1f;
    // the last measured distance from this transform (axis origin) to the camera
    // used to scale labels axis line thickness and label interval
    public float CharacterSize = 2.0f;//
    # endregion
    void Start()
    {
        LastDistance = (float)System.Math.Round(Vector3.Distance(transform.position, Camera.transform.position));
        BuildAxes();
    }
    void Update()
    {
        // recreate axis when distance from origin changes 
        //float currentDistance = (float)System.Math.Round(Vector3.Distance(transform.position, Camera.transform.position));
        float currentDistance = (float)Vector3.Distance(transform.position, Camera.transform.position);
        bool nearX = Math.Abs(Camera.transform.position.x) > Math.Abs(Camera.transform.position.z);

        // when the cam's relative position changes 

        if ((LastDistance < currentDistance) || (LastDistance > currentDistance))
        {
            //BuildAxes();
            //TODO: Smarter way: Remove delete from drawaxes. instead update the values..
        }

        LastDistance = currentDistance;
    }
    private void DestroyChildren()
    {
		//TODO: a nice fade bewteen would be nice
        //Transform[] allTransforms = gameObject.GetComponentsInChildren<Transform>();

        //foreach (Transform childObjects in allTransforms)
        //{
        //    if (gameObject.transform.IsChildOf(childObjects.transform) == false)
        //        Destroy(childObjects.gameObject);
        //}
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

            LabelRangeMax = axis_range;
            LabelRangeMin = -axis_range;

            for (int i = LabelRangeMin; i <= LabelRangeMax; i += LabelInterval)
            {

                // Y Vertical
                // The origin is nearly always far from camera so this will be fatter
                if (i != 0)
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

            for (int i = LabelRangeMin; i <= LabelRangeMax; i += LabelInterval )
            {
                // z
                if (i != 0)
                {
                    Vector3 start = new Vector3(0, -TickLineHeight, i);
                    Vector3 end = new Vector3(0, TickLineHeight, i);
                    GameObject line = createLine(start, end, LineThickness, zColor);
                    line.name = "zTick " + i.ToString();
                    line.transform.parent = transform;
                    LabelAxes(line, SigFigs(i), zColor);
                }
            }
            for (int i = LabelRangeMin; i <= LabelRangeMax; i += LabelInterval)
            {
                // x
                if (i != 0)
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
            GameObject yLine = createLine(y_start, y_end, LineThickness *3, yColor);
            GameObject zLine = createLine(z_start, z_end, LineThickness, zColor);

            xLine.transform.parent = transform;
            yLine.transform.parent = transform;
            zLine.transform.parent = transform;

        }
    }
    private GameObject createLine(Vector3 start, Vector3 end, float lineSize, Color c)
    {
        return createLine(start, end, lineSize, c, shader);
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
            preFabObj.GetComponent<AlwaysFacing>().Target = Camera;
            preFabObj.GetComponent<TextMesh>().characterSize = (int)(CharacterSize);
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

            }
            preFabObj.transform.GetComponentInChildren<TextMesh>().text = text;
            Component[] facings = GetComponentsInChildren<AlwaysFacing>();

            foreach (AlwaysFacing component in facings)
            {
                component.Target = Camera;
            }

        }
    }
    private string SigFigs(float i)
    {
        return SignificantDigits.ToString(System.Convert.ToDouble(i), 2);
    }
    private string SigFigs(int i)
    {

        return i.ToString();
    }
    private string SigFifths(float i)
    {
        i = (float)Math.Round(i / 5.0) * 5;
        return SignificantDigits.ToString(System.Convert.ToDouble(i), 2);
    }
}







