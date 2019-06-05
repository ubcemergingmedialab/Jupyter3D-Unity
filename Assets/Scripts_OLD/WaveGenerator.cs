using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour {
    [SerializeField]
    float scale = 0.1f;
    [SerializeField]
    float speed = 1.0f;
    [SerializeField]
    float epsilonSquared = 0.1f;
    [SerializeField]
    Vector3 initPosition = new Vector3(0, 0, 0);

    private Vector3[] basePosition;
    private float beginTime;
    private Mesh mesh;
    private bool pause = false;
    private float pauseTime = 0;



    private void Start()
    {
        beginTime = Time.time;
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void FixedUpdate()
    {
        if (basePosition == null)
        {
                Debug.Log("reset mesh");
                basePosition = mesh.vertices;

        }

        Vector3[] newVertices = new Vector3[basePosition.Length];
        Debug.Log(basePosition.Length);
        float deltaTime = Time.time - beginTime;
        for (int i = 0; i < newVertices.Length; i++)
        {
            Vector3 vertex = basePosition[i];
            Vector2 init = new Vector2(initPosition.x, initPosition.z);
            Vector2 vert = new Vector2(vertex.x, vertex.z);
            float r = Vector2.Distance(init, vert);

            float val = deltaTime - (r / speed);

            if (val == 0)
            {
                vertex.y += scale / (2 * (2 * Mathf.PI) *  speed * (Mathf.Sqrt(Mathf.Pow(deltaTime, 2) - Mathf.Pow(r, 2) / Mathf.Pow(speed, 2))));// + epsilonSquared));
            }
            else if(val > 0)
            {
                vertex.y += scale / (speed * (4 * Mathf.PI) * (Mathf.Sqrt(Mathf.Pow(deltaTime, 2) - Mathf.Pow(r, 2) / Mathf.Pow(speed, 2))));// + epsilonSquared));
            }

            //vertex.y += Mathf.Sin(Time.time - (beginTime-pauseTime) * speed + (Mathf.Pow(basePosition[i].x - initPosition.x, 2) + Mathf.Pow(basePosition[i].z - initPosition.z, 2))) * scale;
            //vertex.y += (scale / Mathf.Sqrt((Mathf.Pow(vertex.x - initPosition.x, 2)) + (Mathf.Pow(vertex.z - initPosition.z, 2)) - speed * Mathf.Pow(deltaTime,2) + epsilonSquared));
            newVertices[i] = vertex;
        }
        mesh.vertices = newVertices;
        mesh.RecalculateNormals();
    }

    public void reset()
    {

        beginTime = Time.time;
        mesh.vertices = basePosition;
            
    }
}
