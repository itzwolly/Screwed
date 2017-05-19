using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehaviour : MonoBehaviour {
    [SerializeField] private CursorLockMode _cursorLockMode;

	// Use this for initialization
	void Start () {
        Cursor.lockState = _cursorLockMode;
    }
	
	// Update is called once per frame
	void Update () {
		//if (Cursor.lockState == CursorLockMode.None) {
  //          if (Input.GetMouseButtonUp(0)) {
  //              Cursor.lockState = _cursorLockMode;
  //          }
  //      }
	}
}
