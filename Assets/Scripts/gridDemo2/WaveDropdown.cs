using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveDropdown : MonoBehaviour {

	public Dropdown dropdown;

	// Update is called once per frame
	void Update () {
		int value = dropdown.value;
		ProceduralGrid2.funcVR = value;
	}
}
