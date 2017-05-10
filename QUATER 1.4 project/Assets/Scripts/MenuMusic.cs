using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour {
    public AudioClip Music;
    private float _volume;
	// Use this for initialization
	void Start () {
        Utils.ChangeMusicVolume(100);
        _volume = Utils.MusicVolume();
        Debug.Log("music volume = "+_volume);
        gameObject.GetComponent<AudioSource>().clip = Music;
        gameObject.GetComponent<AudioSource>().loop = true;
        gameObject.GetComponent<AudioSource>().volume = _volume/100f;
        gameObject.GetComponent<AudioSource>().Play();
    }

    private void OnDestroy()
    {
        gameObject.GetComponent<AudioSource>().Stop();
    }
}
