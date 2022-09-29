using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public TrailRenderer trail;
    public bool isMelee;
    public int dmg;

    void OnDisable()
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        if(rigid != null)
            rigid.velocity = UnityEngine.Vector3.zero;
        if(trail != null)
           trail.enabled = false;
    }
    void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Floor"){
            Invoke("Disable",3f);
        } 
    }
    void OnTriggerEnter(Collider other){
        if(other.tag == "Wall" || other.tag == "Enemy"){
            Disable();
        }
    }
    void Disable(){
        if(!isMelee)
            gameObject.SetActive(false);
    }
}
