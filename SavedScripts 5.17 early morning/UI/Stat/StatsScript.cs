using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatsScript : MonoBehaviour {
    //public Text TimeInLevel;

    // Use this for initialization
    void Start () {
       }

    public void UpdateThisStat()
    {
        gameObject.GetComponent<Text>().text = (Utils.GetValueAfterString("Assets\\Statistics\\" + SceneManager.GetActiveScene().name + ".txt", gameObject.name + ":")).ToString();
        
        if(gameObject.name== "Completedlevelwithoutdmg")
        {
            if(gameObject.GetComponent<Text>().text=="1")
            {
                gameObject.GetComponent<Text>().text = "true";
            }
            else
            {
                gameObject.GetComponent<Text>().text = "false";
            }

        }
        //Debug.Log(gameObject.GetComponent<Text>().text);
    }

}
