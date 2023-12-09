using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
    private bool isSettingsOpen;

    [Header("Resolution")]
    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedRes;
    [SerializeField] private TextMeshProUGUI resolutionlabel;
    public Toggle fullscreentog;

    [Header("Audio")]
    public AudioMixer MainMixer;
    public static float globalVolume;
    public static float musicVolume;
    public static float sfxVolume;
    public Slider GlobalVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;


    private void Start()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        globalVolume = PlayerPrefs.GetFloat("GlobalVolumeSlider");
        musicVolume = PlayerPrefs.GetFloat("MusicVolumeSlider");
        sfxVolume = PlayerPrefs.GetFloat("SFXVolumeSlider");

        GlobalVolumeSlider.value = PlayerPrefs.GetFloat("GlobalVolumeSlider");
        MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolumeSlider");
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolumeSlider");

        settingsMenu.SetActive(false);
        isSettingsOpen = false;

        fullscreentog.isOn = Screen.fullScreen;
        bool foundRes = false;
        for(int i = 0; i < resolutions.Count; i++)
        {
            if(Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;
                selectedRes = i;
                UpdateResLabel();
            }
        }

        if(!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);
            selectedRes = resolutions.Count - 1;
            UpdateResLabel();
        }
    }

    public void GlobalVolumeChanged(float value)
    {
        globalVolume = value;
        UpdateMixerValues();
    }

    public void MusicVolumeChanged(float value)
    {
        musicVolume = value;
        UpdateMixerValues();
    }

    public void SFXVolumeChanged(float value)
    {
        sfxVolume = value;
        UpdateMixerValues();
    }

    public void UpdateMixerValues()
    {
        MainMixer.SetFloat("MasterVolume", Mathf.Log10(globalVolume) * 20);
        MainMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        MainMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);

        PlayerPrefs.SetFloat("GlobalVolumeSlider", globalVolume);
        PlayerPrefs.SetFloat("MusicVolumeSlider", musicVolume);
        PlayerPrefs.SetFloat("SFXVolumeSlider", sfxVolume);
    }

    public void LastRes()
    {
        selectedRes--;

        if(selectedRes < 0)
        {
            selectedRes = 0;
        }

        UpdateResLabel();
    }

    public void NextRes()
    {
        selectedRes++;
        if(selectedRes > resolutions.Count - 1)
        {
            selectedRes = resolutions.Count - 1;
        }
        UpdateResLabel();
    }

    private void UpdateResLabel()
    {
        resolutionlabel.text = resolutions[selectedRes].horizontal.ToString() + " x " + resolutions[selectedRes].vertical.ToString();
    }

    public void ApplyRes()
    {
        Screen.SetResolution(resolutions[selectedRes].horizontal, resolutions[selectedRes].vertical, fullscreentog.isOn);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void SettingsButton()
    {
        isSettingsOpen = true;
        settingsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackButton()
    {
        if(isSettingsOpen)
        {
            isSettingsOpen=false;
            settingsMenu.SetActive(false);
        }
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
