using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate Vector3 GraphFunctionControllerV2(float u, float v, float t);//, Vector2 centre, float amplitude = 0, float speed = 0, float epsilon = 0.001f);

public enum GraphFunctions
{
    Sine, WavePulse, Sine1D, Sine2D
}

public class GraphFunctionController : MonoBehaviour
{
    public static GraphFunctionController instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public float epsilon = 0.0001f;
    public Vector2 centrePoint;
    public float velocity;
    public float amplitude;

    public static Vector3 SineFunction(float u, float v, float t)
    {
        return new Vector3(u, Mathf.Sin(Mathf.PI * (u + t)), v);
    }

    public static Vector3 Sine1DFunction(float u, float v, float t)
    {
        return new Vector3(u, Mathf.Sin(Mathf.PI * (u + t)), 1);
    }


    public static Vector3 Sine2DFunction(float u, float v, float t)
    {
        //		return Mathf.Sin(pi * (x + z + t));
        float y = Mathf.Sin(Mathf.PI * (u + t));
        y += Mathf.Sin(Mathf.PI * (v + t));
        y *= 0.5f;
        return new Vector3(u, y, v);
    }
    public static Vector3 WavePulse(float u, float v, float t)//, Vector2 init, float amplitude, float speed)
    {

        Vector2 vert = new Vector2(u, v);
        float r = Vector2.Distance(instance.centrePoint, vert);
        float y = 0;
        float val = t - (r / instance.velocity);

        if (val == 0)
        {
            y += instance.amplitude / (2 * (2 * Mathf.PI) * instance.velocity * (Mathf.Sqrt(Mathf.Pow(t, 2) - Mathf.Pow(r, 2) / Mathf.Pow(instance.velocity, 2))));// + epsilonSquared));
        }
        else if (val > 0)
        {
            y += instance.amplitude / (instance.velocity * (4 * Mathf.PI) * (Mathf.Sqrt(Mathf.Pow(t, 2) - Mathf.Pow(r, 2) / Mathf.Pow(instance.velocity, 2))));// + epsilonSquared));
        }
        return new Vector3(u, y, v);
    }

}


