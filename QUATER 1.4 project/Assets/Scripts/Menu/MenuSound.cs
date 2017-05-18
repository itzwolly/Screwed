using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSound : MonoBehaviour {
    private AudioSource audio;
    public AudioClip HoverClip;
    public AudioClip ClickClip;
    // Use this for initialization
    void Start () {
        audio = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HoverSound()
    {
        audio.PlayOneShot(HoverClip);
    }
    public void ClickSound()
    {
        audio.PlayOneShot(ClickClip);
    }
}
