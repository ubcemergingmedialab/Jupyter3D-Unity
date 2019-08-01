using UnityEngine;
using UnityEngine.UI;

//This script changes the collider size of the box collider whenever the wave amplitude and boundaries change
//Created by Harvey Huang

public class BoxColliderSize2 : MonoBehaviour
{
    //Make sure there is a BoxCollider component attached to your GameObject
    public UnityEngine.BoxCollider m_Collider;
    private Vector3 size;


    void Start()
    {
        //Fetch the Collider from the GameObject
        m_Collider = GetComponent<UnityEngine.BoxCollider>();

    }

    // this continuously updates the box collider the amplitude and boundaries changes for the wave
    // The update attempts to follow the size of the wave as closely as possible
    // The collider values are listed in the debug log (the console in unity)
    void Update()
    {

        m_Collider.size = new Vector3(ProceduralGrid2.boundaries *2f, ProceduralGrid2.amplitude * 2f, ProceduralGrid2.boundaries * 2f);
        Debug.Log("Current BoxCollider Size : " + m_Collider.size);
    }
}