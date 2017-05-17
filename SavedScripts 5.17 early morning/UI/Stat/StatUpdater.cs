using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUpdater : MonoBehaviour {

	public void UpdateStats()
    {
        foreach(Transform tra in gameObject.transform)
        {
            tra.gameObject.GetComponent<StatsScript>().UpdateThisStat();
        }
    }
}
