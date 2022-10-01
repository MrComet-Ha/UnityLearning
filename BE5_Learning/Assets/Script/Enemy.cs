using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //Data
    public enum Type { A, B, C, D };
    public Type enemyType;
    //Mob stats
    public int maxHealth;
    public int curHealth;

    //Mob AI
    public bool isChase;
    public bool isAttack;
    public bool isDead;

    //Attack
    public BoxCollider meleeArea;

    //Reference
    public Transform target;
    public ObjectManager obj;
    public Rigidbody rigid;
    public BoxCollider boxCol;
    public MeshRenderer[] meshs;
    public Animator anim;
    public NavMeshAgent nav;

    void Awake(){
        rigid = GetComponent<Rigidbody>();
        boxCol = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
    }

    //Data Reset
    void OnEnable()
    {
        anim = GetComponentInChildren<Animator>();
        curHealth = maxHealth;
        if(enemyType != Type.D)
            Invoke("ChaseStart",2f);
    }

    void OnDisable()
    {
        CancelInvoke("ChaseStart");
        StopCoroutine(Attack());
        isChase=false;
        isAttack=false;
        gameObject.layer = 8;
        foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.white;
        }
        
    }

    void Update(){
        if(nav.enabled && enemyType != Type.D){
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }
    void FixedUpdate(){
        Targeting();
        FreezeVelocity();
    }

    //AI 
    void ChaseStart(){
        isChase = true;
        anim.SetBool("isWalk",true);
    }

    void Targeting(){
        if(!isDead && enemyType != Type.D){
            float targetRadius = 1.5f;
            float targetRange = 3f;

            switch(enemyType){
                case Type.A :
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;
                case Type.B :
                    targetRadius = 1f;
                    targetRange = 10f;
                    break;
                case Type.C :
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;
                
            }
            RaycastHit[] rayHits =Physics.SphereCastAll(
                    transform.position,
                    targetRadius,
                    transform.forward,
                    targetRange,
                    LayerMask.GetMask("Player"));
            if(rayHits.Length > 0 && !isAttack){
                StartCoroutine(Attack());
            }
        }  
    }
    
    IEnumerator Attack(){
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack",isAttack);

        switch(enemyType){
            case Type.A :
                yield return new WaitForSeconds(0.3f);
                meleeArea.enabled = true;
                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;
                yield return new WaitForSeconds(0.5f);
                break;
            case Type.B :
                yield return new WaitForSeconds(0.2f);
                rigid.AddForce(transform.forward * 30,ForceMode.Impulse);
                meleeArea.enabled = true;
                yield return new WaitForSeconds(0.7f);
                rigid.velocity = UnityEngine.Vector3.zero;
                rigid.angularVelocity = UnityEngine.Vector3.zero;
                meleeArea.enabled = false;
                yield return new WaitForSeconds(1.5f);
                break;
            case Type.C :
                yield return new WaitForSeconds(0.5f);
                GameObject missile = obj.CreateObj("EnemyMissile");
                missile.transform.position = transform.position;
                missile.transform.rotation = transform.rotation;
                Rigidbody rigidM = missile.GetComponent<Rigidbody>();
                rigidM.velocity = transform.forward * 35;
                yield return new WaitForSeconds(2f);
                break;
        }

        isChase = true;
        isAttack = false;
        
        anim.SetBool("isAttack",isAttack);
    }
    //Physics
    void FreezeVelocity(){
        if(isChase){
            rigid.velocity = UnityEngine.Vector3.zero;
            rigid.angularVelocity = UnityEngine.Vector3.zero;
        }
    }
    

    
    //Collision
    //Collision setup
    void OnTriggerEnter(Collider other){
        if(other.tag == "Melee"){
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.dmg;
            UnityEngine.Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamaged(reactVec,false));
        }
        else if(other.tag == "Bullet"){
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.dmg;
            UnityEngine.Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamaged(reactVec,false));
        }
    }
    public void HitByGrenade(Vector3 explodePos){
        curHealth -= 100;
        Vector3 reactVec = transform.position - explodePos;
        StartCoroutine(OnDamaged(reactVec,true));
    }
    
    //Disable
    void Disable(){
        gameObject.SetActive(false);
    }

    //Hit Logic
    
    IEnumerator OnDamaged(UnityEngine.Vector3 reactVec, bool isGre){
        foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.red;
        }
        yield return new WaitForSeconds(0.1f);
        if(curHealth > 0){
            foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.white;
        }
        }
        else{
            foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.gray;
        }
            gameObject.layer = 11;
            isChase = false;
            nav.enabled = false;
            isDead = true;
            anim.SetTrigger("doDie");

            if(isGre){
                reactVec = reactVec.normalized;
                reactVec += UnityEngine.Vector3.up * 3;
                rigid.freezeRotation=false;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
            }
            else{
                reactVec = reactVec.normalized;
                reactVec += UnityEngine.Vector3.up;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            }
            if(enemyType != Type.D)
                Invoke("Disable",3f);
        }
        yield break;
    }

}
