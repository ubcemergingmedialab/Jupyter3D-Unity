using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]

public class ProcedualGrid : MonoBehaviour {

    // initializing lists and the mesh
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    
    // grid setting
    public float cellSize = 1;
    public Vector3 gridOffset; // to change the initial x y z position pf the grid
    public int gridSize; // Square grid, else create two variables 
    public GraphFunction current_function;  //gets the current function to pass to TextEquation.cs
    
    [Range(0, 1)] // discrete or continuous
    public int meshImplementation = 0;

    

    // functions list 
    //public GraphFunctionName function;
    public static float fun;
    public GraphFunction[] functions = { //Array of all the methods/functions to graph that are available to be used
        SineFunction, Sine2DFunction1, Sine2DFunction2, MultiSineFunction, MultiSine2DFunction,
        Cone, RippleDynamic
    };

    // Variables for the obove funstions 
    public static float amplitude = 1;
    public static float k = ((2f * pi) / 15); // chnage the wavelength
    public static float speed;  // can be changed but used 5 for nice display 
    public static float func = 0;


    /*
     * A wavelength of 1 produces no wave at all, instead the whole plane goes up and down uniformly. 
     * Other small wavelengths produce ugly waves that can even move backwards.
     * This problem is causes by the limited resolution of our plane mesh. Because vertices are spaces one unit apart, 
     * it cannot deal with wavelengths of 2 or smaller. In general, you have to keep the wavelength greater than twice the edge 
     * length of the triangles in your mesh. You don't want to cut it too close, because waves made up of two or three quads don't look good.
     * Either use larger wavelengths, or increase the resolution of your mesh. The simplest approach is to just use another mesh. 
     * Here is an alternative plane mesh that consists of 100×100 quads, instead of just 10×10. Each quad is still 1×1 unit, 
     * so you'll have to zoom out and multiply the wave properties by 10 to get the same result as before.
     * 
     */


    const float pi = Mathf.PI;


    // Use this for initialization
    void Awake () {

        mesh = GetComponent<MeshFilter>().mesh;
		
	}
	
	void Update () {

        

        if (meshImplementation  == 0)
        {
            MakeDiscreteProceduralGrid();

        } else if (meshImplementation == 1)
        {
            MakeContiguousProceduralGrid();
        }
        UpdateMesh();
		
	}

    void MakeDiscreteProceduralGrid()
        // Populating the informations, creating these arrays and filling it with the appropriate information 
    {
        GraphFunction f = functions[(int) func]; // Method delegation part using the array of functions defined above
        current_function = f; //passes f to the current function
        float sec = Time.time;        // Variable t refers to time 


        // set array sizes
        vertices = new Vector3[gridSize * gridSize * 4];  // only 4 vertices - GridSize * Gridsize, to make more and in two dimensions
        triangles = new int[gridSize * gridSize * 6];     // 6 sides for 2 triangles to make a quad - so not sharing any vertices, all distinct mesh 

        // set tracker intergers
        int v = 0;  // tracker for vertices 
        int t = 0;  // tracker for triangles

        // set vertex offset - because we don't have a permanent size 
        float vertexOffSet = cellSize * 0.5f; // deviding is more expensive than multiplying - to do this have in the middle at the origin  - shifter at the origin rather than being in a corner

        for (int x = 0; x < gridSize; x++) // iterating through the x dimension 
        {
            for (int z = 0; z < gridSize; z++) // iterating through the z dimension
            {
                Vector3 cellOffSet = new Vector3 (x * cellSize, f(x,z,sec) , z * cellSize) ; // to not have all cell stack on top of each other, change one cell offset - using x and y two know exactly which cell we are at 

                // populate the vertices and triangles arrays 
                // addind the tracker to keep going thats is 0123 then 4567 then ... in order to iterate through all vertices of the grid
                vertices[0 + v] = new Vector3(-vertexOffSet, 0, -vertexOffSet) + gridOffset + cellOffSet;
                vertices[1 + v] = new Vector3(-vertexOffSet, 0, vertexOffSet)  + gridOffset + cellOffSet;
                vertices[2 + v] = new Vector3(vertexOffSet, 0, -vertexOffSet)  + gridOffset + cellOffSet;
                vertices[3 + v] = new Vector3(vertexOffSet, 0, vertexOffSet)   + gridOffset + cellOffSet;

                // hard coding the pattern as it is always the same - two triangles, sharing two vertices, different order to keep it as clockwise
                // associting the triangle with the current vertices - that is + v (tracker)
                triangles[0 + t] = 0 + v;
                triangles[1 + t] = triangles[4 + t] = 1 + v;
                triangles[2 + t] = triangles[3 + t] = 2 + v;
                triangles[5 + t] = 3 + v;

                v += 4;
                t += 6;
            }
        }

    }

    void MakeContiguousProceduralGrid()
    // Populating the informations, creating these arrays and filling it with the appropriate information 
    {
        GraphFunction f = functions[(int) func]; // Method delegation part using the array of functions defined above
        current_function = f; //passes f to the current function
        float sec = Time.time;        // Variable t refers to time 

        // set array sizes
        vertices = new Vector3[(gridSize + 1) * (gridSize + 1)];  // girdsize two dimentions, + plus one to have that final corner 
        triangles = new int[gridSize * gridSize * 6];     // 6 sides for 2 triangles to make a quad 
        
        // set tracker intergers
        int v = 0;  // tracker for vertices 
        int t = 0;  // tracker for triangles
        ;
        // set vertex offset - because we don't have a permanent size 
        float vertexOffSet = cellSize * 0.5f; // deviding is more expensive than multiplying - to do this have in the middle at the origin  - shifter at the origin rather than being in a corner


        // Setting the vertices 
        float xmin = -10;
        float xmax = 10;
        float zmin = -10;
        float zmax = 10;
        float x, z;
        for (int i = 0; i <= gridSize; i++) // iterating through the x dimension - less or equal because we added the + 1
        {
            x = xmin + (xmax - xmin) * i / (float)gridSize;
            for (int j = 0; j <= gridSize; j++) // iterating through the z dimension - less or equal because we added the + 1
            {
                z = zmin + (zmax - zmin)*j / (float)gridSize;
                vertices[v] = new Vector3 ((x * cellSize) - vertexOffSet, f(x, z, sec) - vertexOffSet, (z * cellSize) - vertexOffSet);  // starting off at half of the cell and shifting it 
                v++;
            }
        }

        v = 0; // reset vertex tracker, as it got incresed in the previous for loop

        // Setting each cell's triangles
        for (int i = 0; i < gridSize; i++) // iterating through the x dimension - no need the equal side cause no + 1
        {
            for (int j = 0; j < gridSize; j++) // iterating through the z dimension - no need the equal side cause no + 1
            {
                // first triangle, using the origin point 
                // second, to the right of the first
                // third, up one row of the first 
                // fourth, up one row but into the right space of the first 

                triangles[0 + t] = 0 + v; 
                triangles[1 + t] = triangles[4 + t] = 1 + v;
                triangles[2 + t] = triangles[3 + t] = (gridSize + 1) + v; // same position as one but up one row
                triangles[5 + t] = (gridSize + 1) + v + 1; // up right of the first 

                v++;
                t += 6; //6 because of the triangle situation 
            }
            v++; // iterate our vertices one more time so that we start at a new row 
        }
    }

    

    void UpdateMesh()
    {
        mesh.Clear(); // clearing the mesh to make sure there is no existing information 

        mesh.vertices = vertices;   // assigning our vertices 
        mesh.triangles = triangles; // assigning our triangles 

        mesh.RecalculateNormals(); // fixing the lightening issue with the new normals
    }




    static float SineFunction(float x, float z, float t)        // float function because it needs to return a value 
    {
        return amplitude * Mathf.Sin(k * (x + t * speed));
    }

    static float MultiSineFunction(float x, float z, float t) // static becayse it is not associated with any specific object or vlaue instances
    {
        float y = amplitude * Mathf.Sin(k * (x + t * speed));
        y += amplitude * Mathf.Sin(k * (x + 2f * t * speed)) / 2f;       // adding complexity 
        y *= 2f / 3f;
        return y;
    }

    static float Sine2DFunction1(float x, float z, float t)
    {
        return amplitude * Mathf.Sin(k * (x + z + t * speed));
    }

    static float Sine2DFunction2(float x, float z, float t)
    {
        float y = amplitude * Mathf.Sin(k * (x + t * speed));
        y += amplitude * Mathf.Sin(k * (z + t * speed));
        y *= 0.5f;
        return y;
    }

    static float MultiSine2DFunction(float x, float z, float t)
    {
        float y = amplitude * Mathf.Sin(k * (x + z + t * 0.5f * speed));
        y += amplitude * Mathf.Sin(k * (x + t * speed));
        y += amplitude * Mathf.Sin(k * (z + 2f * t * speed)) * 0.5f;
        y *= 1f / 5.5f;
        return y;

    }

    static float Cone(float x, float z, float t)
    {
        float d = Mathf.Sqrt(x * x + z * z);
        float y = d;
        return y;
    }

    //static float Ripple(float x, float z, float t)
    //{
    //    float d = Mathf.Sqrt(x * x + z * z);
    //    float y = Mathf.Sin(4f * pi * d);
    //    return y;
    //}

    //static float RippleFading(float x, float z, float t)
    //{
    //    float d = Mathf.Sqrt(x * x + z * z);
    //    float y = Mathf.Sin(pi * (4f * 0.1f * k * d));
    //    y /= 1f + .5f * d;
    //    return y;
    //}

    // Mexican Hat
    // Mathematical implementation
    static float RippleDynamic(float x, float z, float t)
    {
       // x -= 10;
       // z -= 10;
        float d = Mathf.Sqrt(x * x + z * z);
        float y = Mathf.Sin(pi * (4f * 0.1f*k*d - t * speed));
        y /= 1f + .5f * d;
        return y;
    }
}
