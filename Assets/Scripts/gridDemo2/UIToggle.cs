using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class UIToggle : MonoBehaviour {

	public GameObject uiCanvas;
	bool on;

	// Use this for initialization
	void Start () {
		on = true;
	}

	void Update() {
        joystickABfunctionality();
    }
    private void joystickABfunctionality() {
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.AKey))
        {
            on = false;
            uiCanvas.SetActive(on);
        }
        else if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.BKey)) {
            on = true;
            uiCanvas.SetActive(on);
        }
    }
}
