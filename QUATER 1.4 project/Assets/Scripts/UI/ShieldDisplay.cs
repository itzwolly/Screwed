using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ShieldDisplay : MonoBehaviour
{

    [SerializeField] private GameObject _player;
    [SerializeField] private Image _image;
    [SerializeField] private Image _shieldImage;
    [Range(0, 1)] [SerializeField] private float _threshold;
    [Range(0, 5)] [SerializeField] private float _timeBetweenBlinks;
    [SerializeField] private int _amountOfBlinks;


    [SerializeField]
    private Color _color;

    private CombatControls _combatControls;
    private Color _originalColor;
    private bool _blinkOnce;

    public float FillAmount {
        get { return _image.fillAmount; }
    }

    // Use this for initialization
    void Start () {
        _combatControls = _player.GetComponent<CombatControls>();
        _originalColor = _image.color;
        //_blinkImage.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        _image.fillAmount = Convert.ToSingle(_combatControls.ShieldAmount) / _combatControls.MaxShieldAmount;

        if (_image.fillAmount == 1) {
            if (!_blinkOnce) {
                _shieldImage.gameObject.SetActive(false);
                StartCoroutine(DoBlinks(_amountOfBlinks, _timeBetweenBlinks));
            }
        } else {
            _blinkOnce = false;
            _shieldImage.gameObject.SetActive(true);
            //_blinkImage.gameObject.SetActive(false);
            _image.color = Color.white;

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
                _image.color = (_image.color == Color.white ? _color : Color.white);
                if (_image.color == _color) {
                    _shieldImage.gameObject.SetActive(false);
                } else {
                    _shieldImage.gameObject.SetActive(true);
                }
                yield return new WaitForSeconds(pSeconds);
            }
            
        }
        _image.color = _color;
        _shieldImage.gameObject.SetActive(false);
    }
}
