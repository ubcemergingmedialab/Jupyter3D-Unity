using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tester
public class TextEquation2 : MonoBehaviour
{

    private string displayText; // creates a string and stores the name of function
    private TextMesh testMesh; // generates a textmesh that displays function name in 3D 

    // Update is called once per frame
    void Update()
    {
        display();
    }
    //Function for displaying the text equation
    void display()
    {


        //the text equation is stored as elements in the funcVR grid. Thus, we are checking 
        //which element is currently being used
        if (ProceduralGrid2.funcVR == 0)
        {
            displayText = "Ocean Wave 1";
        }
        else if (ProceduralGrid2.funcVR == 1)
        {
            displayText = "Ocean Wave 2";
        }
        else if (ProceduralGrid2.funcVR == 2)
        {
            displayText = "Drum Wave";
        }
        else if (ProceduralGrid2.funcVR == 3)
        {
            displayText = "Multi Sine 1";
        }
        else if (ProceduralGrid2.funcVR == 4)
        {
            displayText = "Multi Sine 2";
        }
        else if (ProceduralGrid2.funcVR == 5)
        {
            displayText = "Mexican Hat";
        }
        //this creates new component TextMesh
        testMesh = GetComponent<TextMesh>();
        //this displays the name of function in TextMesh
        testMesh.text = displayText;
    }
}