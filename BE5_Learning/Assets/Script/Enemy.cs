using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;

    Rigidbody rigid;
    BoxCollider boxCol;
    Material mat;

    void Awake(){
        rigid = GetComponent<Rigidbody>();
        boxCol = GetComponent<BoxCollider>();
        mat = GetComponent<MeshRenderer>().material;
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Melee"){
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.dmg;
            UnityEngine.Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine("OnDamaged");
        }
        else if(other.tag == "Bullet"){
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.dmg;
            UnityEngine.Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine("OnDamaged");
        }
    }
    void Disable(){
        gameObject.SetActive(false);
    }
    IEnumerator OnDamaged(UnityEngine.Vector3 reactVec){
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        if(curHealth > 0){
            mat.color = Color.white;
        }
        else{
            mat.color = Color.gray;
            gameObject.layer = 11;
            reactVec = reactVec.normalized;
            reactVec += UnityEngine.Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            Invoke("Disable",3f);
        }
        yield break;
    }
}
