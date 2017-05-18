using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSequence : MonoBehaviour
{
    public GameObject introScreen1;
    public GameObject introScreen2;
    // Use this for initialization
    void Start()
    {
        Screen1();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Screen1()
    {
        Debug.Log("Screen 1 is active");
        introScreen2.SetActive(!true);
        introScreen1.SetActive(!false);
    }
    public void Screen2()
    {
        Debug.Log("Screen 2 is active");
        introScreen2.SetActive(!false);
        introScreen1.SetActive(!true);
    }
    public void StartGame()
    {
        introScreen2.SetActive(false);
        introScreen1.SetActive(false);
        Application.LoadLevel(3);
    }
}
