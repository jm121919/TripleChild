using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class NpcName : MonoBehaviour
{
    public GameObject setNpcName;
    public GameObject npcText;
    string prevText;
    void Start()
    {
        setNpcName = Resources.Load<GameObject>("MonsterPrefab/NpcNameText");
        setNpcName = Instantiate(setNpcName, gameObject.transform);
        setNpcName.GetComponent<RectTransform>().anchoredPosition = new Vector3(0.1f, 2, 0);

        npcText = Resources.Load<GameObject>("MonsterPrefab/NpcText");
        npcText = Instantiate(npcText, gameObject.transform);
        npcText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0.1f, 2, 0);
        npcText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            StartCoroutine("NpcTextStart");
            if (Input.GetKeyDown(KeyCode.G))
            {
                for(int i = 0; i < 3; i++)
                {
                    GameManager.Instance.players[i].GetComponent<Player>().SetStatus();
                }
            }   
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if(GameManager.Instance.gold >= 10)
                {
                    prevText = npcText.transform.Find("Text").GetComponent<TextMeshProUGUI>().text;
                    npcText.transform.Find("Text").GetComponent<TextMeshProUGUI>().SetText("완료되었네");
                    StartCoroutine("NpcTextStart");
                    for (int i = 0; i < 3; i++)
                    {
                        GameManager.Instance.players[i].SetStatus();
                        GameManager.Instance.players[i].ReviveSet();
                    }
                    GameManager.Instance.gold -= 10;
                    GameManager.Instance.SetGoldText();
                }
                else
                {
                    npcText.transform.Find("Text").GetComponent<TextMeshProUGUI>().SetText("골드가 부족하네..");
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            StartCoroutine("NpcTextStop");
        }
    }
    IEnumerator NpcTextStart()
    {
        npcText.SetActive(true);
        setNpcName.SetActive(false);
        yield return null;
    }

    IEnumerator NpcTextStop()
    {
        npcText.transform.Find("Text").GetComponent<TextMeshProUGUI>().SetText(prevText);
        npcText.SetActive(false);
        setNpcName.SetActive(true);
        yield return null;
    }
}
