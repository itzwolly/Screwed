using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletDisplay : MonoBehaviour {
    [SerializeField] private Text _text;
    [SerializeField] private GameObject _player;

    private CombatControls _combatControls;
    private Color _color;

	// Use this for initialization
	void Start () {
        if (_player != null) {
            _combatControls = _player.GetComponent<CombatControls>();
            _color = _text.color;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (_combatControls.WeaponHandler.CurrentWeaponType == WeaponType.Ranged) {
            if (_combatControls.AmmoCount == 0) {
                _text.color = new Color(0.9f, 0, 0, 0.9f);
            } else {
                _text.color = _color;
            }
            _text.text = _combatControls.AmmoCount.ToString();
        } else {
            _text.text = "";
        }
    }
}
