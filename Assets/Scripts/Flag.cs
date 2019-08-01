using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HTC.UnityPlugin.Vive;

// ** Flag scripts for the flag planting task, 
//    @author Kyle Mas, written on 07-10-19 ** // 

public class Flag : MonoBehaviour
{
    // public variables for the controllers
    public Transform RightController;
    public Transform Aimcircle;

    // variable to store the new position of the flag
    Vector3 newPosition;

    // public variables for the flag object to be planted
    public GameObject flag;
    public GameObject prefab;


    // Use this for initialization

    void Start()
    {
        newPosition = transform.position;

        // renders the component on the scene when set to true
        GetComponent<Renderer>().enabled = true;
    }


    // Update is called once per frame
    void Update()
    {
        // Checks if the grip button is pressed on the right controller
        if (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Grip))
        {
            // Renders the flag object
            GetComponent<Renderer>().enabled = true;

            RaycastHit hit;
            var ray = new Ray();

            // store controller positions on variable
            ray.origin = RightController.position;
            ray.direction = RightController.forward;

            // If raycast hits, spawn flag at hit point
            if (Physics.Raycast(ray, out hit))
            {
                //position of hit point stored on newPosition variable
                newPosition = hit.point;

                //set position of flag to newPosition
                flag.transform.position = newPosition;

                // reset new position variable
                newPosition = new Vector3(0, 0, 0);
            }


            // instantiate new instance of flag prefab
            GameObject obj = Instantiate(prefab, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity) as GameObject;

        }
    }
}