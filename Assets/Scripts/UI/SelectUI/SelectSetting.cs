using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SelectSetting : PanelUIComponent
{
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    public void ControlBGMMixer()
    {
        float sound = bgmSlider.value;

        if(sound == -40f)
        {
            AudioMgr.Instance.audioMixer.SetFloat("BGM", -40f); 
        }
        else
        {
            AudioMgr.Instance.audioMixer.SetFloat("BGM", sound);
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
            AudioMgr.Instance.audioMixer.SetFloat("SFX", sound);
        }
    }

    public void ToggleAudioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
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
