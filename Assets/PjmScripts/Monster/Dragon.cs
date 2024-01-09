using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Dragon : Monster
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        coroutine = FlyCo();
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    new void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainGame")
        {
            StopCoroutine(coroutine);
        }
    }
    public override void BasicAttack()
    {
        animator.SetTrigger("attack2");
        GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(this.Atk);//몬스터평타
    }
    public override void SkillUse(int index, Monster enemyMonster)
    {
        switch (index)
        {
            case 0:
                animator.SetTrigger("breatheFire");
                GameObject fire = Instantiate(skillEffects[0], GameObject.Find("FirePoint").transform);
                fire.transform.LookAt(GameManager.Instance.players[GameManager.Instance.prevCharacter].transform);
                GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(enemyMonster.GetSkill(0).skillDamage);//몬스터스킬1
                Debug.Log(" 적 데미지" + enemyMonster.GetSkill(0).skillDamage + "선택된 캐릭의 체력 : " + GameManager.Instance.players[GameManager.Instance.prevCharacter].Hp);
                break;
            case 1:
                animator.SetTrigger("attack2");
                Instantiate(skillEffects[1], GameManager.Instance.players[GameManager.Instance.prevCharacter].transform.position, GameManager.Instance.players[GameManager.Instance.prevCharacter].transform.rotation);
                GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(enemyMonster.GetSkill(1).skillDamage);//몬스터스킬2
                Debug.Log(" 적 데미지" + enemyMonster.GetSkill(1).skillDamage + "선택된 캐릭의 체력 : " + GameManager.Instance.players[GameManager.Instance.prevCharacter].Hp);
                break;
        }
    }
    public override void CustomDieProgress()
    {
        animator.SetTrigger("death");
    }
    IEnumerator FlyCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);// 몬스터 생성하고 기다림, 시간 양보 그래야 생성이 정상적좌표로댐
            agent.baseOffset = 3f;
            animator.SetTrigger("idleTakeoff");
            yield return new WaitForSeconds(5);// 날고있는시간
            agent.baseOffset = 0;//지면으로
            animator.SetTrigger("idleLand");
        }
    }
}