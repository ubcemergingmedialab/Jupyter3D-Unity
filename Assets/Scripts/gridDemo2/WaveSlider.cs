using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WaveSlider : MonoBehaviour {

	public void SliderA_Changed(float newVal){
		ProceduralGrid2.amplitude = newVal;
	}

	public void SliderW_Changed(float newVal){
		ProceduralGrid2.wavelength = newVal;
	}

	public void SliderF_Changed(float newVal){
		ProceduralGrid2.frequency = newVal;
	}

}
