using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class testText : MonoBehaviour
{
    public TextMeshProUGUI testText1;
    // Start is called before the first frame update
    void Start()
    {
        testText1 = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        testText1.text = GameManager.Instance.score.ToString();
    }
}
