using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCycle : MonoBehaviour
{
    public int dmg;
    public Vector3 movement;

    void Awake()
    {
        movement=transform.position;   
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "BorderBullet"){
            gameObject.SetActive(false);
        }
    }
}
