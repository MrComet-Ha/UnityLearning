using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Data
    public int life;
    public int maxLife;
    public bool isHit;
    public float shotSpeed;
    public float maxShotSpeed;
    public int power;
    public int maxPower;
    public int rapid;
    public int maxRapid;
    public int bomb;
    public int maxBomb;
    public GameObject[] followers;
    public bool isInvincible;

    //Mobile
    public bool[] control;
    public bool isControl;
    public bool isBtnA;
    public bool isBtnB;

    //Movement
    public float speed;
    public bool isStkLeft;
    public bool isStkRight;
    public bool isStkTop;
    public bool isStkBottom;

    //Bullet
    public float maxShotDelay;
    public float curShotDelay;
    public GameObject BombEffect;
    public bool isBomb;

    //Reference
    public GameManager gm;
    public ObjectManager obj;
    public Animator anim;
    
    public SpriteRenderer spr;


    void Awake()
    {

        gm.UpdateBombIcon();
        gm.UpdateLifeIcon();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        maxShotDelay = rapid * Time.deltaTime;
        Move();
        Reload();
        Fire();
        Bomb();
    }

    //Movement

    public void PanelControl(int type){
        for(int i = 0; i<9;i++){
            control[i] = i == type;
        }
    }
    public void PanelDown(){
        isControl = true;
    }
    public void PanelUp(){
        isControl = false;
    }
    void Move(){
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if(control[0]){h=-1;v=1;}
        if(control[1]){h=0;v=1;}
        if(control[2]){h=1;v=1;}
        if(control[3]){h=-1;v=0;}
        if(control[4]){h=0;v=0;}
        if(control[5]){h=1;v=0;}
        if(control[6]){h=-1;v=-1;}
        if(control[7]){h=0;v=-1;}
        if(control[8]){h=1;v=-1;}

        if((isStkLeft && h == -1 )||(isStkRight && h == 1)||!isControl)
            h = 0;
        if((isStkTop && v == 1)||(isStkBottom && v == -1)||!isControl)
            v = 0;

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h,v,0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if(Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal")){
            anim.SetInteger("Input",(int)h);
        }
    }
 
    //Shot
    public void BtnADown(){
        isBtnA = true;
    }
    public void BtnAUp(){
        isBtnA = false;
    }
    public void BtnB(){
        isBtnA = true;
    }
    void Fire(){
        /*if(!Input.GetKey(KeyCode.Space))
            return;*/
        if(!isBtnA)
            return;
        if(curShotDelay < maxShotDelay)
            return;
        switch(power){
            case 1 : 
                GameObject bullet = obj.CreateObj("PlayerBulletA");
                bullet.transform.position = transform.position;
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up*shotSpeed, ForceMode2D.Impulse);
                break;
            case 2 :
                    GameObject bulletL = obj.CreateObj("PlayerBulletA");
                    bulletL.transform.position = transform.position + Vector3.left * 0.1f;
                    GameObject bulletR = obj.CreateObj("PlayerBulletA"); 
                    bulletR.transform.position = transform.position + Vector3.right * 0.1f;
                    Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                    Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                    rigidL.AddForce(Vector2.up*shotSpeed, ForceMode2D.Impulse);
                    rigidR.AddForce(Vector2.up*shotSpeed, ForceMode2D.Impulse);
                break;
            default :
                GameObject bulletLL = obj.CreateObj("PlayerBulletA");
                GameObject bulletCC = obj.CreateObj("PlayerBulletB"); 
                GameObject bulletRR = obj.CreateObj("PlayerBulletA");
                bulletLL.transform.position = transform.position + Vector3.left * 0.2f;
                bulletCC.transform.position = transform.position;
                bulletRR.transform.position = transform.position + Vector3.right * 0.2f;
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                rigidLL.AddForce(Vector2.up*shotSpeed, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up*shotSpeed, ForceMode2D.Impulse);
                rigidRR.AddForce(Vector2.up*shotSpeed, ForceMode2D.Impulse);
                break;
        }
        curShotDelay = 0;
    }
    void Reload(){
        curShotDelay += Time.deltaTime;
    }
    void Bomb(){
        /*if(!Input.GetKeyDown(KeyCode.LeftShift))
            return;*/
        if(!isBtnB)
            return;
        if(isBomb)
            return;
        if(bomb == 0)
            return;
        bomb --;
        isBomb = true;
        BombEffect.SetActive(true);
        Invoke("BombOff",4f);
        GameObject[] enemiesS = obj.GetPool("EnemyS");
        GameObject[] enemiesM = obj.GetPool("EnemyM");
        GameObject[] enemiesL = obj.GetPool("EnemyL");
        for(int i = 0; i < enemiesS.Length; i++){
            if(enemiesS[i].activeSelf)
            {
                EnemyMovement enemyLogic = enemiesS[i].GetComponent<EnemyMovement>();
                enemyLogic.OnHit(50);
            }
        }
        for(int i = 0; i < enemiesM.Length; i++){
            if(enemiesM[i].activeSelf)
            {
                EnemyMovement enemyLogic = enemiesM[i].GetComponent<EnemyMovement>();
                enemyLogic.OnHit(50);
            }
        }
        for(int i = 0; i < enemiesL.Length; i++){
            if(enemiesL[i].activeSelf)
            {
                EnemyMovement enemyLogic = enemiesL[i].GetComponent<EnemyMovement>();
                enemyLogic.OnHit(50);
            }
        }
        GameObject[] bulletsA = obj.GetPool("EnemyBulletA");
        GameObject[] bulletsB = obj.GetPool("EnemyBulletB");
        GameObject[] bossBulletsA = obj.GetPool("BossBulletA");
        GameObject[] bossBulletsB = obj.GetPool("BossBulletB");
        for(int i = 0; i < bulletsA.Length; i++){
            if(bulletsA[i].activeSelf)
                bulletsA[i].SetActive(false);
        }
        for(int i = 0; i < bulletsB.Length; i++){
            if(bulletsB[i].activeSelf)
                bulletsB[i].SetActive(false);
        }
        for(int i = 0; i < bossBulletsA.Length; i++){
            if(bossBulletsA[i].activeSelf)
                bossBulletsA[i].SetActive(false);
        }
        for(int i = 0; i < bossBulletsB.Length; i++){
            if(bossBulletsB[i].activeSelf)
                bossBulletsB[i].SetActive(false);
        }
        isBtnB=false;
        gm.UpdateBombIcon();
    }
    void BombOff(){
        isBomb = false;
        BombEffect.SetActive(false);
    }
    //Follower
    void AddFollower(){
        if(power == 4){
            followers[0].SetActive(true);
        }
        else if(power == 5){
            followers[1].SetActive(true);
        }
        else if(power == 6){
            followers[2].SetActive(true);
        }
    }
    //Score
    public void AddScore(int score){
        gm.score += score;
    }

    //Invincible
    public void SetInvincible(){
        isInvincible = true;
        InvincibleSpr();
        Invoke("EndInvincible",2);
    }
    public void InvincibleSpr(){
        Color clean = new Color(1,1,1,1);
        Color blank = new Color(1,1,1,0.5f);
        if(!isInvincible){
            spr.color = clean;
            for(int i=0;i<followers.Length;i++){
                if(followers[i].activeSelf){
                    followers[i].GetComponent<SpriteRenderer>().color=clean;
                }
            }
            CancelInvoke("InvincibleSpr");
        }
        else
        {
            if(spr.color == clean){
                spr.color = blank;
                for(int i=0;i<followers.Length;i++){
                if(followers[i].activeSelf){
                    followers[i].GetComponent<SpriteRenderer>().color=blank;
                }
            }
                
            }
            else{
                spr.color = clean;
                for(int i=0;i<followers.Length;i++){
                    if(followers[i].activeSelf){
                        followers[i].GetComponent<SpriteRenderer>().color=clean;
                    }
                }
                Invoke("InvincibleSpr",0.2f);
            }
        }
    }
    void EndInvincible(){
        isInvincible = false;
    }
    //Collision
    void OnTriggerEnter2D(Collider2D other)
    {
        //Border
        if(other.gameObject.tag == "Border"){
            switch(other.gameObject.name){
                case "Left" : 
                    isStkLeft = true;
                    break;
                case "Right" : 
                    isStkRight = true;
                    break;
                case "Top" : 
                    isStkTop = true;
                    break;
                case "Bottom" : 
                    isStkBottom = true;
                    break;
            }
        }
        
        //Enemy
        else if((other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyBullet")&&!isInvincible){
            if (isHit)
                return;
            isHit = true;
            if(other.gameObject.GetComponent<EnemyMovement>() != null)
                other.GetComponent<EnemyMovement>().OnHit(30);  
            else
                other.gameObject.SetActive(false);  
            if(life>0){
                life --;
                gm.PlayerRespawn();
            }
            else{
                gm.GameOver();
            }
            gm.CallExplosion(transform.position,"P");
            gameObject.SetActive(false);
        }
        
        //Item
        else if(other.gameObject.tag == "Item"){
            Item item = other.GetComponent<Item>();
            switch(item.type){
                case "Coin" : 
                    gm.score += 500;
                    break;
                case "Power" :
                    if(power == maxPower)
                        gm.score += 250;
                    else
                        power ++;
                        AddFollower();
                    break;
                case "Rapid" :
                    if(rapid == maxRapid)
                        gm.score += 250;
                    else
                        rapid -=3;
                    break;
                case "Shot" :
                    if(shotSpeed == maxShotSpeed)
                        gm.score += 250;
                    else
                        shotSpeed +=2;
                    break;
                case "Heal" :
                    if(life == maxLife)
                        gm.score += 250;
                    else
                        life ++;
                        gm.UpdateLifeIcon();
                    break;
                case "Bomb" :
                    if(bomb == maxBomb)
                        gm.score += 250;
                    else{
                        bomb += 1;
                        gm.UpdateBombIcon();
                    }
                    break;
            }
            other.gameObject.SetActive(false);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Border"){
            switch(other.gameObject.name){
                case "Left" : 
                    isStkLeft = false;
                    break;
                case "Right" : 
                    isStkRight = false;
                    break;
                case "Top" : 
                    isStkTop = false;
                    break;
                case "Bottom" : 
                    isStkBottom = false;
                    break;
            }
        }
    }
}
