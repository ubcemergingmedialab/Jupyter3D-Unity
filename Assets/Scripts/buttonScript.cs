using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using UnityEngine.SceneManagement;

public class buttonScript : MonoBehaviour {

	public void ResetScene(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

		//this resets the variables back to the original numbers which are 1
		ProceduralGrid2.amplitude = 1;
		ProceduralGrid2.wavelength = 1;
	}

	public void pauseScene(){
		ProceduralGrid2.play = false;
	}

	public void playScene(){
		ProceduralGrid2.play = true;
	}

	public void NextScene(){
		SceneManager.LoadScene("welcomeScene");
	}


}
