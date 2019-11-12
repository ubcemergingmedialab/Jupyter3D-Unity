// Created by Amelia He, this script is used to display the name of the equations in VR 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEquation2 : MonoBehaviour
{
    // creates a string and stores the name of function
    private string displayText;
    // generates a textmesh that displays function name in 3D
    private TextMesh testMesh;

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        display();
    }

    //Function for displaying the text equation
    void display()
    {

        displayText = ProceduralGrid2.gridName; 
        //this creates new component TextMesh
        testMesh = GetComponent<TextMesh>();
        //this displays the name of function in TextMesh
        testMesh.text = displayText;
    }
}