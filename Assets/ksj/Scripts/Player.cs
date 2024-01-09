using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Unit
{
    public enum characterClasses//캐릭터 직업종류
    {
        archer,
        warrior,
        wizard
    }
    [SerializeField]
    public characterClasses myClass;//직업 선택

    public Sprite unitImage;//setStatus하는 곳에서
    //이미지 resource라던가에서 불러오게하기.
    public override void SetStatus()
    {
        Hp = MaxHp;
        Sp = MaxSp;
        NowAp = 0;//전투에다
    }
    public Sprite GetPlayerImage()
    {
        return unitImage;
    }
    public override void TakeDamage(float damage)
    {
        this.Hp -= damage;
    }
    public override void Die()
    {
        animator.SetBool("IsDie", true);
    }
    public override Skill GetSkill(int index)
    {
        //index번째 스킬 사용
        //index번째 스킬 sp
        //sp - 스킬[num].sp
        return Skills[index];
    }

    private Rigidbody rb;
    protected Animator animator;

    private float x;//x방향값
    private float z;//z방향값
    private float moveSpeed;//이동 속도
    private float runSpeed;//뛰는 이동 속도
    private float rotationSpeed;//방향 바꾸는 속도
    private float jumpSpeed;//점프 속도
    private float gravityForce;//중력
    private float directionMove;//애니메이션 걷는 방향
    public bool isGrounded;//현재 점프할 수 있는지
    public GameObject attackRange;
    private GameObject dashEffect;
    public GameObject arrow;
    public GameObject magic;
    public Transform targetMonster;
    public bool isAttack;
    private Ray ray;
    private RaycastHit hitData;
    public int currentSkillNum;
    public bool dieCheck;
    Vector3 nowPos;//warrior
    public GameObject[] aura;
    GameObject nowAura;
    public int auraSelect;
    public bool isWarriorAttack;
    public bool attok;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        SetStatus();//현재스탯을 맥스스탯으로 채워줌
        isGrounded = true;
        moveSpeed = 4.5f;
        runSpeed = 6f;
        rotationSpeed = 150f;
        jumpSpeed = 7f;
        gravityForce = 9.8f;
        directionMove = 0;
        attackRange = transform.Find("AttackRange").gameObject;//공격범위 오브젝트 찾기 옮겨줘도 됨
        if (myClass != characterClasses.wizard)
        {
            dashEffect = transform.Find("DashEffect").gameObject;
        }
        attackRange.SetActive(false);//처음엔 공격을 안하고 있어야함
        isAttack = false;
        ray = new Ray(transform.position, transform.forward);
        aura = new GameObject[3];
        if (characterClasses.archer == myClass)
        {
            unitImage = Resources.Load<Sprite>("PlayerImage/archer");
            aura[0] = transform.Find("AuraCharge").gameObject;
        }
        if (characterClasses.warrior == myClass)
        {
            unitImage = Resources.Load<Sprite>("PlayerImage/warrior");
            aura[1] = transform.Find("AuraCharge").gameObject;
        }
        if (characterClasses.wizard == myClass)
        {
            unitImage = Resources.Load<Sprite>("PlayerImage/wizzard");
            aura[2] = transform.Find("AuraCharge").gameObject;
        }

        Skills = new Skill[3];
        Skills = transform.Find("Skills").gameObject.GetComponents<Skill>();
        //Debug.Log(Skills[0].skillName + "/" + Skills[1].skillName);
        //currentSkillNum = 1;//기본공격번호
        auraSelect = 3;
        dieCheck = false;
        isWarriorAttack = false;
        attok = false;
    }
    private void FixedUpdate()
    {
        rb.rotation = rb.rotation * Quaternion.Euler(0, x * rotationSpeed * Time.deltaTime, 0);//방향

        if (rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.down * gravityForce);//중력
        }
    }
    // Update is called once per frame
    void Update()
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        if (dieCheck == true && scene.name == "MainGame")
        {
            for (int i = 0; i < 3; i++)
            {
                Revive();
                GameManager.Instance.InitPlayerPostion(0);
            }
            
        }
        if (dieCheck == false)
        {
            if (GameManager.Instance.isBattle == false && isAttack == false)//몬스터찾기
            {
                MonsterFind();
            }
            //----------------------------이동----------------------------// 
            if (GameManager.Instance.isBattle == false)//전투중일때 조작못하게 막아놓음
            {
                PlayerMove();
            }
            if (GameManager.Instance.isBattle == true)
            {
                x = 0;
                z = 0;
            }
            if (animator)
            {
                animator.SetFloat("Speed", x * x + z * z);
                animator.SetFloat("Direction", z, directionMove, Time.deltaTime);
                //----------------------------점프----------------------------// 
                if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.isBattle == false)
                {
                    if (isGrounded)
                    {
                        JumpTo();
                        animator.SetBool("IsJump", true);//점프 위로올라감
                        StartCoroutine("JumpCo");
                    }
                }
                if (animator.GetBool("JumpDown"))
                {
                    StopCoroutine("JumpCo");
                }
                //----------------------------공격----------------------------// 
                

                if (scene.name == "MainGame")
                {
                    if (GameManager.Instance.isBattle == false)//여기 레이로 고치기
                    {
                        if(GameManager.Instance.foundEnemy.GetComponent<Monster>() != null)
                        {
                            if (Input.GetKeyDown(KeyCode.F))//공격테스트
                            {
                                PlayerAttack();//애니메이션
                                isAttack = true;
                            }
                        }
                    }
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.SetBool("IsAttack", false);//어택이끝나면 어택을 할수 있게됨
                    attackRange.SetActive(false);
                    isAttack = false;
                }
                //////////////////////////////구르기/////////////////////////////////
                if (myClass != characterClasses.wizard)
                {
                    if (Input.GetKeyDown(KeyCode.R) && GameManager.Instance.isBattle == false)
                    {
                        animator.SetBool("IsRoll", true);
                        dashEffect.SetActive(true);
                    }
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Rolling") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f)
                    {
                        animator.SetBool("IsRoll", false);
                        dashEffect.SetActive(false);
                    }
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damaged") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.SetBool("IsDamaged", false);
                }
                if (Hp <= 0 && !dieCheck)
                {
                    Die();
                    dieCheck = true;
                }

            }
        }
        if (myClass == characterClasses.archer)
        {
            if (auraSelect == 0)
            {
                aura[0].SetActive(true);
            }
            else
            {
                aura[0].SetActive(false);
            }
        }
        else if (myClass == characterClasses.warrior)
        {
            if (auraSelect == 1)
            {
                aura[1].SetActive(true);
            }
            else
            {
                aura[1].SetActive(false);
            }
        }
        else if (myClass == characterClasses.wizard)
        {
            if (auraSelect == 2)
            {
                aura[2].SetActive(true);
            }
            else
            {
                aura[2].SetActive(false);
            }
        }
    }
    public void PlayerMove()//플레이어 이동
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("IsRun", true);
            float tempSpeed = moveSpeed;
            while (tempSpeed < runSpeed)
            {
                tempSpeed = tempSpeed + Time.deltaTime * 10f;
            }
            transform.Translate(new Vector3(0, 0, z) * tempSpeed * Time.deltaTime); //이동
        }
        else
        {
            animator.SetBool("IsRun", false);
            transform.Translate(new Vector3(0, 0, z) * moveSpeed * Time.deltaTime); //이동
        }

    }
    void JumpTo()//점프 rigidbody작용
    {
        isGrounded = false;//점프못하게 막기

        Vector3 jumpVelocity = Vector3.up * jumpSpeed;

        //rb.velocity = Vector3.zero;
        rb.AddForce(jumpVelocity, ForceMode.Impulse);

        //Debug.Log(jumpVelocity);
    }
    public void PlayerAttack()//공격 애니메이션, 공격범위 생성
    {
        NowAp = 0;

        animator.SetBool("IsAttack", true);
        attackRange.SetActive(true);
        targetMonster = GameManager.Instance.foundEnemy.transform;
        Vector3 tagertMonsterPosition = new Vector3(targetMonster.position.x, transform.position.y, targetMonster.position.z);
        transform.LookAt(tagertMonsterPosition);//화살쏠때 바라보게하기
        if (this.myClass == characterClasses.archer)//아처일때 공격
        {
            if (!isAttack)
            {
                //Debug.Log("화살공격");
                ArrowInit();
            }
        }
        else if (this.myClass == characterClasses.warrior)
        {
            if (GameManager.Instance.isBattle)
            {
                isWarriorAttack = true;
                Vector3 tempPos;

                nowPos = GameManager.Instance.playerObject[1].transform.position;
                Invoke("Slashed", 1f);
                tempPos = GameManager.Instance.foundEnemy.transform.TransformPoint(Vector3.forward * 1.5f);
                //gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, tempPos, Time.deltaTime * 5f);
            }
        }
        else if (this.myClass == characterClasses.wizard)
        {
            if (!isAttack)
            {
                MagicInit();
            }
        }
    }
    private void Slashed()
    {
        WarriorAttack temp = GameManager.Instance.players[1].GetComponent<WarriorAttack>();
        attok = false;
        isWarriorAttack = false;
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, nowPos, Time.deltaTime * 5f);
    }
    private void MonsterFind()
    {
        ray = new Ray(transform.position + Vector3.up, transform.forward);
        if (Physics.Raycast(ray, out hitData))
        {
            GameManager.Instance.foundEnemy = hitData.collider.gameObject;//수정해야되는거
        }
    }
    private void ArrowInit()
    {
        //arrow = Resources.Load<GameObject>("ksj/Prefab/Arrow");
        arrow = Resources.Load("PlayerWeapon/Arrow") as GameObject;

        Instantiate(arrow, attackRange.transform.position + Vector3.forward, gameObject.transform.rotation);
    }//
    private void MagicInit()
    {
        //arrow = Resources.Load<GameObject>("ksj/Prefab/Arrow");
        magic = Resources.Load("PlayerWeapon/Magic") as GameObject;

        Instantiate(magic, attackRange.transform.position + Vector3.forward, gameObject.transform.rotation);
    }//

    public void DamagedAnimation()
    {
        animator.SetBool("IsDamaged", true);

    }
    public void Revive()
    {
        GameManager.Instance.players[0].SetStatus();
        GameManager.Instance.players[1].SetStatus();
        GameManager.Instance.players[2].SetStatus();
        GameManager.Instance.players[0].ReviveSet();
        GameManager.Instance.players[1].ReviveSet();
        GameManager.Instance.players[2].ReviveSet();
        RevivePosition();
        GameManager.Instance.SetGoldText();
    }
    public void ReviveSet()
    {
        animator.SetBool("IsDie", false);
        isAttack = false;
        attackRange.SetActive(false);
        dieCheck = false;
    }
    public void RevivePosition()
    {
        transform.position = new Vector3(7, 0, 10);//부활좌표
    }
    IEnumerator JumpCo()//내려올떄를 찾는 함수
    {
        float jumpVal = 0;
        while (true)
        {
            jumpVal = 0;
            jumpVal = rb.velocity.y;//점프한뒤 y값을 계속
            //Debug.Log(jumpVal);
            yield return null;
            if (jumpVal < 0)//&& animator.GetBool("IsJump")
            {
                animator.SetBool("IsJump", false);//내려오기 시작할때 
                animator.SetBool("JumpDown", true);//down애니메이션 시작
                break;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")//ground에 닿았을때
        {
            isGrounded = true;
            //Debug.Log(isGrounded);
            animator.SetBool("JumpDown", false);//내려오는 애니메이션 끄기
        }
    }
}