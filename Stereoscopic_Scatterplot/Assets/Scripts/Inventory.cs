using UnityEngine;
using System;
using System.IO;
//using System.IO.Stream;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
public class Inventory : MonoBehaviour
{
    #region
    private int Version = 1;
    public GUISkin MenuSkin;
    public Transform MainCamera;
    public Texture ColorButtonTexture;
    public bool DoubleMenu = false; // Main menu controls this
    public float MenuWidth = 200;
    private float MenuHeight = 200;
    private string filename = "TestSave.txt";
    private float ScaleStep = 0.25f;
    private float ScaleLine = 0.0f;
    private GameObject CurrentSelection;
    private Color MenuDefaultGUIColor;
    private Color SelectedColor = Color.green;
    private Color[] AssignableColors;
    #endregion
    void Start()
    {
        MenuDefaultGUIColor = GUI.contentColor;
        MenuHeight = Screen.height;
        AssignableColors = new Color[4];
        AssignableColors[0] = Color.white;
        AssignableColors[1] = Color.red;
        AssignableColors[2] = Color.green;
        AssignableColors[3] = Color.blue;
    }
    void Update()
    {
        MenuDefaultGUIColor = GUI.contentColor;
        MenuHeight = Screen.height;
    }
    void OnGUI()
    {
        if (MenuSkin)
        {

            GUI.skin = MenuSkin;
            if (MenuSkin.box.fixedWidth != 0)
            {
                MenuWidth = MenuSkin.box.fixedWidth;
                // dont override skin set width.
            }
        }

        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));





        DisplayMenus();
        GUILayout.EndArea();

        if (DoubleMenu)
        {
            GUILayout.BeginArea(new Rect(Screen.width / 2, 0, Screen.width, Screen.height));
            DisplayMenus();
            GUILayout.EndArea();
        }

    }

    void DisplayMenus()
    {


        GUI.enabled = true;
        GUILayout.BeginHorizontal();
        GUILayout.Space((Screen.width / 2) - (MenuWidth * 2));
        //Debug.Log("DisplayMenus");

        if (CurrentSelection == null)
        {

            GUILayout.Space(MenuWidth);
        }
        else
        {
            GUILayout.BeginVertical("box", GUILayout.Width(MenuWidth));
            SubMenuObjectProperties(CurrentSelection);
            GUILayout.EndVertical();

        }

        GUILayout.BeginVertical("box", GUILayout.Width(MenuWidth));

        SelectableObjectsMenu();

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUI.enabled = true;
        //Debug.Log("DisplayMenus exit");
    }

    void SelectableObjectsMenu()
    {
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {

                GUI.color = CurrentButtonColor(child.gameObject);
                if (GUILayout.Button(child.name))
                {
                    SelectObject(child.gameObject);
                }
                GUI.contentColor = MenuDefaultGUIColor;
                GUI.color = MenuDefaultGUIColor;

            }
        }
        else
        {
            GUILayout.Label("0 Objects");
        }

        //Debug.Log("SelectableObjectsMenu exit");

    }
    void SubMenuObjectProperties(GameObject obj)
    {
        if (CurrentSelection != null)
        {

            GUILayout.Label(obj.name);

            if (MainCamera)
            {
                if (GUILayout.Button("Look At"))
                {
                    MainCamera.LookAt(obj.transform);
                }
            }

            GUILayout.BeginHorizontal();

            if (obj.GetComponent<LineRenderer>())
            {
                GUILayout.Label("Thickness");
                if (GUILayout.Button(" + "))
                {
                    ScaleLine += ScaleStep;
                    obj.GetComponent<LineRenderer>().SetWidth(ScaleLine, ScaleLine);

                }
                if (GUILayout.Button(" - "))
                {
                    ScaleLine -= ScaleStep;
                    obj.GetComponent<LineRenderer>().SetWidth(ScaleLine, ScaleLine);
                }
            }
            else
            {
                if ((obj.GetComponent<MeshFilter>()) && (obj.GetComponent<MeshFilter>().name == "Plane"))
                {
                    GUILayout.Label("Plane Size");
                    if (GUILayout.Button(" + "))
                    {
                        obj.transform.localScale += new Vector3(ScaleStep, ScaleStep, ScaleStep);
                    }
                    if (GUILayout.Button(" - "))
                    {
                        obj.transform.localScale += new Vector3(ScaleStep, ScaleStep, ScaleStep);
                    }
                }
                else
                {
                    GUILayout.Label("Point Size");
                    if (GUILayout.Button(" + "))
                    {
                        SetScaleChildren(obj, ScaleStep);
                    }
                    if (GUILayout.Button(" - "))
                    {
                        SetScaleChildren(obj, (-ScaleStep));
                    }

                }
            }
            GUILayout.EndHorizontal();

            ColorButtons(obj);


            if (GUILayout.Button("Delete"))
            {
                Destroy(obj); // note this makes SelectedObject null, making it unselected.
            }

        }

    }

    void SelectObject(GameObject obj)
    {
        if (CurrentSelection)
        {
            if (CurrentSelection.GetInstanceID() == obj.GetInstanceID())
            {
                //toggle selection off when selected twice.
                CurrentSelection = null;
            }
            else
            {
                CurrentSelection = obj;
            }
        }
        else
        {
            CurrentSelection = obj;
        }
    }

    Color CurrentButtonColor(GameObject obj)
    {
        if (CurrentSelection)
        {
            if (CurrentSelection.GetInstanceID() == obj.GetInstanceID())
            {
                return SelectedColor;
            }
            else
            {
                return MenuDefaultGUIColor;
            }
        }
        else
        {
            return MenuDefaultGUIColor;
        }
    }

    void TexturedButton(Texture texture)
    {
        if (texture)
        {
            if (GUI.Button(new Rect(0, 0, texture.height + 8, texture.width + 8), texture))
            {

            }
        }
        else
        {
            if (GUI.Button(new Rect(0, 0, 100, 20), "x"))
            {

            }


        }

    }
	public void SaveScene() {

	}
    public void SaveFile(string filename, System.Object obj)
    {
        try
        {
            Stream fileStream = File.Open(filename, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fileStream, obj);
            fileStream.Close();
        }
        catch (Exception e)
        {
            Debug.LogWarning("Save.SaveFile(): Failed to serialize object to a file " + filename + " (Reason: " + e.ToString() + ")");
        }
    }
    public static System.Object LoadFile(String filename)
    {
        try
        {
            Debug.Log("LoadFile...");
            Stream fileStream = File.Open(filename, FileMode.Open, FileAccess.Read);
            BinaryFormatter formatter = new BinaryFormatter();
            System.Object obj = formatter.Deserialize(fileStream);
            fileStream.Close();
            return obj;
        }
        catch (Exception e)
        {
            Debug.LogWarning("SaveLoad.LoadFile(): Failed to deserialize a file " + filename + " (Reason: " + e.ToString() + ")");
            return null;
        }
    }
    void ColorButtons(GameObject obj)
    {
        GUILayout.BeginHorizontal();
        if (AssignableColors.Length > 0)
        {

            foreach (Color c in AssignableColors)
            {
                GUI.contentColor = c;

                if (ColorButtonTexture)
                {
                    if (GUILayout.Button(ColorButtonTexture))
                    {
                        SetColorChildren(obj, c);
                    }
                }
                else
                {
                    if (GUILayout.Button("x"))
                    {
                        SetColorChildren(obj, c);
                    }


                }

            }
            GUI.contentColor = MenuDefaultGUIColor;
            GUI.color = MenuDefaultGUIColor;
        }
        GUILayout.EndHorizontal();
    }
    void SetColorChildren(GameObject obj, Color color)
    {
        if (obj.renderer)
        {
            obj.renderer.material.color = color;
        }
        if (obj.transform.childCount > 0)
        {
            Debug.Log(obj.transform.childCount.ToString());
            foreach (Transform child in obj.transform)
            {
                if (child.renderer)
                {
                    child.renderer.material.color = color;
                }
            }

        }
        
    }
    void SetScaleChildren(GameObject obj, float scale)
    {
        if (obj.transform.childCount > 0)
        {
            Debug.Log(obj.transform.childCount.ToString());
            foreach (Transform child in obj.transform)
            {
                child.transform.localScale += new Vector3(scale, scale, scale);
            }

        }

    }

}
