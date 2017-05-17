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

[RequireComponent(typeof(CombatControls))]
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

    private CombatControls _combatControls;

    // Use this for initialization
    void Start () {
        audio = gameObject.GetComponent<AudioSource>();

        _weaponType = WeaponType.Melee;
        _weaponAOEType = WeaponAOEType.Single;
        _combatControls = GetComponent<CombatControls>();
        
    }

    // Update is called once per frame
    void Update () {
        if (!_combatControls.Animation.isPlaying) {
            _combatControls.Animation.Play("IdleEditable");
        }
		if (Input.GetKeyUp("1")) {
            if (_hasGun) {
                _combatControls.Animation = _weapons[1].GetComponent<Animation>();
                if (!_weapons[1].activeInHierarchy) {
                    _weapons[0].SetActive(false);
                    _weapons[1].SetActive(true);
                }
                if (_weaponType == WeaponType.Melee) {
                    audio.PlayOneShot(GunSelectClip);
                    _combatControls.Animation.Play("SwitchEditable");
                }
                _weaponType = WeaponType.Ranged;
            }
        } else if (Input.GetKeyUp("2")) {
            _combatControls.Animation = _weapons[0].GetComponent<Animation>();
            if (!_weapons[0].activeInHierarchy) {
                _weapons[0].SetActive(true);
                _weapons[1].SetActive(false);
            }
            if (_weaponType == WeaponType.Ranged) {
                audio.PlayOneShot(KnifeSelectClip);
                _combatControls.Animation["SwitchEditable"].speed = 3;
                _combatControls.Animation.Play("SwitchEditable");
            }
            _weaponType = WeaponType.Melee;
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
                _combatControls.Animation = _weapons[1].GetComponent<Animation>();
                if (!_weapons[1].activeInHierarchy) {
                    _weapons[0].SetActive(false);
                    _weapons[1].SetActive(true);
                }
                if (_weaponType == WeaponType.Melee) {
                    audio.PlayOneShot(GunSelectClip);
                    _combatControls.Animation.Play("SwitchEditable");
                }
                _weaponType = WeaponType.Ranged;
            }
        } else { 
            _combatControls.Animation = _weapons[0].GetComponent<Animation>();
            if (!_weapons[0].activeInHierarchy) {
                _weapons[0].SetActive(true);
                _weapons[1].SetActive(false);
            }
            if (_weaponType == WeaponType.Ranged) {
                audio.PlayOneShot(KnifeSelectClip);
                _combatControls.Animation["SwitchEditable"].speed = 3;
                _combatControls.Animation.Play("SwitchEditable");
            }
            _weaponType = WeaponType.Melee;
        }
    }

    private void OnGUI() {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Current WeaponType: " + _weaponType + Environment.NewLine + "Current AOEType: " + _weaponAOEType);
    }
}
