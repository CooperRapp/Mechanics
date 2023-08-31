using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    RaycastHit hit;
    Ray ray;
    Vector3 mousePos;

    Animator anim;

    [Header("Loading Unlocked Levels")]
    public static int levelsUnlocked = 0;
    public Transform LevelHolder;

    [Header("Transition Stuff")]
    public Camera cam;
    public Transform camera;
    bool moveCamera;

    public Image fadeToBlack;
    float alpha;

    float time;
    float actionTime;

    Transform whichLevel;

    string levelTitle = "";

    public static bool firstTime = true;
    bool once = true;

    public static bool clickedLevel;

    public TMP_FontAsset normalFont;
    public TMP_FontAsset isometricFont;
    public TMP_FontAsset dyslexicFont;

    void Awake()
    {
        alpha = 0f;

        SetChangeFont(SettingsManager.changefont);

        levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked");

        for(int i = 1; i < levelsUnlocked + 1; i++)
        {
            Transform Level = LevelHolder.GetChild(i);
            Level.rotation = Quaternion.Euler(0f, -90f, 90f);

            Transform LevelSelector = Level.GetChild(Level.childCount - 1); // grabbing the collider in each level prefab that allows for level selecting interaction // CHANGED FROM2
            LevelSelector.tag = "level";
            LevelSelector.GetChild(0).gameObject.SetActive(false); // hide the lock
            LevelSelector.GetChild(1).gameObject.SetActive(false); // hide the roof
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(firstTime)
        {
            if(once)
            {
                camera.position = new Vector3(-59.6f, 171f, 59.5f);
                once = false;
            }
            float y = Mathf.MoveTowards(camera.position.y, 77f, 25f * Time.deltaTime);
            camera.position = new Vector3(camera.position.x, y, camera.position.z);

            if (y <= 77f) firstTime = false;
        }

        mousePos = Input.mousePosition;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "level") // if they are hovering over the level then play the rotating animation
            {
                Debug.Log(hit.transform.name);

                // rotating level
                anim = hit.collider.transform.parent.gameObject.GetComponent<Animator>();
                if (!anim.enabled) anim.enabled = true;
                anim.Play("rotate");

                if (Input.GetMouseButtonDown(0)) // if they click on level begin the transition into it
                {
                    time = Time.time;
                    actionTime = Time.time;
                    moveCamera = true;
                    whichLevel = hit.collider.transform.parent;
                    levelTitle = hit.collider.transform.parent.name;
                    clickedLevel = true;
                }
            }
            if (hit.collider.tag != "level")
            {
                if (anim != null)
                {
                    anim.gameObject.transform.rotation = Quaternion.Euler(0f, -90f, 90f); // reset the level rotation
                    anim.enabled = false;
                }
            }
        }
        //else
        //{
        //    if (anim != null)
        //    {
        //        anim.gameObject.transform.rotation = Quaternion.Euler(0f, -90f, 90f); // reset the level rotation
        //        anim.enabled = false;
        //    }
        //}

        if (moveCamera)
        {
            TransitionLevel();
        }
    }

    void TransitionLevel()
    {
        if(Time.time < time + 1.5f)
        {
            camera.position = Vector3.MoveTowards(camera.position, whichLevel.position, 75f * Time.deltaTime);
            cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, 1f, 10f * Time.deltaTime);
            alpha = Mathf.MoveTowards(alpha, 255f, 0.75f * Time.deltaTime);
            fadeToBlack.color = new Color(0f, 0f, 0f, alpha);
        }
        if (Time.time > time + 1.5f)
        {
            if (levelTitle != "Levels") SceneManager.LoadScene(levelTitle);
        }
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
                    SettingsManager.changefont = 2;
                }
            }
        }
    }
}
