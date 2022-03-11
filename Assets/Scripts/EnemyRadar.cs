using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadar : MonoBehaviour
{
    private Enemy _enemyparent;
    public bool evasiveManeuver = false;

    private void OnTriggerEnter2D(Collider2D detect) // enemy radar detects incoming laser
    {
        
        if (detect.tag == "Laser")
        {
            Debug.Log("Laser detected");
             evasiveManeuver = true;
        }

    }
}
