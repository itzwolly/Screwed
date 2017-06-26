using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponHandler))]
public class Pickup : MonoBehaviour {
    [SerializeField] private GameObject _gunPickup;

    public AudioClip GunPickupClip;
    private AudioSource audio;

    // Use this for initialization
    void Start () {
        audio = gameObject.AddComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Pickup") {
            audio.PlayOneShot(GunPickupClip);
            GetComponent<WeaponHandler>().HasGun = true;
            GetComponent<CombatControls>().IncreaseAmmo();
            Destroy(other.transform.parent.gameObject);
        }
    }
}
