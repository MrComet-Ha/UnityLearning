using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float speed;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;
    float viewHeight;
    void Awake()
    {
        viewHeight = (Camera.main.orthographicSize * 2);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        if(transform.position.y<viewHeight*(-1)){
            transform.position = new Vector3(0,viewHeight,0);
        }
    }
}
