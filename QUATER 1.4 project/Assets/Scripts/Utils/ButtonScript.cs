using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    // Use this for initialization
    public GameObject UnlockedBrush;
    public GameObject LockedBrush;

    private GameObject _unlocked;
    private GameObject _locked;

	void Start () {
        //Debug.Log();
        //LevelSelect.GetComponent<LevelSelectScript>().AddButton(gameObject);
        Destroy(gameObject.GetComponent<MeshFilter>());
        //Destroy(gameObject.GetComponent<BoxCollider>());
    }
	
	public void Lock()
    {
        //Debug.Log(gameObject.name + " is locked");
        Destroy(_unlocked);
        _locked = GameObject.Instantiate(LockedBrush);
        _locked.transform.position = transform.position;
    }

    public void Unlock()
    {
        //Debug.Log(gameObject.name + " is unlocked");
        Destroy(_locked);
        _unlocked = GameObject.Instantiate(UnlockedBrush);
        _unlocked.transform.position = transform.position;
    }
}
