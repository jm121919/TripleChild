using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField]
    private string unitName;
    public string UnitName
    {
        get
        {
            return unitName;
        }
        set
        {
            unitName = value;
        }
    }
    // Start is called before the first frame update
    [SerializeField]
    protected Transform SkillsTrans;
    [SerializeField]
    protected Skill[] skills;//��ų ����Ʈ
    [SerializeField]
    private float hp;//���� hp
    [SerializeField]
    private float maxHp;//�ִ� hp
    [SerializeField]
    private float sp;//���� sp
    [SerializeField]
    private float maxSp;//�ִ� sp
    [SerializeField]
    private float atk;//���ݷ�
    [SerializeField]
    private float nowAp;//�������� ����ap
    [SerializeField]
    private float unitAp;//������ �ӵ�
    public Skill[] Skills 
    { 
        get { return skills; }
        set { skills = value; }
    }
    public float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
        }
    }

    public float MaxHp
    {
        get
        {
            return maxHp;
        }
        set
        {
            maxHp = value;
        }
    }

    public float Sp
    {
        get
        {
            return sp;
        }
        set
        {
            sp = value;
        }
    }

    public float MaxSp
    {
        get
        {
            return maxSp;
        }
        set
        {
            maxSp = value;
        }
    }

    public float Atk
    {
        get
        {
            return atk;
        }
        set
        {
            atk = value;
        }
    }

    public float NowAp
    {
        get
        {
            return nowAp;
        }
        set
        {
            nowAp = value;
        }
    }

    public float UnitAp
    {
        get
        {
            return unitAp;
        }
        set
        {
            unitAp = value;
        }
    }
    public abstract void TakeDamage(float damage);
    public abstract void Die();
    public abstract Skill GetSkill(int index);
    public abstract void SetStatus();
}
