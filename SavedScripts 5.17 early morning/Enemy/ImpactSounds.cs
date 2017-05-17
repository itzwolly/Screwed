using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSounds : MonoBehaviour {

    private AudioSource audio;
    // Use this for initialization
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }


    public void PlayImpactSound(AudioClip sound)
    {
        Debug.Log("MAKING SOUND");
        audio.PlayOneShot(sound);
    }

}
