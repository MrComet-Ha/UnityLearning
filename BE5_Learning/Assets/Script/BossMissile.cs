using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMissile : Bullet
{
    
    public Transform target;
    NavMeshAgent nav;
    
    void OnEnable()
    {
        nav = GetComponent<NavMeshAgent>();
        Invoke("Disable",12f);
    }

    void Update(){
        if(gameObject.activeSelf)
            nav.SetDestination(target.position);
    }
}
