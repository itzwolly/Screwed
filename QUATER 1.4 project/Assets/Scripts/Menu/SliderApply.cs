using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderApply : MonoBehaviour {
    
    private List<GameObject> _sliders;
	void Start () {
        _sliders = new List<GameObject>();
	}
	
    public void ApplySliders()
    {
        foreach(GameObject slider in _sliders)
        {
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
        _sliders.Add(value);
    }
}
