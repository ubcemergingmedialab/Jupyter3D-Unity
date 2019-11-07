using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class switchScene : MonoBehaviour {
	public void NextScene()
	{
		Debug.Log(SceneManager.GetActiveScene().name);
		SceneManager.LoadScene("welcomeScene");
	}
}
