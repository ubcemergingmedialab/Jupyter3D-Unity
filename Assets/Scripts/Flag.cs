using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.AI;

using HTC.UnityPlugin.Vive;



public class Flag : MonoBehaviour
{



    Vector3 newPosition;



    // Use this for initialization

    void Start()
    {

        newPosition = transform.position;

        GetComponent<Renderer>().enabled = false;

    }



    // Update is called once per frame

    void Update()
    {



        if (Input.GetMouseButtonDown(0))

        {

            GetComponent<Renderer>().enabled = true;

            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);



            // If raycast hits, spawn flag at hit point

            if (Physics.Raycast(ray, out hit))

            {

                newPosition = hit.point;

                transform.position = newPosition;

                //good luck!

            }

        }



        if (Input.GetMouseButtonDown(1))

        {

            GetComponent<Renderer>().enabled = false;

        }

    }

}