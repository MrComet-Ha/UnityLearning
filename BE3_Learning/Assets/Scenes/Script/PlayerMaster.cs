using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaster : MonoBehaviour
{
    public GameManager gm;
    public PlayerInteraction pinter;
    public Rigidbody2D rigid;
    public float h;
    public float v;
    public bool hDown;
    public bool hUp;
    public bool vDown;
    public bool vUp;

    //Mobile key setup
    public bool up_down;
    public bool down_down;
    public bool left_down;
    public bool right_down;
    public bool up_up;
    public bool down_up;
    public bool left_up;
    public bool right_up;
    public int up_value;
    public int down_value;
    public int left_value;
    public int right_value;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        rigid=GetComponent<Rigidbody2D>();
        pinter = GetComponent<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        h=gm.isAction ? 0 : Input.GetAxisRaw("Horizontal") + left_value + right_value;
        v=gm.isAction ? 0 : Input.GetAxisRaw("Vertical") + up_value + down_value;
        hDown = gm.isAction ? false : Input.GetButton("Horizontal") || right_down || left_down;
        hUp = gm.isAction ? false : Input.GetButtonUp("Horizontal") || right_up || left_up;
        vDown = gm.isAction ? false : Input.GetButton("Vertical") || up_down || down_down;
        vUp = gm.isAction ? false : Input.GetButtonUp("Vertical") || up_up || down_up;

        left_down = false;
        right_down = false;
        up_down = false;
        down_down = false;
        left_up = false;
        right_up = false;
        up_up = false;
        down_up = false;

    }
}
