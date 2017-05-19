using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CombatControls))]
public class StatsDisplay : MonoBehaviour {
    public CursorLockMode lockCursor;
    public Text totalKillsText;
    public Text timeText;
    public Text knifeText;
    public Text shotsText;
    public Text headshotsText;

    public Text _blockedShots;
    public GameObject[] _completedWithoutDamageMarks ;
    public Text _secretsGathered;
    public Text _successfulHeadshots;

    public Text _totalTimesKnifed;
    public Text _totalSuccessfulKnives;
    public Text _successfulShots;
    public Text _totalRangedKills;

    public GameObject statsScreen;
    private bool state;
    public GameObject mainCamera;

    private CombatControls _combatControls;

    // Use this for initialization
    void Start () {
        state = false;
        Cursor.lockState = lockCursor;
        _combatControls = GetComponent<CombatControls>();
    }
	
	// Update is called once per frame
	void Update () {
        if (state)
        {
            Time.timeScale = 0.0f;
            mainCamera.GetComponent<MouseLook>().enabled = false;
            gameObject.GetComponent<CombatControls>().enabled = false;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                obj.GetComponent<EnemyMovement>().enabled = false;
            }
        }
        else
        {
            Time.timeScale = 1.0f;
            mainCamera.GetComponent<MouseLook>().enabled = true;
            gameObject.GetComponent<CombatControls>().enabled = true;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                obj.GetComponent<EnemyMovement>().enabled = true;
            }
        }
	}

    public void DisplayStats()
    {
        statsScreen.SetActive(!state);
        //totalKillsText.text = Utils.GetTotalKills("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt").ToString();
        totalKillsText.text = _combatControls._totalKills.ToString();
        //timeText.text = Utils.GetTimeInLevel("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt").ToString();
        timeText.text = _combatControls._timeInLevel.ToString("0");
        //knifeText.text = Utils.GetSuccessfullKnives("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt").ToString();
        knifeText.text = _combatControls._knifeKillNumber.ToString();
        //shotsText.text = Utils.GetSuccessfullShots("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt").ToString();
        shotsText.text = _combatControls._totalShots.ToString();
        //headshotsText.text = Utils.GetSuccessfullHeadshots("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt").ToString();
        headshotsText.text = _combatControls._totalHeadshotKills.ToString();

        _blockedShots.text = _combatControls._blockedShots.ToString();
        if (_combatControls._completedLevelWithoutDmg) {
            _completedWithoutDamageMarks[0].gameObject.SetActive(true);
            _completedWithoutDamageMarks[1].gameObject.SetActive(false);
        } else {
            _completedWithoutDamageMarks[1].gameObject.SetActive(true);
            _completedWithoutDamageMarks[0].gameObject.SetActive(false);
        }
        
        _secretsGathered.text = _combatControls._secretsGathered.ToString();
        _successfulHeadshots.text = _combatControls._successfullHeadshots.ToString();

        _totalTimesKnifed.text = _combatControls._totalKnives.ToString();
        _totalSuccessfulKnives.text = _combatControls._successfullKnives.ToString();
        _successfulShots.text = _combatControls._successfullShots.ToString();
        _totalRangedKills.text = _combatControls._totalRangedKills.ToString();

        Cursor.lockState = CursorLockMode.Confined;
        state = !state;
    }
}
