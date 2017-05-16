using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WeaponType {
    None,
    Melee,
    Ranged
}

public enum WeaponAOEType {
    None,
    Single,
    Multi
}

public class WeaponHandler : MonoBehaviour {

    public AudioClip KnifeSelectClip;
    public AudioClip GunSelectClip;
    private AudioSource audio;

    private WeaponType _weaponType;
    private WeaponAOEType _weaponAOEType;
    [SerializeField] private GameObject[] _weapons;
    [SerializeField] private bool _hasGun;

    public WeaponType CurrentWeaponType {
        get { return _weaponType; }
    }
    public WeaponAOEType CurrentWeaponAOEType {
        get { return _weaponAOEType; }
    }
    public GameObject[] Weapons {
        get { return _weapons; }
    }
    public bool HasGun {
        get { return _hasGun; }
        set { _hasGun = value; }
    }


    // Use this for initialization
    void Start () {
        audio = gameObject.GetComponent<AudioSource>();

        _weaponType = WeaponType.Melee;
        _weaponAOEType = WeaponAOEType.Single;
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyUp("1")) {
            if (_hasGun) {
                _weaponType = WeaponType.Ranged;
                audio.PlayOneShot(GunSelectClip);
                if (!_weapons[1].activeInHierarchy) {
                    _weapons[0].SetActive(false);
                    _weapons[1].SetActive(true);
                }
            }
        } else if (Input.GetKeyUp("2")) {
            _weaponType = WeaponType.Melee;
            audio.PlayOneShot(KnifeSelectClip);
            if (!_weapons[0].activeInHierarchy) {
                _weapons[0].SetActive(true);
                _weapons[1].SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            SwitchToNextWeapon();
        }

        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue == 0.1f || scrollValue == -0.1f) {
            SwitchToNextWeapon();
        }

        if (Input.GetKeyUp(KeyCode.F)) {
            if (_weaponType == WeaponType.Melee) {
                if (_weaponAOEType == WeaponAOEType.Single) {
                    _weaponAOEType = WeaponAOEType.Multi;
                } else if (_weaponAOEType == WeaponAOEType.Multi) {
                    _weaponAOEType = WeaponAOEType.Single;
                }
            }
        }
	}

    private void SwitchToNextWeapon() {
        if (_weaponType == WeaponType.Melee) {
			if (_hasGun) {
                audio.PlayOneShot(GunSelectClip);
                _weaponType = WeaponType.Ranged;
				if (!_weapons[1].activeInHierarchy) {
					_weapons[0].SetActive(false);
					_weapons[1].SetActive(true);
				}
			}
        } else {
            _weaponType = WeaponType.Melee;
            audio.PlayOneShot(KnifeSelectClip);
            if (!_weapons[0].activeInHierarchy) {
                _weapons[0].SetActive(true);
                _weapons[1].SetActive(false);
            }
        }
    }

    private void OnGUI() {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Current WeaponType: " + _weaponType + Environment.NewLine + "Current AOEType: " + _weaponAOEType);
    }
}
