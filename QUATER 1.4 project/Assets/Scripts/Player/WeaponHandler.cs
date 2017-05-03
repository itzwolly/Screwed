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
    private WeaponType _weaponType;
    private WeaponAOEType _weaponAOEType;
    [SerializeField] private GameObject[] _weapons;

    public WeaponType CurrentWeaponType {
        get { return _weaponType; }
    }
    public WeaponAOEType CurrentWeaponAOEType {
        get { return _weaponAOEType; }
    }

    // Use this for initialization
    void Start () {
        _weaponType = WeaponType.Melee;
        _weaponAOEType = WeaponAOEType.Single;
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyUp("1")) {
            _weaponType = WeaponType.Ranged;
            if (!_weapons[1].activeInHierarchy) {
                _weapons[0].SetActive(false);
                _weapons[1].SetActive(true);
            }
        } else if (Input.GetKeyUp("2")) {
            _weaponType = WeaponType.Melee;
            if (!_weapons[0].activeInHierarchy) {
                _weapons[0].SetActive(true);
                _weapons[1].SetActive(false);
            }
        }
        
        if (Input.GetKeyUp(KeyCode.F)) {
            if (_weaponType == WeaponType.Melee) {
                if (_weaponAOEType == WeaponAOEType.Single) {
                    _weaponAOEType = WeaponAOEType.Multi;
                } else if (_weaponAOEType == WeaponAOEType.Multi) {
                    _weaponAOEType = WeaponAOEType.Single;
                }
                Debug.Log(_weaponAOEType);
            }
        }
	}

    private void OnGUI() {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Current WeaponType: " + _weaponType + Environment.NewLine + "Current AOEType: " + _weaponAOEType);
    }
}
