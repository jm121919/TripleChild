using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public int prevCharacter;
    public Player[] players; // monobehavior를 상속받으면 new를 쓰면안됨.
    public GameObject[] playerObject;
    public int score = 0;
    public Transform playerTransform;
    private static bool isFirstLoad = false;
    public GameObject foundEnemy;
    public bool isBattle;
    public bool getFade;//토요일
    public string fadeSceneName;
    public bool windowOn = false;//ui온오프
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
            playerTransform = playerObject[0].transform;//궁수로 일단 초기화
        }
        if (instance == null)
        {
            instance = this;

            Debug.Log("생성");
            isFirstLoad = true;//첫기동인지 확인
        }
        else if (instance != this)//스태틱 instance에 저장된 게임매니저가 자기자신이 아니면
        {
            Destroy(gameObject);
            Debug.Log("재생성 방지");
        }

        DontDestroyOnLoad(gameObject);
    }
    public void StartGame()//게임 시작할때 넣기
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
        getFade = true;//토요일
        fadeSceneName = sceneName;
    }
    public void MonsterLoad()
    {
        StartCoroutine(MonsterSpawnCo());//버튼 누르기 전까지 생성안됌
    }
    public void InitPlayerPostion(int num)
    {
        playerTransform = playerObject[prevCharacter].transform;
        switch (num)
        {
            case 0:
                //Debug.Log("궁수" + num + "번" + "prevCharacter" + prevCharacter);
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
                //Debug.Log("전사" + num + "번" + "prevCharacter" + prevCharacter);
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
                //Debug.Log("마법사" + num + "번" + "prevCharacter" + prevCharacter);
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
        goldTempText.SetText("보유골드 : " + gold.ToString());
        Debug.Log(gold);
    }
    void SpawnMonster(string monsterName)
    {
        // 마을앞 임의의 사각형 구역의 랜덤 좌표
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-8, 11), 1.5f, Random.Range(-15, 6));// 가운데 값이 생성될 높이 3으로 한이유는 적당히 위에 생성하면 네브메쉬가 알아서 지형으로 고정해줌
        // 하지만 위에서 내려오는듯한 느낌이나서 레이케스트를 다시써야할거같음
        // 몬스터를 생성하고 스폰 포인트의 위치에 배치
        GameObject monster = Instantiate(Resources.Load<GameObject>("MonsterPrefab/" + monsterName), randomSpawnPosition, Quaternion.Euler(new Vector3(0, Random.Range(0, 181), 0)));

        ////몬스터의 높이를 조절하여 지형의 바닥에 정확히 위치 (네브메쉬관련오류로 보류)
        //RaycastHit hit;
        // if (Physics.Raycast(monster.transform.position, Vector3.down, out hit))
        // {
        //     float yOffset = hit.distance; // 지형에서 몬스터까지의 거리
        //     monster.transform.position -= new Vector3(0f, yOffset, 0f);
        // }
    }
    IEnumerator MonsterSpawnCo()//몬스터 필드랜덤생성 코루틴
    {
        while (true)//게임 끝날때까지 몬스터 스폰
        {
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
            if (GameObject.FindGameObjectsWithTag("Monster").Length < 5 && scene.name == "MainGame")//월드맵에서만
            //몬스터를 필드에 n마리 로 조정 =을 붙히면 n마리일때 생성이라 n+1마리됨
            {
                switch (Random.Range(1, 4))// 몬스터 소환 확률 case늘려서 조정가능
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
            yield return new WaitForSeconds(1); //몬스터 소환 주기
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