using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Variable
    public float speed;
    public int jumpPow;
    public int jumpCount;
    public int dodgeCool;
    public int swapCool;
    
    //Data
    //Key
    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;
    bool iDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;

    //Jump
    bool isJump;
    int jumpCnt;

    //Dodge
    bool isDodge;
    UnityEngine.Vector3 dodgeVec;

    //Weapon
    public GameObject[] weapons;
    public bool[] hasWeapons;
    GameObject equipWeapon;
    bool isSwap;

    //Movement
    UnityEngine.Vector3 moveVec;
    
    //Item
    GameObject nearObject;
    
    //Reference
    Animator anim;
    Rigidbody rigid;
    
    

    
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        ResetJump();
    }

    //Main Logic
    void Update()
    {
        GetInput();
        Move();
        Jump();
        Dodge();
        Interaction();
        Swap();
        Turn();
    }

    //GetInput
    void GetInput(){
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButtonDown("Walk");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interaction");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    //Movement
    void Move(){
        moveVec = new UnityEngine.Vector3(hAxis,0,vAxis).normalized;
        if(isDodge)
            moveVec = dodgeVec;
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) *(isDodge ? 2f : 1f) * Time.deltaTime;
        anim.SetBool("isRun",moveVec != UnityEngine.Vector3.zero);
        anim.SetBool("isWalk",wDown);
    }
    void Turn(){
        transform.LookAt(transform.position + moveVec);
    }
    //Jump
    void Jump(){
        if(jDown && moveVec == UnityEngine.Vector3.zero && jumpCnt > 0 && !isDodge && !isSwap){
            rigid.AddForce(Vector3.up * jumpPow,ForceMode.Impulse);
            anim.SetBool("isJump",true);
            anim.SetTrigger("doJump");
            isJump = true;
            jumpCnt-=1;
        }
    }
    void ResetJump(){
        anim.SetBool("isJump",false);
        isJump = false;
        jumpCnt = jumpCount;
    }

    //Dodge
    void Dodge(){
        if(jDown && moveVec != UnityEngine.Vector3.zero && !isJump && !isDodge && !isSwap){
            dodgeVec = moveVec;
            anim.SetTrigger("doDodge");
            isDodge = true;
            CancelInvoke("EndDodge");
            Invoke("EndDodge",dodgeCool * Time.deltaTime);
        }
    }
    void EndDodge(){
        ResetJump();
        isDodge = false;
    }

    //Interaction
    void Interaction(){
        if(iDown && nearObject != null && !isJump && !isDodge && !isSwap){
            if(nearObject.tag == "Weapon"){
                Item item = nearObject.GetComponent<Item>();
                int itemIndex = item.value;
                hasWeapons[itemIndex] = true;
                nearObject.SetActive(false);
            }
        }
    }

    void Swap(){
        
        int weaponIndex = -1;
        if(sDown1 && hasWeapons[0]) weaponIndex = 0;
        if(sDown2 && hasWeapons[1]) weaponIndex = 1;
        if(sDown3 && hasWeapons[2]) weaponIndex = 2;
        if(weaponIndex == -1)
                return;       
        if((sDown1 || sDown2 || sDown3) && !isJump && !isDodge && !isSwap){
            
            if(equipWeapon == weapons[weaponIndex]){
                return;
            } 
            if(equipWeapon != null)
                equipWeapon.SetActive(false);
            equipWeapon = weapons[weaponIndex];
            equipWeapon.SetActive(true);
            anim.SetTrigger("doSwap");
            isSwap = true;
            CancelInvoke("SwapCool");
            Invoke("SwapCool",swapCool * Time.deltaTime);
        }
    }
    void SwapCool(){
        isSwap = false;
    }
    //Collision
    //Floor
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Floor"){
            ResetJump();
        }
    }
    
    //Items
    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Weapon"){
            nearObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        nearObject = null;
    }
}
