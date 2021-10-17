using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SelectSetting : PanelUIComponent
{
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    [SerializeField] GameObject[] bgmToggle = new GameObject[2];
    [SerializeField] GameObject[] sfxToggle = new GameObject[2];

    public void ControlBGMMixer()
    {
        float sound = bgmSlider.value;

        if(sound == -40f)
        {
            AudioMgr.Instance.audioMixer.SetFloat("BGM", -40f); 
        }
        else
        {
            if (bgmToggle[1].activeSelf)
            {
                ToggleBGM();
            }
            else
            {
                AudioMgr.Instance.audioMixer.SetFloat("BGM", sound);
            }
        }
    }

    public void ControlSFXMixer()
    {
        float sound = sfxSlider.value;

        if (sound == -40f)
        {
            AudioMgr.Instance.audioMixer.SetFloat("SFX", -40f);
        }
        else
        {
            if (sfxToggle[1].activeSelf)
            {
                ToggleSFX();
            }
            else
            {
                AudioMgr.Instance.audioMixer.SetFloat("SFX", sound);
            }
        }
    }

    public void ToggleBGM()
    {
        bgmToggle[0].SetActive(bgmToggle[1].activeSelf);
        bgmToggle[1].SetActive(!bgmToggle[0].activeSelf);

        if(bgmToggle[0].activeSelf)
        {
            AudioMgr.Instance.audioMixer.SetFloat("BGM", bgmSlider.value);
        }
        else if(bgmToggle[1].activeSelf)
        {
            AudioMgr.Instance.audioMixer.SetFloat("BGM", -40f);
        } 
    }

    public void ToggleSFX()
    {
        sfxToggle[0].SetActive(sfxToggle[1].activeSelf);
        sfxToggle[1].SetActive(!sfxToggle[0].activeSelf);

        if (sfxToggle[0].activeSelf)
        {
            AudioMgr.Instance.audioMixer.SetFloat("SFX", sfxSlider.value);
        }
        else if (sfxToggle[1].activeSelf)
        {
            AudioMgr.Instance.audioMixer.SetFloat("SFX", -40f);
        }
    }


    public override void SetPanel(UIParam u = null)
    {
        gameObject.SetActive(true);
    }

    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
    }
}
