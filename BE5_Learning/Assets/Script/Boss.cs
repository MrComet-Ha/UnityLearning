using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public Transform missilePortA;
    public Transform missilePortB;
    
    Vector3 lookVec;
    Vector3 tauntVec;
    public bool isLook;

    void Awake(){
        rigid = GetComponent<Rigidbody>();
        boxCol = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
    }

    void OnEnable(){
        anim = GetComponentInChildren<Animator>();
        isLook = true;
        curHealth = maxHealth;
        nav = GetComponent<NavMeshAgent>();
        nav.isStopped = true;
        StartCoroutine(StartThink());
    }
    IEnumerator StartThink(){
        yield return new WaitForSeconds(2f);
        if(gameObject.activeSelf)
            StartCoroutine(Think());
    }
    void Update()
    {
        if(isDead){
            StopAllCoroutines();
            return;
        }
        if(isLook){
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h,0,v) * 5f;
            transform.LookAt(target.position + lookVec);
        }
        else
            nav.SetDestination(tauntVec);
    }

    IEnumerator Think(){
        yield return new WaitForSeconds(0.5f);
        int ranAction = Random.Range(0,5);
        switch(ranAction){
            case 0 :
            case 1 :
                StartCoroutine(MissileShot());
                break;
            case 2 :
            case 3 : 
                StartCoroutine(RockShot());
                break;
            case 4 : 
                StartCoroutine(Taunt());
                break;
        }
    }

    IEnumerator MissileShot(){
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.25f);
        GameObject missileA = obj.CreateObj("BossMissile");
        missileA.transform.position = missilePortA.transform.position;
        BossMissile bossMissileA = missileA.GetComponent<BossMissile>();
        bossMissileA.target = target;
        yield return new WaitForSeconds(0.25f);
        GameObject missileB = obj.CreateObj("BossMissile");
        missileB.transform.position = missilePortB.transform.position;
        BossMissile bossMissileB = missileB.GetComponent<BossMissile>();
        bossMissileB.target = target;
        yield return new WaitForSeconds(2f);
        StartCoroutine(Think());
    }
    IEnumerator RockShot(){
        isLook = false;
        anim.SetTrigger("doBigShot");
        yield return new WaitForSeconds(0.5f);
        GameObject rock = obj.CreateObj("BossRock");
        rock.transform.position = transform.position;
        rock.transform.rotation = transform.rotation;
        yield return new WaitForSeconds(2.5f);
        isLook = true;
        StartCoroutine(Think());
    }
    IEnumerator Taunt(){
        tauntVec = target.position + lookVec;
        isLook = false;
        nav.isStopped = false;
        boxCol.enabled = false;
        anim.SetTrigger("doTaunt");
        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(1f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(3f);
        isLook = true;
        nav.isStopped = true;
        boxCol.enabled = true;
        
        StartCoroutine(Think());
    }
}
