using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyPointerDisplay : MonoBehaviour {
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _indicatorPrefab;

    // enemy left and indicator right
    private Dictionary<GameObject, GameObject> _enemyIndicatorPair = new Dictionary<GameObject, GameObject>();

    // Use this for initialization
    void Start () {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) {
            if (obj.GetComponent<EnemyMovement>().IsRanged) {
                GameObject indicator = Instantiate(_indicatorPrefab);
                indicator.transform.parent = transform;
                indicator.transform.localPosition = _camera.WorldToViewportPoint(indicator.transform.position);
                indicator.transform.localScale = new Vector3(0.8f, 0.8f, 0);
                _enemyIndicatorPair.Add(obj, indicator);
                indicator.gameObject.SetActive(false);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        foreach (KeyValuePair<GameObject, GameObject> pair in _enemyIndicatorPair) {
            if (pair.Key != null) {
                if (pair.Key.GetComponent<EnemyMovement>().CurrentState == EnemyMovement.State.shoot) {
                    pair.Value.gameObject.SetActive(true);
                    Vector3 shooterDir = pair.Key.transform.position - _player.transform.position;
                    shooterDir.y = 0;
                    shooterDir.Normalize();

                    Vector3 fwd = _player.transform.forward;
                    float a = Vector3.Dot(fwd, shooterDir);
                    float angle = (a + 1f) * 90f;

                    Vector3 rhs = _player.transform.right;
                    if (Vector3.Dot(rhs, shooterDir) > 0) {
                        angle *= -1f;
                    }

                    Quaternion indicatorRot = Quaternion.Euler(0, 180, angle);
                    if (angle > -90 && angle < 0) {
                        indicatorRot.x = -0.7f;
                        indicatorRot.y = 0.7f;
                    } else if (angle < 90 && angle > 0) {
                        indicatorRot.x = 0.7f;
                        indicatorRot.y = 0.7f;
                    }

                    pair.Value.transform.localRotation = indicatorRot;
                } else {
                    pair.Value.gameObject.SetActive(false);
                }
            }
        } 
    }
}
