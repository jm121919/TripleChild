using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;



public class Monster : Unit
{
    // Start is called before the first frame update
    public Animator animator;
    protected NavMeshAgent agent;//�׺�޽� ��� ������Ʈ
    public GameObject uiPrafab;
    public GameObject spawnEffect;
    public GameObject dieEffect;
    public GameObject[] skillEffects;
    public float invokeTime;
    public bool isDeath;
    public IEnumerator coroutine;
    // �Լ�����
    private void Awake()
    {
        skills = SkillsTrans.GetComponents<Skill>();
    }
    protected void Start() //������Ʈ�� ��������� �ν����Ϳ��� �ڵ�����  ��ų�� ��������, �����ͷ� ���
    {
        isDeath = false;
        invokeTime = 1f;
        Skills = transform.Find("Skills").gameObject.GetComponents<Skill>(); // ��ų������ �迭�� �ٳ־���
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        SetStatus();
        uiPrafab = Resources.Load<GameObject>("MonsterPrefab/HPSP");
        uiPrafab = Instantiate(uiPrafab, gameObject.transform);
        uiPrafab.GetComponent<RectTransform>().anchoredPosition = new Vector3(-2, 1.5f, 0);
        uiPrafab.SetActive(false);
    }
    protected void Update()
    {

    }
    public IEnumerator MoveCo()//�׺�޽� ����� �̿��� �����̵�
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        yield return new WaitForSeconds(invokeTime);
        while (true)
        {
            if (scene.name == "MainGame")
            {
                agent.SetDestination(new Vector3(Random.Range(-8, 11), 0, Random.Range(-15, 6)));//�����Ǵ� ��ġ �ȿ����� �����̵�
                yield return new WaitForSeconds(5);//�������������� �ð� ������ ���ο� �������ΰ�
            }
            yield return null;
        }
    }
    public IEnumerator checkDieCo()
    {
        while (true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                yield return new WaitForSeconds(4);//�װ��� 3�ʸ���ٸ�
                Destroy(gameObject);
                GameManager.Instance.isBattle = false;
                BattleManager.Instance.BattleFinish();
                //StopCoroutine(checkDieCo());// ������ �����Ǹ� ��ƾ�� �ȵ��ư�
            }
            yield return null;
        }
    }
    public override void SetStatus()
    {
        Hp = MaxHp;
        Sp = MaxSp;
        NowAp = 0;//��������
    }

    public virtual void CustomDieProgress()//���ø��޼��� ����
    {

    }

    public override void Die()//���ø��޼��� ����
    {
        isDeath = true;
        CustomDieProgress();
        //�״� �ִϸ��̼� �ڸ� ������� �־���ߴ�
        StartCoroutine(checkDieCo());
        GameManager.Instance.gold += 10;
    }
    public override void TakeDamage(float damage)
    {
        this.Hp -= damage;
        if (this.Hp <= 0)
        {
            Die();
        }
        Debug.Log("������ ���ݷ� :" + damage + "�� ä�� : " + Hp);
    }
    public override Skill GetSkill(int index)
    {
        return Skills[index];
    }
    public virtual void BasicAttack()
    {
        // �ִϸ��̼� ���
        GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(this.Atk);//������Ÿ
    }
    public virtual void SkillUse(int index, Monster enemyMonster)
    {
        switch (index)
        {
            case 0:
                Instantiate(skillEffects[0]);
                GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(enemyMonster.GetSkill(0).skillDamage);//���ͽ�ų1
                Debug.Log(" �� ������" + enemyMonster.GetSkill(0).skillDamage + "���õ� ĳ���� ü�� : " + GameManager.Instance.players[GameManager.Instance.prevCharacter].Hp);
                break;
            case 1:
                Instantiate(skillEffects[1]);
                GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(enemyMonster.GetSkill(1).skillDamage);//���ͽ�ų2
                Debug.Log(" �� ������" + enemyMonster.GetSkill(1).skillDamage + "���õ� ĳ���� ü�� : " + GameManager.Instance.players[GameManager.Instance.prevCharacter].Hp);
                break;
        }
    }
}