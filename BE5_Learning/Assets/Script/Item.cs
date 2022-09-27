using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type{ Ammo, Coin, Grenade, Heart, Weapon };
    public Type type;
    public int value;

    Rigidbody rigid;
    SphereCollider sphere;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphere = GetComponent<SphereCollider>();
    }

    void Update()
    {
        transform.Rotate(UnityEngine.Vector3.up*20*Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Floor"){
            rigid.isKinematic=true;
            sphere.enabled=false;
        }    
    }
}
