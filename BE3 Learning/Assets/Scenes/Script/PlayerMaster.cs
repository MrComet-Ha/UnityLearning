using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaster : MonoBehaviour
{
    public GameManager gm;
    public Rigidbody2D rigid;
    public float h;
    public float v;
    public bool hDown;
    public bool hUp;
    public bool vDown;
    public bool vUp;
    
    // Start is called before the first frame update
    void Awake()
    {
        rigid=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        h=gm.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        v=gm.isAction ? 0 : Input.GetAxisRaw("Vertical");
        hDown = gm.isAction ? false : Input.GetButton("Horizontal");
        hUp = gm.isAction ? false : Input.GetButtonUp("Horizontal");
        vDown = gm.isAction ? false : Input.GetButton("Vertical");
        vUp = gm.isAction ? false : Input.GetButtonUp("Vertical");
    }
}
