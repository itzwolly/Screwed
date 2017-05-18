using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombatControls : MonoBehaviour {

    [SerializeField] private AudioClip _usingClip;
    [SerializeField] private AudioClip _stoppedUsingClip;
    public AudioClip ShootClip;
    public AudioClip KnifeClip;
    public AudioClip DeathClip;
    public AudioClip WinClip;
    public AudioClip HitClip;
    public AudioClip DryFireClip;
    public AudioClip EnemyGotHitBodyClip;
    public AudioClip EnemyGotHitHeadClip;
    public AudioClip BackstabClip;
    private AudioSource audio;
    private float _volume;
    [SerializeField] private GameObject _statManager;

    [SerializeField] private WeaponHandler _weaponHandler;
    [SerializeField] private float _distance;
    [SerializeField] private float _angle;

    [SerializeField] private Camera _camera;
    [SerializeField] private int _ammoIncrease;
    [SerializeField] private int _health;

    [SerializeField] private int _maxShieldAmount;
    [SerializeField] private int _minShieldAmount;
    [SerializeField] private int _increaseAmount;
    [SerializeField] private int _decreaseAmount;
    private int _lastShieldAmmount;

    [SerializeField] private int _comboResetTime;

    [SerializeField] private MonoBehaviour[] _disableAfterDeath;
    [SerializeField] private ResolutionBehaviour _afterDeathBehaviour;

    [SerializeField] private GameObject[] _cracks;
    [SerializeField] private float[] _fadeSecondsForCracks;
    [Range(0, 1)] [SerializeField] private float[] _fadeAlphaForCracks;

    [SerializeField] private int[] _weaponDamage;

    [SerializeField] private EnemyHandler _enemyHandler;

    [Space(10)]
    [Header("PostProcessing")]
    [SerializeField] private PostProcessingProfile _profile;
    [Space(5)]
    [Header("Vignette")]
    [SerializeField] private VignetteModel.Mode _vignetteMode;
    [SerializeField] private Color _vignetteColor;
    [SerializeField] private Vector2 _vignetteCenter;
    [Range(0, 1)] [SerializeField] private float _vignetteIntensity;
    [Range(0, 1)] [SerializeField] private float _vignetteSmoothness;
    [Range(0, 1)] [SerializeField] private float _vignetteRoundness;
    [SerializeField] private bool _vignetteIsRounded;

    private VignetteModel.Mode _profileMode;
    private Color _profileColor;
    private Vector2 _profileCenter;
    private float _profileIntensity;
    private float _profileSmoothness;
    private float _profileRoundness;
    private bool _profileIsRounded;

    private VignetteModel.Settings _profileSettings;

    //[SerializeField] private string[] _levelNames;

    private int _ammoCount;
    private Animation _anim;
    private int _currentShieldAmmount;
    private bool _blocking;
    private int _level;

    private float _timer;
    private bool _startTimer;
    private Color _originalColor;

    private int _comboCount;
    private int _comboWait;

    private float _timeInLevel = 0;
    private int _successfullHeadshots = 0;
    private int _totalHeadshotKills = 0;
    private int _successfullShots = 0;
    private int _totalShots = 0;
    private int _totalRangedKills = 0;
    private int _totalKnives = 0;
    private int _successfullKnives = 0;
    private int _knifeKillNumber = 0;
    private bool _completedLevelWithoutDmg = true;
    private int _secretsGathered = 0;
    private int _blockedShots = 0;
    private int _totalKills = 0;

    public int AmmoCount
    {
        get { return _ammoCount; }
    }
    public WeaponHandler WeaponHandler
    {
        get { return _weaponHandler; }
    }
    public int Health
    {
        get { return _health; }
    }
    public bool IsDead
    {
        get { return _health == 0; }
    }
    public int ShieldAmount
    {
        get { return _currentShieldAmmount; }
    }
    public int MaxShieldAmount
    {
        get { return _maxShieldAmount; }
    }
    public Animation Animation
    {
        get { return _anim; }
        set { _anim = value; }
    }


    public void IncreaseAmmo() {
        _ammoCount += _ammoIncrease;
    }
    // Use this for initialization
    void Start() {
        // audio = gameObject.AddComponent<AudioSource>();
        audio = GetComponent<AudioSource>();
        _anim = _weaponHandler.Weapons[0].GetComponent<Animation>();
        _level = Utils.LatestLevel();
        _volume = Utils.EffectVolume() / 100;

        _profileSettings = _profile.vignette.settings;
        _profileMode = _profileSettings.mode;
        _profileColor = _profileSettings.color;
        _profileCenter = _profileSettings.center;
        _profileIntensity = _profileSettings.intensity;
        _profileSmoothness = _profileSettings.smoothness;
        _profileRoundness = _profileSettings.roundness;
        _profileIsRounded = _profileSettings.rounded;

        Debug.Log(_volume);
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
    void Update() {
        /**
        Debug.Log(SceneManager.GetActiveScene().name +" Timeinlevel: "+(int)_timeInLevel + " Completedlevelwithoutdmg: "+_completedLevelWithoutDmg);
        Debug.Log(" Totalshots: "+_totalShots+" Successfullshots: "+_successfullShots+" Successfullheadshots: "+_successfullHeadshots+" Headshotkills: "+_totalHeadshotKills);
        Debug.Log(" Totalknives: "+_totalKnives+" Successfullknives: "+_successfullKnives+" Knifekills: "+_knifeKillNumber);
        Debug.Log(" Blockedshots: "+_blockedShots+" Totalkills: "+_totalKills);
        Debug.Log(" Secretsgathered: "+_secretsGathered);
        /**/


        if (Input.GetKeyDown(KeyCode.Escape)) {
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
        if (Input.GetKeyDown(KeyCode.Slash)) {
            Utils.ResetLastLevel();
            //Debug.Log(Utils.GetLastNumberFromFile("Assets\\SaveInfo.txt"));
        }

        if (Input.GetButtonDown("Fire1")) {
            RaycastHit hit = new RaycastHit();
            Vector3 ray = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (_weaponHandler.CurrentWeaponType == WeaponType.Ranged) {
                if (_ammoCount > 0) {
                    _anim.Stop("IdleEditable"); // stop idle animation
                    if (!_anim.isPlaying) { // fail safe
                        if (_anim["ShootEditable"].speed != 3.0f) {
                            _anim["ShootEditable"].speed = 3.0f;
                        }
                        audio.PlayOneShot(ShootClip, _volume);
                        _anim.Play("ShootEditable"); // play shoot animation
                        RangedDamage(ray, _camera.transform.forward, hit, "Enemy");
                    }
                } else {
                    audio.PlayOneShot(DryFireClip, _volume);
                }
            } else if (_weaponHandler.CurrentWeaponType == WeaponType.Melee) {
                _anim.Stop("IdleEditable");
                if (!_anim.isPlaying) {
                    if (_anim["AttackEditable"].speed != 2.0f) {
                        _anim["AttackEditable"].speed = 2.0f;
                    }
                    audio.PlayOneShot(KnifeClip);
                    _anim.Play("AttackEditable");
                    MeleeDamage(transform.position, _distance, "Enemy", _weaponHandler.CurrentWeaponAOEType);
                }
            }
        }
        //Debug.Log(_blocking + " with health = " + _health);

        if (Input.GetMouseButtonDown(1) && _currentShieldAmmount > _minShieldAmount) {
            Debug.Log("Started using");
            audio.PlayOneShot(_usingClip, _volume);
            _blocking = true;
        }

        if (Input.GetMouseButtonUp(1)) {
            if (_blocking) {
                Debug.Log("Stopped using");
                audio.PlayOneShot(_stoppedUsingClip, _volume);
            }
            _blocking = false;
        }

        if (_blocking) {
            ///health stays the same here
            //Debug.Log("blocking");
            
            ApplyVignette(_vignetteMode, _vignetteColor, _vignetteCenter, _vignetteIntensity, _vignetteSmoothness, _vignetteRoundness, _vignetteIsRounded);
            _currentShieldAmmount -= _decreaseAmount;
        } else {
            ApplyVignette(_profileMode, _profileColor, _profileCenter, _profileIntensity, _profileSmoothness, _profileRoundness, _profileIsRounded);
        }

        if (_currentShieldAmmount <= 0 || _blocking == false) {
            if (_blocking) {
                Debug.Log("Stopped using");
                audio.PlayOneShot(_stoppedUsingClip, _volume);
            }
            _blocking = false;

            if (_currentShieldAmmount < _maxShieldAmount) {
                _currentShieldAmmount += _increaseAmount;
            }
        }

        //if (_currentShieldAmmount <= _maxShieldAmount)
        //{
        //    //Debug.Log("Stopped using");
        //    //audio.PlayOneShot(_stoppedUsingClip);
        //    Debug.Log(((float)_currentShieldAmmount / MaxShieldAmount));
        //    if (_currentShieldAmmount < _lastShieldAmmount)
        //    {
        //        if (!audio.isPlaying)
        //        {
        //            Debug.Log("Using");
        //            audio.PlayOneShot(_usingClip,((float)_currentShieldAmmount/MaxShieldAmount));
        //        }
        //    }
        //    else if (_currentShieldAmmount > _lastShieldAmmount)
        //    {
        //        if (!audio.isPlaying)
        //        {
        //            Debug.Log("Charging");
        //            audio.PlayOneShot(_chargingClip, ((float)_currentShieldAmmount / MaxShieldAmount));
        //        }
        //    }
        //}

        _lastShieldAmmount = _currentShieldAmmount;
        if (IsDead) {
            //gameObject.SetActive(false);
            _afterDeathBehaviour.DisableAfterDeath();
            audio.PlayOneShot(DeathClip, _volume);
            Cursor.lockState = CursorLockMode.None;
        } else if (HasWon()) {
            //Debug.Log(Utils.LatestLevel());
            audio.PlayOneShot(WinClip, _volume);
            _afterDeathBehaviour.DisableAfterWin();
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void ApplyVignette(VignetteModel.Mode pMode, Color pColor, Vector2 pCenter, float pIntensity, float pSmoothness, float pRoundness, bool pIsRounded) {
        _profileSettings.mode = pMode;
        _profileSettings.color = pColor;
        _profileSettings.center = pCenter;
        _profileSettings.intensity = pIntensity;
        _profileSettings.smoothness = _profileSmoothness;
        _profileSettings.roundness = pRoundness;
        _profileSettings.rounded = pIsRounded;

        _profile.vignette.settings = _profileSettings;
    }

    private void FixedUpdate() {
        //Debug.Log(_comboCount); 
        _timeInLevel += Time.fixedDeltaTime;
        if (_comboCount > 0) {
            if (_comboWait >= _comboResetTime) {
                _comboCount = 0;
                _comboWait = 0;
            }
            _comboWait++;
        }

        //if (_startTimer == true) {
        //    _timer += Time.fixedDeltaTime;
        //    Debug.Log(_timer);
        //    if (_timer > 0.5f) {
        //        _startTimer = false;
        //        Debug.Log("Revert back to regular color");
        //        //Utils.ChangeGameObjectsColor(GameObject.FindGameObjectsWithTag("Enemy"), Color.red, new Color(1, 1, 1, 1));
        //        _timer = 0;
        //    }
        //}
    }

    private void RangedDamage(Ray pOther, RaycastHit pHit, string pTarget) {
        if (Physics.Raycast(pOther, out pHit)) {
            //Debug.Log(pHit.collider.transform.name + " was hit" + " with tag " + pHit.transform.tag);
            _totalShots++;
            if (pHit.collider.transform.name == "Enemy Head") {
                //Debug.Log("You headshot you filthy animal");
                pHit.collider.GetComponent<AudioSource>().PlayOneShot(EnemyGotHitHeadClip, _volume);
                _successfullShots++;
                _successfullHeadshots++;
                _comboCount++;
                _comboWait = 0;
                TakeDamage(pHit.transform, true);
            } else if (pHit.transform.tag == pTarget) {
                pHit.collider.GetComponentInChildren<ImpactSounds>().PlayImpactSound(EnemyGotHitBodyClip, _volume);
                _successfullShots++;
                _comboCount++;
                _comboWait = 0;
                //Debug.Log(pHit.transform.name + " has been hit using a ranged weapon");
                TakeDamage(pHit.transform, false);
            }
        }
    }

    private void RangedDamage(Vector3 pFrom, Vector3 pTo, RaycastHit pHit, string pTarget) {
        DecreaseBulletCount();
        if (Physics.Raycast(pFrom, pTo, out pHit)) {
            _totalShots++;
            //Debug.Log(pHit.collider.transform.name + " was hit" + " with tag " + pHit.transform.tag);
            if (pHit.collider.transform.name == "Enemy Head") {
                //Debug.Log("You headshot you filthy animal");
                pHit.collider.GetComponent<AudioSource>().PlayOneShot(EnemyGotHitHeadClip, _volume);
                _successfullShots++;
                _successfullHeadshots++;
                _comboCount++;
                _comboWait = 0;
                TakeDamage(pHit.transform, true);
            } else if (pHit.transform.tag == pTarget) {
                pHit.collider.GetComponentInChildren<ImpactSounds>().PlayImpactSound(EnemyGotHitBodyClip, _volume);
                _successfullShots++;
                _comboCount++;
                _comboWait = 0;
                //Debug.Log(pHit.transform.name + " has been hit using a ranged weapon");
                TakeDamage(pHit.transform, false);
            }
        }
    }

    private void DecreaseBulletCount() {
        _ammoCount--;
    }

    private void TakeDamage(Transform pTarget, bool headshot) {
        //Debug.Log("Changing to red..");
        pTarget.LookAt(gameObject.transform.position);
        if (_weaponHandler.CurrentWeaponType == WeaponType.Melee) {
            if (headshot) {
                pTarget.GetComponent<EnemyScript>().DecreaseHealth(_weaponDamage[0] * 2);
            } else {
                Debug.Log(pTarget.name + " got hit!");
                pTarget.GetComponent<EnemyScript>().DecreaseHealth(_weaponDamage[0]);
            }
           
        } else if (_weaponHandler.CurrentWeaponType == WeaponType.Ranged)
        {
            pTarget.GetComponent<EnemyMovement>().SetWaypoint(gameObject);
            pTarget.GetComponent<EnemyScript>().SetDisturbedLocation(gameObject.transform.position, true);
            //Debug.Log(gameObject.transform.position);
            if (headshot)
                pTarget.GetComponent<EnemyScript>().DecreaseHealth(2*_weaponDamage[1]);
            else
                pTarget.GetComponent<EnemyScript>().DecreaseHealth(_weaponDamage[1]);
        }
        if (pTarget.GetComponent<EnemyScript>().IsDead) {
            if (_weaponHandler.CurrentWeaponType == WeaponType.Melee) {
                _knifeKillNumber++;
            } else {
                _totalRangedKills++;
            }
            if (headshot) {
                _totalHeadshotKills++;
                //Debug.Log("Got Killed By headshot");
            }
            _totalKills++;
            //pTarget.GetComponent<EnemyMovement>().Animation.Stop();
            StartCoroutine(DestroyEnemy(pTarget, pTarget.GetComponent<EnemyMovement>().Animation, pTarget.GetComponent<EnemyMovement>().TimeOnLastFrame));
        }
        //_startTimer = true;
    }

    private IEnumerator DestroyEnemy(Transform pTransform, Animation pAnimation, float pTimeOnLastFrame) {
        //pTransform.GetComponent<EnemyMovement>().SetState(EnemyMovement.State.none);
        pTransform.GetComponent<EnemyMovement>().DisableAllControls = true;
        pAnimation.Play("DeathEditable");
        pTransform.GetComponent<EnemyScript>().TriggerTextAndEnemy();
        yield return new WaitForSeconds(pAnimation["DeathEditable"].length + pTimeOnLastFrame);
        if (pTransform != null) {
            Destroy(pTransform.gameObject);
        }
    }

    private void MeleeDamage(Vector3 pCenter, float pRadius, string pTarget, WeaponAOEType pAoeType) {
        int i = 0;
        Collider[] hitColliders = Physics.OverlapSphere(pCenter, pRadius);
        _totalKnives++;

        while (i < hitColliders.Length) {
            if (hitColliders[i].transform.tag == pTarget) {
                //RaycastHit hit = new RaycastHit();
                ////Debug.Log("before linecast "+pTarget + " | " + hitColliders[i].transform.name);
                //if (Physics.Linecast(gameObject.transform.position, hitColliders[i].transform.position, out hit))
                //{
                //Debug.Log(pTarget + " | " + hit.transform.name);
                //if (hit.transform.tag == pTarget)
                //{
                bool behind = false;
                Vector3 directionToTarget = transform.position - hitColliders[i].transform.position;
                float angle = Vector3.Angle(hitColliders[i].transform.forward, directionToTarget);
                if (Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270) {
                    Debug.Log("backstab");
                    behind = true;
                    hitColliders[i].GetComponentInChildren<ImpactSounds>().PlayImpactSound(BackstabClip, _volume);

                }

                _successfullKnives++;
                if (pAoeType == WeaponAOEType.Single) {
                    Debug.Log(hitColliders[i].transform.name + " | " + pRadius);
                    //Debug.Log("Enemy equals null: " + (GetClosestEnemy(hitColliders, pRadius) == null));
                    if (GetClosestEnemy(hitColliders, pRadius) != null) {
                        // Debug.Log(GetClosestEnemy(hitColliders, pRadius) + " has been hit.");
                        _comboCount++;
                        _comboWait = 0;
                        hitColliders[i].GetComponentInChildren<ImpactSounds>().PlayImpactSound(EnemyGotHitBodyClip, _volume);
                        //Debug.Log("get hit soundource is "+hitColliders[i].GetComponent<AudioSource>().name);
                        TakeDamage(GetClosestEnemy(hitColliders, pRadius), behind);
                        break;
                    }
                } else if (pAoeType == WeaponAOEType.Multi) {
                    _comboCount++;
                    _comboWait = 0;
                    //Debug.Log(hitColliders[i].name + " has been hit.");
                    hitColliders[i].GetComponent<AudioSource>().PlayOneShot(EnemyGotHitBodyClip, _volume);
                    TakeDamage(hitColliders[i].transform, behind);
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
                //Debug.Log("OUT if: " + collider.transform.name + " | " + "( " + dist + ", " + pMinDist + " )" + " | " + angle + ", " + _angle);
                if (dist < pMinDist && angle < _angle) {
                    //Debug.Log("IN if: " + collider.transform.name + " | " + "( " + dist + ", " + pMinDist + " )" + " | " + angle + ", " + _angle);
                    enemy = collider.transform;
                    pMinDist = dist;
                }
            }
        }
        return enemy;
    }

    public void DecreaseHealth(int pAmount) {
        if (_blocking) {
            _blockedShots++;
        } else if (_health > 0) {
            _completedLevelWithoutDmg = false;
            _comboCount = 0;
            _comboWait = 0;
            audio.PlayOneShot(HitClip);
            _health -= pAmount;
            if (_health == 3) {
                _cracks[0].SetActive(true);
                StartCoroutine(FadeTo(_cracks[0].GetComponent<Image>(), _fadeAlphaForCracks[0], _fadeSecondsForCracks[0]));
            }
            if (_health == 2) {
                _cracks[1].SetActive(true);
                StartCoroutine(FadeTo(_cracks[1].GetComponent<Image>(), _fadeAlphaForCracks[1], _fadeSecondsForCracks[1]));
            }
            if (_health == 1) {
                _cracks[2].SetActive(true);
                StartCoroutine(FadeTo(_cracks[2].GetComponent<Image>(), _fadeAlphaForCracks[2], _fadeSecondsForCracks[2]));
            }
            if (_health < 0) {
                _health = 0;
            }
        }
    }

    private IEnumerator FadeTo(Image pImage, float aValue, float aTime) {
        float alpha = pImage.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            pImage.color = newColor;
            yield return null;
        }
    }

    private void ApplyVignette() {
        _profileSettings.mode = _vignetteMode;
        _profileSettings.color = _vignetteColor;
        _profileSettings.center = _vignetteCenter;
        _profileSettings.intensity = _vignetteIntensity;
        _profileSettings.smoothness = _vignetteSmoothness;
        _profileSettings.roundness = _vignetteRoundness;
        _profileSettings.rounded = _vignetteIsRounded;

        _profile.vignette.settings = _profileSettings;
    }


    public bool HasWon() {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0) {
            return false;
        } else {
            if (_enemyHandler.EnemiesLeft() == 0)
            {
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
                Utils.NextLevel("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt", allInfo);
                return true;
            } else
            {
                return false;
            }
        }
    }

}
