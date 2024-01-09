using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Monster
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //StartCoroutine(MoveCo()); 걷는 모션 없어서 꺼놈
    }
    public override void BasicAttack()
    {
        animator.SetTrigger("attack1A");
        GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(this.Atk);//몬스터평타
    }
    public override void SkillUse(int index, Monster enemyMonster)
    {
        switch (index)
        {
            case 0:
                animator.SetTrigger("jump");
                Instantiate(skillEffects[1], GameManager.Instance.players[GameManager.Instance.prevCharacter].transform.position, GameManager.Instance.players[GameManager.Instance.prevCharacter].transform.rotation).transform.rotation = Quaternion.Euler(-90, 0, 0);
                GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(enemyMonster.GetSkill(1).skillDamage);//몬스터스킬2
                Debug.Log(" 적 데미지" + enemyMonster.GetSkill(1).skillDamage + "선택된 캐릭의 체력 : " + GameManager.Instance.players[GameManager.Instance.prevCharacter].Hp);
                break;
            case 1:
                animator.SetTrigger("magic");
                Instantiate(skillEffects[0], transform);
                GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(enemyMonster.GetSkill(0).skillDamage);//몬스터스킬1
                Debug.Log(" 적 데미지" + enemyMonster.GetSkill(0).skillDamage + "선택된 캐릭의 체력 : " + GameManager.Instance.players[GameManager.Instance.prevCharacter].Hp);
                break;
        }
    }
    public override void CustomDieProgress()
    {
        animator.SetTrigger("death");
    }
    // Update is called once per frame
    new void Update()
    {

    }
}