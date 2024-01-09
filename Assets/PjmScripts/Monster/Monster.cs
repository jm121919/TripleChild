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
    protected NavMeshAgent agent;//네브메쉬 요원 컴포넌트
    public GameObject uiPrafab;
    public GameObject spawnEffect;
    public GameObject dieEffect;
    public GameObject[] skillEffects;
    public float invokeTime;
    public bool isDeath;
    public IEnumerator coroutine;
    // 함수라인
    private void Awake()
    {
        skills = SkillsTrans.GetComponents<Skill>();
    }
    protected void Start() //오브젝트가 만들어질때 인스펙터에서 자동으로  스킬을 가져와줌, 각몬스터로 상속
    {
        isDeath = false;
        invokeTime = 1f;
        Skills = transform.Find("Skills").gameObject.GetComponents<Skill>(); // 스킬넣을때 배열로 다넣어줌
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
    public IEnumerator MoveCo()//네브메쉬 요원을 이용한 랜덤이동
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        yield return new WaitForSeconds(invokeTime);
        while (true)
        {
            if (scene.name == "MainGame")
            {
                agent.SetDestination(new Vector3(Random.Range(-8, 11), 0, Random.Range(-15, 6)));//스폰되는 위치 안에서만 랜덤이동
                yield return new WaitForSeconds(5);//목적지가는중의 시간 끝나면 새로운 목적지로감
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
                yield return new WaitForSeconds(4);//죽고나서 3초만기다림
                Destroy(gameObject);
                GameManager.Instance.isBattle = false;
                BattleManager.Instance.BattleFinish();
                //StopCoroutine(checkDieCo());// 어차피 삭제되면 루틴이 안돌아감
            }
            yield return null;
        }
    }
    public override void SetStatus()
    {
        Hp = MaxHp;
        Sp = MaxSp;
        NowAp = 0;//전투에다
    }

    public virtual void CustomDieProgress()//템플릿메서드 패턴
    {

    }

    public override void Die()//템플릿메서드 패턴
    {
        isDeath = true;
        CustomDieProgress();
        //죽는 애니메이션 자리 상속으로 넣어줘야댐
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
        Debug.Log("상대방의 공격력 :" + damage + "내 채력 : " + Hp);
    }
    public override Skill GetSkill(int index)
    {
        return Skills[index];
    }
    public virtual void BasicAttack()
    {
        // 애니메이션 상속
        GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(this.Atk);//몬스터평타
    }
    public virtual void SkillUse(int index, Monster enemyMonster)
    {
        switch (index)
        {
            case 0:
                Instantiate(skillEffects[0]);
                GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(enemyMonster.GetSkill(0).skillDamage);//몬스터스킬1
                Debug.Log(" 적 데미지" + enemyMonster.GetSkill(0).skillDamage + "선택된 캐릭의 체력 : " + GameManager.Instance.players[GameManager.Instance.prevCharacter].Hp);
                break;
            case 1:
                Instantiate(skillEffects[1]);
                GameManager.Instance.players[GameManager.Instance.prevCharacter].TakeDamage(enemyMonster.GetSkill(1).skillDamage);//몬스터스킬2
                Debug.Log(" 적 데미지" + enemyMonster.GetSkill(1).skillDamage + "선택된 캐릭의 체력 : " + GameManager.Instance.players[GameManager.Instance.prevCharacter].Hp);
                break;
        }
    }
}