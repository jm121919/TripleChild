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
    public enum characterClasses//ĳ���� ��������
    {
        archer,
        warrior,
        wizard
    }
    [SerializeField]
    public characterClasses myClass;//���� ����

    public Sprite unitImage;//setStatus�ϴ� ������
    //�̹��� resource��������� �ҷ������ϱ�.
    public override void SetStatus()
    {
        Hp = MaxHp;
        Sp = MaxSp;
        NowAp = 0;//��������
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
        //index��° ��ų ���
        //index��° ��ų sp
        //sp - ��ų[num].sp
        return Skills[index];
    }

    private Rigidbody rb;
    protected Animator animator;

    private float x;//x���Ⱚ
    private float z;//z���Ⱚ
    private float moveSpeed;//�̵� �ӵ�
    private float runSpeed;//�ٴ� �̵� �ӵ�
    private float rotationSpeed;//���� �ٲٴ� �ӵ�
    private float jumpSpeed;//���� �ӵ�
    private float gravityForce;//�߷�
    private float directionMove;//�ִϸ��̼� �ȴ� ����
    public bool isGrounded;//���� ������ �� �ִ���
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
        SetStatus();//���罺���� �ƽ��������� ä����
        isGrounded = true;
        moveSpeed = 4.5f;
        runSpeed = 6f;
        rotationSpeed = 150f;
        jumpSpeed = 7f;
        gravityForce = 9.8f;
        directionMove = 0;
        attackRange = transform.Find("AttackRange").gameObject;//���ݹ��� ������Ʈ ã�� �Ű��൵ ��
        if (myClass != characterClasses.wizard)
        {
            dashEffect = transform.Find("DashEffect").gameObject;
        }
        attackRange.SetActive(false);//ó���� ������ ���ϰ� �־����
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
        //currentSkillNum = 1;//�⺻���ݹ�ȣ
        auraSelect = 3;
        dieCheck = false;
        isWarriorAttack = false;
        attok = false;
    }
    private void FixedUpdate()
    {
        rb.rotation = rb.rotation * Quaternion.Euler(0, x * rotationSpeed * Time.deltaTime, 0);//����

        if (rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.down * gravityForce);//�߷�
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
            if (GameManager.Instance.isBattle == false && isAttack == false)//����ã��
            {
                MonsterFind();
            }
            //----------------------------�̵�----------------------------// 
            if (GameManager.Instance.isBattle == false)//�������϶� ���۸��ϰ� ���Ƴ���
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
                //----------------------------����----------------------------// 
                if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.isBattle == false)
                {
                    if (isGrounded)
                    {
                        JumpTo();
                        animator.SetBool("IsJump", true);//���� ���οö�
                        StartCoroutine("JumpCo");
                    }
                }
                if (animator.GetBool("JumpDown"))
                {
                    StopCoroutine("JumpCo");
                }
                //----------------------------����----------------------------// 
                

                if (scene.name == "MainGame")
                {
                    if (GameManager.Instance.isBattle == false)//���� ���̷� ��ġ��
                    {
                        if(GameManager.Instance.foundEnemy.GetComponent<Monster>() != null)
                        {
                            if (Input.GetKeyDown(KeyCode.F))//�����׽�Ʈ
                            {
                                PlayerAttack();//�ִϸ��̼�
                                isAttack = true;
                            }
                        }
                    }
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.SetBool("IsAttack", false);//�����̳����� ������ �Ҽ� �ְԵ�
                    attackRange.SetActive(false);
                    isAttack = false;
                }
                //////////////////////////////������/////////////////////////////////
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
    public void PlayerMove()//�÷��̾� �̵�
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
            transform.Translate(new Vector3(0, 0, z) * tempSpeed * Time.deltaTime); //�̵�
        }
        else
        {
            animator.SetBool("IsRun", false);
            transform.Translate(new Vector3(0, 0, z) * moveSpeed * Time.deltaTime); //�̵�
        }

    }
    void JumpTo()//���� rigidbody�ۿ�
    {
        isGrounded = false;//�������ϰ� ����

        Vector3 jumpVelocity = Vector3.up * jumpSpeed;

        //rb.velocity = Vector3.zero;
        rb.AddForce(jumpVelocity, ForceMode.Impulse);

        //Debug.Log(jumpVelocity);
    }
    public void PlayerAttack()//���� �ִϸ��̼�, ���ݹ��� ����
    {
        NowAp = 0;

        animator.SetBool("IsAttack", true);
        attackRange.SetActive(true);
        targetMonster = GameManager.Instance.foundEnemy.transform;
        Vector3 tagertMonsterPosition = new Vector3(targetMonster.position.x, transform.position.y, targetMonster.position.z);
        transform.LookAt(tagertMonsterPosition);//ȭ��� �ٶ󺸰��ϱ�
        if (this.myClass == characterClasses.archer)//��ó�϶� ����
        {
            if (!isAttack)
            {
                //Debug.Log("ȭ�����");
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
            GameManager.Instance.foundEnemy = hitData.collider.gameObject;//�����ؾߵǴ°�
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
        transform.position = new Vector3(7, 0, 10);//��Ȱ��ǥ
    }
    IEnumerator JumpCo()//�����Ë��� ã�� �Լ�
    {
        float jumpVal = 0;
        while (true)
        {
            jumpVal = 0;
            jumpVal = rb.velocity.y;//�����ѵ� y���� ���
            //Debug.Log(jumpVal);
            yield return null;
            if (jumpVal < 0)//&& animator.GetBool("IsJump")
            {
                animator.SetBool("IsJump", false);//�������� �����Ҷ� 
                animator.SetBool("JumpDown", true);//down�ִϸ��̼� ����
                break;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")//ground�� �������
        {
            isGrounded = true;
            //Debug.Log(isGrounded);
            animator.SetBool("JumpDown", false);//�������� �ִϸ��̼� ����
        }
    }
}