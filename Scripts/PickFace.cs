using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class PickFace : MonoBehaviour
{
    RaycastHit hit;
    Ray2D ray;
    Vector3 mousePos;
    Vector3 changeInMousePos;

    public GraphicRaycaster m_Raycaster;
    public EventSystem m_EventSystem;
    PointerEventData m_PointerEventData;

    AsyncOperation asyncop;

    public static Vector3 face;

    public Transform playerCube;
    public Rigidbody playerCubeRB;

    public static bool play;
    static bool oneTime;
    static bool oneTime2;
    bool oneTime3;
    bool oneTime4;
    bool oneTime5;

    bool m1, m2, m3;

    Vector3 mechanicPosition;
    Quaternion mechanicRotation;

    bool mechanic1, mechanic2, mechanic3;
    bool turnOnColliders;

    [SerializeField] int launcherCount = 1;
    [SerializeField] int springCount = 2;
    [SerializeField] int freezeCount = 2;

    public TextMeshProUGUI launcherText;
    public TextMeshProUGUI springText;
    public TextMeshProUGUI freezeText;

    bool bottomFace, rightFace, frontFace, backFace, leftFace;

    public static bool springOnBottom;
    bool applyMechanic;
    bool reuseMechanics;
    bool removeMechanic;

    public Sprite rocketlauncher_sprite;
    public Sprite spring_sprite;
    public Sprite freezegun_sprite;
    public Sprite rocketLaucherBP;
    public Sprite springBP;
    public Sprite freezeGunBP;
    public Sprite xSprite;
    public Sprite checkSprite;

    public List<Button> faceButtons = new List<Button>();
    public List<Image> faceIcons = new List<Image>();
    [SerializeField] List<Image> usedfaceIcons = new List<Image>();
    public Image bottomFaceIcon;

    public GameObject backText;
    public GameObject frontText;
    public GameObject leftText;
    public GameObject rightText;
    public GameObject bottomText;

    [Header("Slot Positions")]
    public List<Image> slotImages = new List<Image>();
    public List<TextMeshProUGUI> slotFaces = new List<TextMeshProUGUI>();
    public List<Transform> slotPositions = new List<Transform>();
    public List<TextMeshProUGUI> slotUseCount = new List<TextMeshProUGUI>();
    public static List<TextMeshProUGUI> slotUseCount_STATIC = new List<TextMeshProUGUI>();
    public Animator slotPanelAnim;

    [Header("Mechanics")]
    public GameObject rocketLauncher;
    public GameObject spring;
    public GameObject freezeGun;

    [Header("Buttons/Images")]
    public Button rl_button;
    public Image rl_image;
    public Button spr_button;
    public Image spr_image;
    public Button fg_button;
    public Image fg_image;
    public Button playButton;

    [Header("Audio Stuff")]
    public AudioSource SFX;
    public AudioClip removeSFX;
    public AudioClip clearSFX;
    public AudioClip applySFX;

    [Header("Retry")]
    public Vector3 retryPosition;
    public Animator opener;
    public float openerForceStart;
    public float openerForce;
    public static bool retry = false;
    public List<Collider> retryOffColliders = new List<Collider>();
    static List<int> previous_mechanics = new List<int>();
    static List<Vector3> previous_faces = new List<Vector3>();
    bool remove;

    [Header("Hover")]
    public GameObject mech1Hover;
    public GameObject mech2Hover;
    public GameObject mech3Hover;
    public GameObject removeHover;
    public GameObject clearHover;
    public GameObject playHover;
    public GameObject backHover;
    public GameObject frontHover;
    public GameObject leftHover;
    public GameObject rightHover;
    public GameObject bottomHover;
    public GameObject restartHover;
    public GameObject settingsHover;
    public GameObject homeHover;

    [Header("Selected")]
    public GameObject removeSelected;
    public GameObject mech1Selected;
    public GameObject mech2Selected;
    public GameObject mech3Selected;

    [Header("Tutorial Stuff")]
    public GameObject signTut;
    public GameObject rotateTut;
    public GameObject slotTut;
    public GameObject startMarkerTut;
    public bool tutorial;

    public Animator buildPanelAnim;
    public Animator cameraAnim;

    float time;

    public bool chaos;

    void Awake()
    {
        GameObject g = GameObject.FindGameObjectWithTag("SFX");
        SFX = g.GetComponent<AudioSource>();
    }

    void Start()
    {
        launcherText.text = launcherCount.ToString();
        springText.text = springCount.ToString();
        freezeText.text = freezeCount.ToString();

        m1 = m2 = m3 = false;

        asyncop = null;

        if(tutorial && play)
        {
            rotateTut.SetActive(false);
            signTut.SetActive(false);
            slotTut.SetActive(true);
        }
    }

    void Update()
    {
        Debug.Log("1: " + mechanic1);
        Debug.Log("2: " + mechanic2);
        Debug.Log("3: " + mechanic3);

        detectHover();

        mechcanicButtonInterativity();

        if (launcherCount > 0) m1 = true;
        if (springCount > 0) m2 = true;
        if (freezeCount > 0) m3 = true;

        if (UseMechanic.mechanicFace.Count == 0) playButton.interactable = false;
        if (UseMechanic.mechanicFace.Count != 0) playButton.interactable = true;

        if(Lava.died)
        {
            StartCoroutine(RetryLevelCoroutine());
        }

        if (OutOfBounds.died)
        {
            StartCoroutine(RetryLevelCoroutine());
        }

        if (retry)
        {
            Retry();
        }

        if(Signature.signed)
        {
            Play();
            Signature.signed = false;
        }
        
        if(turnOnColliders)
        {
            if(oneTime3)
            {
                time = Time.time;
                oneTime3 = false;
            }
            if (Time.time > time + 1.5f)
            {
                for (int i = 0; i < retryOffColliders.Count; i++)
                {
                    retryOffColliders[i].enabled = true;
                    if (retryOffColliders[i].gameObject.name == "lava") retryOffColliders[i].isTrigger = true;
                }
                turnOnColliders = true;
            }    
        }
    }

    public void FrontFace()
    {
        face = new Vector3(-1f, 0f, 0f);
        if(!remove && !frontFace) ApplyMechanic("frontIcon");
        if (remove && frontFace) RemoveMechanic("frontIcon");
    }

    public void BackFace()
    {
        face = new Vector3(1f, 0f, 0f);
        if (!remove && !backFace) ApplyMechanic("backIcon");
        if (remove && backFace) RemoveMechanic("backIcon");
    }

    public void LeftFace()
    {
        face = new Vector3(0f, 0f, 1f);
        if (!remove && !leftFace) ApplyMechanic("leftIcon");
        if (remove && leftFace) RemoveMechanic("leftIcon");
    }

    public void RightFace()
    {
        face = new Vector3(0f, 0f, -1f);
        if (!remove && !rightFace) ApplyMechanic("rightIcon");
        if (remove && rightFace) RemoveMechanic("rightIcon");
    }

    public void BottomFace()
    {
        face = new Vector3(0f, 1f, 0f);
        if (!remove && !bottomFace) ApplyMechanic("bottomIcon");
        if (remove && bottomFace) RemoveMechanic("bottomIcon");
    }

    void detectHover()
    {
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PointerEventData, results);
        foreach (RaycastResult result in results)
        {
            //Debug.Log("Hit " + result.gameObject.name);

            if (result.gameObject.name == "RocketLauncherButton") mech1Hover.SetActive(true);
            else mech1Hover.SetActive(false);

            if (result.gameObject.name == "SpringButton") mech2Hover.SetActive(true);
            else mech2Hover.SetActive(false);

            if (result.gameObject.name == "FreezeButton") mech3Hover.SetActive(true);
            else mech3Hover.SetActive(false);

            if (result.gameObject.name == "RemoveButton") removeHover.SetActive(true);
            else removeHover.SetActive(false);

            if (result.gameObject.name == "ClearButton") clearHover.SetActive(true);
            else clearHover.SetActive(false);

            //if (result.gameObject.name == "PlayButton") playHover.SetActive(true);
            //else playHover.SetActive(false);

            if (result.gameObject.name == "back") backHover.SetActive(true);
            else backHover.SetActive(false);

            if (result.gameObject.name == "front") frontHover.SetActive(true);
            else frontHover.SetActive(false);

            if (result.gameObject.name == "left") leftHover.SetActive(true);
            else leftHover.SetActive(false);

            if (result.gameObject.name == "right") rightHover.SetActive(true);
            else rightHover.SetActive(false);

            if (result.gameObject.name == "bottom") bottomHover.SetActive(true);
            else bottomHover.SetActive(false);

            if (result.gameObject.name == "RestartButton") restartHover.SetActive(true);
            else restartHover.SetActive(false);

            if (result.gameObject.name == "HomeButton") homeHover.SetActive(true);
            else homeHover.SetActive(false);

            if (result.gameObject.name == "SettingsButton") settingsHover.SetActive(true);
            else settingsHover.SetActive(false);
        }
    }

    //void detectClick()
    //{
    //    ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            mousePos = Input.mousePosition; // dont think i need this anymore

    //            if (hit.collider.tag == "cube")
    //            {
    //                Debug.Log(face);
    //                face = hit.transform.InverseTransformDirection(hit.normal);

    //                if (!remove) applyMechanic = true;
    //                if (remove) removeMechanic = true;
    //                //time = Time.time;
    //            }
    //        }
    //    }
    //}

    //void //AddSlot(Sprite mechanic, Vector3 img_scale, int useCount)
    //{
    //    GameObject g = Instantiate(slot, Vector3.zero, Quaternion.identity);
    //    g.GetComponentInChildren<Image>().sprite = mechanic;
    //    g.GetComponentInChildren<Image>().transform.localScale = img_scale;
    //    g.GetComponentInChildren<TextMeshProUGUI>().text = (slots.Count + 1) + ":        X " + useCount;
    //    g.transform.parent = panel.transform;
    //    g.transform.localScale = new Vector3(2.1f, 2.1f, 2.1f);
    //    g.transform.localPosition = slot1;
    //    g.transform.localRotation = Quaternion.Euler(Vector3.zero);
    //    slots.Add(g);
    //}

    void ApplyMechanic(String whichFace)
    {
        if (UseMechanic.mechanicFace.Count == 0 )
        {
            previous_mechanics.Clear();
            previous_faces.Clear();
        }

        int i = 0;
        if (whichFace != "bottomIcon")
        {
            for (i = 0; i < faceIcons.Count; i++)
            {
                if (faceIcons[i].gameObject.name == whichFace)
                {
                    break;
                }
            }
            usedfaceIcons.Add(faceIcons[i]);
        }

        SFX.clip = applySFX;
        SFX.Play();

        if (mechanic1 && launcherCount >= 1)
        {
            SpawnMechanic(rocketLauncher);
            launcherText.text = launcherCount.ToString();
            UseMechanic.mechanicFace.Add(face);

            faceIcons[i].sprite = rocketLaucherBP;
            faceIcons[i].GetComponent<Animator>().SetTrigger("rl_d");

            previous_mechanics.Add(1);
            previous_faces.Add(face);
        }

        if (mechanic2 && springCount >= 1)
        {
            SpawnMechanic(spring);
            springText.text = springCount.ToString();
            UseMechanic.mechanicFace.Add(face);

            if(whichFace != "bottomIcon")
            {
                faceIcons[i].sprite = springBP;
                faceIcons[i].GetComponent<Animator>().SetTrigger("s_d");
            }
            else
            {
                bottomFaceIcon.sprite = springBP;
                bottomFaceIcon.GetComponent<Animator>().SetTrigger("s_d");
            }

            previous_mechanics.Add(2);
            previous_faces.Add(face);
        }

        if (mechanic3 && freezeCount >= 1)
        {
            SpawnMechanic(freezeGun);
            freezeText.text = freezeCount.ToString();
            UseMechanic.mechanicFace.Add(face);

            faceIcons[i].sprite = freezeGunBP;
            faceIcons[i].GetComponent<Animator>().SetTrigger("fg_d");

            previous_mechanics.Add(3);
            previous_faces.Add(face);
        }

        if (whichFace == "bottomIcon") bottomText.SetActive(false);
        if (whichFace == "frontIcon") frontText.SetActive(false);
        if (whichFace == "leftIcon") leftText.SetActive(false);
        if (whichFace == "rightIcon") rightText.SetActive(false);
        if (whichFace == "backIcon") backText.SetActive(false);

        if (whichFace != "bottomIcon") faceIcons.Remove(faceIcons[i]);

        if (m1 && launcherCount <= 0)
        {
            mechanic_one();
            m1 = false;
        }
        if (m2 && springCount <= 0)
        {
            mechanic_two();
            m2 = false;
        }
        if (m3 && freezeCount <= 0)
        {
            mechanic_three();
            m3 = false;
        }
    }

    void SpawnMechanic(GameObject mechanic)
    {
        if (face == new Vector3(0f, 0f, 1f) && !leftFace) // left face
        {
            if (mechanic1)
            {
                mechanicPosition = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z + 0.6f);
                mechanicRotation = Quaternion.Euler(0f, 90f, 90f);
                launcherCount--;
                //AddSlot(rocketlauncher_sprite, new Vector3(2f, 2f, 2f), 2);
            }

            if (mechanic2)
            {
                mechanicPosition = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z + 0.1f);
                mechanicRotation = Quaternion.Euler(0f, 90f, 0f);
                springCount--;
                //AddSlot(spring_sprite, new Vector3(1.5f, 0.8f, 1f), 1);
            }

            if (mechanic3)
            {
                mechanicPosition = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z + 0.1f);
                mechanicRotation = Quaternion.Euler(0f, 90f, 0f);
                freezeCount--;
                //AddSlot(freezegun_sprite, new Vector3(1.2f, 1.2f, 1.2f), 1);
            }

            GameObject mech = Instantiate(mechanic, mechanicPosition, mechanicRotation, playerCube.transform);
            leftFace = true;
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
        if (face == new Vector3(-1f, 0f, 0f) && !frontFace) // front face
        {
            if (mechanic1)
            {
                mechanicPosition = new Vector3(playerCube.position.x - 0.6f, playerCube.position.y, playerCube.position.z);
                mechanicRotation = Quaternion.Euler(0f, 0f, 90f);
                launcherCount--;
                //AddSlot(rocketlauncher_sprite, new Vector3(2f, 2f, 2f), 2);
            }

            if (mechanic2)
            {
                mechanicPosition = new Vector3(playerCube.position.x - 0.1f, playerCube.position.y, playerCube.position.z);
                mechanicRotation = Quaternion.Euler(0f, 0f, 0f);
                springCount--;
                //AddSlot(spring_sprite, new Vector3(1.5f, 0.8f, 1f), 1);
            }

            if (mechanic3)
            {
                mechanicPosition = new Vector3(playerCube.position.x - 0.1f, playerCube.position.y, playerCube.position.z);
                mechanicRotation = Quaternion.Euler(0f, 0f, 0f);
                freezeCount--;
                //AddSlot(freezegun_sprite, new Vector3(1.2f, 1.2f, 1.2f), 1);
            }

            GameObject mech = Instantiate(mechanic, mechanicPosition, mechanicRotation, playerCube.transform);
            frontFace = true;
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
        if (face == new Vector3(1f, 0f, 0f) && !backFace) // back face
        {
            if (mechanic1)
            {
                mechanicPosition = new Vector3(playerCube.position.x + 0.6f, playerCube.position.y, playerCube.position.z);
                mechanicRotation = Quaternion.Euler(0f, 180f, 90f);
                launcherCount--;
                //AddSlot(rocketlauncher_sprite, new Vector3(2f, 2f, 2f), 2);
            }

            if (mechanic2)
            {
                mechanicPosition = new Vector3(playerCube.position.x + 0.1f, playerCube.position.y, playerCube.position.z);
                mechanicRotation = Quaternion.Euler(0f, 180f, 0f);
                springCount--;
                //AddSlot(spring_sprite, new Vector3(1.5f, 0.8f, 1f), 1);
            }

            if (mechanic3)
            {
                mechanicPosition = new Vector3(playerCube.position.x + 0.1f, playerCube.position.y, playerCube.position.z);
                mechanicRotation = Quaternion.Euler(0f, 180f, 0f);
                freezeCount--;
                //AddSlot(freezegun_sprite, new Vector3(1.2f, 1.2f, 1.2f), 1);
            }

            GameObject mech = Instantiate(mechanic, mechanicPosition, mechanicRotation, playerCube.transform);
            backFace = true;
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
        if (face == new Vector3(0f, 0f, -1f) && !rightFace) // right face
        {
            if (mechanic1)
            {
                mechanicPosition = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z - 0.6f);
                mechanicRotation = Quaternion.Euler(0f, -90f, 90f);
                launcherCount--;
                //AddSlot(rocketlauncher_sprite, new Vector3(2f, 2f, 2f), 2);
            }

            if (mechanic2)
            {
                mechanicPosition = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z - 0.1f);
                mechanicRotation = Quaternion.Euler(0f, -90f, 0f);
                springCount--;
                //AddSlot(spring_sprite, new Vector3(1.5f, 0.8f, 1f), 1);
            }

            if (mechanic3)
            {
                mechanicPosition = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z - 0.1f);
                mechanicRotation = Quaternion.Euler(0f, -90f, 0f);
                freezeCount--;
                //AddSlot(freezegun_sprite, new Vector3(1.2f, 1.2f, 1.2f), 1);
            }

            GameObject mech = Instantiate(mechanic, mechanicPosition, mechanicRotation, playerCube.transform);
            rightFace = true;
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
        if (mechanic2 && face == new Vector3(0f, 1f, 0f) && !bottomFace) // bottom face
        {
            mechanicPosition = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z);
            mechanicRotation = Quaternion.Euler(0f, 0f, 90f);
            springCount--;
            //AddSlot(spring_sprite, new Vector3(1.5f, 0.8f, 1f), 1);

            GameObject mech = Instantiate(mechanic, mechanicPosition, mechanicRotation, playerCube.transform);
            playerCube.position = new Vector3(playerCube.position.x, playerCube.position.y + 0.5f, playerCube.position.z);

            bottomFace = true;
            springOnBottom = true;
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    void mechcanicButtonInterativity()
    {
        if (launcherCount <= 0)
        {
            rl_button.interactable = false;
            rl_image.color = new Color32(150, 150, 150, 255);
        }
        else
        {
            rl_button.interactable = true;
            rl_image.color = new Color32(255, 255, 255, 255);
            launcherText.text = launcherCount.ToString();
        }

        if (springCount <= 0)
        {
            spr_button.interactable = false;
            spr_image.color = new Color32(100, 100, 100, 255);
        }
        else
        {
            spr_button.interactable = true;
            spr_image.color = new Color32(255, 255, 255, 255);
            springText.text = springCount.ToString();
        }

        if (freezeCount <= 0)
        {
            fg_button.interactable = false;
            fg_image.color = new Color32(100, 100, 100, 255);
        }
        else
        {
            fg_button.interactable = true;
            fg_image.color = new Color32(255, 255, 255, 255);
            freezeText.text = freezeCount.ToString();
        }
    }

    public void Clear()
    {
        if(!play)
        {
            SFX.clip = clearSFX;
            SFX.Play();

            for (int i = 0; i < usedfaceIcons.Count; i++) // move everything back to faceIcons
            {
                faceIcons.Add(usedfaceIcons[i]);
            }
            usedfaceIcons.Clear();

            for (int j = 0; j < faceIcons.Count; j++) // turn the icons off
            {
                faceIcons[j].gameObject.SetActive(false);

            }
            if (springOnBottom) // turn off bottom spring if its on
            {
                bottomFaceIcon.gameObject.SetActive(false);
                springOnBottom = false;
            }

            backText.SetActive(true);
            frontText.SetActive(true);
            leftText.SetActive(true);
            rightText.SetActive(true);
            bottomText.SetActive(true);

            bottomFace = rightFace = frontFace = backFace = leftFace = false;

            if (mechanic1) mechanic_one();
            if (mechanic2) mechanic_two();
            if (mechanic3) mechanic_three();

            for (int k = 0; k < playerCube.childCount; k++)
            {
                if (playerCube.GetChild(k).gameObject.tag == "rocketlauncher")
                {
                    launcherCount++;
                    launcherText.text = launcherCount.ToString();
                    Destroy(playerCube.GetChild(k).gameObject);
                }
                if (playerCube.GetChild(k).gameObject.tag == "spring")
                {
                    springCount++;
                    springText.text = springCount.ToString();
                    Destroy(playerCube.GetChild(k).gameObject);
                }
                if (playerCube.GetChild(k).gameObject.tag == "freezegun")
                {
                    freezeCount++;
                    freezeText.text = freezeCount.ToString();
                    Destroy(playerCube.GetChild(k).gameObject);
                }
            }
            UseMechanic.mechanicFace.Clear();
            slotUseCount_STATIC.Clear();
        }
    }

    public void Remove()
    {
        if(!play)
        {
            remove = !remove;

            for (int j = 0; j < faceButtons.Count; j++)
            {
                faceButtons[j].interactable = true;
            }

            if (remove)
            {
                removeSelected.SetActive(true);
                for (int i = 0; i < usedfaceIcons.Count; i++)
                {
                    if (usedfaceIcons[i].sprite.name == rocketLaucherBP.name) usedfaceIcons[i].GetComponent<Animator>().SetTrigger("rl");
                    if (usedfaceIcons[i].sprite.name == springBP.name) usedfaceIcons[i].GetComponent<Animator>().SetTrigger("s");
                    if (usedfaceIcons[i].sprite.name == freezeGunBP.name) usedfaceIcons[i].GetComponent<Animator>().SetTrigger("fg");
                }
                for (int j = 0; j < faceIcons.Count; j++)
                {
                    faceIcons[j].gameObject.SetActive(false);
                }
                if (springOnBottom) bottomFaceIcon.GetComponent<Animator>().SetTrigger("s");
                if (!springOnBottom) bottomFaceIcon.gameObject.SetActive(false);
            }
            if (!remove)
            {
                removeSelected.SetActive(false);
                for (int i = 0; i < usedfaceIcons.Count; i++)
                {
                    if (usedfaceIcons[i].sprite.name == rocketLaucherBP.name) usedfaceIcons[i].GetComponent<Animator>().SetTrigger("rl_d");
                    if (usedfaceIcons[i].sprite.name == springBP.name) usedfaceIcons[i].GetComponent<Animator>().SetTrigger("s_d");
                    if (usedfaceIcons[i].sprite.name == freezeGunBP.name) usedfaceIcons[i].GetComponent<Animator>().SetTrigger("fg_d");
                }
                if (springOnBottom) bottomFaceIcon.GetComponent<Animator>().SetTrigger("s_d");
            }
            mechanic1 = false;
            mechanic2 = false;
            mechanic3 = false;
            mech1Selected.SetActive(false);
            mech2Selected.SetActive(false);
            mech3Selected.SetActive(false);
        }
    }

    void RemoveMechanic(String whichFace)
    {
        int whichChild = 0;
        Debug.Log("YAY");
        for (int i = 0; i < UseMechanic.mechanicFace.Count; i++)
        {
            if (UseMechanic.mechanicFace[i] == face)
            {
                whichChild = i;
                if (face == new Vector3(0f, 0f, 1f))
                {
                    leftFace = false;
                    leftText.SetActive(true);
                }
                if (face == new Vector3(-1f, 0f, 0f))
                {
                    frontFace = false;
                    frontText.SetActive(true);
                }
                if (face == new Vector3(1f, 0f, 0f))
                {
                    backFace = false;
                    backText.SetActive(true);
                }
                if (face == new Vector3(0f, 0f, -1f))
                {
                    rightFace = false;
                    rightText.SetActive(true);
                }
                if (face == new Vector3(0f, 1f, 0f))
                {
                    bottomFace = false;
                    bottomText.SetActive(true);
                }

                if (playerCube.GetChild(whichChild + 1).gameObject.tag == "rocketlauncher")
                {
                    launcherCount++;
                    launcherText.text = launcherCount.ToString();
                }
                if (playerCube.GetChild(whichChild + 1).gameObject.tag == "spring")
                {
                    springCount++;
                    springText.text = springCount.ToString();
                }
                if (playerCube.GetChild(whichChild + 1).gameObject.tag == "freezegun")
                {
                    freezeCount++;
                    freezeText.text = freezeCount.ToString();
                }

                Destroy(playerCube.GetChild(whichChild + 1).gameObject);
                UseMechanic.mechanicFace.RemoveAt(whichChild);
                SFX.clip = removeSFX;
                SFX.Play();
                remove = false;
                removeMechanic = false;
                removeSelected.SetActive(false);
                break;
            }
        }

        int k = 0;
        if(whichFace != "bottomIcon")
        {
            for (int j = 0; j < usedfaceIcons.Count; j++)
            {
                if (usedfaceIcons[j].gameObject.name == whichFace) k = j;
                if (usedfaceIcons[j].sprite.name == rocketLaucherBP.name) usedfaceIcons[j].GetComponent<Animator>().SetTrigger("rl_d");
                if (usedfaceIcons[j].sprite.name == springBP.name) usedfaceIcons[j].GetComponent<Animator>().SetTrigger("s_d");
                if (usedfaceIcons[j].sprite.name == freezeGunBP.name) usedfaceIcons[j].GetComponent<Animator>().SetTrigger("fg_d");
            }
            Debug.Log(whichFace);
            faceIcons.Add(usedfaceIcons[k]);
            usedfaceIcons[k].gameObject.SetActive(false);
            usedfaceIcons.Remove(usedfaceIcons[k]);
        }
        else
        {
            bottomFaceIcon.sprite = xSprite;
            bottomFaceIcon.gameObject.SetActive(false);
            springOnBottom = false;
        }
    }

    public void ReuseMechanics()
    {
        for (int i = 0; i < previous_mechanics.Count; i++)
        {
            if (previous_mechanics[i] == 1)
            {
                mechanic1 = true;
                mechanic2 = false;
                mechanic3 = false;
                face = previous_faces[i];
                SpawnMechanic(rocketLauncher);
                UseMechanic.mechanicFace.Add(previous_faces[i]);
                launcherText.text = launcherCount.ToString();
            }
            if (previous_mechanics[i] == 2)
            {
                mechanic1 = false;
                mechanic2 = true;
                mechanic3 = false;
                face = previous_faces[i];
                SpawnMechanic(spring);
                UseMechanic.mechanicFace.Add(previous_faces[i]);
                springText.text = springCount.ToString();
            }
            if (previous_mechanics[i] == 3)
            {
                mechanic1 = false;
                mechanic2 = false;
                mechanic3 = true;
                face = previous_faces[i];
                SpawnMechanic(freezeGun);
                UseMechanic.mechanicFace.Add(previous_faces[i]);
                freezeText.text = freezeCount.ToString();
            }
        }
    }

    public void mechanic_one()
    {
        mechanic1 = !mechanic1;
        mechanic2 = false;
        mechanic3 = false;

        if(mechanic1) mech1Selected.SetActive(true);
        if(!mechanic1) mech1Selected.SetActive(false);
        mech2Selected.SetActive(false);
        mech3Selected.SetActive(false);

        if (mechanic1)
        {
            if (!springOnBottom)
            {
                bottomFaceIcon.gameObject.SetActive(true);
                bottomFaceIcon.sprite = xSprite;
                bottomFaceIcon.GetComponent<Animator>().SetTrigger("x");
            }

            for (int i = 0; i < faceIcons.Count; i++)
            {
                faceIcons[i].gameObject.SetActive(true);
                faceIcons[i].sprite = checkSprite;
                faceIcons[i].GetComponent<Animator>().SetTrigger("check");
            }
            for (int j = 0; j < faceButtons.Count; j++)
            {
                faceButtons[j].interactable = true;
                if (faceButtons[j].name == "bottom") faceButtons[j].interactable = false;
            }
        }

        if (!mechanic1)
        {
            for(int i = 0; i < faceIcons.Count; i++)
            {
                faceIcons[i].gameObject.SetActive(false);
            }
            if (!springOnBottom) bottomFaceIcon.gameObject.SetActive(false);

            for(int j = 0; j < faceButtons.Count; j++)
            {
                faceButtons[j].interactable = false;
            }
        }

        if (remove)
        {
            for(int i = 0; i < usedfaceIcons.Count; i++)
            {
                usedfaceIcons[i].GetComponent<Animator>().enabled = false;
            }
            if(springOnBottom) bottomFaceIcon.GetComponent<Animator>().enabled = false;
            remove = false;
            removeSelected.SetActive(false);
        }
    }

    public void mechanic_two()
    {
        mechanic1 = false;
        mechanic2 = !mechanic2;
        mechanic3 = false;

        if (mechanic2) mech2Selected.SetActive(true);
        if (!mechanic2) mech2Selected.SetActive(false);
        mech1Selected.SetActive(false);
        mech3Selected.SetActive(false);

        if (mechanic2)
        {
            for (int i = 0; i < faceIcons.Count; i++)
            {
                faceIcons[i].gameObject.SetActive(true);
                faceIcons[i].sprite = checkSprite;
                faceIcons[i].GetComponent<Animator>().SetTrigger("check");
            }

            if (!springOnBottom)
            {
                bottomFaceIcon.gameObject.SetActive(true);
                bottomFaceIcon.sprite = checkSprite;
                bottomFaceIcon.GetComponent<Animator>().SetTrigger("check");
            }

            for (int j = 0; j < faceButtons.Count; j++)
            {
                faceButtons[j].interactable = true;
            }
        }

        if (!mechanic2)
        {
            for (int i = 0; i < faceIcons.Count; i++)
            {
                faceIcons[i].gameObject.SetActive(false);
            }
            if (!springOnBottom) bottomFaceIcon.gameObject.SetActive(false);

            for (int j = 0; j < faceButtons.Count; j++)
            {
                faceButtons[j].interactable = false;
            }
        }

        if (remove)
        {
            for (int i = 0; i < usedfaceIcons.Count; i++)
            {
                usedfaceIcons[i].GetComponent<Animator>().enabled = false;
            }
            if (springOnBottom) bottomFaceIcon.GetComponent<Animator>().enabled = false;
            remove = false;
            removeSelected.SetActive(false);
        }
    }

    public void mechanic_three()
    {
        mechanic1 = false;
        mechanic2 = false;
        mechanic3 = !mechanic3;

        if (mechanic3) mech3Selected.SetActive(true);
        if (!mechanic3) mech3Selected.SetActive(false);
        mech1Selected.SetActive(false);
        mech2Selected.SetActive(false);

        if (mechanic3)
        {
            if (!springOnBottom)
            {
                bottomFaceIcon.gameObject.SetActive(true);
                bottomFaceIcon.sprite = xSprite;
                bottomFaceIcon.GetComponent<Animator>().SetTrigger("x");
            }

            for (int i = 0; i < faceIcons.Count; i++)
            {
                faceIcons[i].gameObject.SetActive(true);
                faceIcons[i].sprite = checkSprite;
                faceIcons[i].GetComponent<Animator>().SetTrigger("check");
            }
            for (int j = 0; j < faceButtons.Count; j++)
            {
                faceButtons[j].interactable = true;
                if (faceButtons[j].name == "bottom") faceButtons[j].interactable = false;
            }
        }

        if (!mechanic3)
        {
            for (int i = 0; i < faceIcons.Count; i++)
            {
                faceIcons[i].gameObject.SetActive(false);
            }
            if (!springOnBottom) bottomFaceIcon.gameObject.SetActive(false);

            for (int j = 0; j < faceButtons.Count; j++)
            {
                faceButtons[j].interactable = false;
            }
        }

        if (remove)
        {
            for (int i = 0; i < usedfaceIcons.Count; i++)
            {
                usedfaceIcons[i].GetComponent<Animator>().enabled = false;
            }
            if (springOnBottom) bottomFaceIcon.GetComponent<Animator>().enabled = false;
            remove = false;
            removeSelected.SetActive(false);
        }
    }

    void SlotPanel()
    {
        List<int> whichMechanic = new List<int>();

        for (int i = 0; i < slotPositions.Count; i++)
        {
            slotPositions[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < UseMechanic.mechanicFace.Count; i++) // figuring out which slot has which face
        {
            if (UseMechanic.mechanicFace[i] == new Vector3(-1f, 0f, 0f)) slotFaces[i].text = "FRONT";
            if (UseMechanic.mechanicFace[i] == new Vector3(1f, 0f, 0f)) slotFaces[i].text = "BACK";
            if (UseMechanic.mechanicFace[i] == new Vector3(0f, 0f, 1f)) slotFaces[i].text = "LEFT";
            if (UseMechanic.mechanicFace[i] == new Vector3(0f, 0f, -1f)) slotFaces[i].text = "RIGHT";
            if (UseMechanic.mechanicFace[i] == new Vector3(0f, 1f, 0f)) slotFaces[i].text = "BOTTOM";
        }

        for (int i = 0; i < playerCube.childCount; i++) // figuring out which slot has which mechanic
        {
            if (playerCube.GetChild(i).tag == "rocketlauncher") whichMechanic.Add(1);
            if (playerCube.GetChild(i).tag == "spring") whichMechanic.Add(2);
            if (playerCube.GetChild(i).tag == "freezegun") whichMechanic.Add(3);
        }

        for (int j = 0; j < whichMechanic.Count; j++) // applying the correct mechanic image to the correct slot
        {
            if (whichMechanic[j] == 1)
            {
                slotImages[j].sprite = rocketlauncher_sprite;
                slotImages[j].transform.localPosition = new Vector3(0f, -18f, 0f);
                slotImages[j].transform.localScale = new Vector3(3f, 3f, 3f);

                if (!chaos) slotUseCount[j].text = "x2";
                if (chaos) slotUseCount[j].text = "x6";

            }
            if (whichMechanic[j] == 2)
            {
                slotImages[j].sprite = spring_sprite;
                slotImages[j].transform.localPosition = new Vector3(0f, -10f, 0f);
                slotImages[j].transform.localScale = new Vector3(2.2f, 1.2f, 1f);

                if (!chaos) slotUseCount[j].text = "x1";
                if (chaos) slotUseCount[j].text = "x3";
            }
            if (whichMechanic[j] == 3)
            {
                slotImages[j].sprite = freezegun_sprite;
                slotImages[j].transform.localPosition = new Vector3(-3f, -18f, 0f);
                slotImages[j].transform.localScale = new Vector3(2f, 2f, 2f);

                if (!chaos) slotUseCount[j].text = "x1";
                if (chaos) slotUseCount[j].text = "x3";
            }
        }
        slotUseCount_STATIC = slotUseCount;

        if (UseMechanic.mechanicFace.Count == 1)
        {
            slotPositions[0].localPosition = Vector3.zero;
            slotPositions[1].gameObject.SetActive(false);
            slotPositions[2].gameObject.SetActive(false);
            slotPositions[3].gameObject.SetActive(false);
            slotPositions[4].gameObject.SetActive(false);
        }
        if (UseMechanic.mechanicFace.Count == 2)
        {
            slotPositions[0].localPosition = new Vector3(-57.5f, 0f, 0f);
            slotPositions[1].localPosition = new Vector3(57.5f, 0f, 0f);
            slotPositions[2].gameObject.SetActive(false);
            slotPositions[3].gameObject.SetActive(false);
            slotPositions[4].gameObject.SetActive(false);
        }
        if (UseMechanic.mechanicFace.Count == 3)
        {
            slotPositions[0].localPosition = new Vector3(-115f, 0f, 0f);
            slotPositions[1].localPosition = Vector3.zero;
            slotPositions[2].localPosition = new Vector3(115f, 0f, 0f);
            slotPositions[3].gameObject.SetActive(false);
            slotPositions[4].gameObject.SetActive(false);
        }
        if (UseMechanic.mechanicFace.Count == 4)
        {
            slotPositions[0].localPosition = new Vector3(-172.5f, 0f, 0f);
            slotPositions[1].localPosition = new Vector3(-57.5f, 0f, 0f);
            slotPositions[2].localPosition = new Vector3(57.5f, 0f, 0f);
            slotPositions[3].localPosition = new Vector3(172.5f, 0f, 0f);
            slotPositions[4].gameObject.SetActive(false);
        }
        if (UseMechanic.mechanicFace.Count == 5)
        {
            slotPositions[0].localPosition = new Vector3(-230f, 0f, 0f);
            slotPositions[1].localPosition = new Vector3(-115f, 0f, 0f);
            slotPositions[2].localPosition = Vector3.zero;
            slotPositions[3].localPosition = new Vector3(115f, 0f, 0f);
            slotPositions[4].localPosition = new Vector3(230f, 0f, 0f);
        }
    }

    public void Play()
    {
        play = true;

        if(chaos)
        {
            for(int i = 0; i < playerCube.childCount; i++)
            {
                if(playerCube.GetChild(i).gameObject.tag == "rocketlauncher")
                {
                    playerCube.GetChild(i).gameObject.GetComponent<RocketLauncherClass>().rocketCount *= 3;
                }
                if (playerCube.GetChild(i).gameObject.tag == "spring")
                {
                    playerCube.GetChild(i).gameObject.GetComponent<SpringClass>().springCount *= 3;
                }
                if (playerCube.GetChild(i).gameObject.tag == "freezegun")
                {
                    playerCube.GetChild(i).gameObject.GetComponent<FreezeGunClass>().freezeCount *= 3;
                }
            }
        }

        SlotPanel();

        if (!chaos) cameraAnim.SetTrigger("off");
        if (chaos) cameraAnim.SetTrigger("chaos");

        buildPanelAnim.SetTrigger("off");

        slotPanelAnim.SetTrigger("in");

        for (int i = 0; i < retryOffColliders.Count; i++)
        {
            retryOffColliders[i].enabled = false;
            if (retryOffColliders[i].gameObject.name == "lava") retryOffColliders[i].isTrigger = false;
        }
        Vector3 bottomForce = new Vector3(0f, -1f, 0f);

        if (!chaos) opener.SetTrigger("open");
        if (chaos) opener.SetTrigger("chaos");

        playerCube.transform.localPosition = retryPosition;
        playerCubeRB.constraints = RigidbodyConstraints.None;
        playerCubeRB.constraints = RigidbodyConstraints.FreezeRotation;
        playerCubeRB.AddForce(-bottomForce * openerForceStart);
        turnOnColliders = true;
        oneTime3 = true;

        if(tutorial)
        {
            signTut.SetActive(false);
            rotateTut.SetActive(false);
            startMarkerTut.SetActive(false);
            slotTut.SetActive(true);
        }
    }

    public void Retry()
    {
        Vector3 bottomForce = new Vector3(0f, -1f, 0f);
        if(oneTime4)
        {
            if (!chaos) opener.SetTrigger("open");
            if (chaos) opener.SetTrigger("chaos");
            ReuseMechanics();
            SlotPanel();
            if (play)
            {
                if (!chaos) cameraAnim.SetTrigger("offScreen");
                if (chaos) cameraAnim.SetTrigger("OSchaos");

                buildPanelAnim.SetTrigger("offScreen");
                slotPanelAnim.SetTrigger("offScreen");
            }
            oneTime5 = true;
            oneTime4 = false;
        }
        // another bool
        if(Time.timeSinceLevelLoad > time + 0.5f && oneTime5)
        {
            for (int i = 0; i < retryOffColliders.Count; i++)
            {
                retryOffColliders[i].enabled = false;
                if (retryOffColliders[i].gameObject.name == "lava") retryOffColliders[i].isTrigger = false;
            }
            playerCube.transform.localPosition = retryPosition;
            playerCubeRB.constraints = RigidbodyConstraints.FreezeRotation;
            playerCubeRB.AddForce(-bottomForce * openerForceStart);
            oneTime5 = false;
        }

        if(Time.timeSinceLevelLoad > time + 1.5f)
        {
            for (int i = 0; i < retryOffColliders.Count; i++)
            {
                retryOffColliders[i].enabled = true;
                if (retryOffColliders[i].gameObject.name == "lava") retryOffColliders[i].isTrigger = true;
            }
        }

        if (Time.timeSinceLevelLoad > time + 2f)
        {
            retry = false;
            oneTime = false;
            oneTime2 = false;
        }
    
    }

    IEnumerator RetryLevelCoroutine()
    {
        if(!oneTime)
        {
            Physics.gravity = new Vector3(0f, -9.8f, 0f);
            asyncop = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            UseMechanic.mechanicFace.Clear();
            slotUseCount_STATIC.Clear();
            asyncop.allowSceneActivation = true;
            oneTime = true;
        }

        while(asyncop != null && !asyncop.isDone)
        {
            yield return null;
        }

        if(!oneTime2)
        {
            time = Time.timeSinceLevelLoad;
            retry = true;
            oneTime4 = true;
            if (Lava.died) Lava.died = false;
            if (OutOfBounds.died) OutOfBounds.died = false;
            ZeroGravity.inField = false;

            if (chaos)
            {
                for (int i = 0; i < playerCube.childCount; i++)
                {
                    if (playerCube.GetChild(i).gameObject.tag == "rocketlauncher")
                    {
                        playerCube.GetChild(i).gameObject.GetComponent<RocketLauncherClass>().rocketCount *= 3;
                    }
                    if (playerCube.GetChild(i).gameObject.tag == "spring")
                    {
                        playerCube.GetChild(i).gameObject.GetComponent<SpringClass>().springCount *= 3;
                    }
                    if (playerCube.GetChild(i).gameObject.tag == "freezegun")
                    {
                        playerCube.GetChild(i).gameObject.GetComponent<FreezeGunClass>().freezeCount *= 3;
                    }
                }
            }
        } 
    }
}
