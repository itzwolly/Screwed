using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour {
    public CursorLockMode lockCursor;
    public Text totalKillsText;
    public Text timeText;
    public Text knifeText;
    public Text shotsText;
    public Text headshotsText;
    public GameObject statsScreen;
    private bool state;
    public GameObject mainCamera;
    // Use this for initialization
    void Start () {
        state = false;
        Cursor.lockState = lockCursor;
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
        if (!state)
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
        totalKillsText.text = Utils.GetTotalKills("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt").ToString();
        timeText.text = Utils.GetTimeInLevel("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt").ToString();
        knifeText.text = Utils.GetSuccessfullKnives("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt").ToString();
        shotsText.text = Utils.GetSuccessfullShots("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt").ToString();
        headshotsText.text = Utils.GetSuccessfullHeadshots("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt").ToString();
        Cursor.lockState = CursorLockMode.Confined;
        state = !state;
    }
}
