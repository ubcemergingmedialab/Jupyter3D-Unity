using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using UnityEngine.SceneManagement;

public class ControllerInput2 : MonoBehaviour
{
    CharacterController characterController;

    // Constants
    const float pi = Mathf.PI;

    // Increments of the variables by the controllers
    public float AmplitudeIncrement = 0.02f;
    public float WavelengthDecrement = 2.0f;
    public float BoundariesIncrement = 1.0f;

    private Vector3 moveDirection = Vector3.zero;

    private void OnEnable()
    {
        ViveInput.AddListenerEx(HandRole.LeftHand, ControllerButton.Trigger, ButtonEventType.Down, OnTrigger);
    }
    private void OnDisable()
    {
        ViveInput.RemoveListenerEx(HandRole.LeftHand, ControllerButton.Trigger, ButtonEventType.Down, OnTrigger);
    }

    private void OnTrigger()
    {

    }
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        triggerFunctionality();
        joystickYLeftFunctionality();
        joystickYRightFunctionality();
        gripFunctionality();
        buttonFunctionality();
        restartScene();
    }

    private void triggerFunctionality()
    {

        if (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger))
        {
            ProceduralGrid2.play = !ProceduralGrid2.play;
        }
        
    }

    private void joystickYLeftFunctionality()
    {

        float joyStickYLEFT = ViveInput.GetAxisEx(HandRole.LeftHand, ControllerAxis.JoystickY);

        if (joyStickYLEFT > 0.5f)
        {
            ProceduralGrid2.amplitude += AmplitudeIncrement;
        }
        if (joyStickYLEFT < -0.5f)
        {
            ProceduralGrid2.amplitude -= AmplitudeIncrement;
        }
        // ProcedualGrid.amplitude = ViveInput.GetAxisEx(HandRole.LeftHand, ControllerAxis.JoystickY) * BaseAmp + 1;
    }

    private void joystickYRightFunctionality()
    {

        float joyStickYRight = ViveInput.GetAxisEx(HandRole.RightHand, ControllerAxis.JoystickY);

        if (joyStickYRight > 0.5f)
        {
            ProceduralGrid2.boundaries += BoundariesIncrement;
        }
        if (joyStickYRight < -0.5f)
        {
            ProceduralGrid2.boundaries -= BoundariesIncrement;
        }
    }

    private void gripFunctionality()
    {
        if ((ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.Grip)) && (ProceduralGrid.k <= (((2f * pi) / 25)) * 2))
        {
            ProceduralGrid2.wavelength *= WavelengthDecrement;

        }

    }

    private void buttonFunctionality()
    {
        if (ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.Pad))
        {

            if (ProceduralGrid2.funcVR < 5)
            {
                ProceduralGrid2.funcVR += 1;
                //ProcedualGrid.function = ProcedualGrid.func;
            }
            else
            {
                ProceduralGrid2.funcVR = 0;

            }

        }

    }

    private void joystickXYFunctionality()
    {

        float joyStickY = ViveInput.GetAxisEx(HandRole.RightHand, ControllerAxis.JoystickY);
        float joyStickX = ViveInput.GetAxisEx(HandRole.RightHand, ControllerAxis.JoystickX);

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Horizontal"));

    }

    private void restartScene()
    {
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Pad))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            ProceduralGrid2.amplitude = 1;
            ProceduralGrid2.wavelength = 1;
        }
    }

}