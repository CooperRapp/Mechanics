using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PickFace3D : MonoBehaviour
{
    RaycastHit hit;
    Ray ray;
    Vector3 mousePos;
    Vector3 changeInMousePos;
    //[SerializeField] float rotationSpeed = 0.001f;

    public static Vector3 face;

    public Transform playerCube;
    public Transform viewportCube;
    public GameObject leftArrow;
    public GameObject rightArrow;

    //public static bool play;
    bool oneTime;

    Quaternion mechanicRotationPL;
    Vector3 mechanicPositionPL;

    Vector3 mechanicPositionVP;
    Quaternion mechanicRotationVP;
    Vector3 mechanicScaleVP;

    bool mechanic1, mechanic2, mechanic3;

    [SerializeField] int launcherCount = 1;
    [SerializeField] int springCount = 2;
    [SerializeField] int freezeCount = 2;

    public TextMeshProUGUI launcherText;
    public TextMeshProUGUI springText;
    public TextMeshProUGUI freezeText;

    bool faceTop, faceBottom, faceLeftWall, faceLeftOpen, faceRightWall, faceRightOpen;

    public static bool springOnBottom;
    bool applyMechanic;
    bool reuseMechanics;
    bool removeMechanic;

    public Sprite rocketlauncher_sprite;
    public Sprite spring_sprite;
    public Sprite freezegun_sprite;

    [Header("Play Button")]
    public MeshRenderer pbColor;
    public Material pbGreen;
    public Material pbRed;

    [Header("Slot Positions")]
    public Vector2 slot1;
    public Vector2 slot2;
    public Vector2 slot3;
    public Vector2 slot4;
    public Vector2 slot5;
    public GameObject slot;
    public GameObject panel;
    public GameObject parentPanel;
    public static List<GameObject> slots = new List<GameObject>();
    public GameObject buildPanel;
    public List<GameObject> test_slots = new List<GameObject>();

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

    [Header("Retry")]
    static List<int> previous_mechanics = new List<int>();
    static List<Vector3> previous_faces = new List<Vector3>();
    bool remove;

    [Header("Audio Stuff")]
    public AudioSource SFX;
    public AudioClip removeSFX;
    public AudioClip reuseSFX;

    [Header("Mechanial Arm")]
    public Animator mechanicalArm;
    public Animator viewportCubeAnim;
    public GameObject pillarRL;
    public MeshRenderer pillarRL_parent;
    public GameObject pillarS;
    public MeshRenderer pillarS_parent;
    public GameObject pillarFG;
    public MeshRenderer pillarFG_parent;
    public Animator pillarsController;
    public Material selectedPillar;
    public Material normalPillar;
    public Material hoverPillar;
    public ParticleSystem reuseParticles;

    [Header("Reuse/Remove Buttons")]
    public MeshRenderer reuseButton;
    public MeshRenderer removeButton;
    public Material hoverLight;

    float time;

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

        oneTime = true;
    }

    void Update()
    {
        detectClick();

        playButtonColor();

        if(applyMechanic)
        {
            ApplyMechanic();
            if (mechanic1) mechanic_one();
            if (mechanic2) mechanic_two();
            if (mechanic3) mechanic_three();
        }

        if(reuseMechanics)
        {
            ReuseMechanics();
        }

        if(removeMechanic)
        {
            RemoveMechanic();
        }

        if(BuildPanel.play && oneTime)
        {
            Play();
            oneTime = false;
        }    
    }

    void detectClick()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePos = Input.mousePosition; // dont think i need this anymore

                if (hit.collider.tag == "pillar_RL") mechanic_one();
                if (hit.collider.tag == "pillar_S") mechanic_two();
                if (hit.collider.tag == "pillar_FG") mechanic_three();

                if (hit.collider.tag == "cube")
                {
                    Debug.Log(face);
                    face = hit.transform.InverseTransformDirection(hit.normal);

                    if (!remove) applyMechanic = true;
                    if (remove) removeMechanic = true;
                    time = Time.time;
                }
            }

            // REUSE & REMOVE HOVER -----------------------------
            if (hit.collider.tag == "reuseMech")
            {
                reuseButton.material = hoverLight;
            }
            else reuseButton.material = normalPillar;

            if (hit.collider.tag == "removeMech" && !remove)
            {
                removeButton.material = hoverLight;
            }
            else if(hit.collider.tag != "removeMech" && !remove) removeButton.material = normalPillar;
            else if (remove) removeButton.material = hoverLight;
            // --------------------------------------------------

            // PILLAR HOVER -------------------------------------
            if (hit.collider.tag == "pillar_RL" && !mechanic1) pillarRL_parent.material = hoverPillar;
            else if (hit.collider.tag != "pillar_RL" && !mechanic1) pillarRL_parent.material = normalPillar;

            if (hit.collider.tag == "pillar_S" && !mechanic2) pillarS_parent.material = hoverPillar;
            else if (hit.collider.tag != "pillar_S" && !mechanic2) pillarS_parent.material = normalPillar;

            if (hit.collider.tag == "pillar_FG" && !mechanic3) pillarFG_parent.material = hoverPillar;
            else if (hit.collider.tag != "pillar_FG" && !mechanic3) pillarFG_parent.material = normalPillar;
            // --------------------------------------------------

        }
    }

    void AddSlot(Sprite mechanic, Vector3 img_scale, int useCount)
    {
        GameObject g = Instantiate(slot, Vector3.zero, Quaternion.identity);
        g.GetComponentInChildren<Image>().sprite = mechanic;
        g.GetComponentInChildren<Image>().transform.localScale = img_scale;
        g.GetComponentInChildren<TextMeshProUGUI>().text = (slots.Count + 1) + ":        X " + useCount;
        g.transform.parent = panel.transform;
        g.transform.localScale = new Vector3(2.1f, 2.1f, 2.1f);
        g.transform.localPosition = slot1;
        g.transform.localRotation = Quaternion.Euler(Vector3.zero);
        slots.Add(g);
    }

    //void OnMouseDrag()
    //{
    //    changeInMousePos = Input.mousePosition - mousePos;
    //    transform.Rotate(Vector3.up, -(changeInMousePos.x * rotationSpeed), Space.World);
    //    transform.Rotate(Vector3.right, -(changeInMousePos.y * rotationSpeed), Space.World);
    //}

    void ApplyMechanic()
    {
        if (slots.Count == 0 )
        {
            previous_mechanics.Clear();
            previous_faces.Clear();
        }

        if (mechanic1 && launcherCount >= 1)
        {
            viewportCubeAnim.enabled = true;
            if (Time.time < time + 0.001f)
            {
                mechanicalArm.SetTrigger("grabRL");
                viewportCubeAnim.SetTrigger("yummy");
            }

            if (Time.time > time + 0.5f)
            {
                pillarRL.transform.parent.parent.GetComponent<Animator>().enabled = false;
                pillarRL.SetActive(false);
            }

            if (Time.time > time + 1.4f)
            {
                SpawnMechanic(rocketLauncher);
                launcherText.text = launcherCount.ToString();
                UseMechanic.mechanicFace.Add(face);

                previous_mechanics.Add(1);
                previous_faces.Add(face);

                if (launcherCount >= 1)
                {
                    pillarRL.transform.parent.parent.GetComponent<Animator>().enabled = true;
                    pillarsController.SetTrigger("pillarRL");
                }
            }
            if (Time.time > time + 2f)
            {
                viewportCubeAnim.enabled = false;
                applyMechanic = false;
            }

        }
        if (mechanic2 && springCount >= 1)
        {
            viewportCubeAnim.enabled = true;
            if (Time.time < time + 0.001f)
            {
                mechanicalArm.SetTrigger("grabS");
                viewportCubeAnim.SetTrigger("yummy");
            }

            if (Time.time > time + 0.5f)
            {
                pillarS.transform.parent.parent.GetComponent<Animator>().enabled = false;
                pillarS.SetActive(false);
            }

            if (Time.time > time + 1.4f)
            {
                SpawnMechanic(spring);
                springText.text = springCount.ToString();
                UseMechanic.mechanicFace.Add(face);

                previous_mechanics.Add(2);
                previous_faces.Add(face);

                if (springCount >= 1)
                {
                    pillarS.transform.parent.parent.GetComponent<Animator>().enabled = true;
                    pillarsController.SetTrigger("pillarS");
                }
            }
            if(Time.time > time + 2f)
            {
                viewportCubeAnim.enabled = false;
                applyMechanic = false;
            }
        }
        if (mechanic3 && freezeCount >= 1)
        {
            viewportCubeAnim.enabled = true;
            if (Time.time < time + 0.001f)
            {
                mechanicalArm.SetTrigger("grabFG");
                viewportCubeAnim.SetTrigger("yummy");
            }

            if (Time.time > time + 0.5f)
            {
                pillarFG.transform.parent.parent.GetComponent<Animator>().enabled = false;
                pillarFG.SetActive(false);
            }

            if (Time.time > time + 1.4f)
            {
                SpawnMechanic(freezeGun);
                freezeText.text = freezeCount.ToString();
                UseMechanic.mechanicFace.Add(face);

                previous_mechanics.Add(3);
                previous_faces.Add(face);

                if (freezeCount >= 1)
                {
                    pillarFG.transform.parent.parent.GetComponent<Animator>().enabled = true;
                    pillarsController.SetTrigger("pillarFG");
                }
            }
            if (Time.time > time + 2f)
            {
                viewportCubeAnim.enabled = false;
                applyMechanic = false;
            }
        }
    }

    void SpawnMechanic(GameObject mechanic)
    {
        if (face == new Vector3(0f, 0f, 1f) && !faceRightOpen) //facing right open
        {            
            if (!ZeroGravity.inField)
            {
                if (mechanic1)
                {
                    mechanicPositionPL = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z + 0.6f);
                    mechanicPositionVP = new Vector3(viewportCube.position.x, viewportCube.position.y, viewportCube.position.z + 1.8f);
                    mechanicRotationVP = Quaternion.Euler(0f, 90f, 90f);
                    mechanicScaleVP = new Vector3(1f, 1f, 1f);
                    launcherCount--;
                    AddSlot(rocketlauncher_sprite, new Vector3(2f, 2f, 2f), 2);
                }

                if (mechanic2)
                {
                    mechanicPositionPL = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z + 0.1f);
                    mechanicPositionVP = new Vector3(viewportCube.position.x, viewportCube.position.y, viewportCube.position.z + 0.1f);
                    mechanicRotationVP = Quaternion.Euler(0f, 90f, 0f);
                    mechanicScaleVP = new Vector3(0.05f, 0.05f, 0.05f);
                    springCount--;
                    AddSlot(spring_sprite, new Vector3(1.5f, 0.8f, 1f), 1);
                }

                if (mechanic3)
                {
                    mechanicPositionPL = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z + 0.1f);
                    mechanicPositionVP = new Vector3(viewportCube.position.x, viewportCube.position.y, viewportCube.position.z + 0.1f);
                    mechanicRotationVP = Quaternion.Euler(0f, 90f, 0f);
                    mechanicScaleVP = new Vector3(0.15f, 0.35f, 0.35f);
                    freezeCount--;
                    AddSlot(freezegun_sprite, new Vector3(1.2f, 1.2f, 1.2f), 1);
                }
            }
            else if(ZeroGravity.inField)
            {
                if (mechanic1)
                {
                    mechanicPositionPL = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z - 0.6f);
                    mechanicPositionVP = new Vector3(viewportCube.position.x, viewportCube.position.y, viewportCube.position.z - 1.8f);
                    mechanicRotationVP = Quaternion.Euler(0f, -90f, 90f);
                    mechanicScaleVP = new Vector3(1f, 1f, 1f);
                    launcherCount--;
                    AddSlot(rocketlauncher_sprite, new Vector3(2f, 2f, 2f), 2);
                }

                if (mechanic2)
                {
                    mechanicPositionPL = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z - 0.1f);
                    mechanicPositionVP = new Vector3(viewportCube.position.x, viewportCube.position.y, viewportCube.position.z - 0.1f);
                    mechanicRotationVP = Quaternion.Euler(0f, -90f, 0f);
                    mechanicScaleVP = new Vector3(0.05f, 0.05f, 0.05f);
                    springCount--;
                    AddSlot(spring_sprite, new Vector3(1.5f, 0.8f, 1f), 1);
                }

                if (mechanic3)
                {
                    mechanicPositionPL = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z - 0.1f);
                    mechanicPositionVP = new Vector3(viewportCube.position.x, viewportCube.position.y, viewportCube.position.z - 0.1f);
                    mechanicRotationVP = Quaternion.Euler(0f, -90f, 0f);
                    mechanicScaleVP = new Vector3(0.15f, 0.35f, 0.35f);
                    freezeCount--;
                    AddSlot(freezegun_sprite, new Vector3(1.2f, 1.2f, 1.2f), 1);
                }
            }
            

            GameObject mech = Instantiate(mechanic, mechanicPositionPL, mechanicRotationVP, playerCube.transform);
            //mech.transform.localRotation = mechanicRotationVP;
            //mech.transform.localPosition = new Vector3(0f, 0f, 0.6f);

            Quaternion currentRotatation = viewportCube.rotation;
            viewportCube.rotation = Quaternion.Euler(0f, 0f, 0f);
            GameObject mech_VP = Instantiate(mechanic, mechanicPositionVP, mechanicRotationVP, viewportCube.transform);
            mech_VP.transform.localScale = mechanicScaleVP;
            viewportCube.rotation = currentRotatation;

            faceRightOpen = true;
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
        if (face == new Vector3(-1f, 0f, 0f) && !faceLeftOpen) //facing left open
        {
            if (mechanic1)
            {
                mechanicPositionPL = new Vector3(playerCube.position.x - 0.6f, playerCube.position.y, playerCube.position.z);
                mechanicPositionVP = new Vector3(viewportCube.position.x - 1.8f, viewportCube.position.y, viewportCube.position.z);
                mechanicRotationVP = Quaternion.Euler(0f, 0f, 90f);
                mechanicScaleVP = new Vector3(1f, 1f, 1f);
                launcherCount--;
                AddSlot(rocketlauncher_sprite, new Vector3(2f, 2f, 2f), 2);
            }

            if (mechanic2)
            {
                mechanicPositionPL = new Vector3(playerCube.position.x - 0.1f, playerCube.position.y, playerCube.position.z);
                mechanicPositionVP = new Vector3(viewportCube.position.x - 0.1f, viewportCube.position.y, viewportCube.position.z);
                mechanicRotationVP = Quaternion.Euler(0f, 0f, 0f);
                mechanicScaleVP = new Vector3(0.05f, 0.05f, 0.05f);
                springCount--;
                AddSlot(spring_sprite, new Vector3(1.5f, 0.8f, 1f), 1);
            }

            if (mechanic3)
            {
                mechanicPositionPL = new Vector3(playerCube.position.x - 0.1f, playerCube.position.y, playerCube.position.z);
                mechanicPositionVP = new Vector3(viewportCube.position.x - 0.1f, viewportCube.position.y, viewportCube.position.z);
                mechanicRotationVP = Quaternion.Euler(0f, 0f, 0f);
                mechanicScaleVP = new Vector3(0.15f, 0.35f, 0.35f);
                freezeCount--;
                AddSlot(freezegun_sprite, new Vector3(1.2f, 1.2f, 1.2f), 1);
            }

            GameObject mech = Instantiate(mechanic, mechanicPositionPL, mechanicRotationVP, playerCube.transform);

            Quaternion currentRotatation = viewportCube.rotation;
            viewportCube.rotation = Quaternion.Euler(0f, 0f, 0f);
            GameObject mech_VP = Instantiate(mechanic, mechanicPositionVP, mechanicRotationVP, viewportCube.transform);
            mech_VP.transform.localScale = mechanicScaleVP;
            viewportCube.rotation = currentRotatation;

            faceLeftOpen = true;
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
        if (face == new Vector3(1f, 0f, 0f) && !faceRightWall) //facing right wall
        {
            if (mechanic1)
            {
                mechanicPositionPL = new Vector3(playerCube.position.x + 0.6f, playerCube.position.y, playerCube.position.z);
                mechanicPositionVP = new Vector3(viewportCube.position.x + 1.8f, viewportCube.position.y, viewportCube.position.z);
                mechanicRotationVP = Quaternion.Euler(0f, 180f, 90f);
                mechanicScaleVP = new Vector3(1f, 1f, 1f);
                launcherCount--;
                AddSlot(rocketlauncher_sprite, new Vector3(2f, 2f, 2f), 2);
            }

            if (mechanic2)
            {
                mechanicPositionPL = new Vector3(playerCube.position.x + 0.1f, playerCube.position.y, playerCube.position.z);
                mechanicPositionVP = new Vector3(viewportCube.position.x + 0.1f, viewportCube.position.y, viewportCube.position.z);
                mechanicRotationVP = Quaternion.Euler(0f, 180f, 0f);
                mechanicScaleVP = new Vector3(0.05f, 0.05f, 0.05f);
                springCount--;
                AddSlot(spring_sprite, new Vector3(1.5f, 0.8f, 1f), 1);
            }

            if (mechanic3)
            {
                mechanicPositionPL = new Vector3(playerCube.position.x + 0.1f, playerCube.position.y, playerCube.position.z);
                mechanicPositionVP = new Vector3(viewportCube.position.x + 0.1f, viewportCube.position.y, viewportCube.position.z);
                mechanicRotationVP = Quaternion.Euler(0f, 180f, 0f);
                mechanicScaleVP = new Vector3(0.15f, 0.35f, 0.35f);
                freezeCount--;
                AddSlot(freezegun_sprite, new Vector3(1.2f, 1.2f, 1.2f), 1);
            }

            GameObject mech = Instantiate(mechanic, mechanicPositionPL, mechanicRotationVP, playerCube.transform);

            Quaternion currentRotatation = viewportCube.rotation;
            viewportCube.rotation = Quaternion.Euler(0f, 0f, 0f);
            GameObject mech_VP = Instantiate(mechanic, mechanicPositionVP, mechanicRotationVP, viewportCube.transform);
            mech_VP.transform.localScale = mechanicScaleVP;
            viewportCube.rotation = currentRotatation;

            faceRightWall = true;
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
        if (face == new Vector3(0f, 0f, -1f) && !faceLeftWall) //facing left wall
        {
            if (!ZeroGravity.inField)
            {
                if (mechanic1)
                {
                    mechanicPositionPL = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z - 0.6f);
                    mechanicPositionVP = new Vector3(viewportCube.position.x, viewportCube.position.y, viewportCube.position.z - 1.8f);
                    mechanicRotationVP = Quaternion.Euler(0f, -90f, 90f);
                    mechanicScaleVP = new Vector3(1f, 1f, 1f);
                    launcherCount--;
                    AddSlot(rocketlauncher_sprite, new Vector3(2f, 2f, 2f), 2);
                }

                if (mechanic2)
                {
                    mechanicPositionPL = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z - 0.1f);
                    mechanicPositionVP = new Vector3(viewportCube.position.x, viewportCube.position.y, viewportCube.position.z - 0.1f);
                    mechanicRotationVP = Quaternion.Euler(0f, -90f, 0f);
                    mechanicScaleVP = new Vector3(0.05f, 0.05f, 0.05f);
                    springCount--;
                    AddSlot(spring_sprite, new Vector3(1.5f, 0.8f, 1f), 1);
                }

                if (mechanic3)
                {
                    mechanicPositionPL = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z - 0.1f);
                    mechanicPositionVP = new Vector3(viewportCube.position.x, viewportCube.position.y, viewportCube.position.z - 0.1f);
                    mechanicRotationVP = Quaternion.Euler(0f, -90f, 0f);
                    mechanicScaleVP = new Vector3(0.15f, 0.35f, 0.35f);
                    freezeCount--;
                    AddSlot(freezegun_sprite, new Vector3(1.2f, 1.2f, 1.2f), 1);
                }
            }
            else if(ZeroGravity.inField)
            {
                if (mechanic1)
                {
                    mechanicPositionPL = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z + 0.6f);
                    mechanicPositionVP = new Vector3(viewportCube.position.x, viewportCube.position.y, viewportCube.position.z + 1.8f);
                    mechanicRotationVP = Quaternion.Euler(0f, 90f, 90f);
                    mechanicScaleVP = new Vector3(1f, 1f, 1f);
                    launcherCount--;
                    AddSlot(rocketlauncher_sprite, new Vector3(2f, 2f, 2f), 2);
                }

                if (mechanic2)
                {
                    mechanicPositionPL = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z + 0.1f);
                    mechanicPositionVP = new Vector3(viewportCube.position.x, viewportCube.position.y, viewportCube.position.z + 0.1f);
                    mechanicRotationVP = Quaternion.Euler(0f, 90f, 0f);
                    mechanicScaleVP = new Vector3(0.05f, 0.05f, 0.05f);
                    springCount--;
                    AddSlot(spring_sprite, new Vector3(1.5f, 0.8f, 1f), 1);
                }

                if (mechanic3)
                {
                    mechanicPositionPL = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z + 0.1f);
                    mechanicPositionVP = new Vector3(viewportCube.position.x, viewportCube.position.y, viewportCube.position.z + 0.1f);
                    mechanicRotationVP = Quaternion.Euler(0f, 90f, 0f);
                    mechanicScaleVP = new Vector3(0.15f, 0.35f, 0.35f);
                    freezeCount--;
                    AddSlot(freezegun_sprite, new Vector3(1.2f, 1.2f, 1.2f), 1);
                }
            }

            GameObject mech = Instantiate(mechanic, mechanicPositionPL, mechanicRotationVP, playerCube.transform);

            Quaternion currentRotatation = viewportCube.rotation;
            viewportCube.rotation = Quaternion.Euler(0f, 0f, 0f);
            GameObject mech_VP = Instantiate(mechanic, mechanicPositionVP, mechanicRotationVP, viewportCube.transform);
            mech_VP.transform.localScale = mechanicScaleVP;
            viewportCube.rotation = currentRotatation;

            faceLeftWall = true;
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
        if (mechanic2 && face == new Vector3(0f, 1f, 0f) && !faceBottom) // Vector3(0f, -1f, 0f)
        {
            mechanicPositionPL = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z);
            mechanicPositionVP = new Vector3(viewportCube.position.x, viewportCube.position.y, viewportCube.position.z);
           
            if(!ZeroGravity.inField) mechanicRotationVP = Quaternion.Euler(0f, 0f, 90f);
            else if(ZeroGravity.inField) mechanicRotationVP = Quaternion.Euler(0f, 0f, -90f);

            mechanicScaleVP = new Vector3(0.05f, 0.05f, 0.05f);
            springCount--;
            AddSlot(spring_sprite, new Vector3(1.5f, 0.8f, 1f), 1);

            GameObject mech = Instantiate(mechanic, mechanicPositionPL, mechanicRotationVP, playerCube.transform);
            if (!ZeroGravity.inField) playerCube.position = new Vector3(playerCube.position.x, playerCube.position.y + 0.5f, playerCube.position.z);
            else if(ZeroGravity.inField) playerCube.position = new Vector3(playerCube.position.x, playerCube.position.y - 0.5f, playerCube.position.z);

            Quaternion currentRotatation = viewportCube.rotation;
            viewportCube.rotation = Quaternion.Euler(0f, 0f, 0f);
            GameObject mech_VP = Instantiate(mechanic, mechanicPositionVP, mechanicRotationVP, viewportCube.transform);
            mech_VP.transform.localScale = mechanicScaleVP;
            viewportCube.rotation = currentRotatation;

            faceBottom = true;
            springOnBottom = true;
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
        //if (mechanic2 && face == new Vector3(0f, 1f, 0f) && !faceTop)
        //{
        //    mechanicPositionPL = new Vector3(playerCube.position.x, playerCube.position.y, playerCube.position.z);
        //    mechanicPositionVP = new Vector3(viewportCube.position.x, viewportCube.position.y, viewportCube.position.z);
        //    mechanicRotationVP = Quaternion.Euler(0f, 0f, -90f);
        //    mechanicScaleVP = new Vector3(0.05f, 0.05f, 0.05f);
        //    springCount--;

        //    GameObject mech = Instantiate(mechanic, mechanicPositionPL, mechanicRotationVP);
        //    mech.transform.parent = playerCube.transform;

        //    Quaternion currentRotatation = viewportCube.rotation;
        //    viewportCube.rotation = Quaternion.Euler(0f, 0f, 0f);
        //    GameObject mech_VP = Instantiate(mechanic, mechanicPositionVP, mechanicRotationVP);
        //    mech_VP.transform.parent = viewportCube.transform;
        //    mech_VP.transform.localScale = mechanicScaleVP;
        //    viewportCube.rotation = currentRotatation;

        //    faceTop = true;
        //}
    }

    void playButtonColor()
    {
        if (UseMechanic.mechanicFace.Count == 0)
        {
            pbColor.material = pbRed;
            pbColor.gameObject.tag = "Untagged";
        }
        else
        {
            pbColor.material = pbGreen;
            pbColor.gameObject.tag = "playbutton";
        }
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

    public void Remove()
    {
        remove = !remove;
    }

    void RemoveMechanic()
    {
        if (Time.time < time + 0.001f) mechanicalArm.SetTrigger("reuse"); // play animation

        if (Time.time > time + 0.80f)
        {
            int whichChild = 0;
            for (int i = 0; i < UseMechanic.mechanicFace.Count; i++)
            {
                if (UseMechanic.mechanicFace[i] == face)
                {
                    whichChild = i;
                    if (face == new Vector3(0f, 0f, 1f)) faceRightOpen = false;
                    if (face == new Vector3(-1f, 0f, 0f)) faceLeftOpen = false;
                    if (face == new Vector3(1f, 0f, 0f)) faceRightWall = false;
                    if (face == new Vector3(0f, 0f, -1f)) faceLeftWall = false;
                    if (face == new Vector3(0f, 1f, 0f)) faceBottom = false;

                    if (playerCube.GetChild(whichChild + 3).gameObject.tag == "rocketlauncher")
                    {
                        launcherCount++;
                        launcherText.text = launcherCount.ToString();
                    }

                    if (playerCube.GetChild(whichChild + 3).gameObject.tag == "spring")
                    {
                        springCount++;
                        springText.text = springCount.ToString();
                    } 
                    if (playerCube.GetChild(whichChild + 3).gameObject.tag == "freezegun")
                    {
                        freezeCount++;
                        freezeText.text = freezeCount.ToString();
                    }

                    Destroy(playerCube.GetChild(whichChild + 3).gameObject); // add 3 becuase the player is [0] and the eyes/mouth is [1-3]
                    Destroy(viewportCube.GetChild(whichChild + 3).gameObject);
                    Destroy(panel.transform.GetChild(whichChild).gameObject);
                    UseMechanic.mechanicFace.RemoveAt(whichChild);
                    slots.RemoveAt(whichChild);
                    SFX.clip = removeSFX;
                    SFX.Play();
                    reuseParticles.Play();
                    remove = false;
                    removeMechanic = false;
                    break;
                }
            }
            remove = false;
            removeMechanic = false;
        }
        
    }

    public void Retry()
    {
        time = Time.time;
        if(UseMechanic.mechanicFace.Count == 0) reuseMechanics = true;
    }

    void ReuseMechanics()
    {
        if (Time.time < time + 0.001f)
        {
            viewportCubeAnim.enabled = true;
            viewportCubeAnim.SetTrigger("reuse"); // play animation
        }
        if (Time.time > time + 0.80f) // apply past mechanics
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
            SFX.clip = reuseSFX;
            SFX.Play();
        }
        if (Time.time > time + 1.5f)
        {
            viewportCubeAnim.enabled = false;
            reuseMechanics = false;
        }
    }

    public void mechanic_one()
    {
        mechanic1 = true;
        pillarRL_parent.material = selectedPillar;
        mechanic2 = false;
        pillarS_parent.material = normalPillar;
        mechanic3 = false;
        pillarFG_parent.material = normalPillar;

        pillarsController.SetTrigger("rotateRL");
    }
    public void mechanic_two()
    {
        mechanic1 = false;
        pillarRL_parent.material = normalPillar;
        mechanic2 = true;
        pillarS_parent.material = selectedPillar;
        mechanic3 = false;
        pillarFG_parent.material = normalPillar;

        pillarsController.SetTrigger("rotateS");
    }
    public void mechanic_three()
    {
        mechanic1 = false;
        pillarRL_parent.material = normalPillar;
        mechanic2 = false;
        pillarS_parent.material = normalPillar;
        mechanic3 = true;
        pillarFG_parent.material = selectedPillar;

        pillarsController.SetTrigger("rotateFG");
    }

    public void Play()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i == 0) slots[i].transform.localPosition = slot1;
            if (i == 1) slots[i].transform.localPosition = slot2;
            if (i == 2) slots[i].transform.localPosition = slot3;
            if (i == 3) slots[i].transform.localPosition = slot4;
            if (i == 4) slots[i].transform.localPosition = slot5;
        }
        buildPanel.SetActive(false);
        mechanicalArm.gameObject.SetActive(false);
        viewportCube.gameObject.SetActive(false);
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
        parentPanel.SetActive(true);
    }
}
