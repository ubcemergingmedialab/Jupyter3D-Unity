using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Graph : MonoBehaviour {

    public int resolution = 10;
    public Transform pointPrefab;

    public GraphFunctions function;

    static GraphFunctionControllerV2[] functions = {
        GraphFunctionController.SineFunction, GraphFunctionController.WavePulse, GraphFunctionController.Sine1DFunction, GraphFunctionController.Sine2DFunction
    };

    private float beginTime;
    private bool pause = false;
    private float pauseTime = 0;

    private float t;
    private float t1Prev;
    private Transform[] t1PrevPositions;
    private float t2Prev;
    private Transform[] t2PrevPositions;

    private Transform[] points;

    void Awake()
    {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;

        points = new Transform[resolution * resolution];
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
            t1Prev = 0;
            t2Prev = 0;
            t1PrevPositions = points;
            t2PrevPositions = points;
        }

        beginTime = Time.time;
    }

    // Update is called once per frame
    void Update () {
        if (!pause)
        {
            t = Time.time - (beginTime - pauseTime);
            Debug.Log("Delta t: " + (t - t1Prev) + ", TCurrent: " + t + ", T1Prev: " + t1Prev + ", T2Prev: " + t2Prev);
            //Debug.Log("Delta t: " + (t - t1Prev) +  ", TCurrent: " + t + ", T1Prev: " + t1Prev + ", T2Prev: " + t2Prev);
            GraphFunctionControllerV2 f = functions[(int)function];
            float step = 2f / resolution;
            for (int i = 0, z = 0; z < resolution; z++)
            {
                float v = (z + 0.5f) * step - 1f;
                for (int x = 0; x < resolution; x++, i++)
                {
                    float u = (x + 0.5f) * step - 1f;
                    points[i].localPosition = f(u, v, t); //, init);
                }
            }
            t2Prev = t1Prev;
            t1Prev = t;
            t2PrevPositions = t1PrevPositions;
            t1PrevPositions = points;
        }
    }

    

    public void pauseWave()
    {
        if (!pause)
        {
            pauseTime = Time.time;
            pause = true;
        }
        else
        {
            beginTime = Time.time;
            pause = false;

        }
    }
}
/*
[CustomEditor(typeof(Graph))]
public class GraphEditor : Editor
{
    SerializedProperty transformProperty;

    private void OnEnable()
    {
        transformProperty = serializedObject.FindProperty("pointPrefab");
    }
    public override void OnInspectorGUI()
    {
        var graph = target as Graph;

        EditorGUILayout.IntSlider(graph.resolution, 10, 50);
        EditorGUILayout.PropertyField(transformProperty);
        EditorGUILayout.EnumPopup(graph.function);

        switch (graph.function)
        {
            case GraphFunctions.Sine:
                break;
            case GraphFunctions.WavePulse:
                break;
        }
    }
}
*/