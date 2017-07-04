using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyPointerDisplay : MonoBehaviour {
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _indicatorPrefab;

    // enemy is key and indicator is value
    private Dictionary<GameObject, GameObject> _enemyIndicatorPair = new Dictionary<GameObject, GameObject>();

    private bool _doOnce = false;

    // Use this for initialization
    void Start () {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) {
            Debug.Log("Found: " + obj + " which is definitely an Enemy");
            if (obj.GetComponent<EnemyMovement>().IsRanged) {
                Debug.Log("Definitely ranged too.");
                GameObject indicator = Instantiate(_indicatorPrefab);
                indicator.transform.SetParent(transform);
                indicator.transform.localPosition = _camera.WorldToViewportPoint(indicator.transform.position);
                indicator.transform.localScale = new Vector3(0.8f, 0.8f, 0);
                _enemyIndicatorPair.Add(obj, indicator);
                indicator.gameObject.SetActive(false);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (_enemyIndicatorPair.Count > 0) {
            foreach (KeyValuePair<GameObject, GameObject> pair in _enemyIndicatorPair) {
                if (pair.Key != null) {
                    if (pair.Key.activeInHierarchy) {
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
                } else {
                    if (pair.Value.gameObject.activeInHierarchy) {
                        pair.Value.gameObject.SetActive(false);
                        // note i can't delete the key in the pair, but i can delete the value.
                        // so if you use this list somewhere else make sure you check all the pair.values 
                        // that are not null to get the objects that are actually in the game and shown.
                        // if pair.values is null that means it's not active in the scene!
                        _enemyIndicatorPair.Remove(pair.Value);
                    }
                }
            }
        }
    }
}
