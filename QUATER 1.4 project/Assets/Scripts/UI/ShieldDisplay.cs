using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ShieldDisplay : MonoBehaviour {

    [SerializeField] private GameObject _player;
    [SerializeField] private Image _image;
    [SerializeField] private Image _blinkImage;
    [Range(0, 1)] [SerializeField] private float _threshold;
    [Range(0, 5)] [SerializeField] private float _timeBetweenBlinks;
    [SerializeField] private int _amountOfBlinks;

    private CombatControls _combatControls;
    private Color _originalColor;
    private bool _blinkOnce;

	// Use this for initialization
	void Start () {
        _combatControls = _player.GetComponent<CombatControls>();
        _originalColor = _image.color;
        _blinkImage.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        _image.fillAmount = Convert.ToSingle(_combatControls.ShieldAmount) / _combatControls.MaxShieldAmount;

        if (_image.fillAmount == 1) {
            if (!_blinkOnce) {
                StartCoroutine(DoBlinks(_amountOfBlinks, _timeBetweenBlinks));
            }
        } else {
            _blinkOnce = false;
            _blinkImage.gameObject.SetActive(false);

            if (_image.fillAmount < _threshold) {
                _image.color = Color.red;
            } else {
                _image.color = _originalColor;
            }
        }
    }

    IEnumerator DoBlinks(int pNumBlinks, float pSeconds) {
        _blinkOnce = true;

        for (int i = 0; i < pNumBlinks * 2; i++) {
            if (_blinkOnce) {
                _blinkImage.gameObject.SetActive(!_blinkImage.gameObject.activeSelf);

                yield return new WaitForSeconds(pSeconds);
            }
            
        }
        
        _blinkImage.gameObject.SetActive(true);
    }
}
