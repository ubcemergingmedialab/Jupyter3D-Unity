using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tester
public class TextEquation : MonoBehaviour
{

    private string displayText;
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

    void display()
    {



        if (ProceduralGrid.funcVR == 0)
        {
            displayText = "Ocean Wave 1";
        }
        else if (ProceduralGrid.funcVR == 1)
        {
            displayText = "Ocean Wave 2";
        }
        else if (ProceduralGrid.funcVR == 2)
        {
            displayText = "Drum Wave";
        }
        else if (ProceduralGrid.funcVR == 3)
        {
            displayText = "Multi Sine 1";
        }
        else if (ProceduralGrid.funcVR == 4)
        {
            displayText = "Multi Sine 2";
        }
        else if (ProceduralGrid.funcVR == 5)
        {
            displayText = "Mexican Hat";
        }

        testMesh = GetComponent<TextMesh>();
        testMesh.text = displayText;
    }
}