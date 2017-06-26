using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitmarkerDisplay : MonoBehaviour {

    [SerializeField] private float _waitInSeconds;
    private bool _enemyDestroyed;

    public float WaitInSeconds {
        get { return _waitInSeconds; }
    }
    public bool EnemyDestroyed {
        get { return _enemyDestroyed; }
        set { _enemyDestroyed = value; }
    }

    private void Update() {
        if (_enemyDestroyed == true) {
            StartCoroutine(Wait(_waitInSeconds));
        }
    }

    public IEnumerator Wait(float waitTime) {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(false);
        _enemyDestroyed = false;
    }
}
