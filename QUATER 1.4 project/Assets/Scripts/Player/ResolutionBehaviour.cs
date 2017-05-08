using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionBehaviour : MonoBehaviour {

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _resolutionMessage;

    [SerializeField] private string _deathMessage;
    [SerializeField] private string _winMessage;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void DisableAfterWin() {
        if (_hud != null) {
            if (_hud.activeSelf) {
                DisablePlayerScripts();
                DisablePlayerCamera();
                _hud.SetActive(false);
                SetResolutionText(_winMessage);
            }
        }
    }

    public void DisableAfterDeath() {
        if (_hud != null) {
            if (_hud.activeSelf) {
                DisablePlayerScripts();
                DisablePlayerCamera();
                _hud.SetActive(false);
                SetResolutionText(_deathMessage);
            }
        }
    }

    private void SetResolutionText(string pText) {
        _resolutionMessage.GetComponentInChildren<UnityEngine.UI.Text>().text = pText;
        _resolutionMessage.SetActive(true);
    }

    private void DisablePlayerScripts() {
        foreach (MonoBehaviour obj in GetComponents<MonoBehaviour>()) {
            obj.enabled = false;
        }
    }

    private void DisablePlayerCamera() {
        _mainCamera.GetComponent<MouseLook>().enabled = false;
    }
}
