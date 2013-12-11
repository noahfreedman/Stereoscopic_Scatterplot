using UnityEngine;

// display a list of objects that are a child of this component's transform.
// methods to select (??) 
// delete
// show edit sub menus (??)
public class Inventory : MonoBehaviour
{
    public bool ShowMenu = true;
    public float MenuAreaWidth = 200;
    private float AreaHeight = 100;
    public float ScreenXOffset = 10;
    public bool DoubleMenu = false; // Main menu controls this
    private GameObject CurrentSelection;
    private Rect MenuAreaRect;
    private Rect MenuAreaRightScreen;
    private Color MenuDefaultGUIColor;
    public Color SelectedColor = Color.green;
    private bool ShowSelectedObjectMenu = false; //toggle showing the pop up menu
    public Color[] AssignableColors;

    void Start()
    {
        MenuDefaultGUIColor = GUI.contentColor;

        ScreenXOffset = (Screen.width / 2) - ScreenXOffset;

        AreaHeight = Screen.height;
        MenuAreaRect = new Rect(ScreenXOffset, 0, MenuAreaWidth, AreaHeight);
        MenuAreaRightScreen = new Rect(Screen.width, 0, MenuAreaWidth, AreaHeight);

    }

    void OnGUI()
    {

        GUILayout.BeginArea(MenuAreaRect);

        DisplayGUI();
        GUILayout.EndArea();
        if (DoubleMenu)
        {

            GUILayout.BeginArea(MenuAreaRightScreen);
            DisplayGUI();
            GUILayout.EndArea();
        }
    }
    void MenuDisplaySelectedObject(GameObject obj)
    {
        GUI.enabled = true;
        GUILayout.BeginVertical("box");
        GUILayout.Label(obj.name);
        if (GUILayout.Button("Look At"))
        {
            Camera.main.transform.LookAt(obj.transform);
        }

        if (GUILayout.Button("delete"))
        {
            Destroy(obj);
        }

        GUILayout.EndVertical();
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

    void DisplayGUI()
    {
        if (ShowMenu)
        {
            ShowSelectedObjectMenu = (CurrentSelection != null);
            GUILayout.BeginHorizontal();
            if (ShowSelectedObjectMenu)
            {

                MenuDisplaySelectedObject(CurrentSelection);
            }
            GUILayout.BeginVertical("box");
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
            GUILayout.EndHorizontal();
        }
    }
}
