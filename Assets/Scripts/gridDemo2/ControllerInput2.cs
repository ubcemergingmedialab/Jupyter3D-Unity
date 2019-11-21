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

    // Declaring and instantiating a vector3 which will be used to move the function horrizontally
    private Vector3 moveDirection = Vector3.zero;


    private void Start()
    {   // instantiating the characyerController
        characterController = GetComponent<CharacterController>();    // This method controllers how the function can be forwards and backwards
    // Created by Harvey Huag with the help of Rayhan Fakim
    private void joystickXYFunctionality()
    {
        float joyStickY = ViveInput.GetAxisEx(HandRole.RightHand, ControllerAxis.JoystickY);
        float joyStickX = ViveInput.GetAxisEx(HandRole.RightHand, ControllerAxis.JoystickX);

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Horizontal"));
    }

}
