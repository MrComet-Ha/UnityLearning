using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public ObjectManager obj;
    public Vector3 followPos;
    public float followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;

    float maxShotDelay;
    float curShotDelay;
    public int rapid;
    public int maxRapid;
    public float shotSpeed;
    public float maxShotSpeed;

    void Awake()
    {
        parentPos = new Queue<Vector3>();
    }

    void Update()
    {
        maxShotDelay = rapid * Time.deltaTime;
        Watch();
        Follow();
        Reload();
        Fire();
    }

    //Movement
    void Watch(){
        //FIFO
        //Input
        if(!parentPos.Contains(parent.position))
            parentPos.Enqueue(parent.position);

        //Output
        if(parentPos.Count > followDelay)
            followPos = parentPos.Dequeue();
        else if(parentPos.Count < followDelay)
            followPos = parent.position;
    }
    
    void Follow(){
        transform.position = followPos;
    }

    //Shot
    void Fire(){
        if(!Input.GetKey(KeyCode.Space))
            return;
        if(curShotDelay < maxShotDelay)
            return;
        GameObject bullet = obj.CreateObj("FollowerBullet");
        bullet.transform.position = transform.position;
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up*shotSpeed, ForceMode2D.Impulse);
        curShotDelay = 0;
    }
    void Reload(){
          curShotDelay += Time.deltaTime;
    }
}
