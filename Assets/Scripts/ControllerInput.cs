using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

//tester
public class ControllerInput : MonoBehaviour
{
    CharacterController characterController;

    const float pi = Mathf.PI;

    public float BaseSpeed = 10f;
    public float BaseAmp = 0;
    public float MoveSpeed = 6.0f;

    private Vector3 moveDirection = Vector3.zero;



    //public float AmpIncrease = 0.25f;
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
        joystickYFunctionality();
        gripFunctionality();
        buttonFunctionality();
        //joystickXYFunctionality()

    }

    private void triggerFunctionality()
    {
        float trigger = (ViveInput.GetAxisEx(HandRole.LeftHand, ControllerAxis.Trigger));


        if (trigger >= 0.5f && trigger > 0.0f)
        {
            ProcedualGrid.speed = BaseSpeed;
        }
        if (trigger < 0.5f && trigger > 0.0f)
        {
            ProcedualGrid.speed = BaseSpeed * 0.5f;
        }
        if (trigger == 0.0f)
        {
            ProcedualGrid.speed = 0;
        }
    }

    private void joystickYFunctionality()
    {

        float joyStickY = ViveInput.GetAxisEx(HandRole.LeftHand, ControllerAxis.JoystickY);

        if (joyStickY > 0.90f)
        {
            ProcedualGrid.amplitude += BaseAmp;
        }
        if (joyStickY < -0.90f)
        {
            ProcedualGrid.amplitude -= BaseAmp;
        }
        // ProcedualGrid.amplitude = ViveInput.GetAxisEx(HandRole.LeftHand, ControllerAxis.JoystickY) * BaseAmp + 1;
    }

    private void gripFunctionality()
    {
        if (ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.Grip))
        {
            ProcedualGrid.k *= 2.0f;

        }

    }

    private void buttonFunctionality()
    {
        if (ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.Pad))
        {

            if (ProcedualGrid.func < 6)
            {
                ProcedualGrid.func += 1;

            }
            else
            {
                ProcedualGrid.func = 0;

            }

        }

    }
    
    private void joystickXYFunctionality()
    {

        float joyStickY = ViveInput.GetAxisEx(HandRole.RightHand, ControllerAxis.JoystickY);
        float joyStickX = ViveInput.GetAxisEx(HandRole.RightHand, ControllerAxis.JoystickX);

        moveDirection = new Vector3(Input.GetAxis("Horizontal") , 0.0f, Input.GetAxis("Horizontal"));

        // ProcedualGrid.amplitude = ViveInput.GetAxisEx(HandRole.LeftHand, ControllerAxis.JoystickY) * BaseAmp + 1;
        //x and y of joystick moves the x and z of the proceduralgrid gameobject
    }

}