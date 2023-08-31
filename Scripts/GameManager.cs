using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject SettingsMenu;
    public TMP_FontAsset normalFont;
    public TMP_FontAsset isometricFont;
    public TMP_FontAsset dyslexicFont;
    public MeshRenderer startPoint;
    public Material startGreen;
    public Material startBlue;
    bool on;

    void Start()
    {
        on = false;
        SettingsMenu = GameObject.FindGameObjectWithTag("SETTINGS").transform.GetChild(0).gameObject;

        SetChangeFont(SettingsManager.changefont);

        if(SettingsManager.colorblind)
        {
            startPoint.material = startBlue;
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            RestartLevel();
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            Application.Quit();
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            PickFace.play = false;
            PickFace.springOnBottom = false;
            ZeroGravity.inField = false;
            Physics.gravity = new Vector3(0f, -9.8f, 0f);
            UseMechanic.mechanicFace.Clear();
            PickFace.slotUseCount_STATIC.Clear();
            SceneManager.LoadScene("level_map");
        }
    }

    public static void RestartLevel()
    {
        PickFace.play = false;
        PickFace.springOnBottom = false;
        ZeroGravity.inField = false;
        Physics.gravity = new Vector3(0f, -9.8f, 0f);
        UseMechanic.mechanicFace.Clear();
        PickFace.slotUseCount_STATIC.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void HomeMenu()
    {
        PickFace.play = false;
        PickFace.springOnBottom = false;
        ZeroGravity.inField = false;
        Physics.gravity = new Vector3(0f, -9.8f, 0f);
        UseMechanic.mechanicFace.Clear();
        PickFace.slotUseCount_STATIC.Clear();
        SceneManager.LoadScene("home");
    }

    public void Settings()
    {
        on = !on;
        SettingsMenu.SetActive(on);
    }

    void SetChangeFont(int value)
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
                    SettingsManager.changefont = 0;
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
                    SettingsManager.changefont = 1;
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
                    if(texts[i].gameObject.name == "LevelNumber") texts[i].rectTransform.anchoredPosition = new Vector3(550f, -160f, 0);
                    SettingsManager.changefont = 2;
                }
            }
        }
    }
}
