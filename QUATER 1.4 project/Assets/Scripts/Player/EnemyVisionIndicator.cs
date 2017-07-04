using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EnemyVisionIndicator : MonoBehaviour {

    [SerializeField] private GameObject[] _enemyVisionMarkers;
    [SerializeField] private EnemyHandler _enemyHandler;
    [SerializeField] private GameObject _detectionSoundHandler;
    [SerializeField] private AudioClip _detectionClip;

    private Animator _animator;
    private PlayerMovement _playerMovement;
    private AudioSource _audioSource;

    // Use this for initialization
    void Start () {
        _animator = _enemyVisionMarkers[0].GetComponentInChildren<Animator>();
        _audioSource = _detectionSoundHandler.GetComponent<AudioSource>();
        _playerMovement = GetComponent<PlayerMovement>();
        Debug.Log("AudioSource == " + _audioSource == null);
    }
	
	// Update is called once per frame
	void Update () {
        if (_enemyHandler.EnemiesLeft() > 0) {
            Debug.Log("enemies left > 0");

            if (GameObject.FindGameObjectsWithTag("Enemy").All(o => !o.GetComponent<EnemyMovement>()._inVision)) {
                _enemyVisionMarkers[0].SetActive(false);
                _enemyVisionMarkers[1].SetActive(true);
            } else {
                if (!_enemyVisionMarkers[0].activeSelf) {
                    if (!_audioSource.isPlaying) {
                        _audioSource.PlayOneShot(_detectionClip, _playerMovement.Volume);
                    }
                    _enemyVisionMarkers[0].SetActive(true);
                    _enemyVisionMarkers[1].SetActive(false);
                }
            }
        }
	}
}
