using UnityEngine;
using UnityEngine.UI;

public class BoxColliderSize : MonoBehaviour
{
    //Make sure there is a BoxCollider component attached to your GameObject
    public BoxCollider m_Collider;
    private Vector3 size;


    void Start()
    {
        //Fetch the Collider from the GameObject
         m_Collider = GetComponent<BoxCollider>();

    }

    void Update()
    {

        m_Collider.size = new Vector3(20, 1 + ProcedualGrid.amplitude * 2f, 20);
        Debug.Log("Current BoxCollider Size : " + m_Collider.size);
    }
}