/* Created by Rayhan Fakim, this script is used to implement functionalities to the controllers
 * which includes changing the values of the amplitude amd wavelength, playing/pausing,
 * planting the flag, dragging, changing the displayed function and reseting the scene
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using UnityEngine.SceneManagement;

public class ControllerInput2 : MonoBehaviour
{
    // Declaring a Character Controller, this will ease the movement contrained by collisions
    // witout having to deal with a rigidbody
    CharacterController characterController;

    // Constants
    const float pi = Mathf.PI;

    // Increments of the variables by the controllers
    public float AmplitudeIncrement = 0.02f;
    public float WavelengthDecrement = 2.0f;

    // Declaring and instantiating a vector3 which will be used to move the function horrizontally
    private Vector3 moveDirection = Vector3.zero;


    private void Start()
    {   // instantiating the characyerController
        characterController = GetComponent<CharacterController>();
    }

    // this method is called every method functionality for the controllers at every frame
    private void Update()
    {
        leftJoystickFunctionality();
        leftGripFunctionality();
        leftButtonFunctionality();
    }


    // this method controls the left controller's joystick
    private void leftJoystickFunctionality()
    {
        // putting the value of the axis of the joyustick in a float
        float joyStickYLEFT = ViveInput.GetAxisEx(HandRole.LeftHand, ControllerAxis.JoystickY);

        // checks if the axis is positive or negative, then increment or decrement the amplitude
        // we decided to do something only if the absolute value of the float is greater than 0.5
        // in order to have more controller and elimite unwanted changes from the controller
        if (joyStickYLEFT > 0.5f)
        {
            ProceduralGrid2.amplitude += AmplitudeIncrement;
        }
        if (joyStickYLEFT < -0.5f)
        {
            ProceduralGrid2.amplitude -= AmplitudeIncrement;
        }
    }

    // this method controls the left controller's grip
    private void leftGripFunctionality()
    {
        // checks if the grip is pressed and if the wavelength displayed is not too big, that is, it is in the boundaries written below
        // if true, then increment the wavelengh
        if ((ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.Grip)) && (ProceduralGrid2.wavelength <= ProceduralGrid2.wavelength * 2))
        {
            ProceduralGrid2.wavelength *= WavelengthDecrement;
        }

    }

    // this method controls the left controller's joystick press down
    private void leftButtonFunctionality()
    {
        // checks if the left controller's joystick is pressed, then change function to the next week =
        if (ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.Pad))
        {
            // current number of functions [6]
            if (ProceduralGrid2.funcVR < 6)
            {
                ProceduralGrid2.funcVR += 1;
            }
            else
            // making sure it loops around numbers between 0 and 6 only as there are a total of 7 functions
            {
                ProceduralGrid2.funcVR = 0;
            }

        }

    }

    // This method controllers how the function can be forwards and backwards
    // Created by Harvey Huag with the help of Rayhan Fakim
    private void joystickXYFunctionality()
    {
        float joyStickY = ViveInput.GetAxisEx(HandRole.RightHand, ControllerAxis.JoystickY);
        float joyStickX = ViveInput.GetAxisEx(HandRole.RightHand, ControllerAxis.JoystickX);

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Horizontal"));
    }

}
