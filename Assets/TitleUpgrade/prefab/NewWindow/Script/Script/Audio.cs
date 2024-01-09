using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

using UnityEngine.SceneManagement;


using TMPro;

public class Audio : MonoBehaviour
{
    // Start is called before the first frame update
        [SerializeField] private AudioMixer m_AudioMixer;
        [SerializeField] private Slider m_MasterSlider;
        [SerializeField] private Slider m_BGMSlider;
        
    void Start()
    {
        m_MasterSlider.onValueChanged.AddListener(SetMasterVolume);
        m_BGMSlider.onValueChanged.AddListener(SetBGMVolume);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetMasterVolume(float volume)
    {
        m_AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetBGMVolume(float volume)
    {
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    
}
