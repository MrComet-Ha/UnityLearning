using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Variable
    public float speed;
    public int jumpPow;
    public int jumpCount;
    public float dodgeCool;
    public float swapCool;
    public float reloadCool;
    
    //Data
    public int health;
    public int maxHealth;
    bool isDamage;

    //Key
    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;
    bool iDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool fDown;
    bool rDown;
    bool gDown;

    //Jump
    bool isJump;
    int jumpCnt;

    //Dodge
    bool isDodge;
    UnityEngine.Vector3 dodgeVec;

    //Weapon
    public Weapon wp;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    Weapon equipWeapon;
    bool isSwap;

    //Item
    public int ammo;
    public int coin;
    public int hasGre;
    public int maxAmmo;
    public int maxCoin;
    public int maxHasGre;

    //Movement
    UnityEngine.Vector3 moveVec;
    
    //Item
    GameObject nearObject;

    //Attack
    float atkDelay;
    bool isAtkReady = true;
    bool isReload;
    public Camera followCamera;

    //Physics
    bool isBorder;
    
    //Reference
    Animator anim;
    Rigidbody rigid;
    MeshRenderer[] meshs;
    public ObjectManager obj;
    
    
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<MeshRenderer>();
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
        Attack();
        Reload();
        Grenade();
        Turn();
    }

    //Physics

    void FreezeRotation(){
        rigid.angularVelocity = Vector3.zero;
    }

    void StopToWall(){
        Debug.DrawRay(transform.position,moveVec * 5,Color.green);
        isBorder = Physics.Raycast(transform.position,moveVec,5,LayerMask.GetMask("Wall"));
    }
    void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }

    
    //GetInput
    void GetInput(){
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButtonDown("Walk");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interaction");
        rDown = Input.GetButtonDown("Reload");
        fDown = Input.GetButton("Fire1");
        gDown = Input.GetButtonDown("Fire2");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    //Move
    void Move(){
        moveVec = new UnityEngine.Vector3(hAxis,0,vAxis).normalized;
        if(isDodge)
            moveVec = dodgeVec;
        if(isSwap || !isAtkReady || isReload){
            moveVec = UnityEngine.Vector3.zero;
        }
        if(!isBorder)
            transform.position += moveVec * speed * (wDown ? 0.3f : 1f) *(isDodge ? 2f : 1f) * Time.deltaTime;
        anim.SetBool("isRun",moveVec != UnityEngine.Vector3.zero);
        anim.SetBool("isWalk",wDown);
    }
    void Turn(){
        //Keyboard Turn
        transform.LookAt(transform.position + moveVec);

        //Mouse Turn
        if(fDown){
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if(Physics.Raycast(ray, out rayHit, 100)){
                UnityEngine.Vector3 nextVec = rayHit.point - transform.position;
                transform.LookAt(transform.position + nextVec);
            }
        }
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
        if(jDown && moveVec != UnityEngine.Vector3.zero && !isJump && !isDodge && !isSwap && !isReload){
            dodgeVec = moveVec;
            anim.SetTrigger("doDodge");
            isDodge = true;
            CancelInvoke("EndDodge");
            Invoke("EndDodge",dodgeCool);
        }
    }

    void EndDodge(){
        isDodge=false;
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

    //Attack
    void Attack(){
        if(equipWeapon == null)
            return;
        
        atkDelay += Time.deltaTime;
        isAtkReady = equipWeapon.rate < atkDelay;

        if(fDown && isAtkReady && !isDodge && !isSwap && !isReload){
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            atkDelay = 0;
        }
    }

    void Reload(){
        if(equipWeapon == null || equipWeapon.type == Weapon.Type.Melee || ammo == 0 || equipWeapon.curAmmo == equipWeapon.maxAmmo)
            return;
        if(rDown && !isJump && !isDodge && !isSwap && isAtkReady && !isReload){
            anim.SetTrigger("doReload");
            isReload = true;
            Invoke("EndReload",reloadCool);
        }
    
    }
    void EndReload(){
        ammo += equipWeapon.curAmmo;
        equipWeapon.curAmmo = 0;
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }

    void Grenade(){
        if(hasGre == 0)
            return;
        if(gDown && !isReload &&!isSwap){
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if(Physics.Raycast(ray, out rayHit, 100)){
                UnityEngine.Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 12;
            GameObject instGre = obj.CreateObj("ThrowGrenade");
            instGre.transform.position = transform.position;
            Rigidbody rigidGre = instGre.GetComponent<Rigidbody>();
            rigidGre.AddForce(nextVec , ForceMode.Impulse);
            rigidGre.AddTorque(UnityEngine.Vector3.back *30 , ForceMode.Impulse);

            hasGre -= 1;
            grenades[hasGre].SetActive(false);
            }
        }
    }
    //Swap
    void Swap(){
        
        int weaponIndex = -1;
        if(sDown1 && hasWeapons[0]) weaponIndex = 0;
        if(sDown2 && hasWeapons[1]) weaponIndex = 1;
        if(sDown3 && hasWeapons[2]) weaponIndex = 2;
        if(weaponIndex == -1)
                return;       
        if((sDown1 || sDown2 || sDown3) && !isJump && !isDodge && !isSwap && !isReload){
            
            if(equipWeapon == weapons[weaponIndex]){
                return;
            } 
            if(equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);
            anim.SetTrigger("doSwap");
            isSwap = true;
            CancelInvoke("EndSwap");
            Invoke("EndSwap",swapCool);
        }
    }
    void EndSwap(){
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

    void OnTriggerEnter(Collider other)
    {
        //Item
        if(other.tag == "Item"){
            Item item = other.GetComponent<Item>();
            switch(item.type){
                case Item.Type.Ammo :
                    ammo += item.value;
                    if(ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Item.Type.Coin :
                    coin += item.value;
                    if(coin > maxCoin)
                        coin = maxCoin;
                    break;
                case Item.Type.Heart :
                    health += item.value;
                    if(health > maxHealth)
                        health = maxHealth;
                    break;
                case Item.Type.Grenade :
                    grenades[hasGre].SetActive(true);
                    hasGre += item.value;
                    if(hasGre > maxHasGre)
                        hasGre = maxHasGre;
                    break;
            }
            other.gameObject.SetActive(false);
        }
        else if(other.tag == "EnemyBullet"){
            if(!isDamage){
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.dmg;
                if(other.GetComponent<Rigidbody>() != null)
                    other.gameObject.SetActive(false);
                StartCoroutine(OnDamage());
            }
        }
    }

    IEnumerator OnDamage(){
        isDamage = true;
        foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.yellow;
        }

        yield return new WaitForSeconds(0.5f);

        isDamage = false;
        foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.white;
        }
    }

    void OnTriggerExit(Collider other)
    {
        nearObject = null;
    }

}