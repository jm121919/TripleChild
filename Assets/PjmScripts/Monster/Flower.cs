using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flower : Monster
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        coroutine = MoveCo();
        StartCoroutine(coroutine);
    }
    public override void BasicAttack()
    {
        animator.SetInteger("animation", 3);//모션이 몇개없어서 스킬이랑 겹침 꽃만
        GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(this.Atk);//몬스터평타
    }
    public override void SkillUse(int index, Monster enemyMonster)
    {
        switch (index)
        {
            case 0:
                animator.SetInteger("animation", 3);
                Instantiate(skillEffects[0], GameManager.Instance.players[GameManager.Instance.prevCharacter].transform.position, GameManager.Instance.players[GameManager.Instance.prevCharacter].transform.rotation).transform.rotation = Quaternion.Euler(-90, 0, 0);
                GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(enemyMonster.GetSkill(0).skillDamage);//몬스터스킬1
                Debug.Log(" 적 데미지" + enemyMonster.GetSkill(0).skillDamage + "선택된 캐릭의 체력 : " + GameManager.Instance.players[GameManager.Instance.prevCharacter].Hp);
                break;
            case 1:
                animator.SetInteger("animation", 4);
                Instantiate(skillEffects[1], GameManager.Instance.players[GameManager.Instance.prevCharacter].transform.position, GameManager.Instance.players[GameManager.Instance.prevCharacter].transform.rotation).transform.rotation = Quaternion.Euler(-90, 0, 0);
                GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(enemyMonster.GetSkill(1).skillDamage);//몬스터스킬2
                Debug.Log(" 적 데미지" + enemyMonster.GetSkill(1).skillDamage + "선택된 캐릭의 체력 : " + GameManager.Instance.players[GameManager.Instance.prevCharacter].Hp);
                break;
        }
    }

    public override void CustomDieProgress()
    {
        animator.SetInteger("animation", 6);
    }
    // Update is called once per frame
    new void Update()
    {
        if (SceneManager.GetActiveScene().name == "BattleScene")
        {
            agent.isStopped = true;
            StopCoroutine(coroutine);
        }
    }
}