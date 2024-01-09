using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load1Scene : MonoBehaviour
{
    public void OnClickMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        for(int i=0; i < 3; i++)
        {
            GameManager.Instance.playerObject[i].SetActive(false);
        }
        
    }
}
