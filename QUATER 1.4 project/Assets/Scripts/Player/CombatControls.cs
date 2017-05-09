using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControls : MonoBehaviour {
    [SerializeField] private WeaponHandler _weaponHandler;
    [SerializeField] private float _distance;
    [SerializeField] private float _angle;

    [SerializeField] private Camera _camera;
    [SerializeField] private int _ammoCount;
    [SerializeField] private int _health;

    [SerializeField] private int _maxShieldAmount;
    [SerializeField] private int _minShieldAmount;
    [SerializeField] private int _increaseAmount;
    [SerializeField] private int _decreaseAmount;

    [SerializeField] private MonoBehaviour[] _disableAfterDeath;
    [SerializeField] private ResolutionBehaviour _afterDeathBehaviour;

    [SerializeField] private GameObject[] _cracks;
    
    private int _currentShieldAmmount;
    private bool _blocking;

    private float _timer;
    private bool _startTimer;
    private Color _originalColor;

    public int AmmoCount {
        get { return _ammoCount; }
    }
    public WeaponHandler WeaponHandler {
        get { return _weaponHandler; }
    }
    public int Health {
        get { return _health; }
    }

    public bool IsDead {
        get { return _health == 0; }
    }

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetButtonDown("Fire1")) {
            RaycastHit hit = new RaycastHit();
            Vector3 ray = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (_weaponHandler.CurrentWeaponType == WeaponType.Ranged) {
                if (_ammoCount > 0) {
                    RangedDamage(ray, _camera.transform.forward, hit, "Enemy");
                }
            } else if (_weaponHandler.CurrentWeaponType == WeaponType.Melee) {
                MeleeDamage(transform.position, _distance, "Enemy", _weaponHandler.CurrentWeaponAOEType);
            }
        }
        Debug.Log(_blocking + " with health = " + _health);
        if (Input.GetMouseButton(1) && _currentShieldAmmount > _minShieldAmount)
        {
            ///health stays the same here
            //Debug.Log("blocking");
            _blocking = true;
            _currentShieldAmmount -= _decreaseAmount;
        }
        else
        {
            _blocking = false;
            if (_currentShieldAmmount < _maxShieldAmount)
            {
                _currentShieldAmmount += _increaseAmount;
            }
        }

        if (IsDead) {
            //gameObject.SetActive(false);
            _afterDeathBehaviour.DisableAfterDeath();
        } else if (HasWon()) {
            _afterDeathBehaviour.DisableAfterWin();
        }
    }

    private void FixedUpdate() {
        if (_startTimer == true) {
            _timer += Time.deltaTime;
            if (_timer > 0.5) {
                _startTimer = false;
                _timer = 0;
                Utils.ChangeGameObjectsColor(GameObject.FindGameObjectsWithTag("Enemy"), Color.red, new Color(1, 1, 1, 1));
            }
        }
    }

    private void RangedDamage(Ray pOther, RaycastHit pHit, string pTarget) {
        if (Physics.Raycast(pOther, out pHit)) {
            Debug.Log(pOther + " || " + pHit.transform.name + " || " + pHit.transform.tag);
            if (pHit.transform.tag == pTarget) {
                Debug.Log(pHit.transform.name + " has been hit using a ranged weapon");
                TakeDamage(pHit.transform);
            }
        }
    }

    private void RangedDamage(Vector3 pFrom, Vector3 pTo, RaycastHit pHit, string pTarget) {
        DecreaseBulletCount();
        if (Physics.Raycast(pFrom, pTo , out pHit)) {
            Debug.Log(pHit.transform.name + " was hit" + " with tag " + pHit.transform.tag);
            if (pHit.transform.tag == pTarget) {
                Debug.Log(pHit.transform.name + " has been hit using a ranged weapon");
                TakeDamage(pHit.transform);
            }
        }
    }

    private void DecreaseBulletCount() {
        _ammoCount--;
    }

    private void TakeDamage(Transform pTarget) {
        Utils.ChangeGameObjectColorTo(pTarget.gameObject, pTarget.GetComponent<Renderer>().material.color, Color.red);
        Destroy(pTarget.gameObject);
        _startTimer = true;
    }

    private void MeleeDamage(Vector3 pCenter, float pRadius, string pTarget, WeaponAOEType pAoeType) {
        int i = 0;
        Collider[] hitColliders = Physics.OverlapSphere(pCenter, pRadius);

        while (i < hitColliders.Length) {
            if (hitColliders[i].transform.tag == pTarget) {
                if (pAoeType == WeaponAOEType.Single) {
                    if (GetClosestEnemy(hitColliders, pRadius) != null) {
                        Debug.Log(GetClosestEnemy(hitColliders, pRadius) + " has been hit.");
                        TakeDamage(GetClosestEnemy(hitColliders, pRadius));
                        break;
                    }
                } else if (pAoeType == WeaponAOEType.Multi) {
                    Debug.Log(hitColliders[i].name + " has been hit.");
                    TakeDamage(hitColliders[i].transform);
                }
            }
            i++;
        }
    }

    private Transform GetClosestEnemy(Collider[] pColliders, float pMinDist) {
        Transform enemy = null;

        foreach (Collider collider in pColliders) {
            if (collider.transform.tag == "Enemy") {
                float dist = Vector3.Distance(collider.transform.position, transform.position);

                Vector3 targetDir = collider.transform.position - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);

                if (dist < pMinDist && angle < _angle) {
                    enemy = collider.transform;
                    pMinDist = dist;
                }
            }
        }
        return enemy;
    }

    public void DecreaseHealth(int pAmount) {
        if (_health > 0 && !_blocking) {
            _health -= pAmount;
            if (_health == 3) {
                _cracks[0].SetActive(true);
            }
            if (_health == 2) {
                _cracks[1].SetActive(true);
            }
            if (_health == 1) {
                _cracks[2].SetActive(true);
            }
            if (_health < 0) {
                _health = 0;
            }
        }
    }

    private bool HasWon() {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0) {
            return false;
        } else {
            if (Input.GetKeyUp(KeyCode.E)) {
                return true;
            } else {
                return false;
            }
        }
    }
}
