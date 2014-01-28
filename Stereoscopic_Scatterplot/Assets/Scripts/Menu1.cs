using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Menu1 : MonoBehaviour
{
    #region

	public Font font;
    public GUISkin MenuSkin;
    public Transform MainCamera;
    public Transform LeftCamera;
    public Transform RightCamera;
    public Texture OnButtonTexture;

    public Inventory Inventory;
    public GameObject AxisObject;

    // menu state
    public bool DoubleMenu = false;
    private bool ShowingSubmenu = false;
    private bool SubMenuCreateLine = false;
    private bool SubMenuCreatePlane = false;
    private bool SubMenuLoadPoints = false;
    private bool SubMenuGenPoints = false;
    private bool SubMenuOptions = false;
    private bool SubMenuCreateSinglePoint = false;
    private bool SubMenuCreatePlanarFunction = false;
    private bool SubMenuDemo = false;

    // input
    public float MenuWidth = 200;
    private int numberInput = 100;
    private float floatInput = 1.0f;
    private string floatInputString = "1.0";
    private string numberInputString = "500";
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
	private string menuFilePath = "Enter a full file path.";
    public float hSliderValue = 0.0f;
    private float RotationalSpeedMax = 6.0f;
	#endregion

	//For UniFileBrowser
	string message = "";
	float alpha = 1.0f;
	private char pathChar = '/';
	private string docsPath;

	void Start()
    {
		centerPosition = Vector3.zero;

		//Init File Browser
		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) {
			pathChar = '\\';
		}
		docsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
		UniFileBrowser.use.SetPath(docsPath);
	}
	
	void Update()
    {
        // esc opens the option menu, closes submenus.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackButton();
        }
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
        //GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height)); //Allows menu to create a double
        DisplayMenus();

        //GUILayout.EndArea();
        if (DoubleMenu)
        {
            GUILayout.BeginArea(new Rect(Screen.width / 2, 0, Screen.width, Screen.height)); //menu  double
            DisplayMenus();
            GUILayout.EndArea();
        }

		//UniFileBrowser
		var col = GUI.color;
		col.a = alpha;
		GUI.color = col;
		GUI.Label (new Rect(MenuWidth + 10, 50, 500, 1000), message);
		col.a = 1.0f;
		GUI.color = col;
    }

    void DisplayMenus()
    {
        if (ShowingSubmenu)
        {
            GUI.enabled = true;
			//Close inventory
			Inventory.CloseInventory();

            GUILayout.BeginVertical("box", GUILayout.Width(MenuWidth));  // column one - the open/close menu button
            MainMenu();
            /////////// SUBMENUS

            if (SubMenuCreateLine)
            {
                MenuCreateLine();
            }

            if (SubMenuGenPoints)
            {
                MenuGeneratePoints();
            }

            //if (SubMenuCreatePlane)
            //{
            //    MenuCreatePlanes();
            //}
            if (SubMenuCreatePlanarFunction)
            {
                MenuCreatePlanarFunction();
            }
            if (SubMenuDemo)
            {
                MenuDemoMode();
            }
            if (SubMenuOptions)
            {
                MenuOptions();
            }
            if (SubMenuCreateSinglePoint)
            {
                MenuCreateSinglePoint();
            }
            GUILayout.EndVertical();

        }
        else
        {
            MainMenuButton();
        }
    }
	public void CloseMenu() {
        ShowingSubmenu = false;
        SubMenuCreateLine = false;
        SubMenuCreatePlane = false;
        SubMenuCreatePlanarFunction = false;
        SubMenuLoadPoints = false;
        SubMenuGenPoints = false;
        SubMenuOptions = false;
        SubMenuCreateSinglePoint = false;
        SubMenuDemo = false;
	}
	public void ShowNotification() {

	}
    void BackButton()
    {
        ShowingSubmenu = true;
        SubMenuCreateLine = false;
        SubMenuCreatePlane = false;
        SubMenuCreatePlanarFunction = false;
        SubMenuLoadPoints = false;
        SubMenuGenPoints = false;
        SubMenuOptions = false;
        SubMenuCreateSinglePoint = false;
        SubMenuDemo = false;
    }

    void MainMenuButton()
    {
        if (OnButtonTexture)
        {
            if (GUI.Button(new Rect(0, 0, OnButtonTexture.height + 8, OnButtonTexture.width + 8), OnButtonTexture))
            {
                ShowingSubmenu = true;
            }
        }
        else
        {
            if (GUI.Button(new Rect(0, 0, 100, 20), "menu"))

                ShowingSubmenu = true;
        }

    }

    void MenuCreatePlanes()
    {

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
			Vector3 startPosition = VectorFromStrings(string_X_0, string_Y_0, string_Z_0);
            Vector3 endPosition = VectorFromStrings(string_X_1, string_Y_1, string_Z_1);
            Inventory.GetComponent<PlanesList>().AddPlane(startPosition, endPosition);
        }
        if (GUILayout.Button("Back"))
        {
            BackButton();
        }
        GUILayout.EndHorizontal();

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

            Inventory.GetComponent<ScatterplotList>().createSinglePoint(point);
        }
        if (GUILayout.Button("Back"))
        {
            BackButton();
        }
        GUILayout.EndHorizontal();

    }

    void MenuGeneratePoints()
    {

        GUILayout.BeginHorizontal();
        GUILayout.Label("Point 1");
        string_X_0 = GUILayout.TextField(string_X_0);
        string_Y_0 = GUILayout.TextField(string_Y_0);
        string_Z_0 = GUILayout.TextField(string_Z_0);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Number");
        int.TryParse(GUILayout.TextField(numberInputString), out numberInput);
        numberInputString = numberInput.ToString(); 
        centerPosition = VectorFromStrings(string_X_0, string_Y_0, string_Z_0);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Range");
        float.TryParse(GUILayout.TextField(floatInputString), out floatInput);
        floatInputString = floatInput.ToString();
        centerPosition = VectorFromStrings(string_X_0, string_Y_0, string_Z_0);
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate"))
        {
            Inventory.GetComponent<ScatterplotList>().createRandomPoints(centerPosition, numberInput, floatInput);
        }
        if (GUILayout.Button("Back"))
        {
            BackButton();
        }
        GUILayout.EndHorizontal();

    }

    void MenuCreateLine()
    {

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
			Inventory.GetComponent<LinesList>().AddALine(startPosition, endPosition, MainCamera.transform.position);
		}
		if (GUILayout.Button("Back"))
        {
            BackButton();
        }

    }

    void MenuDemoMode()
    {
        GUILayout.Label("Auto Rotation");

		hSliderValue = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), hSliderValue, -RotationalSpeedMax, RotationalSpeedMax);
        if (MainCamera)
        {
            MainCamera.GetComponent<DemoOrbitCamera>().RotationSpeed = hSliderValue;
        }


        GUILayout.Label("Speed");
        if (GUILayout.Button("Stop"))
        {
            hSliderValue = 0.0f;
        }

        if (GUILayout.Button("Back"))
        {
            BackButton();
        }



    }
    private bool isStereoMode = true;
    void MenuOptions()
    {

        GUI.enabled = true;


        AxisObject.active = GUILayout.Toggle(AxisObject.active, "Show Axis");
        isStereoMode = GUILayout.Toggle(isStereoMode, "Stereoscopic Mode");


        //DoubleMenu = isStereoMode;
        GUI.enabled = isStereoMode;
        LeftCamera.GetComponent<Camera>().active = isStereoMode;
        RightCamera.GetComponent<Camera>().active = isStereoMode;
        if (Inventory.GetComponent<Inventory>())
        {
			Inventory.GetComponent<Inventory>().StereoMenu = isStereoMode; //Controls menu doubling
			Inventory.GetComponent<Inventory>().DoubleMenu = DoubleMenu; //Controls whether inventory is on right
			DoubleMenu = isStereoMode; //Controls camera doubling
        }
        //if (disableStereoMode)
        //{
        //    DoubleMenu = false;
        //    GUI.enabled = false;

        //    LeftCamera.GetComponent<Camera>().active = false;
        //    RightCamera.GetComponent<Camera>().active = false;
        //    AxisObject.GetComponent<Inventory>().DoubleMenu = false;
        //}
        //else
        //{
        //    GUI.enabled = true;
        //    //MainCamera.active = true;
        //    LeftCamera.GetComponent<Camera>().active = true;
        //    RightCamera.GetComponent<Camera>().active = true;
        //    AxisObject.GetComponent<Inventory>().DoubleMenu = true;
        //}

        GUI.enabled = true;
        if (GUILayout.Button("Back"))
        {
            BackButton();
        }

    }

    void MainMenu()
    {
        //Menu Hides itself
        if (SubMenuCreateLine
            || SubMenuCreatePlane
            || SubMenuGenPoints
            || SubMenuOptions
            || SubMenuCreateSinglePoint
            || SubMenuCreatePlanarFunction
            || SubMenuDemo)
        {

        }
        else
        {
            //GUILayout.BeginVertical("box", GUILayout.Width(MenuWidth));
            //disableStereoMode = GUILayout.Toggle(disableStereoMode, "   Disable Stero Mode?");
            if (GUILayout.Button("Create Line.."))
            {
                SubMenuCreateLine = true;
            }

            if (GUILayout.Button("Create Point.."))
            {
                SubMenuCreateSinglePoint = true;
            }
            if (GUILayout.Button("Load Point Data.."))
            {
				UniFileBrowser.use.OpenFileWindow(_LoadPoints);
            }
            if (GUILayout.Button("Generate Scatter.."))
            {
                SubMenuGenPoints = true;
            }
            /*if (GUILayout.Button("Create Plane.."))
            {
                SubMenuCreatePlane = true;
            }*/
            if (GUILayout.Button("Create Planar Function.."))
            {
                SubMenuCreatePlanarFunction = true;
            }
            if (GUILayout.Button("Options"))
            {
                SubMenuOptions = true;
			}
			if (GUILayout.Button("Auto-Rotation"))
			{
				SubMenuDemo = true;
			}
			if (GUILayout.Button("Import Scene"))
			{
				//Show Open Scene menu
				UniFileBrowser.use.OpenFileWindow (_OpenScene);
			}
			if (GUILayout.Button("Save Scene"))
			{
				UniFileBrowser.use.SaveFileWindow (_SaveScene);
			}
			if (GUILayout.Button("Clear Scene"))
			{
				Inventory.ClearScene();
			}
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Close")) {
                CloseMenu();
            }
            if (GUILayout.Button("Quit."))
            {
                ShowingSubmenu = false;
                // TODO: Application.Quit isn't working in debug... be sure to try in a build
                //Application.Quit();
				System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            GUILayout.EndHorizontal();
            //GUILayout.EndVertical();
            GUI.enabled = true;
        }
	}
	private void _LoadPoints(string pathToFile) {
		var fileIndex = pathToFile.LastIndexOf (pathChar);
		string filename = pathToFile.Substring (fileIndex+1, pathToFile.Length-fileIndex-1);
		CloseMenu();
		UniFileBrowser.use.SetPath(pathToFile.Substring (0, fileIndex));
		Inventory.GetComponent<LoadedPointsList>().LoadPointsFile(pathToFile, filename);
	}
	private void _OpenScene(string pathToFile) {
		var fileIndex = pathToFile.LastIndexOf (pathChar);
		string filename = pathToFile.Substring (fileIndex+1, pathToFile.Length-fileIndex-1);
		message = "You selected file: " + filename;
		Fade();
		CloseMenu();
		UniFileBrowser.use.SetPath(pathToFile.Substring (0, fileIndex));
		Inventory.OpenScene(pathToFile);
	}
	private void _SaveScene(string pathToFile) {
		var fileIndex = pathToFile.LastIndexOf (pathChar);
		string filename = pathToFile.Substring (fileIndex+1, pathToFile.Length-fileIndex-1);
		message = "You're saving file: " + filename;
		Fade();
		CloseMenu();
		UniFileBrowser.use.SetPath(pathToFile.Substring (0, fileIndex));
		Inventory.SaveScene(pathToFile);
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
	
	//UniFileBrowser
	void Fade () {
		StopCoroutine ("FadeAlpha");	// Interrupt FadeAlpha if it's already running, so only one instance at a time can run
		StartCoroutine ("FadeAlpha");
	}
	
	IEnumerator FadeAlpha () {
		alpha = 1.0f;
		yield return new WaitForSeconds (5.0f);
		for (alpha = 1.0f; alpha > 0.0f; alpha -= Time.deltaTime/4) {
			yield return null;
		}
		message = "";
	}
}
