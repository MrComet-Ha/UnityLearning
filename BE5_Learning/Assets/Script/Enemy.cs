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
    public int score;

    //Mob AI
    public bool isChase;
    public bool isAttack;
    public bool isDead;

    //Attack
    public BoxCollider meleeArea;

    //Sound
    public AudioSource sndBulletHit;
    public AudioSource sndHammerHit;

    //Reference
    public Transform target;
    public ObjectManager obj;
    public GameManager gm;
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
        anim = GetComponentInChildren<Animator>();
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
        if(gm!=null){
            switch(enemyType){
                    case Type.A : gm.enemyACnt -= 1;
                        break;
                    case Type.B : gm.enemyBCnt -= 1;
                        break;
                    case Type.C : gm.enemyCCnt -= 1;
                        break;
                    case Type.D : gm.enemyDCnt -= 1;
                        break;
            }
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
            sndHammerHit.Play();
        }
        else if(other.tag == "Bullet"){
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.dmg;
            UnityEngine.Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamaged(reactVec,false));
            sndBulletHit.Play();
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
        //Color change red
        foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.red;
        }
        yield return new WaitForSeconds(0.1f);
        //If enemy alive
        if(curHealth > 0){
            foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.white;
        }
        }
        //If enemy died
        else{
            foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.gray;
        }
            gameObject.layer = 11;
            isChase = false;
            nav.enabled = false;
            isDead = true;
            anim.SetTrigger("doDie");
            //Grenade kill
            if(isGre){
                reactVec = reactVec.normalized;
                reactVec += UnityEngine.Vector3.up * 3;
                rigid.freezeRotation=false;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
            }
            //Weapon Kill
            else{
                reactVec = reactVec.normalized;
                reactVec += UnityEngine.Vector3.up;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            }
            //Disable
            Invoke("Disable",3f);
            
            //Add Score
            gm.AddScore(score);

            //Item Drop
            int ranDrop = enemyType == Type.D ? 10 : Random.Range(0,10);
            //Drop nothing
            if(ranDrop < 6){
                yield return null;
            }
            //Drop Item
            else if(ranDrop >= 6){
                //Item Physic
                Vector3 ranVec = Vector3.up * Random.Range(2,5) + Vector3.right * Random.Range(-3,3);
                //Coin
                if(ranDrop <= 10){
                    int drop = enemyType == Type.D ? 5 : Random.Range(1,3);
                    for(int i = 0; i < drop; i++){
                        int ranCoin = enemyType == Type.D ? 6 : Random.Range(0,6);
                        GameObject coin;
                        if(ranCoin < 4)
                            coin = obj.CreateObj("BronzeCoin");
                        else if(ranCoin < 6)
                            coin = obj.CreateObj("SilverCoin");
                        else
                            coin = obj.CreateObj("GoldCoin");
                        coin.transform.position = transform.position;
                        coin.transform.rotation = Quaternion.identity;
                        Rigidbody coinRigid = coin.GetComponent<Rigidbody>();
                        coinRigid.AddForce(ranVec,ForceMode.Impulse);
                    }  
                }
                //Supply
                if(ranDrop == 10){
                    int ranItem = Random.Range(0,3);
                    GameObject item;
                    switch(ranItem){
                        case 2 : item = obj.CreateObj("Ammo");
                            break;
                        case 3 : item = obj.CreateObj("Grenade");
                            break;
                        default : item = obj.CreateObj("Health");
                            break;
                    }
                    item.transform.position = transform.position;
                    item.transform.rotation = Quaternion.identity;
                    Rigidbody itemRigid = item.GetComponent<Rigidbody>();
                    itemRigid.AddForce(ranVec,ForceMode.Impulse);
                }
            }
        }
        yield break;
    }

}
