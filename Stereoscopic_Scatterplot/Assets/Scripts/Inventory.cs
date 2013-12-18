using UnityEngine;
using System;
using System.IO;
//using System.IO.Stream;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
public class Inventory : MonoBehaviour
{
    #region
    public GUISkin MenuSkin;
    public Transform MainCamera;
    public Texture ColorButtonTexture;
    public bool DoubleMenu = false; // Main menu controls this
    public bool ShowMinimizeMenu = true;
    public float MenuWidth = 200;
    private float MenuHeight = 200;
    private string filename = "TestSave.txt";
    private string ScaleString = "";
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
        Debug.Log("DisplayMenus");

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
        Debug.Log("DisplayMenus exit");
    }

    void SelectableObjectsMenu()
    {

        Debug.Log("SelectableObjectsMenu");
        if (GUILayout.Button("(load test)"))
        {
            TestLoad(); // note this makes SelectedObject null, making it unselected.
        }
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

        Debug.Log("SelectableObjectsMenu exit");

    }
    void SubMenuObjectProperties(GameObject obj)
    {
        if (CurrentSelection != null)
        {

            GUILayout.Label(obj.name);

            ColorSelector(obj);

            if (MainCamera)
            {
                if (GUILayout.Button("Look At"))
                {
                    MainCamera.LookAt(obj.transform);
                }
            }
            GUILayout.BeginHorizontal();
            ScaleString = GUILayout.TextField(ScaleString);
            if (GUILayout.Button("Set Scale"))
            {
                MainCamera.LookAt(obj.transform);
            }
            GUILayout.EndHorizontal();
            if (GUILayout.Button("delete"))
            {
                Destroy(obj); // note this makes SelectedObject null, making it unselected.
            }
            if (GUILayout.Button("save test"))
            {
                TestSave(obj); // note this makes SelectedObject null, making it unselected.
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

    void TestSave(GameObject obj)
    {

        SaveFile(filename, obj);
    }
    void TestLoad()
    {
        GameObject obj = (GameObject)LoadFile(filename);

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
    public static void SaveFile(string filename, System.Object obj)
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
    void ColorSelector(GameObject obj)
    {
        GUILayout.BeginVertical();
        if (AssignableColors.Length > 0)
        {

            foreach (Color c in AssignableColors)
            {
                GUI.contentColor = c;
                if (GUILayout.Button("Color test"))
                {
                    ColorChildAssignment(obj, c);
                }
            }
            GUI.contentColor = MenuDefaultGUIColor;
            GUI.color = MenuDefaultGUIColor;
        }
        GUILayout.EndVertical();
    }
    void ColorChildAssignment(GameObject obj, Color color)
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


}
