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
        //���� ���۽� AP �ʱ�ȭ
        GameManager.Instance.players[0].NowAp = 0;
        GameManager.Instance.players[1].NowAp = 0;
        GameManager.Instance.players[2].NowAp = 0;

        speeds[0] = GameManager.Instance.players[0].UnitAp;
        speeds[1] = GameManager.Instance.players[1].UnitAp;
        speeds[2] = GameManager.Instance.players[2].UnitAp;
        //�÷��̾� ȭ��ǥ
        players[0] = GameObject.Find("Player1").transform.Find("Player1Arrow").gameObject;
        players[1] = GameObject.Find("Player2").transform.Find("Player2Arrow").gameObject;
        players[2] = GameObject.Find("Player3").transform.Find("Player3Arrow").gameObject;
        //

        //���ݹ�ư ���� �÷��̾� �̸�
        text = GameObject.FindWithTag("PlayerName");
        text.GetComponent<TextMeshProUGUI>().SetText(players[GameManager.Instance.prevCharacter].transform.parent.name);
        //

        //÷���� �� ����
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

        //�ൿ�� �÷��̾� �̹���
        tr[0] = GameObject.Find("APImage").transform.Find("PlayerImage1").GetComponent<RectTransform>();
        tr[1] = GameObject.Find("APImage").transform.Find("PlayerImage2").GetComponent<RectTransform>();
        tr[2] = GameObject.Find("APImage").transform.Find("PlayerImage3").GetComponent<RectTransform>();
        //



        //�÷��̾� �̹��� �����ϱ�
        player_image[0] = GameManager.Instance.players[0].GetPlayerImage();
        player_image[1] = GameManager.Instance.players[1].GetPlayerImage();
        player_image[2] = GameManager.Instance.players[2].GetPlayerImage();
        //

        //�÷��̾� �̹��� �ֱ�
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

    //ĳ���ʹ� �� �ӵ��� �°� ������
    //���� ���������� ���õ� �� 

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
                    tr[s].pivot -= new Vector2(0, -(speeds[s] / 100)); //�̹����� �������� �ӵ� unitAp/100
                    GameManager.Instance.players[s].NowAp += speeds[s]; //�÷��̾��� ������������ �ൿ�� ����
                    if (GameManager.Instance.players[s].NowAp >= 850)//�ൿ���� 850�̰� �� ������ �̹����� MaxAP ��ġ��
                    {
                        if (GameManager.Instance.prevCharacter == s) //�������ε����� ���� s�� ������
                        {
                            tr[s].pivot += new Vector2(0, -(speeds[s] / 100)); //�������� ���� �����༭ �� �ڸ��� ����
                            GameManager.Instance.players[s].NowAp -= speeds[s]; // AP�� �� �̻� ���°��� ����
                            GameObject.FindObjectOfType<ButtonHandler>().BtnEnable(); //��ư Ȱ��ȭ
                            SkillNameSet(s); //��ų�̸��ٲ��ֱ�
                            if (!GameObject.FindObjectOfType<ButtonHandler>().isClick)
                            {
                                continue; //��ư�ڵ鷯�� isClick�� false�̸� �Ʒ� �ڵ� �������� �ʰ� �ٽ� �ݺ�������
                            }
                            GameObject.FindObjectOfType<ButtonHandler>().isClick = false; //��ư �ڵ鷯���� ��ư�� ������ isClick�� true�� �Ǽ� ������ ���� ������ false�� ������༭ ��� ���ݹ���
                            Debug.Log("������ �ߴ�/////////////");
                            GameObject.FindObjectOfType<ButtonHandler>().BtnDisable(); //��ư ��Ȱ��ȭ
                            tr[s].pivot = new Vector2(0.5f, 0.53f); //���� �� �ʱ�ȭ
                            GameManager.Instance.players[s].NowAp = 0; // ���� ���� �ൿ�� �ʱ�ȭ
                            continue; // �ؿ� ���� �������� ���� �ι� ���� ����
                        }
                        //selectIndex�� s�� ���� ������ �� ���� ����
                        GameManager.Instance.players[s].PlayerAttack(); //���õ��� ���� �÷��̾��� ���� ���
                        GameManager.Instance.players[s].isAttack = true;

                        tr[s].pivot = new Vector2(0.5f, 0.53f); //���� �� ��ġ �ʱ�ȭ
                        GameManager.Instance.players[s].NowAp = 0; //���õ��� ���� �÷��̾��� �ൿ�� �ʱ�ȭ
                        GameObject.FindObjectOfType<ButtonHandler>().BtnDisable();// �÷��̾���� ����
                    }
                }
            }
        }
        //���� �ڵ����� ��Ŀ���� �ڵ�
        //���� sp�� �������� ���
        /////////////////////////////////////////////////
        enemyMonster.NowAp += enemyMonster.UnitAp;//�� Ap �ǽð� ���Բ�
        if (enemyMonster.NowAp >= 880 && enemyMonster.isDeath == false)//������ ���� �ð�
        {
            switch (Random.Range(1, 11))//���� 3�����߿��� ����
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
                        skillName.GetComponent<TextMeshProUGUI>().text = "���� ��ų : " + enemyMonster.GetSkill(0).skillName;
                        enemyMonster.SkillUse(0, enemyMonster);//����ų
                        enemyMonster.Sp -= 10;
                    }
                    break;
                case 8:
                case 9:
                case 10:
                    if (enemyMonster.Sp >= 10)
                    {
                        skillName.GetComponent<TextMeshProUGUI>().text = "���� ��ų : " + enemyMonster.GetSkill(1).skillName;
                        enemyMonster.SkillUse(1, enemyMonster);//���ѽ�ų
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

        for (int i = 0; i < 3; i++)  //for�� ���鼭 hp�� 0�� ĳ���� üũ
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

            players[playerindex].SetActive(false); //���� �ε����� �÷��̾� ȭ��ǥ ����
            if (count == 2)
                playerindex++;
            else
                playerindex += count;

            if (playerindex >= 3) //���� playerindex�� 3�̻��̸� 0���� �ʱ�ȭ
                playerindex = playerindex % 3; //3���� ���� �������� ����
            //
            if (GameManager.Instance.players[playerindex].GetComponent<Player>().Hp <= 0) //���� �ε��� �÷��̾��� hp�� 0�̸�
            {
                playerindex++;
                if (playerindex >= 3)
                    playerindex %= 3;
                players[playerindex].SetActive(true); //�� �ε����� �÷��̾� ȭ��ǥ ����
            }
            else
            {
                players[playerindex].SetActive(true); //0 �̻��̸� �׳� ���ֱ�
            }

            //��ư ������ � �÷��̾ ���õǾ����� ǥ��
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

    //���Ϳ� �÷��̾ �������� ������ �����̴� value���� �ٲ���
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
        //�ִ� ü��, �ִ� SP����
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