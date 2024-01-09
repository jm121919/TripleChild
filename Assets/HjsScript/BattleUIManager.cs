using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BattleUIManager : MonoBehaviour
{
    public GameObject[] players;
    GameObject text;
    public RectTransform[] tr;
    public float[] speeds;
    public int selectIndex;
    GameObject[] buttons;
    int activeIndex = 0;
    GameObject status;
    public Slider[] monster_slider;
    public Monster enemyMonster;
    public Sprite[] player_image;
    public Slider[] players_Hp;
    public Slider[] players_Sp;
    public GameObject skillName;
    public int playerindex;
    int count;
    bool isLoad;

    void Start()
    {
        playerindex = GameManager.Instance.prevCharacter;
        selectIndex = GameManager.Instance.prevCharacter;
        players = new GameObject[3];
        tr = new RectTransform[3];
        speeds = new float[3];
        buttons = new GameObject[2];
        monster_slider = new Slider[2];
        player_image = new Sprite[3];
        enemyMonster = GameManager.Instance.foundEnemy.GetComponent<Monster>();

        GameManager.Instance.foundEnemy.GetComponent<Monster>().uiPrafab.SetActive(true);
        //전투 시작시 AP 초기화
        GameManager.Instance.players[0].NowAp = 0;
        GameManager.Instance.players[1].NowAp = 0;
        GameManager.Instance.players[2].NowAp = 0;

        speeds[0] = GameManager.Instance.players[0].UnitAp;
        speeds[1] = GameManager.Instance.players[1].UnitAp;
        speeds[2] = GameManager.Instance.players[2].UnitAp;
        //플레이어 화살표
        players[0] = GameObject.Find("Player1").transform.Find("Player1Arrow").gameObject;
        players[1] = GameObject.Find("Player2").transform.Find("Player2Arrow").gameObject;
        players[2] = GameObject.Find("Player3").transform.Find("Player3Arrow").gameObject;
        //

        //공격버튼 위에 플레이어 이름
        text = GameObject.FindWithTag("PlayerName");
        text.GetComponent<TextMeshProUGUI>().SetText(players[GameManager.Instance.prevCharacter].transform.parent.name);
        //

        //첨에는 다 끄기
        for (int i = 0; i < players.Length; i++)
        {
            if (i == GameManager.Instance.prevCharacter)
            {
                players[i].SetActive(true);
            }
            else
            {
                players[i].SetActive(false);
            }
        }
        //

        //행동력 플레이어 이미지
        tr[0] = GameObject.Find("APImage").transform.Find("PlayerImage1").GetComponent<RectTransform>();
        tr[1] = GameObject.Find("APImage").transform.Find("PlayerImage2").GetComponent<RectTransform>();
        tr[2] = GameObject.Find("APImage").transform.Find("PlayerImage3").GetComponent<RectTransform>();
        //



        //플레이어 이미지 저장하기
        player_image[0] = GameManager.Instance.players[0].GetPlayerImage();
        player_image[1] = GameManager.Instance.players[1].GetPlayerImage();
        player_image[2] = GameManager.Instance.players[2].GetPlayerImage();
        //

        //플레이어 이미지 넣기
        GameObject.Find("APImage").transform.Find("PlayerImage1").GetComponent<Image>().sprite = player_image[0];
        GameObject.Find("APImage").transform.Find("PlayerImage2").GetComponent<Image>().sprite = player_image[1];
        GameObject.Find("APImage").transform.Find("PlayerImage3").GetComponent<Image>().sprite = player_image[2];
        //

        GameObject.FindObjectOfType<ButtonHandler>().BtnDisable();

        monster_slider[0] = GameManager.Instance.foundEnemy.GetComponent<Monster>().uiPrafab.transform.Find("MonsterStatus").transform.Find("HpBar").GetComponent<Slider>();
        monster_slider[1] = GameManager.Instance.foundEnemy.GetComponent<Monster>().uiPrafab.transform.Find("MonsterStatus").transform.Find("SpBar").GetComponent<Slider>();
        SettingStatus();

        PlayerSetting();
        isLoad = false;
    }

    //캐릭터는 각 속도에 맞게 내려감
    //내가 탭을누르면 선택된 놈만 

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(GameManager.Instance.prevCharacter);
        if(enemyMonster.isDeath == false)
        {
            for (int s = 0; s < tr.Length; s++)
            {
                if (GameManager.Instance.players[s].GetComponent<Player>().Hp > 0)
                {
                    tr[s].pivot -= new Vector2(0, -(speeds[s] / 100)); //이미지가 떨어지는 속도 unitAp/100
                    GameManager.Instance.players[s].NowAp += speeds[s]; //플레이어의 전투씬에서의 행동력 증가
                    if (GameManager.Instance.players[s].NowAp >= 850)//행동력이 850이고 이 지점이 이미지도 MaxAP 위치임
                    {
                        if (GameManager.Instance.prevCharacter == s) //선택한인덱스가 현재 s와 같으면
                        {
                            tr[s].pivot += new Vector2(0, -(speeds[s] / 100)); //떨어지는 값을 더해줘서 그 자리에 멈춤
                            GameManager.Instance.players[s].NowAp -= speeds[s]; // AP도 그 이상 차는것을 방지
                            GameObject.FindObjectOfType<ButtonHandler>().BtnEnable(); //버튼 활성화
                            SkillNameSet(s); //스킬이름바꿔주기
                            if (!GameObject.FindObjectOfType<ButtonHandler>().isClick)
                            {
                                continue; //버튼핸들러의 isClick이 false이면 아래 코드 실행하지 않고 다시 반복문으로
                            }
                            GameObject.FindObjectOfType<ButtonHandler>().isClick = false; //버튼 핸들러에서 버튼을 누르면 isClick이 true가 되서 공격이 나감 공격후 false로 만들어줘서 계속 공격방지
                            Debug.Log("공격을 했다/////////////");
                            GameObject.FindObjectOfType<ButtonHandler>().BtnDisable(); //버튼 비활성화
                            tr[s].pivot = new Vector2(0.5f, 0.53f); //공격 후 초기화
                            GameManager.Instance.players[s].NowAp = 0; // 공격 이후 행동력 초기화
                            continue; // 밑에 구문 실행하지 않음 두번 공격 방지
                        }
                        //selectIndex와 s가 같지 않으면 이 구문 실행
                        GameManager.Instance.players[s].PlayerAttack(); //선택되지 않은 플레이어의 공격 모션
                        GameManager.Instance.players[s].isAttack = true;

                        tr[s].pivot = new Vector2(0.5f, 0.53f); //공격 후 위치 초기화
                        GameManager.Instance.players[s].NowAp = 0; //선택되지 않은 플레이어의 행동력 초기화
                        GameObject.FindObjectOfType<ButtonHandler>().BtnDisable();// 플레이어들의 공격
                    }
                }
            }
        }
        //몬스터 자동공격 메커니즘 코드
        //몬스터 sp와 연동할지 고민
        /////////////////////////////////////////////////
        enemyMonster.NowAp += enemyMonster.UnitAp;//적 Ap 실시간 차게끔
        if (enemyMonster.NowAp >= 880 && enemyMonster.isDeath == false)//원까지 가는 시간
        {
            switch (Random.Range(1, 11))//공격 3가지중에서 랜덤
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    enemyMonster.BasicAttack();
                    break;
                case 6:
                case 7:
                    if (enemyMonster.Sp >= 10) 
                    {
                        skillName.GetComponent<TextMeshProUGUI>().text = "몬스터 스킬 : " + enemyMonster.GetSkill(0).skillName;
                        enemyMonster.SkillUse(0, enemyMonster);//썐스킬
                        enemyMonster.Sp -= 10;
                    }
                    break;
                case 8:
                case 9:
                case 10:
                    if (enemyMonster.Sp >= 10)
                    {
                        skillName.GetComponent<TextMeshProUGUI>().text = "몬스터 스킬 : " + enemyMonster.GetSkill(1).skillName;
                        enemyMonster.SkillUse(1, enemyMonster);//약한스킬
                        enemyMonster.Sp -= 10;
                    }
                    break;
            }
            GameManager.Instance.players[GameManager.Instance.prevCharacter].DamagedAnimation();
            enemyMonster.NowAp = 0;
        }

        CheckStatus();
    }

    private void Update()
    {
        count = 1;

        for (int i = 0; i < 3; i++)  //for문 돌면서 hp가 0인 캐릭터 체크
        {
            if (GameManager.Instance.players[i].GetComponent<Player>().Hp <= 0)
                count++;
        }

        if(count >= 4)
        {
            if (!isLoad)
            {
                GameManager.Instance.isBattle = false;
                
                GameManager.Instance.FadeScene("MainGame");
                
                isLoad = true;
            }
            Destroy(GameManager.Instance.foundEnemy);
            
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            players[playerindex].SetActive(false); //현재 인덱스의 플레이어 화살표 끄기
            if (count == 2)
                playerindex++;
            else
                playerindex += count;

            if (playerindex >= 3) //만약 playerindex가 3이상이면 0으로 초기화
                playerindex = playerindex % 3; //3으로 나눈 나머지값 저장
            //
            if (GameManager.Instance.players[playerindex].GetComponent<Player>().Hp <= 0) //현재 인덱스 플레이어의 hp가 0이면
            {
                playerindex++;
                if (playerindex >= 3)
                    playerindex %= 3;
                players[playerindex].SetActive(true); //그 인덱스의 플레이어 화살표 켜줌
            }
            else
            {
                players[playerindex].SetActive(true); //0 이상이면 그냥 켜주기
            }

            //버튼 맨위에 어떤 플레이어가 선택되었는지 표시
            text.GetComponent<TextMeshProUGUI>().SetText(players[playerindex].transform.parent.name);
            GameManager.Instance.InitPlayerPostion(playerindex);

            if (GameObject.FindObjectOfType<ButtonHandler>().skillButton.activeSelf == true)
            {
                GameObject.FindObjectOfType<ButtonHandler>().ReturnSelectButton();
            }
        }
    }
    public void SettingStatus()
    {
        monster_slider[0].maxValue = GameManager.Instance.foundEnemy.GetComponent<Monster>().MaxHp;
        monster_slider[1].maxValue = GameManager.Instance.foundEnemy.GetComponent<Monster>().MaxSp;
    }

    //몬스터와 플레이어가 데미지를 받을때 슬라이더 value값을 바꿔줌
    public void CheckStatus()
    {
        if(GameManager.Instance.foundEnemy != null)
        {
            if (GameManager.Instance.foundEnemy.GetComponent<Monster>() != null)
            {
                monster_slider[0].value = GameManager.Instance.foundEnemy.GetComponent<Monster>().Hp;
                monster_slider[1].value = GameManager.Instance.foundEnemy.GetComponent<Monster>().Sp;
            }
        }

        if (GameManager.Instance.players[0].GetComponent<Player>() != null)
        {
            players_Hp[0].value = GameManager.Instance.players[0].GetComponent<Player>().Hp;
            players_Hp[1].value = GameManager.Instance.players[1].GetComponent<Player>().Hp;
            players_Hp[2].value = GameManager.Instance.players[2].GetComponent<Player>().Hp;

            players_Sp[0].value = GameManager.Instance.players[0].GetComponent<Player>().Sp;
            players_Sp[1].value = GameManager.Instance.players[1].GetComponent<Player>().Sp;
            players_Sp[2].value = GameManager.Instance.players[2].GetComponent<Player>().Sp;
        }
    }

    public void PlayerSetting()
    {
        //최대 체력, 최대 SP설정
        players_Hp[0].maxValue = GameManager.Instance.players[0].GetComponent<Player>().MaxHp;
        players_Hp[1].maxValue = GameManager.Instance.players[1].GetComponent<Player>().MaxHp;
        players_Hp[2].maxValue = GameManager.Instance.players[2].GetComponent<Player>().MaxHp;

        players_Sp[0].maxValue = GameManager.Instance.players[0].GetComponent<Player>().MaxSp;
        players_Sp[1].maxValue = GameManager.Instance.players[1].GetComponent<Player>().MaxSp;
        players_Sp[2].maxValue = GameManager.Instance.players[2].GetComponent<Player>().MaxSp;
    }

    void SkillNameSet(int s)
    {
        GameObject.FindObjectOfType<ButtonHandler>().skillButton.transform.Find("First_Skill").transform.Find("SkillNameFir").
            GetComponent<TextMeshProUGUI>().SetText(GameManager.Instance.players[s].Skills[1].skillName);
        GameObject.FindObjectOfType<ButtonHandler>().skillButton.transform.Find("Second_Skill").transform.Find("SkillNameSec").
           GetComponent<TextMeshProUGUI>().SetText(GameManager.Instance.players[s].Skills[2].skillName);
    }
}