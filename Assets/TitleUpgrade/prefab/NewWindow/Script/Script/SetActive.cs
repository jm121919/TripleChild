using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class SetActive : MonoBehaviour
{
    private bool state;
    public GameObject MainMenu;
    public GameObject QuitMenu;
    public GameObject PartyWindow;
    public GameObject SettingWindow;
    public GameObject DefaultWindow;//���� �����ִ� â
    public GameObject TargetWindow;//�Ѿߵ� â

    // Start is called before the first frame update
    public void Start()
    {
        if(MainMenu != null)
            MainMenu.SetActive(false);
        if(QuitMenu != null)    
            QuitMenu.SetActive(false);
        if (PartyWindow != null)
            PartyWindow.SetActive(false);
        if (SettingWindow != null)
            SettingWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(MainMenu != null)
        {
            if (GameManager.Instance.windowOn == false)
            {
                if (Input.GetKeyDown(KeyCode.Escape))//escŰ�� �������
                {
                    if (state == false)
                    {
                        Time.timeScale = 0;
                        MainMenu.SetActive(true);
                        state = true;
                        Debug.Log("����Ű������/////");
                    }
                    else
                    {
                        Time.timeScale = 1;
                        MainMenu.SetActive(false);
                        state = false;
                        Debug.Log("������///////////////");
                    }
                }
            }
            
        }
    }
    public void ChangeWindow()//â�� �ٲٴ� �Լ�
    {
        DefaultWindow.SetActive (false);
        TargetWindow.SetActive(true);
        GameManager.Instance.windowOn  = true;
    }
    public void OpenWindow()
    { TargetWindow.SetActive(true);}
    public void CloseWindow()
    {
        Time.timeScale = 1;
        GameManager.Instance.windowOn = false;
        Debug.Log("xǥ�� ������!!!!!!!!!!!!!!!!!!!!");
        DefaultWindow.SetActive(false);
    }
}

