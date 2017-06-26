using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(Slider)]
public class SliderValues : MonoBehaviour {
    public SliderApply Apply;
    public MenuManager MenuManager;
    public PauseMenu PauseMenuManager;
    Slider _slider;
    bool _changedValues;
    float _savedValue;
	// Use this for initialization
	void Start () {
        Apply.AddSlider(gameObject);
        _slider = gameObject.GetComponent<Slider>();
        _slider.value = Utils.GetValueAfterString("Assets\\SaveInfo.txt", gameObject.name + ":")/100f;
        _changedValues = false;
        _savedValue = _slider.value;
    }

    private void Update()
    {
        if (_slider.value != _savedValue)
        {
            //Debug.Log("changedValues - " + _changedValues);
            if (MenuManager != null)
            {
                MenuManager.ChangedValues();
            } else
            {
                PauseMenuManager.ChangedValues();
            }
            
            _savedValue = _slider.value;
        }
    }

    public bool ChangedValues()
    {
        return _changedValues;
    }

    public void OffPage()
    {
        _changedValues = false;
    }
}
