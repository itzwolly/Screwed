using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponHandler))]
public class Pickup : MonoBehaviour {
    [SerializeField] private GameObject _gunPickup;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Pickup") {
            GetComponent<WeaponHandler>().HasGun = true;
            Destroy(other.transform.parent.gameObject);
        }
    }
}
