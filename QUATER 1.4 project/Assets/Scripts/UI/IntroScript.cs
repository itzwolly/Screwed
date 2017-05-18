using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour
{
    private bool screen2 = false;
    private bool screen1 = true;
    private bool startGame = false;
    public GameObject mainCamera;
    public GameObject introScreen1;
    public GameObject introScreen2;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (screen2)
        {
            Time.timeScale = 0.0f;
            mainCamera.GetComponent<MouseLook>().enabled = false;
            gameObject.GetComponent<CombatControls>().enabled = false;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                obj.GetComponent<EnemyMovement>().enabled = false;
            }
            introScreen2.SetActive(true);
            introScreen1.SetActive(false);
        }
        if (screen1)
        {
            Time.timeScale = 0.0f;
            mainCamera.GetComponent<MouseLook>().enabled = false;
            gameObject.GetComponent<CombatControls>().enabled = false;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                obj.GetComponent<EnemyMovement>().enabled = false;
            }
            introScreen2.SetActive(false);
            introScreen1.SetActive(true);
        }
        if (startGame)
        {
            Time.timeScale = 1.0f;
            mainCamera.GetComponent<MouseLook>().enabled = true;
            gameObject.GetComponent<CombatControls>().enabled = true;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                obj.GetComponent<EnemyMovement>().enabled = true;
            }
            introScreen2.SetActive(false);
            introScreen1.SetActive(false);
        }
    }
    public void Screen1()
    {
        screen1 = true;
        screen2 = false;
        startGame = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;
    }
    public void Screen2()
    {
        screen1 = !true;
        screen2 = !false;
        startGame = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;
    }
    public void StartGame()
    {
        screen1 = !true;
        screen2 = false;
        startGame = !false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
    }
}
