using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Rigidbody2D rigid;
    public GameObject pl;
    Transform player;
    Vector3 offset;
    
    // Start is called before the first frame update
    void Start()
    {
        player = pl.transform;
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.position + offset;
        if(transform.position.y < -3){
            transform.position = new Vector3(transform.position.x, -3 ,transform.position.z);
        }
    }
}
