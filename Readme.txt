Stereoscopic Statistics v1.0
==========================

The goal of this project is to enable 3D stereoscopic visualizations of data to assist in the teaching of statistics and to enable clearer visual analysis of data. No existing tools were found that are designed to display data in stereoscopic 3D visualizations.

Requirements:
	- Unity 3
	- Graphics card with decent 3D rendering capability
	- 3D TV or monitor that accepts side-by-side format stereoscopic input


Code overview:
	- This project uses the Unity 3D game engine. It uses the open-source Stereoskopix FOV2GO for Unity to project the output from the Main Camera as two side-by-side images from slightly different camera angles that can be combined into one image on 3D TVs.

	- The main scene is Stereoscopic_Scatterplot/Assets/Scatter0.unity. The Cube object has data points added to it by the GeneratePoints.cs script. The GeneratePoints.cs script contains several parameters which can be set within the Unity editor. 

	- The points in the scatterplot can be imported from an external file or randomly generated. There are two paramters which identify the absolute file path and file name to load data points from. The format for the data is comma-deliminated x, y, and z coordinates. A first line of header data is ignored if any non-digits or commas are detected.

	- The collection of data points is normalized to fit inside the cube. There is an paramted on GeneratePoints.cs to normalize the data points by either indiviudally scaling each axis (and distoring the projection), or maintaining the aspect ratio of the collection of data points as they are normalized to the scale of the the display cube. Alternatively, if the 'Auto Detect Data Range' option is disabled, then parameters for minimum and maximum X, Y, and Z values 

	- The cube can be rotated to by dragging it with the mouse. The cube on the right-side projection accepts mouse input. There is also a parameter to change the materials that the cube is rendered from.

Areas for further work:
	- Accepting input for lines or triangles in addition to points that can be rendered inside the display cube.
	- Scaling the display cube to different rectangular dimensions based on input, and properly repositioning the camera.
	- Camera controls, possibly by arrow key.
	- Adding scale markers to the cube, or labels indicating axis name.
	- A GUI to set parameters in, so the project does not need to be run from the Unity editor.



This project was developed using the FOV2GO  toolkit created by the MxR Lab at USC: http://diy.mxrlab.com
