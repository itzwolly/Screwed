using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {

    public static void ChangeGameObjectColor(GameObject pGameObject, Color pFrom, Color pTo) {
        if (pGameObject.transform.GetComponent<Renderer>().material.color == pFrom) {
            pGameObject.transform.GetComponent<Renderer>().material.color = pTo;
        }
    }

    public static void ChangeGameObjectsColor(GameObject[] pGameObjects, Color pFrom, Color pTo) {
        foreach (GameObject obj in pGameObjects) {
            if (obj.transform.GetComponent<Renderer>().material.color == pFrom) {
                obj.transform.GetComponent<Renderer>().material.color = pTo;
            }
        }
    }
}
