using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
 public class TextEquation : MonoBehaviour {
 
     private string displayText;
     private TextMesh testMesh;
 
     // Use this for initialization
     void Start () {
         display();
         
     }
     
     // Update is called once per frame
     void Update () {
         
     }
 
     void display()
     {
         if(current_function==SineFunction){
            displayText = "SineFunction";
         }
         else if(current_function==Sine2DFunction1){
            displayText = "Sine2DFunction1";
         }
        else if(current_function==Sine2DFunction2){
            displayText = "Sine2DFunction2";
         }
         else if(current_function==MultiSineFunction){
            displayText = "MultiSineFunction";
         }
         else if(current_function==MultiSine2DFunction){
            displayText = "MultiSine2DFunction";
         }
         else if(current_function==Cone){
            displayText = "Cone";
         }
         else (current_function==RippleDynamic){
            displayText = "RippleDynamic";
         }
         testMesh = GetComponent<TextMesh>();
         testMesh.text = displayText;
     }
 }