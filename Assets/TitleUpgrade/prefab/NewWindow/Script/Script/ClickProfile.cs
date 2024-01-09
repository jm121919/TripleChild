using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickProfile : MonoBehaviour
{
    public int PrevScene;
    public TextMeshProUGUI NameDetail;
    public TextMeshProUGUI ClassDetail;
    public TextMeshProUGUI HP_Detail;
    public TextMeshProUGUI SP_Detail;
    public TextMeshProUGUI ATK_Deatail;
    public TextMeshProUGUI SPD_Detail;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    public void ClickPro()
    {
        if(GameObject.FindGameObjectWithTag("FirstImage"))
        {
            //GameManager.instance.player[0]
        }
    }

    public void TransforStat(player[num])
    {
        switch[num]
            {
            case 0:
                {
                    GameManager.instance.player[0].hp
                        .....기타 스탯등등
                }
        }
    }
    */
    public void AncherStatView()
    {
        NameDetail.text = GameManager.Instance.players[0].UnitName;
        ClassDetail.text = GameManager.Instance.players[0].myClass.ToString();
        HP_Detail.text = GameManager.Instance.players[0].Hp.ToString()+"/"+ GameManager.Instance.players[0].MaxHp.ToString();
        SP_Detail.text = GameManager.Instance.players[0].Sp.ToString()+"/"+ GameManager.Instance.players[0].MaxSp.ToString();
        ATK_Deatail.text = GameManager.Instance.players[0].Atk.ToString();
        SPD_Detail.text = GameManager.Instance.players[0].UnitAp.ToString();
    }

    public void WarriorStatView()
    {
        NameDetail.text = GameManager.Instance.players[1].UnitName;
        ClassDetail.text = GameManager.Instance.players[1].myClass.ToString();
        HP_Detail.text = GameManager.Instance.players[1].Hp.ToString() + "/" + GameManager.Instance.players[1].MaxHp.ToString();
        SP_Detail.text = GameManager.Instance.players[1].Sp.ToString() + "/" + GameManager.Instance.players[1].MaxSp.ToString();
        ATK_Deatail.text = GameManager.Instance.players[1].Atk.ToString();
        SPD_Detail.text = GameManager.Instance.players[1].UnitAp.ToString();
    }
    public void WizzardStatView()
    {
        NameDetail.text = GameManager.Instance.players[2].UnitName;
        ClassDetail.text = GameManager.Instance.players[2].myClass.ToString();
        HP_Detail.text = GameManager.Instance.players[2].Hp.ToString() + "/" + GameManager.Instance.players[2].MaxHp.ToString();
        SP_Detail.text = GameManager.Instance.players[2].Sp.ToString() + "/" + GameManager.Instance.players[2].MaxSp.ToString();
        ATK_Deatail.text = GameManager.Instance.players[2].Atk.ToString();
        SPD_Detail.text = GameManager.Instance.players[2].UnitAp.ToString();
    }
    /*public void GoWorldScene()
    {
        SceneManager.LoadScene("WorldScene");
       
    }*/
    public void GoBacktothePrevScene()
    {
        
        SceneManager.LoadScene("MainMenu");

    }
}
