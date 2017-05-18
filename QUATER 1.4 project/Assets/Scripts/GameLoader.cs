using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(WaitForSplash());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator WaitForSplash()
        {
        yield return new WaitForSeconds(5);
        Application.LoadLevel(1);
        }
}
