using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActive : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClickArcher()
    {
        GameManager.Instance.InitPlayerPostion(0);//0�� �ü�
    }
    public void OnClickWarrior()
    {
        GameManager.Instance.InitPlayerPostion(1);//1�� ����
    }
    public void OnClickWizard()
    {
        GameManager.Instance.InitPlayerPostion(2);//2�� ������
    }
}
