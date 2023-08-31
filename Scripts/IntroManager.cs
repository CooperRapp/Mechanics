using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IntroManager : MonoBehaviour
{
    public ParticleSystem dust;
    public ParticleSystem launch;
    public ParticleSystem explosion;

    public GameObject cube;
    public Animator cubeAnim;
    public GameObject rocket;
    public Animator rocketAnim;
    public GameObject platform;

    public GameObject rocketlauncherCUBE;
    public GameObject rocketlauncherROCKET;

    public GameObject menu;

    public AudioSource SFX;
    public AudioClip rocketlauncher_SFX;
    public AudioClip wallexplosion_SFX;

    public GraphicRaycaster m_Raycaster;
    public EventSystem m_EventSystem;
    PointerEventData m_PointerEventData;
    public GameObject playHover;
    public GameObject settingsHover;
    public GameObject creditsHover;
    public GameObject quitHover;

    bool done1, done2, done3, done4, done5;

    float time;
    float time2;

    public GameObject SettingsPanel;
    public GameObject CreditsPanel;
    public GameObject FadePanel;
    public Animator FullScreenFade;

    bool clickedPlay;

    public static bool introAnim;
    bool oneTime;

    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
        done1 = done2 = done3 = done4 = false;
        clickedPlay = false;
        oneTime = false;

        SettingsPanel = GameObject.FindGameObjectWithTag("SETTINGS").transform.GetChild(0).gameObject;
        SettingsPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!introAnim)
        {
            if (Time.time > time + 1.78f && !done1)
            {
                dust.Play();
                done1 = true;
            }

            if (Time.time > time + 2.6f && !done2)
            {
                rocketlauncherCUBE.SetActive(true);
                rocketlauncherROCKET.SetActive(false);
                done2 = true;
            }

            if (Time.time > time + 3f && !done3)
            {
                cubeAnim.enabled = true;
                cubeAnim.SetTrigger("flip");
                rocketAnim.enabled = true;
                rocketAnim.SetTrigger("launch");
                done3 = true;
            }

            if (Time.time > time + 4.1f & !done4)
            {
                launch.Play();
                SFX.clip = rocketlauncher_SFX;
                SFX.Play();
                done4 = true;
            }

            if (Time.time > time + 4.85f && !done5)
            {
                explosion.Play();
                SFX.clip = wallexplosion_SFX;
                SFX.Play();
                platform.SetActive(false);
                cube.SetActive(false);
                rocket.SetActive(false);
                menu.SetActive(true);
                done5 = true;
                introAnim = true;
            }
        }

        if(!oneTime && introAnim)
        {
            platform.SetActive(false);
            cube.SetActive(false);
            rocket.SetActive(false);
            menu.SetActive(true);
            oneTime = true;
        }

        if (Time.time > time2 + 1.15f && clickedPlay && introAnim)
        {
            SceneManager.LoadScene("level_map");
        }

        if (introAnim)
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);
            foreach(RaycastResult res in results)
            {
                if (res.gameObject.name == "Play") playHover.SetActive(true);
                else playHover.SetActive(false);

                if (res.gameObject.name == "Settings") settingsHover.SetActive(true);
                else settingsHover.SetActive(false);

                if (res.gameObject.name == "Credits") creditsHover.SetActive(true);
                else creditsHover.SetActive(false);

                if (res.gameObject.name == "Quit") quitHover.SetActive(true);
                else quitHover.SetActive(false);
            }
        }
    }

    public void Play()
    {
        clickedPlay = true;
        time2 = Time.time;
        FullScreenFade.SetTrigger("in");
    }

    public void Settings()
    {
        SettingsPanel.SetActive(true);
    }

    public void Back()
    {
        SettingsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        FadePanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Credits()
    {
        CreditsPanel.SetActive(true);
        FadePanel.SetActive(false);
    }
}
