using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public int prevCharacter;
    public Player[] players; // monobehavior�� ��ӹ����� new�� ����ȵ�.
    public GameObject[] playerObject;
    public int score = 0;
    public Transform playerTransform;
    private static bool isFirstLoad = false;
    public GameObject foundEnemy;
    public bool isBattle;
    public bool getFade;//�����
    public string fadeSceneName;
    public bool windowOn = false;//ui�¿���
    public GameObject goldText;
    public float gold;
    public TextMeshProUGUI goldTempText;
    public bool goldFind;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameManager searchGM = FindObjectOfType<GameManager>();
                instance = searchGM;
                if (instance != null)
                {
                    Debug.Log("error");
                }
            }
            return instance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        goldFind = false;
        gold = 0;
        if (isFirstLoad)
        {
            prevCharacter = 0;
            playerObject[0].transform.position = new Vector3(0, 0, 0);
            playerObject[1].transform.position = new Vector3(0, 0, 0);
            playerObject[2].transform.position = new Vector3(0, 0, 0);
            playerTransform = playerObject[0].transform;//�ü��� �ϴ� �ʱ�ȭ
        }
        if (instance == null)
        {
            instance = this;

            Debug.Log("����");
            isFirstLoad = true;//ù�⵿���� Ȯ��
        }
        else if (instance != this)//����ƽ instance�� ����� ���ӸŴ����� �ڱ��ڽ��� �ƴϸ�
        {
            Destroy(gameObject);
            Debug.Log("����� ����");
        }

        DontDestroyOnLoad(gameObject);
    }
    public void StartGame()//���� �����Ҷ� �ֱ�
    {
        FadeScene("Select");
        Invoke("SetCharacterFalse", 0.5f);
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null)
            {
                playerObject[i] = Instantiate(playerObject[i],new Vector3(0,0,-5),Quaternion.identity);
                players[i] = playerObject[i].GetComponent<Player>();
                playerObject[i].SetActive(true);
                DontDestroyOnLoad(players[i]);
            }
        }
        score += 10;
    }
    public void SetCharacterFalse()
    {
        for (int i = 0; i < players.Length; i++)
        {
            playerObject[i].SetActive(false);
        }
    }
    public void SelectToMain()
    {
        InitPlayerPostion(prevCharacter);
        players[prevCharacter].RevivePosition();
        Invoke("MonsterLoad", 1f);
    }
    public void FadeScene(string sceneName)
    {
        getFade = true;//�����
        fadeSceneName = sceneName;
    }
    public void MonsterLoad()
    {
        StartCoroutine(MonsterSpawnCo());//��ư ������ ������ �����ȉ�
    }
    public void InitPlayerPostion(int num)
    {
        playerTransform = playerObject[prevCharacter].transform;
        switch (num)
        {
            case 0:
                //Debug.Log("�ü�" + num + "��" + "prevCharacter" + prevCharacter);
                if (!isBattle)
                {
                    playerObject[0].SetActive(true);
                    playerObject[1].SetActive(false);
                    playerObject[2].SetActive(false);
                    playerObject[0].transform.position = playerTransform.position;
                    playerObject[0].transform.rotation = playerTransform.rotation;
                }
                playerObject[0].tag = "Player";
                playerObject[1].tag = "Untagged";
                playerObject[2].tag = "Untagged";
                prevCharacter = 0;
                break;
            case 1:
                //Debug.Log("����" + num + "��" + "prevCharacter" + prevCharacter);
                if (!isBattle)
                {
                    playerObject[0].SetActive(false);
                    playerObject[1].SetActive(true);
                    playerObject[2].SetActive(false);
                    playerObject[1].transform.position = playerTransform.position;
                    playerObject[1].transform.rotation = playerTransform.rotation;
                }
                playerObject[0].tag = "Untagged";
                playerObject[1].tag = "Player";
                playerObject[2].tag = "Untagged";
                prevCharacter = 1;
                break;
            case 2:
                //Debug.Log("������" + num + "��" + "prevCharacter" + prevCharacter);
                if (!isBattle)
                {
                    playerObject[0].SetActive(false);
                    playerObject[1].SetActive(false);
                    playerObject[2].SetActive(true);
                    playerObject[2].transform.position = playerTransform.position;
                    playerObject[2].transform.rotation = playerTransform.rotation;
                }
                playerObject[0].tag = "Untagged";
                playerObject[1].tag = "Untagged";
                playerObject[2].tag = "Player";
                prevCharacter = 2;
                break;
        }
    }
    public void SetGoldText()
    {
        goldTempText.SetText("������� : " + gold.ToString());
        Debug.Log(gold);
    }
    void SpawnMonster(string monsterName)
    {
        // ������ ������ �簢�� ������ ���� ��ǥ
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-8, 11), 1.5f, Random.Range(-15, 6));// ��� ���� ������ ���� 3���� �������� ������ ���� �����ϸ� �׺�޽��� �˾Ƽ� �������� ��������
        // ������ ������ �������µ��� �����̳��� �����ɽ�Ʈ�� �ٽý���ҰŰ���
        // ���͸� �����ϰ� ���� ����Ʈ�� ��ġ�� ��ġ
        GameObject monster = Instantiate(Resources.Load<GameObject>("MonsterPrefab/" + monsterName), randomSpawnPosition, Quaternion.Euler(new Vector3(0, Random.Range(0, 181), 0)));

        ////������ ���̸� �����Ͽ� ������ �ٴڿ� ��Ȯ�� ��ġ (�׺�޽����ÿ����� ����)
        //RaycastHit hit;
        // if (Physics.Raycast(monster.transform.position, Vector3.down, out hit))
        // {
        //     float yOffset = hit.distance; // �������� ���ͱ����� �Ÿ�
        //     monster.transform.position -= new Vector3(0f, yOffset, 0f);
        // }
    }
    IEnumerator MonsterSpawnCo()//���� �ʵ巣������ �ڷ�ƾ
    {
        while (true)//���� ���������� ���� ����
        {
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
            if (GameObject.FindGameObjectsWithTag("Monster").Length < 5 && scene.name == "MainGame")//����ʿ�����
            //���͸� �ʵ忡 n���� �� ���� =�� ������ n�����϶� �����̶� n+1������
            {
                switch (Random.Range(1, 4))// ���� ��ȯ Ȯ�� case�÷��� ��������
                {
                    case 1:
                        SpawnMonster("Dragon");
                        break;
                    case 2:
                        SpawnMonster("FlowerDryad");
                        break;
                    case 3:
                        SpawnMonster("RockMonster");
                        break;
                    default:
                        break;
                }
            }
            yield return new WaitForSeconds(1); //���� ��ȯ �ֱ�
        }
    }
    // Update is called once per frame
    void Update()
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "BattleScene" && foundEnemy != null)
        {
            GameManager.Instance.isBattle = true;
        }
        if (scene.name == "MainGame")
        {
            GameManager.Instance.isBattle = false;
            BattleManager.Instance.isSpawn = false;
            InitPlayerPostion(prevCharacter);
            if(goldFind == false)
            {
                goldText = GameObject.Find("GoldCanvas");
                goldTempText = goldText.transform.Find("GoldText").GetComponent<TextMeshProUGUI>();
                goldFind = true;
                GameManager.Instance.SetGoldText();
            }
            
        }
        
    }
}