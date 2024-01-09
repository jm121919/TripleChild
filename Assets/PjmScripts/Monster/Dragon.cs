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
        GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(this.Atk);//������Ÿ
    }
    public override void SkillUse(int index, Monster enemyMonster)
    {
        switch (index)
        {
            case 0:
                animator.SetTrigger("breatheFire");
                GameObject fire = Instantiate(skillEffects[0], GameObject.Find("FirePoint").transform);
                fire.transform.LookAt(GameManager.Instance.players[GameManager.Instance.prevCharacter].transform);
                GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(enemyMonster.GetSkill(0).skillDamage);//���ͽ�ų1
                Debug.Log(" �� ������" + enemyMonster.GetSkill(0).skillDamage + "���õ� ĳ���� ü�� : " + GameManager.Instance.players[GameManager.Instance.prevCharacter].Hp);
                break;
            case 1:
                animator.SetTrigger("attack2");
                Instantiate(skillEffects[1], GameManager.Instance.players[GameManager.Instance.prevCharacter].transform.position, GameManager.Instance.players[GameManager.Instance.prevCharacter].transform.rotation);
                GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(enemyMonster.GetSkill(1).skillDamage);//���ͽ�ų2
                Debug.Log(" �� ������" + enemyMonster.GetSkill(1).skillDamage + "���õ� ĳ���� ü�� : " + GameManager.Instance.players[GameManager.Instance.prevCharacter].Hp);
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
            yield return new WaitForSeconds(5);// ���� �����ϰ� ��ٸ�, �ð� �纸 �׷��� ������ ��������ǥ�δ�
            agent.baseOffset = 3f;
            animator.SetTrigger("idleTakeoff");
            yield return new WaitForSeconds(5);// �����ִ½ð�
            agent.baseOffset = 0;//��������
            animator.SetTrigger("idleLand");
        }
    }
}