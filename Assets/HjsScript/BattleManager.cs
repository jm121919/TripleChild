using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    Vector3 prevPos;
    Quaternion prevRotate;
    int prevIndex;
    float fov;
    GameObject[] player;
    public Transform battleCameraTransform;
    public static BattleManager instance;
    bool isSelect = false;
    int nowIndex;
    public bool isSpawn = false;
    public static BattleManager Instance
    {
        get
        {
            if (instance == null)
            {
                BattleManager searchBM = FindObjectOfType<BattleManager>();
                instance = searchBM;
                if (instance != null)
                {
                    Debug.Log("error");
                }
            }
            return instance;
        }
    }
    void Start()
    {
        if (instance == null)// 싱글톤
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else//모든경우로 해야댐 아닐때
        {
            if (instance == this)
                Destroy(gameObject);
        }
        prevIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        if (prevIndex != SceneManager.GetActiveScene().buildIndex)
        {
            GameManager.Instance.isBattle = true;
            prevIndex = SceneManager.GetActiveScene().buildIndex;
            if (!isSpawn)
            {
                characterSpawn();
                isSpawn = true;
            }

        }
        if (scene.name == "BattleScene" && GameManager.Instance.isBattle == false)// 배틀에서 메인게임 돌아가기
        {
            GameManager.Instance.goldFind = false;
            SceneManager.LoadScene("MainGame");
        }
        if (scene.name == "BattleScene")
        {
            if(GameManager.Instance.foundEnemy != null)
            {
                if (GameManager.Instance.foundEnemy.GetComponent<Monster>() != null)
                {
                    Vector3 targetPlayerPosition = new Vector3(GameManager.Instance.playerObject[GameManager.Instance.prevCharacter].transform.position.x, transform.position.y, GameManager.Instance.playerObject[GameManager.Instance.prevCharacter].transform.position.z);
                    GameManager.Instance.foundEnemy.transform.LookAt(targetPlayerPosition);
                }
            }            
        }
    }
    private void characterSpawn()
    {
        nowIndex = GameManager.Instance.prevCharacter;
        for (int i = 0; i < 3; i++)
        {
            if (i == nowIndex)
            {

            }
            else
            {
                GameManager.Instance.playerObject[i].SetActive(true);
                if (!isSelect)
                {
                    Vector3 tempPos;
                    tempPos = GameManager.Instance.playerObject[nowIndex].transform.TransformPoint(Vector3.right * 3);
                    GameManager.Instance.playerObject[i].transform.position = tempPos;
                    if (GameManager.Instance.foundEnemy != null)
                    {
                        Vector3 tagertMonsterPosition = new Vector3(GameManager.Instance.foundEnemy.transform.position.x, GameManager.Instance.playerObject[i].transform.position.y, GameManager.Instance.foundEnemy.transform.position.z);
                        GameManager.Instance.playerObject[i].transform.LookAt(tagertMonsterPosition);
                    }
                    isSelect = true;
                    GameManager.Instance.playerObject[i].tag = "Untagged";
                }
                else
                {
                    Vector3 tempPos;
                    tempPos = GameManager.Instance.playerObject[nowIndex].transform.TransformPoint(Vector3.left * 3);
                    GameManager.Instance.playerObject[i].transform.position = tempPos;
                    if (GameManager.Instance.foundEnemy != null)
                    {
                        Vector3 tagertMonsterPosition = new Vector3(GameManager.Instance.foundEnemy.transform.position.x, GameManager.Instance.playerObject[i].transform.position.y, GameManager.Instance.foundEnemy.transform.position.z);
                        GameManager.Instance.playerObject[i].transform.LookAt(tagertMonsterPosition);
                    }
                    GameManager.Instance.playerObject[i].tag = "Untagged";
                    isSelect = false;
                }
            }
        }
    }
    public void BattleFinish()
    {
        for (int i = 0; i < 3; i++)
        {
            GameManager.Instance.players[i].isAttack = false;
            if (prevIndex == i)
            {
                GameManager.Instance.playerObject[i].tag = "Player";
            }
            else
            {
                GameManager.Instance.playerObject[i].tag = "Untagged";
            }
        }
        GameManager.Instance.InitPlayerPostion(GameManager.Instance.prevCharacter);
    }
}