using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    GameObject other;
    public float spinspeed;
    void Awake(){
        other = GameObject.FindGameObjectWithTag("Manager");
        ScoreManagement manager = other.GetComponent<ScoreManagement>();
        manager.itemtotal += 1;
    }
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * spinspeed,Space.World);
    }
}
