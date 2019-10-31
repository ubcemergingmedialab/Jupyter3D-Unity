/*  M. Lamoureux, July 21, 2019
    Largely based on the GridDemo/ProceduralGrid code that was written by Rayhan Fakim.
    I took out the VR controls, so I can run this on a Mac.
    Did careful work on memory allocation, so we don't waste time rebuilding the grid with each frame display.
    The compiled app runs smoothly on a Mac; however, it is jerky on

    R. Fakim, July 23, 2019
    Made some changes to input compatibility with controllers in VR

    M. Lamoureux, July 22, 2019
    Here, we want to do actual finite difference code to greate the waves.

*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

/*
	Let's make some decisions about the grid.
	We have an x,z grid in the horizontal plane and y is the vertical direction.
	We will assume the x,z values range between -1 and 1, although this could be changed.
	Our amplitude will be on the order of 1, wavelength on the order of 1, and frequency on the order of 1 Hz
	To see things differently, we really should move the camera around.

	The gridSize is an integer, it is the number of points in both the x and z directions. Don't use gridSize+1 anywhere!
	Sometimes it is useful to have gridSize to be an odd integer, so we hit the orgin (0,0).
    This odd number is great for the Mexican hat example.
 */


public class ProceduralGrid2 : MonoBehaviour
{

    // initializing lists and the mesh
    Mesh mesh;
    Vector3[] vertices; // Note the syntax. "vertices" is a pointer to a list of Vector3's. Memory is allocated in makeGrid().
    int[] triangles; // "triangeles" is a point to a list of integer indices. Memory for the list is allocated in makeGrid().

    // The following six variables are set in the Uniy Inspector
    public static float amplitude = .5f;
    public static float wavelength = 1;
    public static float frequency = 1;
    public static float boundaries = 1;

    // Finite differential variables
    float[,] yVals;  // This will be 3 arrays of floats, to hold past, present and future values of FD calculations (whay float?)
    int yPast, yPresent, yFuture;

    // Time related variables - time guard
    public static bool play = true; // Time will only increase if play is set to true
    private float sec = 1;
    public static float speedOfWave = 1 / 120f;

    public int gridSize; // Square grid, else create two variables , represents the number of mesh on the scene
    public GraphFunctionName funcUnity;

    // functions list , to match the index variable "funcUnity" above.
    public GraphFunction[] functions = { //Array of all the methods/functions to graph that are available to be used
        SineFunction, Sine2DFunction1, Sine2DFunction2, MultiSineFunction, MultiSine2DFunction, MexicanHat, Gauss
    };

    // variable for you to change the function of the waves
    // funcVR is for controllers (priority)
    public static float funcVR = 0;

    // variables used to allow changing the function in vr and unity at the same time
    private int prevFunc = 0;
    private int prevGridSize = 10;

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
  	const float tau = 2.0f*Mathf.PI;  // This is a more useful constant. Read the "Tau manifesto" online!


    // Awake() and Update() are system functions called by unity, to setup the animation, and update it each frame
    // Use this for initialization
    void Awake()
    {

        mesh = GetComponent<MeshFilter>().mesh;
        makeGrid();

    }

    void Update() {

        if (funcVR == 5)
        {
            makeGrid();
        }

        updateGrid();
        mesh.Clear(); // clearing the mesh to make sure there is no existing information
        mesh.MarkDynamic(); // This makes the mesh more responsive to frequent changes.
		    mesh.vertices = vertices;   // assigning our vertices
    	  mesh.triangles = triangles; // assigning our triangles
        mesh.RecalculateNormals(); // fixing the lightening issue with the new normals
    }

    // We make the computational grid. Usually we only have to do this at "Awake" or when grid size changes.
    // It eats up memory, so we are careful onlu to call it once.

    void makeGrid() {
    	// force gridSize into a reasonable range.
    	if (gridSize <3)
    		gridSize = 3;
    	if (gridSize > 301)
    		gridSize = 301;
    	// set array sizes
        vertices = new Vector3[gridSize*gridSize];  	// gridsize in two dimentions
        triangles = new int[gridSize * gridSize * 6];   // 6 sides for 2 triangles in each square of grid
         // set tracker integers
        int v = 0;  // tracker for vertices
        int t = 0;  // tracker for triangles

        // we avoid offsets and scalings. This should be done in the camera, not in our computational grid.

        // Setting the vertices
        float xmin = -boundaries;
        float xmax = boundaries;
        float zmin = -boundaries;
        float zmax = boundaries;
        float x, z;
        for (int i = 0; i < gridSize; i++) { // iterating through the x dimension
            x = xmin + i*(xmax - xmin)/(gridSize-1);
            for (int j = 0; j < gridSize; j++) { // iterating through the z dimension
                z = zmin + j*(zmax - zmin)/(gridSize-1);
                vertices[v].x = x;
                vertices[v].y = 0;
                vertices[v].z = z;
                v++;
            }
        }

        v = 0; // reset vertex tracker, as it got incresed in the previous for loop

        // Setting each cell's triangles
        for (int i = 0; i < gridSize-1; i++) { // iterating through the x dimension
            for (int j = 0; j < gridSize-1; j++) { // iterating through the z dimension - no need the equal side cause no + 1
                // triangles are just integer indices that will reference the vertice list.
                // each triangle points to 3 different vertices. There are six per square
                triangles[0 + t] = 0 + v;						// bottom left corner
                triangles[1 + t] = triangles[4 + t] = 1 + v;	// bottom right corner
                triangles[2 + t] = triangles[3 + t] = gridSize + v; // top left corner
                triangles[5 + t] = gridSize + v + 1; // top right corner

                v++;
                t += 6; //6 because of the triangle situation
            }
            v++; // iterate our vertices one more time so that we start at a new row
        }


        // Now we set up the computational arrays for the FD computations, with appropriate index numbers
        yVals = new float[3, gridSize * gridSize];
        yPast = 0; yPresent = 1; yFuture = 2;

        // set up the initial values for the waveform
        float dx = .1f * (1 / 30f);
        float dz = .1f * (1 / 30f);
        for (v = 0; v < gridSize * gridSize; v++)
        {
            yVals[yPast, v] = Gauss(vertices[v].x, vertices[v].z, 0f);
            yVals[yPresent, v] = Gauss(vertices[v].x - dx, vertices[v].z, 0f);
        }
    }

    // On each frame update, all we have to do is update the y-values in the grid. All else is the same.
    // In this function, we can also adjust amplitude, wavelength, frequency in the resulting waveforms


    void updateGrid()
    {
        if (prevGridSize != (int)gridSize)
        {  // if the gridSize changes via the User, we have to rebuild the grid.
            makeGrid();
            prevGridSize = (int)gridSize;
        }

        if (prevFunc != (int)funcUnity)
        { // Maybe this is Rayhan's hack? Not sure why we need this.
            funcVR = (int)funcUnity;
            prevFunc = (int)funcUnity;
        }


        GraphFunction f = functions[(int)funcVR]; // Method delegation part using the array of functions defined above



        if (play)
        {
            // loops through the list of functions
            if (funcVR < 6)
            {
                sec += speedOfWave;
                // set vertex offset - because we don't have a permanent size
                for (int v = 0; v < gridSize * gridSize; v++)
                {
                    vertices[v].y = amplitude * f(vertices[v].x / wavelength, vertices[v].z / wavelength, frequency * sec);
                }
            }
            else
                for (int v = 0; v < gridSize * gridSize; v++)
                    vertices[v].y = amplitude * yVals[yPresent, v];

            zeroFDedges();  // Let's kill off the edges, to give a hard reflecting boundary for the waves
            oneFDstep();
        }



    }




    // These are various wave-like functions. We normalize them to have a wavelength of 1, and frequency of 1 Hz.
    // The amplitude is normalized to 1 as well.
    // The code above allows changes to those assumptions, but we don't put in here in the functions.


    static float SineFunction(float x, float z, float t)        // float function because it needs to return a value
    {
        return Mathf.Sin(tau*(x-t));
    }

    static float MultiSineFunction(float x, float z, float t) // static because it is not associated with any specific object or vlaue instances
    {
        float y = Mathf.Sin(tau*(x-t));
        y += 0.5f*Mathf.Sin(tau*(x - 2f * t));       // adding complexity
        y *= .667f;
        return y;
    }

    static float Sine2DFunction1(float x, float z, float t)
    {
        return Mathf.Sin(tau * (.707f*(x + z) - t ));
    }

    static float Sine2DFunction2(float x, float z, float t)
    {
        float y = Mathf.Sin(tau * (x - t));
        y += Mathf.Sin(tau * (z - t));
        y *= 0.5f;
        return y;
    }

    static float MultiSine2DFunction(float x, float z, float t)
    {
        float y = Mathf.Sin(tau * (x + z - t * 0.5f));
        y += Mathf.Sin(tau *(z - t));
        y += 0.5f*Mathf.Sin(tau * (z - 2f * t ));
        y *= 0.2f;
        return y;

    }

    static float MexicanHat(float x, float z, float t)
    {
	    float d = Mathf.Sqrt(x * x + z * z);
      float y = Mathf.Sin(tau * (d - t));
      y /= 1f + 2f * d;
      return y;
    }

    // Gaussian
    static float Gauss(float x, float z, float t)
    {
        float w = .25f;  // the width of the Gaussian
        return Mathf.Exp(-(x * x + z * z) / (w * w));
    }



    // Finite differential methodes

    // we update the finite difference calculations by one time step
    // The formula comes from the central difference for the 2nd derivative of Y, so
    // y(t+1) = 2*y(t) - y(t-1) + alpha*Laplacian of y(t).
    // The magic constant is alpha = (c*dt/dx)^2, where c = velocity, dt = time step, dx = dz = spatial step

    void oneFDstep()
    {

        float dx = 2f / (gridSize - 1);  // assume xmin, xmax spans a distance of 2.
        float dt = 1f / 30f;  // thirty frames a second, maybe
        float c = 0.1f; // default velocity of 1/10 unit length per second
        float alpha = (c * dt / dx) * (c * dt / dx);

        int v = gridSize;  // Skip the first row, which we want to do (zero boundary)
        for (int i = 1; i < gridSize - 1; i++)
        {
            for (int j = 1; j < gridSize - 1; j++)
            {
                v++;  // move in by one column. Note we miss the first column, which we want to do anyway
                yVals[yFuture, v] = 2f * yVals[yPresent, v] - yVals[yPast, v] - alpha * (4f * yVals[yPresent, v] - yVals[yPresent, v - 1] - yVals[yPresent, v + 1] - yVals[yPresent, v - gridSize] - yVals[yPresent, v + gridSize]);
            }
            v += 2; // skip over the last column as well
        }
        // increment the past/present/future indices
        yPast = (++yPast) % 3;
        yPresent = (++yPresent) % 3;
        yFuture = (++yFuture) % 3;
    }

    // We set the edges of the computational grid to zero. This gives a hard reflector for the wave.
    void zeroFDedges()
    {
        for (int i = 0; i < gridSize; i++) // bottom edge
            yVals[yPast, i] = yVals[yPresent, i] = yVals[yFuture, i] = 0f;
        for (int i = 0; i < gridSize * gridSize; i += gridSize) // left edge
            yVals[yPast, i] = yVals[yPresent, i] = yVals[yFuture, i] = 0f;
        for (int i = (gridSize - 1) * gridSize; i < gridSize * gridSize; i++) // top edge
            yVals[yPast, i] = yVals[yPresent, i] = yVals[yFuture, i] = 0f;
        for (int i = gridSize - 1; i < gridSize * gridSize; i += gridSize) // right edge
            yVals[yPast, i] = yVals[yPresent, i] = yVals[yFuture, i] = 0f;
    }
}
