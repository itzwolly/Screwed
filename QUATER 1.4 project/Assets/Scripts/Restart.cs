using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour {
    public int scene;
    public bool debug;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R) && debug)
            Application.LoadLevel(scene);
    }

    public void RestartLevel()
    {
        Application.LoadLevel(3);
    }
    public void GotoMenu()
    {
        Application.LoadLevel(1);
    }
}
