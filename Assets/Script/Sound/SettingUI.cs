using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("UI Sliders")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.LoadVolume();

            // ใช้ default 0.5f ให้ตรงกับ AudioManager
            bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

            bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
    }


    void OnBGMVolumeChanged(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
    }

    void OnSFXVolumeChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }
}
