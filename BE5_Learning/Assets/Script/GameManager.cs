using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Stage data
    //Score
    public int curScore;
    public int hiScore;
    //Stage Index
    public int stage;
    public float playTime;
    public bool isBattle;
    
    //Enemy
    public List<int> enemyList;
    public string[] enemies;
    public Transform[] spawnPoints;
    public int enemyACnt;
    public int enemyBCnt;
    public int enemyCCnt;
    public int enemyDCnt;

    //Camera,UI
    //Menu
    public GameObject menuCamera;
    public GameObject menuUI;
    public TextMeshProUGUI hiScoreText;
    
    //InGame
    public GameObject gameCamera;
    public GameObject inGameUI;

    //Player
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI hpText;
    public Image wp1Icon;
    public Image wp2Icon;
    public Image wp3Icon;
    public Image wpRIcon;

    //GameOver
    public GameObject overUI;
    public GameObject celebText;
    public TextMeshProUGUI curScoreText;

    //Stage
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI timeText;

    //Enemy
    public TextMeshProUGUI enemyACntText;
    public TextMeshProUGUI enemyBCntText;
    public TextMeshProUGUI enemyCCntText;
    public RectTransform bossHPGroup;
    public RectTransform bossHPBar;
    
    //StageObject
    public GameObject startZone;
    public GameObject itemShop;
    public GameObject weaponShop;

    //BGM
    public AudioSource[] BGMs;

    //Reference
    public ObjectManager obj;
    public Player player;
    public Boss boss;

    //First Loading
    void Awake()
    {
        enemyList = new List<int>();
        LoadScore();
    }

    //Score
    public void AddScore(int score){
        curScore += score;
    }
    void LoadScore(){
        int score;
        if(!PlayerPrefs.HasKey("HiScore")){
            PlayerPrefs.SetInt("HiScore",0);
        }
        score = PlayerPrefs.GetInt("HiScore");
        hiScore = score;
        hiScoreText.text = string.Format("{0:n0}", hiScore);
    }

    //GameStart
    public void GameStart(){
        menuCamera.SetActive(false);
        menuUI.SetActive(false);
        player.gameObject.SetActive(true);
        inGameUI.SetActive(true);
        gameCamera.SetActive(true);
        BGMs[0].gameObject.SetActive(false);
        BGMs[1].gameObject.SetActive(true);
    }

    //GameOver
    public void GameOver(){
        inGameUI.SetActive(false);
        overUI.SetActive(true);

        int hiScore = PlayerPrefs.GetInt("HiScore");
        if(curScore < hiScore){
            PlayerPrefs.SetInt("HiScore",curScore);
            celebText.SetActive(true);
        }
        PlayerPrefs.Save();
    }
    //Restart
    public void Restart(){
        SceneManager.LoadScene(0);
    }
    //Stage Management
    public void StageStart(){
        BGMs[1].gameObject.SetActive(false);
        BGMs[2].gameObject.SetActive(true);
        player.transform.position = Vector3.up * 0.5f;

        isBattle = true;
        startZone.SetActive(false);
        itemShop.SetActive(false);
        weaponShop.SetActive(false);

        foreach(Transform zone in spawnPoints)
            zone.gameObject.SetActive(true);

        StartCoroutine(InBattle());
    }

    void StageEnd(){
        BGMs[1].gameObject.SetActive(true);
        BGMs[2].gameObject.SetActive(false);
        player.transform.position = Vector3.up * 0.5f;

        isBattle = false;
        startZone.SetActive(true);
        itemShop.SetActive(true);
        weaponShop.SetActive(true);

        foreach(Transform zone in spawnPoints)
            zone.gameObject.SetActive(false);

        stage++;
    }

    IEnumerator InBattle(){
        if(stage % 5 == 0){
            enemyDCnt += 1;
            GameObject enemyBoss = obj.CreateObj("EnemyD");
            Enemy enemyLogic = enemyBoss.GetComponent<Enemy>();
            enemyBoss.transform.position = spawnPoints[0].position;
            enemyLogic.target = player.transform;
            enemyLogic.obj = obj;
            enemyLogic.gm = this;
            boss = enemyBoss.GetComponent<Boss>();
        }
        else{
            for(int i = 0; i<stage*3; i++){
            int ran = Random.Range(0,3);
            enemyList.Add(ran);
            }
            while(enemyList.Count > 0){
                int ranZone = Random.Range(0,4);
                Vector3 ranVec =new Vector3(1,0,1) * Random.Range(-5,5);
                GameObject enemy = obj.CreateObj(enemies[enemyList[0]]);
                enemy.transform.position = spawnPoints[ranZone].position + ranVec;
                enemy.transform.rotation = spawnPoints[ranZone].rotation;
                Enemy enemyLogic = enemy.GetComponent<Enemy>();
                enemyLogic.target = player.transform;
                enemyLogic.obj = obj;
                enemyLogic.gm = this;
                switch(enemyList[0]){
                    case 0 : enemyACnt += 1;
                        break;
                    case 1 : enemyBCnt += 1;
                        break;
                    case 2 : enemyCCnt += 1;
                        break;
                }
                enemyList.RemoveAt(0);
                yield return new WaitForSeconds(3f);
            }
            
        }
        while(enemyACnt + enemyBCnt + enemyCCnt + enemyDCnt > 0)
            yield return null;
        
        yield return new WaitForSeconds(4f);
        boss = null;
        StageEnd();
    }
    
    void Update(){
        if(isBattle){
            playTime += Time.deltaTime;
        }
    }

    //UI Update
    void LateUpdate()
    {
        //Stage Index
        scoreText.text = string.Format("{0:n0}",curScore);
        stageText.text = "STAGE " + stage;
        int hour = (int)(playTime / 3600);
        int minute = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime % 60);
        timeText.text = string.Format("{0:00}",hour) + ":" 
            + string.Format("{0:00}",minute) + ":" 
            + string.Format("{0:00}",second);

        //Player Index
        hpText.text = player.health + " / " + player.maxHealth;
        coinText.text = string.Format("{0:n0}",player.coin);
        if(player.equipWeapon == null || player.equipWeapon.type == Weapon.Type.Melee)
            ammoText.text = "- / " + player.ammo;
        else
            ammoText.text = player.equipWeapon.curAmmo + " / " + player.ammo;
        wp1Icon.color = new Color(1,1,1, player.hasWeapons[0] ? 1 : 0);
        wp2Icon.color = new Color(1,1,1, player.hasWeapons[1] ? 1 : 0);
        wp3Icon.color = new Color(1,1,1, player.hasWeapons[2] ? 1 : 0);
        wpRIcon.color = new Color(1,1,1, player.hasGre > 0 ? 1 : 0);

        //Enemy Index
        enemyACntText.text = enemyACnt.ToString();
        enemyBCntText.text = enemyBCnt.ToString();
        enemyCCntText.text = enemyCCnt.ToString();
        //Boss Index
        if(boss != null){
            bossHPGroup.anchoredPosition = Vector3.down * 30;
            bossHPBar.localScale = new Vector3((float)boss.curHealth/boss.maxHealth , 1, 1);
        }
        else{
            bossHPGroup.anchoredPosition = Vector3.up * 100;
        }
    }
}
