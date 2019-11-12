﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDrag : MonoBehaviour {

	float offsetX;
	float offsetY;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BeginDrag() {
		offsetX = transform.position.x - Input.mousePosition.x;
		offsetY = transform.position.y - Input.mousePosition.y;
	}

	public void OnDrag() {
		transform.position = new Vector3 (offsetX + Input.mousePosition.x, offsetY + Input.mousePosition.y);
	}
}
//ocomment
