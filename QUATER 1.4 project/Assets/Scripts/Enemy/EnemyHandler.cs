using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    private List<GameObject> Enemies = new List<GameObject>();
    // Use this for initialization

    public void AddToHandler(GameObject enem)
    {
        //Debug.Log("added to handler: " + enem.name);
        Enemies.Add(enem);
    }

    public void AlertOthers(GameObject enem, float alertDistance)
    {
        //Debug.Log("Allerting others of disturbance at: "+ enem.transform.position);
        Enemies.Remove(enem);
        foreach (GameObject obj in Enemies)
        {
            if ((enem.transform.position - obj.transform.position).magnitude < alertDistance)
            {
                //Debug.Log(obj.name + "has been alerted");
                obj.GetComponent<EnemyMovement>().SetWaypoint(enem);
                obj.GetComponent<EnemyScript>().SetDisturbedLocation(enem.transform.position,true);
            }
        }
    }

    public int EnemiesLeft()
    {
        return Enemies.Count;
    }
}

