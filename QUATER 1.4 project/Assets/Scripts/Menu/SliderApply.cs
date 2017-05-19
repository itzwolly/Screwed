using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderApply : MonoBehaviour {
    
    private List<GameObject> _sliders;
	void Awake () {
        _sliders = new List<GameObject>();
	}
	
    public void ApplySliders()
    {
        Debug.Log("apply "+_sliders.Count);
        foreach(GameObject slider in _sliders)
        {
            Debug.Log("slider is called = "+slider.name);
            Utils.SetValueAfterString("Assets\\SaveInfo.txt", slider.name + ":", (int)(slider.GetComponent<Slider>().value * 100));
        }
    }
    

    public void ResetSliders()
    {
        foreach (GameObject slider in _sliders)
        {
            slider.GetComponent<Slider>().value = Utils.GetValueAfterString("Assets\\SaveInfo.txt", slider.name + ":") / 100f;
        }

    }

    public void ResetValues()
    {
        foreach (GameObject slider in _sliders)
        {
            slider.GetComponent<Slider>().value = 100;
        }
    }

    public void AddSlider(GameObject value)
    {
        //if (_sliders == null || _sliders.Count == 0)
        //{
        //    _sliders = new List<GameObject>();
        //}
        _sliders.Add(value);
        Debug.Log(" added "+_sliders.Count);
    }
}
