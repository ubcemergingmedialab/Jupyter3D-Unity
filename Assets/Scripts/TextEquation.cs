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



        if (ProcedualGrid.funcVR == 0)
        {
            displayText = "SineFunction";
        }
        else if (ProcedualGrid.funcVR == 1)
        {
            displayText = "Sine2DFunction1";
        }
        else if (ProcedualGrid.funcVR == 2)
        {
            displayText = "Sine2DFunction2";
        }
        else if (ProcedualGrid.funcVR == 3)
        {
            displayText = "MultiSineFunction";
        }
        else if (ProcedualGrid.funcVR == 4)
        {
            displayText = "MultiSine2DFunction";
        }
       
        else if (ProcedualGrid.funcVR == 5)
        {
            displayText = "MexicanHat";
        }

        testMesh = GetComponent<TextMesh>();
        testMesh.text = displayText;
    }
}