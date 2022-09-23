using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCycle : MonoBehaviour
{
    public int dmg;
    public bool isRotate;

    void Update()
    {
         if(isRotate){
            transform.Rotate(Vector3.forward * 6);
         }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "BorderBullet"){
            Rigidbody2D rigid;
            rigid = GetComponent<Rigidbody2D>();
            rigid.velocity = UnityEngine.Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
