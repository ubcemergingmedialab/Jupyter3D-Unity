using UnityEngine;
using UnityEngine.UI;

public class BoxColliderSize : MonoBehaviour
{
    //Make sure there is a BoxCollider component attached to your GameObject
    public UnityEngine.BoxCollider m_Collider;
    private Vector3 size;


    void Start()
    {
        //Fetch the Collider from the GameObject
        m_Collider = GetComponent<UnityEngine.BoxCollider>();

    }

    void Update()
    {

        m_Collider.size = new Vector3(ProceduralGrid2.boundaries *2f, ProceduralGrid2.amplitude * 2f, ProceduralGrid2.boundaries * 2f);
        Debug.Log("Current BoxCollider Size : " + m_Collider.size);
    }
}