using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArrowShot : MonoBehaviour
{
    public Transform attackPosition;
    public Transform targetMonster;
    Vector3 monsterPos;
    private Rigidbody rb;
    public GameObject arrowEffect;
    public GameObject effectTemp;
    public string[] skillNames;
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();

        attackPosition = GameManager.Instance.playerObject[0].transform;
        targetMonster = GameManager.Instance.foundEnemy.transform;
        monsterPos = targetMonster.position;
        arrowEffect = Resources.Load("PlayerWeapon/Effect/ArrowEx") as GameObject;//0번

        skillNames = new string[3];
        for (int i = 0; i < 3; i++)
        {
            skillNames[i] = GameManager.Instance.players[0].Skills[i].skillName;
        }
        if (GameManager.Instance.players[0].currentSkillNum >= 1)
        {
            arrowEffect = Resources.Load("PlayerWeapon/Effect/" + skillNames[GameManager.Instance.players[0].currentSkillNum]) as GameObject;//여기에 숫자
        }

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 arrowPosition = Vector3.zero;

        rb.Move(Vector3.Slerp(transform.position, monsterPos, Time.deltaTime * 7f), attackPosition.rotation);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetMonster.rotation, Time.deltaTime);

        //rb.Move(, Quaternion.identity);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Monster>() != null)
        {
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "MainGame")
            {
                GameManager.Instance.foundEnemy = other.gameObject;
                DontDestroyOnLoad(GameManager.Instance.foundEnemy);
                GameManager.Instance.FadeScene("BattleScene");
            }

            ContactPoint contact = other.contacts[0];
            if (GameManager.Instance.players[0].currentSkillNum >= 1)
            {
                effectTemp = Instantiate(arrowEffect, contact.point, Quaternion.identity);
            }
            other.gameObject.GetComponent<Monster>().TakeDamage(GameManager.Instance.players[GameManager.Instance.prevCharacter].Atk);//몬스터 배틀에서 공격당함
            ArrowDestroy();
        }
        Invoke("EffectDestroy", 1f);
        Invoke("ArrowDestroy", 2f);
    }
    private void ArrowDestroy()
    {
        Destroy(gameObject);
    }
    private void EffectDestroy()
    {
        Destroy(effectTemp);
    }
}
