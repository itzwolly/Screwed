using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldDisplay : MonoBehaviour {

    [SerializeField] private GameObject _player;
    [SerializeField] private Image _image;
    [SerializeField] private Color _lowShieldColor;
    [Range(0, 1)] [SerializeField] private float _threshold;

    private CombatControls _combatControls;
    private Color _originalColor;

	// Use this for initialization
	void Start () {
        _combatControls = _player.GetComponent<CombatControls>();
        _originalColor = _image.color;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        _image.fillAmount = _combatControls.ShieldAmount / (float)_combatControls.MaxShieldAmount;
        Debug.Log(_image.fillAmount);
        if (_image.fillAmount < _threshold) {
            _image.color = _lowShieldColor;
        } else {
            _image.color = _originalColor;
        }
	}
}
