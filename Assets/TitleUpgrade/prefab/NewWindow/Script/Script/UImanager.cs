using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class UImanager : MonoBehaviour
{
   
    public TMP_InputField  archerName;
    public TMP_InputField warriorName;
    public TMP_InputField wizardName;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
    public void GoTitleScene()//파티씬으로 가기
    {
        
        SceneManager.LoadScene("TitleScene");
    }
    /*public void GoWorldScene()//월드씬으로 가기
    {
        SceneManager.LoadScene("WorldScene");
    }
    */
    public void GoSelect()//셀렉트 씬으로 가기
    {
        GameManager.Instance.StartGame();
    }
    public void GoMainGame()
    {
        GameManager.Instance.players[0].UnitName= archerName.text;
        GameManager.Instance.players[1].UnitName = warriorName.text;
        GameManager.Instance.players[2].UnitName = wizardName.text;
        GameManager.Instance.prevCharacter = 0;
        GameManager.Instance.SelectToMain();
        GameManager.Instance.FadeScene("MainGame");
        
    }
    public void ClickFirstCharactorImage()
    {
        GameManager.Instance.InitPlayerPostion(0);
    }
    public void ClickSecondCharactorImage()
    {
        GameManager.Instance.InitPlayerPostion(1);
    }
    public void ClickThirdCharactorImage()
    {
        GameManager.Instance.InitPlayerPostion(2);
    }
    
    public void GameEnd()//게임종료
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
