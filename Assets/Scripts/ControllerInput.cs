using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class ControllerInput : MonoBehaviour {

    public float BaseSpeed = 5f;
    public float BaseAmp = 4;
    //public float AmpIncrease = 0.25f;
    private void OnEnable()
    {
        ViveInput.AddListenerEx(HandRole.RightHand, ControllerButton.Trigger, ButtonEventType.Down, OnTrigger);
    }
    private void OnDisable()
    {
        ViveInput.RemoveListenerEx(HandRole.RightHand, ControllerButton.Trigger, ButtonEventType.Down, OnTrigger);
    }

    private void OnTrigger()
    {
          
    }

    private void Update()
    {

        ProcedualGrid.speed = ViveInput.GetAxisEx(HandRole.RightHand, ControllerAxis.Trigger) * BaseSpeed;
        Debug.Log("Changing speed to " + ProcedualGrid.speed);

        //if (ViveInput.GetAxisEx(HandRole.RightHand, ControllerAxis.Trigger) > 0.5f)
        //{
        //    ProcedualGrid.amplitude += AmpIncrease;
        //}

        //if (ViveInput.GetAxisEx(HandRole.RightHand, ControllerAxis.Trigger) < -0.5f)
        //{
        //    ProcedualGrid.amplitude -= AmpIncrease;
        //}


        ProcedualGrid.amplitude = ViveInput.GetAxisEx(HandRole.RightHand, ControllerAxis.JoystickY) * BaseAmp  + 1;
    }
}
