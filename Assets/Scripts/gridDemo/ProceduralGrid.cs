using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://catlikecoding.com/unity/tutorials/basics/mathematical-surfaces/
// https://www.youtube.com/watch?v=dc8LjeaL3k4

// Required components
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class ProceduralGrid : MonoBehaviour
{

    // initializing mesh and lists, to keep track of what is going on with all the mesh
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    // grid settings
    public float cellSize = 1;  // size of the cell 
    public Vector3 gridOffset;  // to determine the initial origin of where the grid appears
    public int gridSize;        // Square grid : assuming we want our width and height to be the same 
                                // if we want to have a rectangle, we would need gridSizeX and gridSizeY

    
    public static bool play = false; // TODO 
    private float time; // TODO 

    public static float boundaries = 10;


    // TODO 
    // functions list 
    public GraphFunctionName funcUnity;
    public GraphFunction[] functions = { //Array of all the methods/functions to graph that are available to be used
        SineFunction, Sine2DFunction1, Sine2DFunction2, MultiSineFunction, MultiSine2DFunction, MexicanHat
    };

    // TODO
    // Variables for the obove funstions 
    public static float amplitude = 1;
    public static float k = ((2f * pi) / 25); // chnage the wavelength
    public static float speed = 10;  // can be changed but used 5 for nice display 
    public static float funcVR = 0;
    // variables used to allow changing the function in vr and unity at the same time 
    private int prevFunc = 0; 


    /* NOTE ------------
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
    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;     // to get the component in unity
    }

    void Update()
    {
        // TODO
        if (play)
        {
            time++;
        }

        MakeContiguousProceduralGrid();
        UpdateMesh();
       
        
    }


   
    void MakeContiguousProceduralGrid()
    // Populating the informations, creating these arrays and filling it with the appropriate information 
    {
        // TODO
        if (prevFunc != (int)funcUnity){
            funcVR = (int)funcUnity;
        }

        // TODO
        GraphFunction f = functions[(int)funcVR]; // Method delegation part using the array of functions defined above

        // set array sizes
        vertices = new Vector3[(gridSize + 1) * (gridSize + 1)];    // single grid that is equal to the number of cells in a given row plus one because of the  final border needed on the x and z axis
        triangles = new int[gridSize * gridSize * 6];     // 6 sides for 2 triangles to make a quad 

        // set tracker intergers
        int v = 0;  // tracker for vertices 
        int t = 0;  // tracker for triangles
        ;
        // set vertex offset - shifting the cells so that they are centered on whole numbers
        float vertexOffSet = cellSize * 0.5f; // deviding is more expensive than multiplying - to do this have in the middle at the origin  - shifter at the origin rather than being in a corner


        // Setting the vertices 
        float xmin = -boundaries;
        float xmax = boundaries;
        float zmin = -boundaries;
        float zmax = boundaries;
        float x, z;
        for (int i = 0; i <= gridSize; i++) // iterating through the x dimension - less or equal because we added the + 1
        {
            x = xmin + (xmax - xmin) * i / (float)gridSize;
            for (int j = 0; j <= gridSize; j++) // iterating through the z dimension - less or equal because we added the + 1
            {
                z = zmin + (zmax - zmin) * j / (float)gridSize;
                vertices[v] = new Vector3((x * cellSize) - vertexOffSet, f(x, z, time/120) - vertexOffSet, (z * cellSize) - vertexOffSet);  // starting off at half of the cell and shifting it 
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

        prevFunc = (int)funcUnity;
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


    // Mexican Hat
    // Mathematical implementation
    static float MexicanHat(float x, float z, float t)
    {
        // x -= 10;
        // z -= 10;
        float d = Mathf.Sqrt(x * x + z * z);
        float y = Mathf.Sin(pi * (4f * 0.1f * k * d - t * speed));
        y /= 1f + .5f * d;
        return y;
    }
}
