using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerMaster master;
    Vector3 dirVec;
    public GameObject scanObj;
    
    void Start()
    {
        master = GetComponent<PlayerMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if(master.vDown && master.v == 1){
            dirVec = UnityEngine.Vector3.up;
        }
        else if(master.vDown && master.v == -1){
            dirVec = UnityEngine.Vector3.down;
        }
        else if(master.hDown && master.h == -1){
            dirVec = UnityEngine.Vector3.left;
        }
        else if(master.hDown && master.h == 1){
            dirVec = UnityEngine.Vector3.right;
        }
        if(Input.GetKeyDown("space") && scanObj != null){
            master.gm.Action(scanObj);
        }
    }
    void FixedUpdate()
    {
        Debug.DrawRay(master.rigid.position,dirVec,new Color(0,1,0,1),0.7f);
        RaycastHit2D scanRay = Physics2D.Raycast(master.rigid.position,dirVec,0.7f,LayerMask.GetMask("Object"));
        if(scanRay.collider != null){
            scanObj = scanRay.collider.gameObject;
        }
        else
            scanObj = null;
    }
}
