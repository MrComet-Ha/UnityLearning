using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCycle : MonoBehaviour
{
    public int dmg;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "BorderBullet"){
            Destroy(gameObject);
        }
    }
}
