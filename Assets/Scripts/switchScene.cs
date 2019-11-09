using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class switchScene : MonoBehaviour {
	public void NextScene()
	{
		if(SceneManager.GetActiveScene().name.Equals("gridDemo2"))
			SceneManager.LoadScene("welcomeScene");
		else
			SceneManager.LoadScene("gridDemo2");
	}
}
