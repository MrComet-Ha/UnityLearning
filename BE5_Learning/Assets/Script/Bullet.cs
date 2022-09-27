using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public TrailRenderer trail;
    public int dmg;

    void OnDisable()
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.velocity = UnityEngine.Vector3.zero;
        if(trail != null)
           trail.enabled = false;
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Floor"){
            Invoke("Disable",3f);
        }
        else if(other.gameObject.tag =="Wall"){
            Disable();
        }
    }
    void Disable(){
        gameObject.SetActive(false);
    }
}
