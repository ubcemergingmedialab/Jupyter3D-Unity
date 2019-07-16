﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using UnityEngine.SceneManagement;

public class ControllerInput : MonoBehaviour
{
    CharacterController characterController;

    const float pi = Mathf.PI;

    public float BaseSpeed = 10f;
    public float BaseAmp = 0;

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
        restartScene();
        //joystickXYFunctionality()

    }

    private void triggerFunctionality()
    {
        bool trigger = (ViveInput.GetPressEx(HandRole.LeftHand, ControllerButton.Trigger));


        if (trigger){
            ProcedualGrid.play = true;
        } else
        {
            ProcedualGrid.play = false;
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

            if (ProcedualGrid.funcVR < 5)
            {
                ProcedualGrid.funcVR += 1;
                //ProcedualGrid.function = ProcedualGrid.func;
            }
            else
            {
                ProcedualGrid.funcVR = 0;

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

    private void restartScene()
    {
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Pad))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            ProcedualGrid.amplitude = 1;
            ProcedualGrid.k = ((2f * pi) / 15);
        }
    }

}