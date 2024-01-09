using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeMonster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        Invoke("MonsterDelay", transform.parent.GetComponent<Monster>().invokeTime);
        //DestroyImmediate(gameObject.transform.parent.GetComponent<Monster>().effectTemp);
        Instantiate(gameObject.transform.parent.GetComponent<Monster>().spawnEffect, transform.parent.transform);
    }
    void MonsterDelay()
    {
        gameObject.SetActive(true);
    }
}
