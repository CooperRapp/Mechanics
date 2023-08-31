using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static float SFX_volume;
    public static float BKM_volume;

    public static int resolutionPick;
    public static bool fullscreen;

    public static bool colorblind;
    public static int changefont;

    public Slider SFX_slider;
    public AudioSource SFX;
    public Slider BKM_slider;
    public AudioSource BKM;

    public Toggle fullscreen_toggle;
    public TMP_Dropdown resolution_dropdown;
    public Toggle colorblind_toggle;
    public TMP_Dropdown font_dropDown;

    Resolution[] resolutions;

    public TMP_FontAsset isometricFont;
    public TMP_FontAsset normalFont;
    public TMP_FontAsset dyslexicFont;

    public Material greenStart;
    public Material blueStart;

    void Awake()
    {
        // MUSIC STUFF ------------------
        SFX = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();
        BKM = GameObject.FindGameObjectWithTag("BKM").GetComponent<AudioSource>();

        SFX_slider = GameObject.FindGameObjectWithTag("SFX_SLIDER").GetComponent<Slider>();
        BKM_slider = GameObject.FindGameObjectWithTag("BKM_SLIDER").GetComponent<Slider>();

        SFX_slider.value = PlayerPrefs.GetFloat("SFXVolume");
        SFX.volume = PlayerPrefs.GetFloat("SFXVolume");
        BKM_slider.value = PlayerPrefs.GetFloat("BKMVolume");
        BKM.volume = PlayerPrefs.GetFloat("BKMVolume");
        // ------------------------------

        // FULLSCREEN MODE ------------------
        fullscreen_toggle = GameObject.FindGameObjectWithTag("FULLSCREEN").GetComponent<Toggle>();
        if(PlayerPrefs.GetInt("FullScreenMode") == 0)
        {
            fullscreen = true;
            SetFullscreen(true);
            if (!fullscreen_toggle.isOn) fullscreen_toggle.isOn = true; 
        }
        else if (PlayerPrefs.GetInt("FullScreenMode") == 1)
        {
            fullscreen = false;
            SetFullscreen(false);
            if (fullscreen_toggle.isOn) fullscreen_toggle.isOn = false;
        }     
        // ------------------------------

        // RESOLUTION ------------------
        resolution_dropdown = GameObject.FindGameObjectWithTag("RESOLUTION").GetComponent<TMP_Dropdown>();
        SetResolution(PlayerPrefs.GetInt("ResolutionValue"));
        resolution_dropdown.value = PlayerPrefs.GetInt("ResolutionValue");
        // ------------------------------

        // COLORBLIND MODE ------------------
        colorblind_toggle = GameObject.FindGameObjectWithTag("COLORBLIND").GetComponent<Toggle>();
        if (PlayerPrefs.GetInt("ColorblindMode") == 1)
        {
            SetColorblind(true);
            colorblind = true;
            colorblind_toggle.isOn = true;
        }
        else if (PlayerPrefs.GetInt("ColorblindMode") == 0)
        {
            SetColorblind(false);
            colorblind = false;
            colorblind_toggle.isOn = false;
        }
        // ------------------------------

        // FONT ------------------
        font_dropDown = GameObject.FindGameObjectWithTag("FONT").GetComponent<TMP_Dropdown>();
        SetChangeFont(PlayerPrefs.GetInt("FontValue"));
        changefont = PlayerPrefs.GetInt("FontValue");
        font_dropDown.value = changefont;
        // ------------------------------
    }
    
    public void SetBKM(float volume)
    {
        BKM.volume = volume;
        PlayerPrefs.SetFloat("BKMVolume", BKM.volume);
    }

    public void SetSFX(float volume)
    {
        SFX.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", SFX.volume);
    }

    public void SetFullscreen(bool toggle)
    {
        if (toggle)
        {
            fullscreen = true;
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            PlayerPrefs.SetInt("FullScreenMode", 0);
        }
        if (!toggle)
        {
            fullscreen = false;
            Screen.fullScreenMode = FullScreenMode.Windowed;
            PlayerPrefs.SetInt("FullScreenMode", 1);
        }
    }

    public void SetResolution(int value)
    {
        if (value == 0)
        {
            Screen.SetResolution(1280, 720, fullscreen);
            Debug.Log("1280x720");
        }
        if (value == 1)
        {
            Screen.SetResolution(1280, 800, fullscreen);
            Debug.Log("1280x800");
        }
        if (value == 2)
        {
            Screen.SetResolution(1920, 1080, fullscreen);
            Debug.Log("1920x1080");
        }
        if (value == 3)
        {
            Screen.SetResolution(1920, 1200, fullscreen);
            Debug.Log("1920x1200");
        }
        if (value == 4)
        {
            Screen.SetResolution(2560, 1440, fullscreen);
            Debug.Log("2560x1440");
        }
        if (value == 5)
        {
            Screen.SetResolution(3840, 2160, fullscreen);
            Debug.Log("3840x2160");
        }
        PlayerPrefs.SetInt("ResolutionValue", value);
    }

    public void SetColorblind(bool toggle)
    {
        if(toggle)
        {
            colorblind = true;
            GameObject sm = GameObject.FindGameObjectWithTag("STARTMARKER");
            PlayerPrefs.SetInt("ColorblindMode", 1);
            if (sm != null)
            {
                sm.GetComponent<MeshRenderer>().material = blueStart;
            }
        }
        if (!toggle)
        {
            colorblind = false;
            GameObject sm = GameObject.FindGameObjectWithTag("STARTMARKER");
            PlayerPrefs.SetInt("ColorblindMode", 0);
            if (sm != null)
            {
                sm.GetComponent<MeshRenderer>().material = greenStart;
            }
        }
    }

    public void SetChangeFont(int value)
    {
        TMP_Text[] texts = GameObject.FindObjectsOfType<TMP_Text>();
        if (value == 0)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                if (texts[i].gameObject.tag == "changeFont")
                {
                    texts[i].font = isometricFont;
                    if (texts[i].gameObject.name == "LevelNumber") texts[i].rectTransform.anchoredPosition = new Vector3(550f, -135f, 0);
                    changefont = 0;
                }
            }
        }
        if (value == 1)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                if (texts[i].gameObject.tag == "changeFont")
                {
                    texts[i].font = normalFont;
                    if (texts[i].gameObject.name == "LevelNumber") texts[i].rectTransform.anchoredPosition = new Vector3(550f, -135f, 0);
                    changefont = 1;
                }
            }
        }
        if (value == 2)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                if (texts[i].gameObject.tag == "changeFont")
                {
                    texts[i].font = dyslexicFont;
                    if (texts[i].gameObject.name == "LevelNumber") texts[i].rectTransform.anchoredPosition = new Vector3(550f, -160f, 0);
                    changefont = 2;
                }
            }
        }
        PlayerPrefs.SetInt("FontValue", value);
    }

    public void Back()
    {
        this.gameObject.SetActive(false);
    }

}
