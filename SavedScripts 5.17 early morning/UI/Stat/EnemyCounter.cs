using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//[RequireComponent(EnemyHandler)]
public class EnemyCounter : MonoBehaviour {
    public EnemyHandler EnemyHandler;
    private Text _text;
	// Use this for initialization
	void Start () {
        _text = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if(_text!=null &&EnemyHandler!=null)
            _text.text = EnemyHandler.EnemiesLeft().ToString();
	}
}
