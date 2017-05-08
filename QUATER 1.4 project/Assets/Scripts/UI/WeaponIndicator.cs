using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponIndicator : MonoBehaviour {

    [SerializeField] private GameObject _player;
    [Tooltip("First image is the knife and second is the gun.")]
    [SerializeField] private GameObject[] _weapons;

    private CombatControls _combatControls;

	// Use this for initialization
	void Start () {
		if (_player != null) {
            _combatControls = _player.GetComponent<CombatControls>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (_combatControls.WeaponHandler.CurrentWeaponType == WeaponType.Melee) {
            _weapons[0].SetActive(false);
            _weapons[1].SetActive(true);
        } else if (_combatControls.WeaponHandler.CurrentWeaponType == WeaponType.Ranged) {
            _weapons[0].SetActive(true);
            _weapons[1].SetActive(false);
        }
	}
}
