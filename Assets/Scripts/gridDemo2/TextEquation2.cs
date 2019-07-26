using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tester
public class TextEquation2 : MonoBehaviour
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
        else if (ProceduralGrid2.funcVR == 6)
        {
            displayText = "Gaussian";
        }

        testMesh = GetComponent<TextMesh>();
        testMesh.text = displayText;
    }
}