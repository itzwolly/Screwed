using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour {

    [SerializeField] private GameObject _player;
    [SerializeField] private Text _text;

    private CombatControls _combatControls;

	// Use this for initialization
	void Start () {
        if (_player != null) {
            _combatControls = _player.GetComponent<CombatControls>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        _text.text = _combatControls.Health.ToString();
	}
}
