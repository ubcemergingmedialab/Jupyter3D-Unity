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



        if (ProcedualGrid.func == 0)
        {
            displayText = "SineFunction";
        }
        else if (ProcedualGrid.func == 1)
        {
            displayText = "Sine2DFunction1";
        }
        else if (ProcedualGrid.func == 2)
        {
            displayText = "Sine2DFunction2";
        }
        else if (ProcedualGrid.func == 3)
        {
            displayText = "MultiSineFunction";
        }
        else if (ProcedualGrid.func == 4)
        {
            displayText = "MultiSine2DFunction";
        }
        else if (ProcedualGrid.func == 5)
        {
            displayText = "Cone";
        }
        else if (ProcedualGrid.func == 6)
        {
            displayText = "RippleDynamic";
        }

        testMesh = GetComponent<TextMesh>();
        testMesh.text = displayText;
    }
}