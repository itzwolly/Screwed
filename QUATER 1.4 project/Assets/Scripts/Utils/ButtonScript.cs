using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    // Use this for initialization
    public GameObject LevelSelect;
    public GameObject UnlockedBrush;
    public GameObject LockedBrush;

  

	void Start () {
        //Debug.Log();
        //LevelSelect.GetComponent<LevelSelectScript>().AddButton(gameObject);
        //Destroy(gameObject.GetComponent<MeshFilter>());
        //Destroy(gameObject.GetComponent<UnityEngine.UI.Image>());
        //Destroy(gameObject.GetComponent<BoxCollider>());
    }
	
	public void Lock()
    {
        Debug.Log(gameObject.name + " is locked");
        // Destroy(_unlocked);
        UnlockedBrush.SetActive(false);
        LockedBrush.SetActive(!false);
    }

    public void Unlock()
    {
        Debug.Log(gameObject.name + " is unlocked");
        //Destroy(_locked);
        UnlockedBrush.SetActive(!false);
        LockedBrush.SetActive(false);
    }
}
