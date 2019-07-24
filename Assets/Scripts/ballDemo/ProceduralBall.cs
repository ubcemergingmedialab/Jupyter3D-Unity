using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implementation of the wave 

public class ProceduralBall : MonoBehaviour {

    public Transform pointPrefab;

    [Range(10, 100)]
    public int resolution = 10;

    public GraphFunctionName function;

    Transform[] points; //Array of all the points
    static GraphFunction[] functions = { //Array of all the methods/functions to graph that are available to be used
        SineFunction, Sine2DFunction1, Sine2DFunction2, MultiSineFunction, MultiSine2DFunction,
        Cone, Ripple, RippleFading, RippleDynamic
    };

    const float pi = Mathf.PI;
     



    void Awake()
    {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;

        Vector3 position;
        position.x = 0f;
        position.y = 0f;
        position.z = 0f; // Hard coding z to be 0 not changing 

        points = new Transform[resolution * resolution]; //An array of points in order to modify them all together with the variable of time


        //for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) // Length of the array points is the same as the resolution
        //{
        //    if (x == resolution) // each time we finish a row (when x = to resolution), we need to reset x = 0
        //    {
        //        x = 0;
        //        z += 1; // each row needs to be offset along the z dimension 
                
        //    }
        for (int i = 0, z = 0; z < resolution; z++) {
            position.z = ((z + 0.5f) * step - 1f);  // since z coordinates only chnages per itertion of the outer loop

            for (int x = 0; x < resolution; x++, i++)
            {

                Transform point = Instantiate(pointPrefab);      //Initialize it
                position.x = ((x + 0.5f) * step - 1f);          //Change position x
                // position.y = position.x * position.x;           //Change position y -> now changing y in the update coordinate as it moves with time
                point.localScale = scale;                      //Rescalling it to scale as defined above
                point.localPosition = position;                 // Assigning position to the point(the cube)

                point.SetParent(transform, false);              // false no need to keeep the object original world position
                points[i] = point; //Adding the point to the array of points

            }
        }


    }

    void Update() // only the y changing 
    {
        float t = Time.time;        // Variable t refers to time 

        GraphFunction f = functions[(int) function] ; // Method delegation part using the array of functions defined above
        //if (function == 0)
        //{
        //    f = SineFunction;
        //} else
        //{
        //    f = MultiSineFunction;
        //}

        for (int i = 0; i < points.Length; i++)
        {
         
            Transform point = points[i];    // reference to the current array element. then put assign it as point
            Vector3 position = point.localPosition; // get the position of point and put it as position
            // position.y = Mathf.Sin(Mathf.PI * (position.x + Time.time * 0.5f)); // derive the y position -> y is a sin function with x as variable 
            //                                                                     // time pi to be in scaleto be in between -1 and 1
            //                                                                    // added the time variable to have, f(x,t) = sin(pi(x + t))
            //if (function == 0)
            //{
            //    position.y = SineFunction(position.x, t);

            //}
            //else
            //{
            //    position.y = MultiSinFunction(position.x, t);

            //} // Not using this anymore, we delegate the methods
            position.y = f(position.x, position.z, t);
            point.localPosition = position; // need to assign back position to the point because we changed only the local variale not the object 
        }
    }

    // List of the implemented functions that can be seen in VR
    // All of them return the y value as the x and z are defined and, only the y is used for different height in order to create different types of waves
    // variable t is time, can be used for different frames of static functions; needs to be implements for the new wave function

    static float SineFunction(float x, float z, float t)        // float function because it needs to return a value 
    {
        return Mathf.Sin(pi * (x + t));
    }

    static float MultiSineFunction (float x, float z, float t) // static becayse it is not associated with any specific object or vlaue instances
    {
        float y =  Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;       // adding complexity 
        y *= 2f / 3f;
        return y;
    }

    static float Sine2DFunction1(float x, float z, float t)
    {
        return Mathf.Sin(pi * (x + z + t));
    }

    static float Sine2DFunction2(float x, float z, float t)
    {
        float y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(pi * (z + t));
        y *= 0.5f;
        return y;
    }

    static float MultiSine2DFunction (float x, float z, float t)
    {
        float y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
        y += Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
        y *= 1f / 5.5f;
        return y;

    }

    static float Cone(float x, float z, float t)
    {
        float d = Mathf.Sqrt(x * x + z * z);
        float y = d;
        return y;
    }

    static float Ripple(float x, float z, float t)
    {
        float d = Mathf.Sqrt(x * x + z * z);
        float y = Mathf.Sin(4f * pi * d);
        return y;
    }

    static float RippleFading(float x, float z, float t)
    {
        float d = Mathf.Sqrt(x * x + z * z);
        float y = Mathf.Sin(4f * pi * d);
        y /= 1f + 10f * d;
        return y;
    }

    // Mexican Hat
    // Mathematical implementation
    static float RippleDynamic(float x, float z, float t)
    {
        float d = Mathf.Sqrt(x * x + z * z);
        float y = Mathf.Sin(pi * (4f * d - t));
        y /= 1f + 10f * d;
        return y;
    }
}
