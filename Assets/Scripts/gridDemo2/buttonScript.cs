using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using UnityEngine.SceneManagement;


// Script written by Kyle Mas on Nov 12, 2019.
// Deals with all the button functionalities like play, pause, back, reset.
public class buttonScript : MonoBehaviour {

	// Resets the scene and wave properties
	public void ResetScene(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

		//this resets the variables back to the original numbers which are 1
		ProceduralGrid2.amplitude = 1;
		ProceduralGrid2.wavelength = 1;
	}

    public void resetWave() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Changes the play (boolean) variable of the ProceduralGrid to false
    // which pauses loading the scene.
    public void pauseScene(){
		ProceduralGrid2.play = false;
	}

	// Changes the play (boolean) variable of the ProceduralGrid to true
	// which continues loading the scene.
	public void playScene(){
		ProceduralGrid2.play = true;
	}

	// When called, loads the scene called "welcomeScene", which contains
	// the project's new welcome scene with instructions/tutorials.
	public void NextScene(){
		SceneManager.LoadScene("welcomeScene");
	}


}
