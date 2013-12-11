using System.Collections.Generic;
using UnityEngine;

public class Menu1 : MonoBehaviour
{
    public bool DoubleMenu = false;
    public GameObject Inventory;
    public GameObject AxisObject;
    public Texture OnButtonTexture;
    private bool ShowMenu = false;
    private bool ShowCreateLineMenu = false;
    private bool ShowCreatePlaneMenu = false;
    private bool ShowLoadPointsMenu = false;
    private bool ShowGenPointsMenu = false;
    private bool ShowOptionsMenu = false;
    private bool ShowCreateSinglePointMenu = false;
    private bool ShowCreatePlanarFunctionMenu = false;
    private string string_X_0 = "0.0";
    private string string_Y_0 = "0.0";
    private string string_Z_0 = "0.0";
    private string string_X_1 = "0.0";
    private string string_Y_1 = "0.0";
    private string string_Z_1 = "0.0";
    private string formula = "Z = 2*X + Y.^3";
    private string string_X_min = "-2.0";
    private string string_X_interval = "0.2";
    private string string_X_max = "2.0";
    private string string_Y_min = "-2.0";
    private string string_Y_interval = "0.2";
    private string string_Y_max = "2.0";
    private Vector3 centerPosition;
    private string menuFileName = "default.csv";
    private string menuFilePath = "";
    //private string LoadPath = "";
    private int MenuWidth = 300;
    private int MenuHeight = 600;
    void Start()
    {
        centerPosition = Vector3.zero;
        menuFileName = Inventory.GetComponent<LoadPoints>().fileName.ToString();
        menuFilePath = Inventory.GetComponent<LoadPoints>()._filePath.ToString();
        MenuHeight = Screen.height;
    }

    void Update()
    {
        // esc opens the option menu, closes submenus.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackButton();
        }
    }

    void BackButton()
    {
        ShowMenu = !ShowMenu;
        ShowCreateLineMenu = false;
        ShowCreatePlaneMenu = false;
        ShowCreatePlanarFunctionMenu = false;
        ShowLoadPointsMenu = false;
        ShowGenPointsMenu = false;
        ShowOptionsMenu = false;
        ShowCreateSinglePointMenu = false;
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, MenuWidth, MenuHeight));
        DisplayMenu();
        GUILayout.EndArea();
        if (DoubleMenu)
        {
            GUILayout.BeginArea(new Rect(Screen.width / 2, 0, MenuWidth, MenuHeight));
            DisplayMenu();
            GUILayout.EndArea();
        }
    }

    void MainMenuButton()
    {
        if (OnButtonTexture)
        {
            if (GUI.Button(new Rect(0, 0, OnButtonTexture.height + 8, OnButtonTexture.width + 8), OnButtonTexture))
            {
                ShowMenu = true;
            }
        }
        else
        {
            if (GUI.Button(new Rect(0, 0, 100, 20), "menu"))

                ShowMenu = true;
        }

    }

    public Vector3 GUIVectorFromStrings(string x, string y, string z)
    {
        float float_X;
        float float_Y;
        float float_Z;
        GUILayout.BeginHorizontal();
        float.TryParse(GUILayout.TextField(x), out float_X);
        float.TryParse(GUILayout.TextField(y), out float_Y);
        float.TryParse(GUILayout.TextField(z), out float_Z);
        GUILayout.EndHorizontal();
        return new Vector3(float_X, float_Y, float_Z);

    }

    public Vector3 VectorFromStrings(string x, string y, string z)
    {
        float float_X;
        float float_Y;
        float float_Z;
        float.TryParse(x, out float_X);
        float.TryParse(y, out float_Y);
        float.TryParse(z, out float_Z);
        return new Vector3(float_X, float_Y, float_Z);

    }

    void MenuCreatePlanes()
    {
        GUI.enabled = true;
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Point 1");
        string_X_0 = GUILayout.TextField(string_X_0);
        string_Y_0 = GUILayout.TextField(string_Y_0);
        string_Z_0 = GUILayout.TextField(string_Z_0);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Point 2");
        string_X_1 = GUILayout.TextField(string_X_1);
        string_Y_1 = GUILayout.TextField(string_Y_1);
        string_Z_1 = GUILayout.TextField(string_Z_1);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Create Plane"))
        {
            Vector3 startPosition = GUIVectorFromStrings(string_X_0, string_Y_0, string_Z_0);
            Vector3 endPosition = VectorFromStrings(string_X_1, string_Y_1, string_Z_1);
            Inventory.GetComponent<PlanesList>().AddPlane(startPosition, endPosition);
        }
        if (GUILayout.Button("Back"))
        {
            BackButton();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    void MenuCreatePlanarFunction()
    {
        GUI.enabled = true;
        GUILayout.BeginVertical("box");
        GUILayout.Label("Enter a function Z in terms of X and Y, using MatLab notation.");
        GUILayout.BeginHorizontal();
        formula = GUILayout.TextField(formula);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("X min, X interval, X max:");
        string_X_min = GUILayout.TextField(string_X_min);
        string_X_interval = GUILayout.TextField(string_X_interval);
        string_X_max = GUILayout.TextField(string_X_max);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Y min, Y interval, Y max:");
        string_Y_min = GUILayout.TextField(string_Y_min);
        string_Y_interval = GUILayout.TextField(string_Y_interval);
        string_Y_max = GUILayout.TextField(string_Y_max);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Generate Planar Function"))
        {
            //Vector3 startPosition = GUIVectorFromStrings (string_X_0, string_Y_0, string_Z_0);
            //Vector3 endPosition = VectorFromStrings (string_X_1, string_Y_1, string_Z_1);
            //Stage.GetComponent<PlanesList> ().AddPlane (startPosition, endPosition);

            MatLabInterface matlab = gameObject.AddComponent(typeof(MatLabInterface)) as MatLabInterface;
            double[] xRange = { double.Parse(string_X_min), double.Parse(string_X_interval), double.Parse(string_X_max) };
            double[] yRange = { double.Parse(string_Y_min), double.Parse(string_Y_interval), double.Parse(string_Y_max) };
            matlab.formulaToMeshgrid(formula, xRange, yRange);
        }
        if (GUILayout.Button("Back"))
        {
            BackButton();
        }
        GUILayout.EndVertical();
    }

    void MenuCreateSinglePoint()
    {
        GUI.enabled = true;
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Point X Y Z:");
        string_X_0 = GUILayout.TextField(string_X_0);
        string_Y_0 = GUILayout.TextField(string_Y_0);
        string_Z_0 = GUILayout.TextField(string_Z_0);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Point"))
        {

            Vector3 point = GUIVectorFromStrings(string_X_0, string_Y_0, string_Z_0);

            Inventory.GetComponent<RandomPoints>().createSinglePoint(point);
        }
        if (GUILayout.Button("Back"))
        {
            BackButton();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    void MenuGeneratePoints()
    {
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Point 1");
        string_X_0 = GUILayout.TextField(string_X_0);
        string_Y_0 = GUILayout.TextField(string_Y_0);
        string_Z_0 = GUILayout.TextField(string_Z_0);
        GUILayout.EndHorizontal();

        centerPosition = VectorFromStrings(string_X_0, string_Y_0, string_Z_0);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate"))
        {
            Inventory.GetComponent<RandomPoints>().createRandomPoints(centerPosition, 333);
        }
        if (GUILayout.Button("Back"))
        {
            BackButton();
        }

        GUILayout.EndHorizontal();
    }

    void MenuLoadPoints()
    {
        string fp = Inventory.GetComponent<LoadPoints>()._filePath;
        string fn = Inventory.GetComponent<LoadPoints>().fileName;
        string labelName = System.IO.Path.Combine(fp, fn);

        GUILayout.BeginVertical("box");
        GUILayout.Label(labelName);

        menuFileName = GUILayout.TextField(menuFileName);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set"))
        {
            Inventory.GetComponent<LoadPoints>().fileName = menuFileName;

        }
        if (GUILayout.Button("Load"))
        {
            Inventory.GetComponent<LoadPoints>().LoadPointsFile();
        }
        if (GUILayout.Button("Back"))
        {
            BackButton();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        List<string> foundRecentFilenames = Inventory.GetComponent<LoadPoints>().recentFiles;
        if (foundRecentFilenames.Count > 0)
        {
            foreach (string recentFile in foundRecentFilenames)
            {
                GUILayout.Label(recentFile);
            }
            GUILayout.EndVertical();
        }

    }

    void MenuCreateLine()
    {
        GUILayout.BeginVertical("box");
        // vertex coord entry grid
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUI.enabled = true;
        GUILayout.Label("Point 1");
        // TODO: rename sLine_0_X
        string_X_0 = GUILayout.TextField(string_X_0);
        string_Y_0 = GUILayout.TextField(string_Y_0);
        string_Z_0 = GUILayout.TextField(string_Z_0);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Point 2");
        string_X_1 = GUILayout.TextField(string_X_1);
        string_Y_1 = GUILayout.TextField(string_Y_1);
        string_Z_1 = GUILayout.TextField(string_Z_1);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        if (GUILayout.Button("Create Line"))
        {
            Vector3 startPosition = GUIVectorFromStrings(string_X_0, string_Y_0, string_Z_0);
            Vector3 endPosition = GUIVectorFromStrings(string_X_1, string_Y_1, string_Z_1);
            Inventory.GetComponent<LinesList>().AddALine(startPosition, endPosition);
        }
        if (GUILayout.Button("Back"))
        {
            BackButton();
        }
        GUILayout.EndVertical();
    }

    void MenuOptions()
    {
        string csvPath = Inventory.GetComponent<LoadPoints>()._filePath.ToString();
        string csvFilename = Inventory.GetComponent<LoadPoints>().fileName.ToString();

        GUI.enabled = true;
        GUILayout.BeginVertical("box");

        AxisObject.active = GUILayout.Toggle(AxisObject.active, "Show Axis?");
        DoubleMenu = GUILayout.Toggle(DoubleMenu, "Show Double Menu?");
        Inventory.GetComponent<Inventory>().DoubleMenu = DoubleMenu;

        if (GUILayout.Button("Back"))
        {
            BackButton();
        }
        GUILayout.EndVertical();
    }

    void DisplayMenu()
    {
        if (ShowMenu)
        {

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();  // column one - the open/close menu button
            MenuMain();
            /////////// SUBMENUS

            if (ShowCreateLineMenu)
            {
                MenuCreateLine();
            }

            if (ShowLoadPointsMenu)
            {
                MenuLoadPoints();
            }

            if (ShowGenPointsMenu)
            {
                MenuGeneratePoints();
            }

            if (ShowCreatePlaneMenu)
            {
                MenuCreatePlanes();
            }
            if (ShowCreatePlanarFunctionMenu)
            {
                MenuCreatePlanarFunction();
            }
            if (ShowOptionsMenu)
            {
                MenuOptions();
            }
            if (ShowCreateSinglePointMenu)
            {
                MenuCreateSinglePoint();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

        }
        else
        {
            MainMenuButton();
        }
    }

    void MenuMain()
    {
        //Menu Hides itself here
        if (ShowCreateLineMenu
            || ShowCreatePlaneMenu
            || ShowLoadPointsMenu
            || ShowGenPointsMenu
            || ShowOptionsMenu
            || ShowCreateSinglePointMenu
            || ShowCreatePlanarFunctionMenu)
        {

        }
        else
        {
            GUILayout.BeginVertical("box");

            if (GUILayout.Button("Create Line.."))
            {
                ShowCreateLineMenu = true;
            }

            if (GUILayout.Button("Create Point.."))
            {
                ShowCreateSinglePointMenu = true;
            }
            if (GUILayout.Button("Load Point Data.."))
            {
                ShowLoadPointsMenu = true;
            }
            if (GUILayout.Button("Generate Scatter.."))
            {
                ShowGenPointsMenu = true;
            }
            if (GUILayout.Button("Create Plane.."))
            {
                ShowCreatePlaneMenu = true;
            }
            if (GUILayout.Button("Create Planar Function.."))
            {
                ShowCreatePlanarFunctionMenu = true;
            }
            if (GUILayout.Button("Options"))
            {
                ShowOptionsMenu = true;
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Minimize"))
            {
                BackButton();
            }
            if (GUILayout.Button("Quit."))
            {
                ShowMenu = false;
                // TODO: Application.Quit isn't working in debug... be sure to try in a build
                Application.Quit();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUI.enabled = true;
        }
    }

}
