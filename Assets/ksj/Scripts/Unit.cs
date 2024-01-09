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
    protected Skill[] skills;//스킬 리스트
    [SerializeField]
    private float hp;//현재 hp
    [SerializeField]
    private float maxHp;//최대 hp
    [SerializeField]
    private float sp;//현재 sp
    [SerializeField]
    private float maxSp;//최대 sp
    [SerializeField]
    private float atk;//공격력
    [SerializeField]
    private float nowAp;//전투에서 현재ap
    [SerializeField]
    private float unitAp;//유닛의 속도
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
