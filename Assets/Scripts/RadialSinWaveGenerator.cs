using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialSinWaveGenerator : MonoBehaviour {

    [SerializeField]
    float scale = 0.1f;
    [SerializeField]
    float speed = 1.0f;
    [SerializeField]
    Vector3 initPosition = new Vector3(0, 0, 0);

    private Vector3[] basePosition;
    private float beginTime;
    private Mesh mesh;
    private bool pause = false;
    private float pauseTime = 0;

    private void Start()
    {
        beginTime = Time.realtimeSinceStartup;
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void Update()
    {
        if (!pause)
        {
            if (basePosition == null)
            {
                Debug.Log("reset mesh");
                basePosition = mesh.vertices;
            }

            Vector3[] newVertices = new Vector3[basePosition.Length];
            for (int i = 0; i < newVertices.Length; i++)
            {
                Vector3 vertex = basePosition[i];
                float deltaTime = Time.time - (beginTime - pauseTime);
                float r = Vector3.Distance(initPosition, vertex);
                vertex.y += Mathf.Sin(deltaTime * speed + (Mathf.Pow(r, 2))) * scale;
                newVertices[i] = vertex;
            }
            mesh.vertices = newVertices;
            mesh.RecalculateNormals();
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
