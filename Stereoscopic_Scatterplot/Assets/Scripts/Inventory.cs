using UnityEngine;
using System;
using System.IO;
//using System.IO.Stream;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
public class Inventory : MonoBehaviour
{
    public Transform MainCamera;
    public bool ShowMinimizeMenu = true;
    public float MenuWidth = 200;
    private float MenuHeight = 200;
    public float MenuXOffset = 100;
    public bool DoubleMenu = false; // Main menu controls this
    private GameObject CurrentSelection;
    private Rect MenuAreaLeftScreen;
    private Color MenuDefaultGUIColor;
    public Color SelectedColor = Color.green;
    private bool ShowSelectedObjectMenu = false; //toggle showing the pop up menu
    public Color[] AssignableColors;
    private string ScaleString = "";
    void Start()
    {
        MenuDefaultGUIColor = GUI.contentColor;

        MenuXOffset = ((Screen.width / 2) - MenuWidth);
        MenuHeight = Screen.height;
        MenuAreaLeftScreen = new Rect(MenuXOffset, 0, MenuWidth, MenuHeight);

        Debug.Log(Screen.width);
    }
    void Update()
    {
        MenuDefaultGUIColor = GUI.contentColor;

        MenuXOffset = (Screen.width - MenuWidth * 2);
        MenuHeight = Screen.height;
        MenuAreaLeftScreen = new Rect(Screen.width / 4, 0, MenuWidth, MenuHeight);

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
        string filename = "TestSave.txt";
        SaveFile(filename, obj);
    }
    void OnGUI()
    {
        if (ShowMinimizeMenu) // Show
        {
            GUILayout.BeginArea(MenuAreaLeftScreen);
            DisplayGUI();
            GUILayout.EndArea();
            if (DoubleMenu)
            {
                GUILayout.BeginArea(new Rect(Screen.width, 0, MenuWidth, MenuHeight));
                DisplayGUI();
                GUILayout.EndArea();
            }
        }
        else // Minimize
        {
            if (GUILayout.Button("nothing"))
            {
                // TODO: minimize button
            }
        }
    }
    void DisplayGUI()
    {
        ShowSelectedObjectMenu = (CurrentSelection != null);
        if (ShowSelectedObjectMenu)
        {
            MenuDisplayObjectEdit(CurrentSelection);
        }
        else
        {
            GUILayout.BeginVertical();
            //GUILayout.Box("", GUILayout.Width(MenuWidth), GUILayout.Height(20));
            GUILayout.EndVertical();
        }
        MenuListSelectableObjects();
    }


    void MenuListSelectableObjects()
    {
        GUILayout.BeginVertical("box", GUILayout.Width(MenuWidth));
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                GUILayout.BeginHorizontal();
                GUI.contentColor = CurrentButtonColor(child.gameObject);
                if (GUILayout.Button(child.name))
                {
                    SelectObject(child.gameObject);
                }
                GUI.contentColor = MenuDefaultGUIColor;
                GUILayout.EndHorizontal();
            }
        }
        else
        {
            GUILayout.Label("0 Objects");
        }
        GUILayout.EndVertical();


    }
    void MenuDisplayObjectEdit(GameObject obj)
    {
        GUI.enabled = true;
        GUILayout.BeginVertical("box");
        GUILayout.Label(obj.name);
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

        GUILayout.EndVertical();
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
}
