﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class switchScene : MonoBehaviour {
	public void NextScene(){
		SceneManager.LoadScene("welcomeScene");
	}
}
