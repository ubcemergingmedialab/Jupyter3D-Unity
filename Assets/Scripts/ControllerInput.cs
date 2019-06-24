using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class ControllerInput : MonoBehaviour {

    public float BaseSpeed = 10f;
    public float BaseAmp = 0;
    //public float BaseK = ((2f * pi) / 2);

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

    private void Update()
    {
        triggerFunctionality();
        joystickYFunctionality();

    }

    private void triggerFunctionality()
    {
        float trigger = (ViveInput.GetAxisEx(HandRole.LeftHand, ControllerAxis.Trigger));
        
        
        if (trigger >= 0.5f && trigger > 0.0f)
        {
            ProcedualGrid.speed = BaseSpeed;
        }
        if (trigger< 0.5f && trigger> 0.0f)
        {
            ProcedualGrid.speed = BaseSpeed* 0.5f;
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


}
