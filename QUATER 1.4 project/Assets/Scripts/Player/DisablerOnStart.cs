using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablerOnStart : MonoBehaviour {
    [SerializeField] private Camera _camera;
    [SerializeField] private float _secondsToFreeze;

	// Use this for initialization
	void Start () {
        StartCoroutine(EnableControlsAfter(_secondsToFreeze));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator EnableControlsAfter(float pAmount)
    {
        GetComponent<PlayerMovement>().enabled = false;
        _camera.GetComponent<MouseLook>().enabled = false;
        yield return new WaitForSeconds(pAmount);
        GetComponent<PlayerMovement>().enabled = true;
        _camera.GetComponent<MouseLook>().enabled = true;
    }
}
