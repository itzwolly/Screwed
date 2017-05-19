using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySongOnTrigger : MonoBehaviour {
    public AudioSource audio;
    public AudioClip SongClip;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)

    {
        if (col.gameObject.tag == "Player")
        {
            audio.Play(1);
            audio.loop = true;
        }
    }

}
