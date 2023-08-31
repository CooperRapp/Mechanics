using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCameraRotate : MonoBehaviour
{
    float time, startTime;
    public float speed;
    public Transform camera;
    bool flag;
    bool freeze, rocketLauncher, spring;
    bool done1, done2, done3;

    public Animator fadePanel;

    [Header("Spring Animation")]
    public GameObject CubeSpring;
    public Animator CubeSpringAnim;
    public GameObject spring1;
    public Animator spring1Anim;
    public GameObject spring2;
    public Animator spring2Anim;
    public GameObject platformSpring;

    [Header("Rocket Launcher Animation")]
    public GameObject CubeRL;
    public Animator CubeRLAnim;
    public GameObject Rocket;
    public Animator RocketAnim;
    public GameObject platformRL;
    public GameObject wall;
    public ParticleSystem launch;
    public ParticleSystem wallPS;

    [Header("Freeze Gun Animation")]
    public GameObject CubeFG;
    public Animator CubeFGAnim;
    public GameObject freezeBullet;
    public Animator freezeBulletAnim;
    public GameObject RocketFG;
    public Animator RocketFGAnim;
    public GameObject platformFG;
    public MeshRenderer platformMR;
    public ParticleSystem launchFG;
    public ParticleSystem freezePS;
    public Material iceMat;
    public Material normalMat;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        flag = false;
        freeze = false;
        rocketLauncher = spring = true;
        done1 = done2 = done3 = false;

        if (IntroManager.introAnim)
        {
            flag = true;
            camera.parent = transform;
            camera.transform.position = new Vector3(-27f, 15f, 9f);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (flag) transform.Rotate(0f, speed * Time.deltaTime, 0f);

        if (Time.time > startTime + 5f && !flag)
        {
            camera.parent = transform;
            camera.transform.position = new Vector3(-27f, 15f, 9f);

            flag = true;
            time = Time.time;
        }

        if(!freeze && flag)
        {
            if (!done1)
            {
                CubeFG.transform.localPosition = Vector3.zero;

                platformMR.material = normalMat;
                RocketFG.SetActive(true);
                RocketFGAnim.SetTrigger("launch");
                freezeBullet.SetActive(true);
                freezeBulletAnim.SetTrigger("freeze");
                CubeFG.SetActive(true);
                platformFG.SetActive(true);
                freezePS.Play();
                done1 = true;
            }
            if (!done2 && Time.time > time + 1f)
            {
                platformMR.material = iceMat;
                launchFG.Play();
                CubeFGAnim.SetTrigger("jump");
                done2 = true;
            }

            if (!done3 && Time.time > time + 1.75f)
            {
                fadePanel.SetTrigger("in");
                done3 = true;
            }

            if (Time.time > time + 2.75f)
            {
                fadePanel.SetTrigger("out");
                CubeFG.SetActive(false);
                RocketFG.SetActive(false);
                freezeBullet.SetActive(false);
                platformFG.SetActive(false);

                done1 = done2 = done3 = false;

                time = Time.time;

                rocketLauncher = false;
                spring = true;
                freeze = true;

            }
        }

        if(!rocketLauncher && flag)
        {
            if (!done1)
            {
                CubeRL.SetActive(true);
                Rocket.SetActive(true);
                platformRL.SetActive(true);
                RocketAnim.SetTrigger("launch");
                launch.Play();
                CubeRLAnim.SetTrigger("jump");
                done1 = true;
            }
            if (!done2 && Time.time > time + 1.15)
            {
                fadePanel.SetTrigger("in");
                wall.SetActive(false);
                wallPS.Play();
                done2 = true;
            }
            if (Time.time > time + 2.15)
            {
                fadePanel.SetTrigger("out");
                CubeRL.SetActive(false);
                Rocket.SetActive(false);
                platformRL.SetActive(false);
                wall.SetActive(true);

                done1 = done2 = false;

                time = Time.time;

                spring = false;
                freeze = true;
                rocketLauncher = true;

            }
        }

        if(!spring && flag)
        {
            if (!done1)
            {
                CubeSpring.SetActive(true);
                spring1.SetActive(true);
                spring2.SetActive(true);
                platformSpring.SetActive(true);
                CubeSpringAnim.SetTrigger("jump");
                spring1Anim.SetTrigger("spring");
                spring2Anim.SetTrigger("spring");
                done1 = true;
            }
            if (!done2 && Time.time > time + 0.8)
            {
                fadePanel.SetTrigger("in");
                done2 = true;
            }
            if (Time.time > time + 1.8)
            {
                fadePanel.SetTrigger("out");
                CubeSpring.SetActive(false);
                spring1.SetActive(false);
                spring2.SetActive(false);
                platformSpring.SetActive(false);

                done1 = done2 = false;

                time = Time.time;

                freeze = false;
                rocketLauncher = true;
                spring = true;
            }
        }

    }
}
