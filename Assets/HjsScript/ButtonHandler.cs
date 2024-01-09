using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    Button[] buttons;
    public bool isClick;
    public GameObject selectButton;
    public GameObject attackButton;
    public GameObject skillSelectButton;
    public GameObject runButton;
    public GameObject skillButton;

    bool isuse;

    private void Start()
    {
        isuse = false;
        buttons = new Button[3];

        buttons[0] = attackButton.GetComponent<Button>();
        buttons[1] = skillSelectButton.GetComponent<Button>();
        buttons[2] = runButton.GetComponent<Button>();

        buttons[0].onClick.AddListener(OnActions);
        buttons[1].onClick.AddListener(ShowSkillButton);
    }

    void OnActions()
    {
        GameManager.Instance.players[GameManager.Instance.prevCharacter].currentSkillNum = 0;
        Invoke("AttackOn", 1f);
        isClick = true;
        if (GameManager.Instance.prevCharacter == 2)//마법사
        {
            ThirdPersonCamera.attackCamera = 2;//
        }
        else//궁수 전사
        {
            ThirdPersonCamera.attackCamera = 3;//
        }
        Invoke("BattleCameraoff", 2.5f);
    }


    void ShowSkillButton()
    {
        ThirdPersonCamera.attackCamera = 1;//선택카메라
        GameManager.Instance.players[GameManager.Instance.prevCharacter].auraSelect = GameManager.Instance.prevCharacter;
        selectButton.SetActive(false);
        skillButton.SetActive(true);
        skillButton.transform.Find("First_Skill").gameObject.SetActive(true);
        skillButton.transform.Find("First_Skill").GetComponent<Button>().onClick.AddListener(FirstSkillUse);
        skillButton.transform.Find("Second_Skill").gameObject.SetActive(true);
        skillButton.transform.Find("Second_Skill").GetComponent<Button>().onClick.AddListener(SecondSkillUse);

    }

    public void FirstSkillUse()
    {
        if(isuse == false)
        {
            GameManager.Instance.players[GameManager.Instance.prevCharacter].auraSelect = 3;//초기화
            GameManager.Instance.players[GameManager.Instance.prevCharacter].currentSkillNum = 1;
            PlayerSkill[] tempskill = GameManager.Instance.players[GameManager.Instance.prevCharacter].transform.Find("Skills").gameObject.GetComponents<PlayerSkill>();
            GameManager.Instance.players[GameManager.Instance.prevCharacter].Sp -= tempskill[GameManager.Instance.players[GameManager.Instance.prevCharacter].currentSkillNum].EssentialSp;
            Debug.Log("스킬사용");
            Invoke("AttackOn", 1f);
            if (GameManager.Instance.prevCharacter == 2)//마법사
            {
                ThirdPersonCamera.attackCamera = 2;//
            }
            else//궁수 전사
            {
                ThirdPersonCamera.attackCamera = 3;//
            }
            Invoke("BattleCameraoff", 2.5f);
            isClick = true;
            ReturnSelectButton();
            isuse = true;
        }
    }
    public void SecondSkillUse()
    {
        if (isuse == false)
        {
            GameManager.Instance.players[GameManager.Instance.prevCharacter].auraSelect = 3;//초기화
            GameManager.Instance.players[GameManager.Instance.prevCharacter].currentSkillNum = 2;
            PlayerSkill[] tempskill = GameManager.Instance.players[GameManager.Instance.prevCharacter].transform.Find("Skills").gameObject.GetComponents<PlayerSkill>();
            GameManager.Instance.players[GameManager.Instance.prevCharacter].Sp -= tempskill[GameManager.Instance.players[GameManager.Instance.prevCharacter].currentSkillNum].EssentialSp;
            Debug.Log("스킬사용");
            Invoke("AttackOn", 1f);
            if (GameManager.Instance.prevCharacter == 2)//마법사
            {
                ThirdPersonCamera.attackCamera = 2;//
            }
            else//궁수 전사
            {
                ThirdPersonCamera.attackCamera = 3;//
            }
            Invoke("BattleCameraoff", 2.5f);
            isClick = true;
            ReturnSelectButton();
            isuse = true;
        }
    }

   public  void ReturnSelectButton()
    {
        BattleCameraoff();
        GameManager.Instance.players[GameManager.Instance.prevCharacter].auraSelect = 3;
        skillButton.SetActive(false);
        selectButton.SetActive(true);
    }

    public void BtnEnable()
    {
        buttons[0].onClick.AddListener(OnActions);
        buttons[1].onClick.AddListener(ShowSkillButton);
        buttons[0].GetComponent<Button>().interactable = true;
        buttons[1].GetComponent<Button>().interactable = true;
        buttons[2].GetComponent<Button>().interactable = true;
    }

    public void BtnDisable()
    {
        buttons[0].GetComponent<Button>().interactable = false;
        buttons[1].GetComponent<Button>().interactable = false;
        buttons[2].GetComponent<Button>().interactable = false;
    }
    void AttackOn()
    {
        isuse = false;
        Debug.Log("애니메이션 캐릭터공격");
        GameManager.Instance.players[GameManager.Instance.prevCharacter].PlayerAttack();
        GameManager.Instance.players[GameManager.Instance.prevCharacter].isAttack = true;
    }
    void BattleCameraoff()
    {
        ThirdPersonCamera.attackCamera = 0;//
    }
}