using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectScript : MonoBehaviour {

    public List<GameObject> Buttons = new List<GameObject>();
    private int _level;
    private int _wait;
	// Use this for initialization
	void Start () {
        _level = Utils.LatestLevel();

        foreach (Transform obj in transform)
        {
            Buttons.Add(obj.gameObject);
        }
        MakeMenu();
    }

    //private void Update()
    //{
    //    if(_wait==2)
    //    {
    //        MakeMenu();
    //    }
        
    //    if(_wait!=3)
    //        _wait++;

    //    //Debug.Log("Menu has " + Buttons.Count + " buttons");


    //}

    private void MakeMenu()
    {
        Debug.Log("Making menu! With highest unlock = " + _level);
        Debug.Log("Menu has " + Buttons.Count + " buttons");
        for (int i = 0; i < Buttons.Count; i++)
        {
            Debug.Log(Buttons[i].name);
            if (i < _level)
            {
                //Debug.Log("unlocking");
                Buttons[i].GetComponent<ButtonScript>().Unlock();
            }
            else
            {
                Buttons[i].GetComponent<ButtonScript>().Lock();
            }
        }

    }

    public void AddButton(GameObject button)
    {
        //Debug.Log(button.name + " was added to list");
        Buttons.Add(button);
    }
}
