using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EndPoint : MonoBehaviour
{
    [Header("End Point Stuff")]
    Animator anim;
    public static int levelNumber;

    public Transform playerCube;
    public Animator playerCubeAnim;
    public Vector3 playerPoint;

    public Transform camera;
    public Camera cam;

    public ParticleSystem cakeEat;
    public ParticleSystem frostingEat;

    float endPointYPos;
    float endPointYPosUpper;
    float endPointYPosLower;
    float endPointState;

    bool startCutScene;

    float time;

    int buildIndex; 

    public GameObject CreditsPage;

    public bool killVelocity;

    bool done;
    bool stop, stop2;

    Volume vol;
    Vignette v;

    public bool chaos;

    void Start()
    {
        anim = GetComponent<Animator>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        anim.enabled = false;
        endPointYPos = this.transform.position.y;
        endPointYPosUpper = endPointYPos + 1f;
        endPointYPosLower = endPointYPos;
        endPointState = 1;

        buildIndex = SceneManager.GetActiveScene().buildIndex - 1;

        done = false;
        stop = false;
        stop2 = false;

        vol = GameObject.FindGameObjectWithTag("volume").GetComponent<Volume>();
    }

    void Update()
    {
        if(endPointState == 1)
        {
            endPointYPos = Mathf.MoveTowards(endPointYPos, endPointYPosUpper, 0.5f * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, endPointYPos, transform.position.z);
            if (endPointYPosUpper - endPointYPos <= 0.1) endPointState = 0;  
        }
        if (endPointState == 0)
        {
            endPointYPos = Mathf.MoveTowards(endPointYPos, endPointYPosLower, 0.5f * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, endPointYPos, transform.position.z);
            if (endPointYPos - endPointYPosLower <= 0.1) endPointState = 1;
        }

        if (startCutScene)
        {
            if(!stop) MoveCamera();

            if (Time.time < time + 0.5f)
            {
                playerCube.position = Vector3.MoveTowards(playerCube.position, playerPoint, 7f * Time.deltaTime);
            }
            if (Time.time > time + 1f && !stop)
            {
                playerCubeAnim.SetTrigger("cake"); // eating cake
                anim.SetTrigger("cake"); // moving cake
                stop = true;
            }
            if(Time.time > time + 1.95f && !stop2)
            {
                cakeEat.Play();
                frostingEat.Play();
                stop2 = true;
            }
            if(Time.time > time + 3f)
            {
                startCutScene = false;
                done = true;
            }
        }

        if (SceneManager.GetActiveScene().name == "level15" && done && PlayerPrefs.GetInt("NOCREDITS") != 1)
        {
            PlayerPrefs.SetInt("NOCREDITS", 1);
            CreditsPage.SetActive(true);
            done = false;
            v.rounded.value = true;
            v.intensity.value = 0.25f;
            v.smoothness.value = 1f;
        }
        else
        {
            if (done)
            { 
                PickFace.play = false;
                PickFace.springOnBottom = false;
                ZeroGravity.inField = false;
                Physics.gravity = new Vector3(0f, -9.8f, 0f);
                UseMechanic.mechanicFace.Clear();
                PickFace.slotUseCount_STATIC.Clear();
                SceneManager.LoadScene("level_map");
                done = false;

                v.rounded.value = true;
                v.intensity.value = 0.25f;
                v.smoothness.value = 1f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "cube")
        {
            if(!startCutScene)
            {
                time = Time.time;
                anim.enabled = true;
                endPointState = 2;
                startCutScene = true;
                camera.GetComponent<Animator>().enabled = false;
                if(killVelocity) playerCube.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }

            if(buildIndex > LevelManager.levelsUnlocked)
            {
                LevelManager.levelsUnlocked++;
                PlayerPrefs.SetInt("LevelsUnlocked", LevelManager.levelsUnlocked);
                PlayerPrefs.Save();
            }
        }
    }

    void MoveCamera()
    {
        camera.position = Vector3.MoveTowards(camera.position, playerCube.position, 35f * Time.deltaTime);
        if(!chaos) cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, 1f, 10f * Time.deltaTime);
        if(chaos) cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, 1f, 20f * Time.deltaTime);

        vol.profile.TryGet(out v);
        v.rounded.value = true;
        v.intensity.value = Mathf.MoveTowards(v.intensity.value, 1f, 1f * Time.deltaTime);
        v.smoothness.value = Mathf.MoveTowards(v.smoothness.value, 0f, 5f * Time.deltaTime);
    }

    public void ChaosTime()
    {
        PickFace.play = false;
        PickFace.springOnBottom = false;
        ZeroGravity.inField = false;
        Physics.gravity = new Vector3(0f, -9.8f, 0f);
        UseMechanic.mechanicFace.Clear();
        PickFace.slotUseCount_STATIC.Clear();
        SceneManager.LoadScene("level16");
    }
}
