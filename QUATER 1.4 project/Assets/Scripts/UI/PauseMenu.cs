using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public GameObject hudToDisable;
    public GameObject pauseMenu;
    public GameObject mainCamera;
    public GameObject player;
    private bool changedValues = false;
    private bool state = false;
    private bool comingFromOptions = false;
    public GameObject exitPanel;
    public GameObject optionsPanel;
    public GameObject applyPanel;
    public GameObject optionsText;
    public GameObject playText;
    public GameObject exitText;
    // Use this for initialization
    void Start () {
        pauseMenu.SetActive(state);
        playText.SetActive(!true);
        optionsText.SetActive(!true);
        exitText.SetActive(!true);
    }

    public void ChangedValues()
    {
        changedValues = true;
    }

    public void Options()
    {
        Debug.Log("Options");
        if (comingFromOptions && changedValues) { Apply(); comingFromOptions = false; changedValues = false; }

        else
        {

            optionsPanel.SetActive(true);
            comingFromOptions = true;
        }
    }

    //public void BGPlay()
    //{
    //    //spriteRenderer.sprite = playSprite;
    //    //Debug.Log("FUCK THAT SHIT");
    //    playText.SetActive(true);
    //}
    //public void BGOptions()
    //{
    //    //spriteRenderer.sprite = optionsSprite;
    //    optionsText.SetActive(true);
    //}
    //public void BGExit()
    //{
    //    //spriteRenderer.sprite = playSprite;
    //    //Debug.Log("FUCK THAT SHIT");
    //    exitText.SetActive(true);
    //}
    //public void BGDefault()
    //{
    //    playText.SetActive(!true);
    //    optionsText.SetActive(!true);
    //    exitText.SetActive(!true);
    //}

    public void Exit()
    {
        if (comingFromOptions && changedValues) { Apply(); comingFromOptions = false; changedValues = false; }
        else
        {
            optionsPanel.SetActive(false);
            exitPanel.SetActive(true);
            comingFromOptions = false;
        }
    }

    public void Default()
    {
        changedValues = false;
        optionsPanel.SetActive(false);
        exitPanel.SetActive(!true);
        applyPanel.SetActive(false);
        comingFromOptions = false;
    }

    public void QuitToMenu()
    {
        Application.LoadLevel(1);
    }

    public void ResetSliders(GameObject button)
    {
        Debug.Log("resetsliders");
        button.GetComponent<SliderApply>().ResetSliders();
        Default();

    }

    public void ResetOptions(GameObject button)
    {
        button.GetComponent<SliderApply>().ResetValues();

    }

    public void ApplyDefault(GameObject button)
    {
        Debug.Log("applydefault");
        button.GetComponent<SliderApply>().ApplySliders();
        Default();
    }

    public void Apply()
    {
        Debug.Log("APLLY CHANGES?");

        optionsPanel.SetActive(false);
        exitPanel.SetActive(!true);
        if (changedValues)
            applyPanel.SetActive(!false);
    }

    // Update is called once per frame
    void Update () {

        if (state)
        {
            Time.timeScale = 0.0f;
            mainCamera.GetComponent<MouseLook>().enabled = false;
            player.GetComponent<CombatControls>().enabled = false;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                obj.GetComponent<EnemyMovement>().enabled = false;
            }
           // Default();
        }
        else
        {
            ////Time.timeScale = 1.0f;
            //mainCamera.GetComponent<MouseLook>().enabled = true;
            //player.GetComponent<CombatControls>().enabled = true;
            //foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            //{
            //    obj.GetComponent<EnemyMovement>().enabled = true;
            //}
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            pauseMenu.SetActive(!state);
            hudToDisable.SetActive(state);
            state = !state;
        }
        
	}

    public void ExitPauseMenu()
    {
        pauseMenu.SetActive(false);
        hudToDisable.SetActive(true);
    }

    public void SwitchState()
    {
        state = !state;
        Default();
    }
}
