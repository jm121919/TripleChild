using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarriorAttack : MonoBehaviour
{
    public GameObject effectTemp;
    public GameObject[] warriorEffect;
    public string[] skillNames;
    void Start()
    {
        skillNames = new string[3];
        warriorEffect = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            skillNames[i] = GameManager.Instance.players[1].Skills[i].skillName;
            //Debug.Log(skillNames[i]);
        }
        warriorEffect[0] = null;
        warriorEffect[1] = Resources.Load("PlayerWeapon/Effect/" + skillNames[1]) as GameObject;//여기에 숫자
        warriorEffect[2] = Resources.Load("PlayerWeapon/Effect/" + skillNames[2]) as GameObject;//여기에 숫자
    }
    private void Update()
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "BattleScene")
        {
            if (GameManager.Instance.players[1].isWarriorAttack && !GameManager.Instance.players[1].attok)
            {
                if (GameManager.Instance.players[1].currentSkillNum >= 1)
                {
                    effectTemp = Instantiate(warriorEffect[GameManager.Instance.players[1].currentSkillNum],GameManager.Instance.foundEnemy.transform.position + Vector3.up*2f ,Quaternion.Euler(new Vector3(90 + 
                        GameManager.Instance.foundEnemy.transform.rotation.x, GameManager.Instance.foundEnemy.transform.rotation.y, -90 + GameManager.Instance.foundEnemy.transform.rotation.z)));
                    
                    GameManager.Instance.players[1].currentSkillNum = 0;
                }
                GameManager.Instance.foundEnemy.GetComponent<Monster>().TakeDamage(GameManager.Instance.players[GameManager.Instance.prevCharacter].Atk);//몬스터 배틀에서 공격당함
                Invoke("EffectDestroy", 1f);
                GameManager.Instance.players[1].attok = true;
                
            }
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(warriorEffect.name + "///////////!!!!!!!!");
        if (other.gameObject.GetComponent<Monster>() != null)
        {
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "MainGame")
            {

                GameManager.Instance.foundEnemy = other.gameObject;
                DontDestroyOnLoad(GameManager.Instance.foundEnemy);

                GameManager.Instance.FadeScene("BattleScene");
            }
        }
        
    }
    private void EffectDestroy()
    {
        Destroy(effectTemp);
    }
}
