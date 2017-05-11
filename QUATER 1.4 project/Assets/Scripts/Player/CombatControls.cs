using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatControls : MonoBehaviour {
    [SerializeField] private GameObject _statManager;

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
    
    [SerializeField] private int _comboResetTime;

    [SerializeField] private MonoBehaviour[] _disableAfterDeath;
    [SerializeField] private ResolutionBehaviour _afterDeathBehaviour;

    [SerializeField] private GameObject[] _cracks;

    [SerializeField] private int[] _weaponDamage;

    //[SerializeField] private string[] _levelNames;

    private Animation _anim;
    private int _currentShieldAmmount;
    private bool _blocking;
    private int _level;

    private float _timer;
    private bool _startTimer;
    private Color _originalColor;

    private int _comboCount;
    private int _comboWait;

    private float _timeInLevel=0;
    private int _successfullHeadshots = 0;
    private int _totalHeadshotKills=0;
    private int _successfullShots = 0;
    private int _totalShots=0;
    private int _totalRangedKills=0;
    private int _totalKnives = 0;
    private int _successfullKnives=0;
    private int _knifeKillNumber=0;
    private bool _completedLevelWithoutDmg=true;
    private int _secretsGathered=0;
    private int _blockedShots=0;
    private int _totalKills = 0;

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
    public int ShieldAmount {
        get { return _currentShieldAmmount; }
    }
    public int MaxShieldAmount {
        get { return _maxShieldAmount; }
    }


    // Use this for initialization
    void Start ()
    {
        _anim = _weaponHandler.Weapons[0].GetComponent<Animation>();
        _level = Utils.LatestLevel();
        /**
        string allInfo = "";
        allInfo = " Timeinlevel: " + Utils.GetTimeInLevel("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt") + " Completedlevelwithoutdmg: " + Utils.GetCompletedWithoutDmg("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt") + "\n" +
            " Totalshots: " + Utils.GetTotalShots("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt") + " Successfullshots: " + Utils.GetSuccessfullShots("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt") + " Successfullheadshots: " + Utils.GetSuccessfullHeadshots("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt") + " Headshotkills: " + Utils.GetHeadshotKills("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt") + " Totalrangedkills: " + Utils.GetTotalRangedKills("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt") + "\n" +
            " Totalknives: " + Utils.GetTotalKnives("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt") + " Successfullknives: " + Utils.GetSuccessfullKnives("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt") + " Knifekills: " + Utils.GetKnifeKills("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt") + "\n" +
            " Blockedshots: " + Utils.GetBlockedShots("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt") + " Totalkills: " + Utils.GetTotalKills("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt") + "\n" +
            " Secretsgathered: " + Utils.GetSecretsGathered("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt");
        Debug.Log(allInfo);
        /**/
    }

    // Update is called once per frame
    void Update ()
    {
        /**
        Debug.Log(SceneManager.GetActiveScene().name +" Timeinlevel: "+(int)_timeInLevel + " Completedlevelwithoutdmg: "+_completedLevelWithoutDmg);
        Debug.Log(" Totalshots: "+_totalShots+" Successfullshots: "+_successfullShots+" Successfullheadshots: "+_successfullHeadshots+" Headshotkills: "+_totalHeadshotKills);
        Debug.Log(" Totalknives: "+_totalKnives+" Successfullknives: "+_successfullKnives+" Knifekills: "+_knifeKillNumber);
        Debug.Log(" Blockedshots: "+_blockedShots+" Totalkills: "+_totalKills);
        Debug.Log(" Secretsgathered: "+_secretsGathered);
        /**/
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //here it goes to in game menu
            int _bool = 0;
            if (_completedLevelWithoutDmg)
                _bool = 1;
            string allInfo = "";
            allInfo = " Timeinlevel: " + (int)_timeInLevel + " Completedlevelwithoutdmg: " + _bool + "\n" +
                " Totalshots: " + _totalShots + " Successfullshots: " + _successfullShots + " Successfullheadshots: " + _successfullHeadshots + " Headshotkills: " + _totalHeadshotKills + " Totalrangedkills: " + _totalRangedKills + "\n" +
                " Totalknives: " + _totalKnives + " Successfullknives: " + _successfullKnives + " Knifekills: " + _knifeKillNumber + "\n" +
                " Blockedshots: " + _blockedShots + " Totalkills: " + _totalKills + "\n" +
                " Secretsgathered: " + _secretsGathered;
            //Debug.Log(allInfo);
            Utils.SaveStats("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt", allInfo);
            _statManager.GetComponent<StatUpdater>().UpdateStats();
        }
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            Utils.ResetLastLevel();
            //Debug.Log(Utils.GetLastNumberFromFile("Assets\\SaveInfo.txt"));
        }

        if (Input.GetButtonDown("Fire1")) {
            RaycastHit hit = new RaycastHit();
            Vector3 ray = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (_weaponHandler.CurrentWeaponType == WeaponType.Ranged) {
                if (_ammoCount > 0) {
                    RangedDamage(ray, _camera.transform.forward, hit, "Enemy");
                }
            } else if (_weaponHandler.CurrentWeaponType == WeaponType.Melee) {
                if (!_anim.isPlaying) {
                    _anim.Play("AttackEditable");
                    MeleeDamage(transform.position, _distance, "Enemy", _weaponHandler.CurrentWeaponAOEType);
                }
            }
        }
        //Debug.Log(_blocking + " with health = " + _health);

        if(Input.GetMouseButtonDown(1) && _currentShieldAmmount > _minShieldAmount)
        {
            _blocking = true;
        }

        if(Input.GetMouseButtonUp(1))
        {
            _blocking = false;
        }
        
        if (_blocking)
        {
            ///health stays the same here
            //Debug.Log("blocking");
            
            _currentShieldAmmount -= _decreaseAmount;
        }
        if(_currentShieldAmmount<=0 || _blocking==false)
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
            //Debug.Log(Utils.LatestLevel());
            if (Utils.LatestLevel() == 2) {
                SceneManager.LoadScene("level02");
            } else {
                _afterDeathBehaviour.DisableAfterWin();
            }
        }
    }

    private void FixedUpdate() {
        //Debug.Log(_comboCount); 
        _timeInLevel+=Time.fixedDeltaTime;
        if (_comboCount>0)
        {
            if(_comboWait>=_comboResetTime)
            {
                _comboCount = 0;
                _comboWait = 0;
            }
            _comboWait++;
        }

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
            //Debug.Log(pHit.collider.transform.name + " was hit" + " with tag " + pHit.transform.tag);
            _totalShots++;
            if (pHit.collider.transform.name=="Enemy Head")
            {
                //Debug.Log("You headshot you filthy animal");
                _successfullShots++;
                _successfullHeadshots++;
                _comboCount++;
                _comboWait = 0;
                TakeDamage(pHit.transform, true);
            }
            else if (pHit.transform.tag == pTarget) {
                _successfullShots++;
                _comboCount++;
                _comboWait = 0;
                //Debug.Log(pHit.transform.name + " has been hit using a ranged weapon");
                TakeDamage(pHit.transform,false);
            }
        }
    }

    private void RangedDamage(Vector3 pFrom, Vector3 pTo, RaycastHit pHit, string pTarget) {
        DecreaseBulletCount();
        if (Physics.Raycast(pFrom, pTo , out pHit)) {
            _totalShots++;
            //Debug.Log(pHit.collider.transform.name + " was hit" + " with tag " + pHit.transform.tag);
            if (pHit.collider.transform.name == "Enemy Head")
            {
                //Debug.Log("You headshot you filthy animal");
                _successfullShots++;
                _successfullHeadshots++;
                _comboCount++;
                _comboWait = 0;
                TakeDamage(pHit.transform, true);
            }
            else if (pHit.transform.tag == pTarget)
            {
                _successfullShots++;
                _comboCount++;
                _comboWait = 0;
                //Debug.Log(pHit.transform.name + " has been hit using a ranged weapon");
                TakeDamage(pHit.transform,false);
            }
        }
    }

    private void DecreaseBulletCount() {
        _ammoCount--;
    }

    private void TakeDamage(Transform pTarget, bool headshot) {
        Utils.ChangeGameObjectColorTo(pTarget.gameObject, pTarget.GetComponent<Renderer>().material.color, Color.red);
        if (_weaponHandler.CurrentWeaponType == WeaponType.Melee) {
            pTarget.GetComponent<EnemyScript>().DecreaseHealth(_weaponDamage[0]);
        } else if (_weaponHandler.CurrentWeaponType == WeaponType.Ranged) {
            if(headshot)
                pTarget.GetComponent<EnemyScript>().DecreaseHealth(2*_weaponDamage[1]);
            else
                pTarget.GetComponent<EnemyScript>().DecreaseHealth(_weaponDamage[1]);
        }
        if (pTarget.GetComponent<EnemyScript>().IsDead) {
            if (_weaponHandler.CurrentWeaponType == WeaponType.Melee)
            {
                _knifeKillNumber++;
            }
            else
            {
                _totalRangedKills++;
            }
            if (headshot)
            {
                _totalHeadshotKills++;
                //Debug.Log("Got Killed By headshot");
            }
            _totalKills++;
            Destroy(pTarget.gameObject);
        }
        _startTimer = true;
    }

    private void MeleeDamage(Vector3 pCenter, float pRadius, string pTarget, WeaponAOEType pAoeType) {
        int i = 0;
        Collider[] hitColliders = Physics.OverlapSphere(pCenter, pRadius);
        _totalKnives++;
        while (i < hitColliders.Length) {
            
            if (hitColliders[i].transform.tag == pTarget) {
                _successfullKnives++;
                //Debug.Log(hitColliders[i].name + " is in range of attacks.");
                if (pAoeType == WeaponAOEType.Single) {
                    //Debug.Log((GetClosestEnemy(hitColliders, pRadius) == null) + ".");
                    if (GetClosestEnemy(hitColliders, pRadius) != null) {
                        //Debug.Log(GetClosestEnemy(hitColliders, pRadius) + " has been hit.");
                        _comboCount++;
                        _comboWait = 0;
                        TakeDamage(GetClosestEnemy(hitColliders, pRadius),false);
                        break;
                    }
                } else if (pAoeType == WeaponAOEType.Multi) {
                    _comboCount++;
                    _comboWait = 0;
                    //Debug.Log(hitColliders[i].name + " has been hit.");
                    TakeDamage(hitColliders[i].transform,false);
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

                //Debug.Log("dist: " + dist + " | " + "pMinDist: " + pMinDist + " | " + "angle: " + angle + " | " + "_angle: " + _angle);

                if (dist < pMinDist && angle < _angle) {
                    enemy = collider.transform;
                    pMinDist = dist;
                }
            }
        }
        return enemy;
    }

    public void DecreaseHealth(int pAmount) {
        if(_blocking)
        {
            _blockedShots++;
        }
        else if (_health > 0) {
            _completedLevelWithoutDmg = false;
            _comboCount = 0;
            _comboWait = 0;

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
                int _bool=0;
                if (_completedLevelWithoutDmg)
                    _bool = 1;
                string allInfo="";
                allInfo = " Timeinlevel: " + (int)_timeInLevel + " Completedlevelwithoutdmg: " + _bool + "\n" + 
                    " Totalshots: " + _totalShots + " Successfullshots: " + _successfullShots + " Successfullheadshots: " + _successfullHeadshots + " Headshotkills: " + _totalHeadshotKills + " Totalrangedkills: " + _totalRangedKills + "\n" +
                    " Totalknives: " + _totalKnives + " Successfullknives: " + _successfullKnives + " Knifekills: " + _knifeKillNumber + "\n" +
                    " Blockedshots: " + _blockedShots + " Totalkills: " + _totalKills+ "\n" +
                    " Secretsgathered: " + _secretsGathered;
                //Debug.Log(allInfo);
                Utils.NextLevel("Assets\\Statistics\\"+SceneManager.GetActiveScene().name+".txt", allInfo);
                return true;
            } else {
                return false;
            }
        }
    }

}
