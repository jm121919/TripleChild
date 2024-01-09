using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    public TextMeshProUGUI textTemp;
    public static bool stop;
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.GetComponent<TextMeshProUGUI>() != null)
        {
            textTemp = gameObject.GetComponent<TextMeshProUGUI>();
            textTemp.text = "Test";
        }
        stop = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("CoolTimeCo");
        //StartCoroutine
        if (stop)
        {
            Debug.Log(stop);
            //StopCoroutine("CoolTimeCo");
        }
        
    }
    IEnumerator CoolTimeCo()
    {
        while(textTemp.color.a > 0)
        {
            textTemp.color = new Color(textTemp.color.r, textTemp.color.g, textTemp.color.b, textTemp.color.a - Time.deltaTime/30.0f);
            yield return null;
            if (stop)
            {
                break;
            }
        }
        
    }
}
