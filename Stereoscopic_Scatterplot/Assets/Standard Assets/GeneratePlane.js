// This script is placed in public domain. The author takes no responsibility for any possible harm.


function Start ()
{
	//GeneratePlane();
}

public static function GeneratePlane (parent:GameObject, points:ArrayList, width, height)
{

	var heightMap : Texture2D;
	var material : Material;
	//var size = Vector3(200, 30, 200);
	// Create the game object containing the renderer
	var plane : GameObject = new GameObject("plane");
	plane.AddComponent(MeshFilter);
	plane.AddComponent("MeshRenderer");
	if (material)
		plane.renderer.material = material;
	else
		plane.renderer.material.color = Color.white;

	// Retrieve a mesh instance
	var mesh : Mesh = plane.GetComponent(MeshFilter).mesh;

	var y = 0;
	var x = 0;

	// Build vertices and UVs
	var vertices = new Vector3[height * width];
	var uv = new Vector2[height * width];
	var tangents = new Vector4[height * width];
	
	var uvScale = Vector2 (1.0 / (width - 1), 1.0 / (height - 1));
	var sizeScale = Vector3 (1, 1, 1);
	
	for (y=0;y<height;y++)
	{
		for (x=0;x<width;x++)
		{
			var vertex = points[y*width + x];
			vertices[y*width + x] = new Vector3(vertex.x, vertex.z, vertex.y);
			uv[y*width + x] = Vector2.Scale(Vector2 (x, y), uvScale);

			// Calculate tangent vector: a vector that goes from previous vertex
			// to next along X direction. We need tangents if we intend to
			// use bumpmap shaders on the mesh.
			var prevZ = (y*width + x - 1 > 0) ? points[y*width + x - 1] : points[0];
			var nextZ = (y*width + x + 1 < points.Count) ? points[y*width + x + 1] : points[points.Count - 1];
			var vertexL = new Vector3( x-1, prevZ.z, y );
			var vertexR = new Vector3( x+1, nextZ.z, y );
			var tan = new Vector3.Scale( sizeScale, vertexR - vertexL ).normalized;
			tangents[y*width + x] = Vector4( tan.x, tan.y, tan.z, -1.0 );
			//Debug.Log(y*width + x);
			//Debug.Log(vertex);
		}
	}
	
	// Assign them to the mesh
	mesh.vertices = vertices;
	mesh.uv = uv;

	// Build triangle indices: 3 indices into vertex array for each triangle
	var triangles = new int[(height - 1) * (width - 1) * 6]; //* 2];
	var index = 0;
	for (y=0;y<height-1;y++)
	{
		for (x=0;x<width-1;x++)
		{
			// For each grid cell output two triangles
			triangles[index++] = (y     * width) + x;
			triangles[index++] = ((y+1) * width) + x;
			triangles[index++] = (y     * width) + x + 1;

			triangles[index++] = ((y+1) * width) + x;
			triangles[index++] = ((y+1) * width) + x + 1;
			triangles[index++] = (y     * width) + x + 1;
			
		}
		/*for (x=0;x<width-1;x++)
		{
			triangles[index++] = (y     * width) + x + 1;
			triangles[index++] = ((y+1) * width) + x;
			triangles[index++] = (y     * width) + x;
			
			triangles[index++] = (y     * width) + x + 1;
			triangles[index++] = ((y+1) * width) + x + 1;
			triangles[index++] = ((y+1) * width) + x;
		}*/
	}
	// And assign them to the mesh
	mesh.triangles = triangles;
		
	// Auto-calculate vertex normals from the mesh
	mesh.RecalculateNormals();
	
	// Assign tangents after recalculating normals
	mesh.tangents = tangents;
	
	return plane;
}
