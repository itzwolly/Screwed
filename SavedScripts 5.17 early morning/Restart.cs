﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour {
    public int scene;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(scene);
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
