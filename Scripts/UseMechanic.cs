using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UseMechanic : MonoBehaviour
{
    private Rigidbody rb;
    public Animator facialExpressions;
    public GameObject rocket;
    public GameObject ice_bullet;

    public GameObject slot1Hover;
    public GameObject slot2Hover;
    public GameObject slot3Hover;
    public GameObject slot4Hover;
    public GameObject slot5Hover;

    public AudioSource SFX;
    public AudioClip rocketlauncher_SFX;
    public AudioClip freeze_SFX;
    public AudioClip spring_SFX;

    public static List<Vector3> mechanicFace = new List<Vector3>();
    public  List<Vector3> mechanicFaces = new List<Vector3>();
    public Transform[] children;

    void Awake()
    {
        GameObject g = GameObject.FindGameObjectWithTag("SFX");
        SFX = g.GetComponent<AudioSource>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        mechanicFaces = mechanicFace;

        children = GetComponentsInChildren<Transform>();

        if (Input.GetKey(KeyCode.Alpha1) && PickFace.play) slot1Hover.SetActive(true);
        else slot1Hover.SetActive(false);

        if (Input.GetKey(KeyCode.Alpha2) && PickFace.play) slot2Hover.SetActive(true);
        else slot2Hover.SetActive(false);

        if (Input.GetKey(KeyCode.Alpha3) && PickFace.play) slot3Hover.SetActive(true);
        else slot3Hover.SetActive(false);

        if (Input.GetKey(KeyCode.Alpha4) && PickFace.play) slot4Hover.SetActive(true);
        else slot4Hover.SetActive(false);

        if (Input.GetKey(KeyCode.Alpha5) && PickFace.play) slot5Hover.SetActive(true);
        else slot5Hover.SetActive(false);

        if (Input.GetKeyUp(KeyCode.Alpha1) && PickFace.play) // && PickFace.play
        {
            if (children[5] != null)
            {
                if (children[5].gameObject.tag == "rocketlauncher")
                {
                    RocketLauncher(mechanicFace[0], children[5]);
                    PickFace.slotUseCount_STATIC[0].GetComponentInChildren<TextMeshProUGUI>().text = "x" + children[5].GetComponent<RocketLauncherClass>().rocketCount.ToString();
                } 
                if (children[5].gameObject.tag == "spring")
                {
                    Spring(mechanicFace[0], children[5]);
                    PickFace.slotUseCount_STATIC[0].GetComponentInChildren<TextMeshProUGUI>().text = "x" + children[5].GetComponent<SpringClass>().springCount.ToString();
                }
                if (children[5].gameObject.tag == "freezegun")
                {
                    FreezeGun(mechanicFace[0], children[5]);
                    PickFace.slotUseCount_STATIC[0].GetComponentInChildren<TextMeshProUGUI>().text = "x" + children[5].GetComponent<FreezeGunClass>().freezeCount.ToString();
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Alpha2) && PickFace.play)
        {
            if (children[7] != null)
            {
                if (children[7].gameObject.tag == "rocketlauncher")
                {
                    RocketLauncher(mechanicFace[1], children[7]);
                    PickFace.slotUseCount_STATIC[1].GetComponentInChildren<TextMeshProUGUI>().text = "x" + children[7].GetComponent<RocketLauncherClass>().rocketCount.ToString();
                }
                if (children[7].gameObject.tag == "spring")
                {
                    Spring(mechanicFace[1], children[7]);
                    PickFace.slotUseCount_STATIC[1].GetComponentInChildren<TextMeshProUGUI>().text = "x" + children[7].GetComponent<SpringClass>().springCount.ToString();
                }
                if (children[7].gameObject.tag == "freezegun")
                {
                    FreezeGun(mechanicFace[1], children[7]);
                    PickFace.slotUseCount_STATIC[1].GetComponentInChildren<TextMeshProUGUI>().text = "x" + children[7].GetComponent<FreezeGunClass>().freezeCount.ToString();
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Alpha3) && PickFace.play)
        {
            if (children[9] != null)
            {
                if (children[9].gameObject.tag == "rocketlauncher")
                {
                    RocketLauncher(mechanicFace[2], children[9]);
                    PickFace.slotUseCount_STATIC[2].GetComponentInChildren<TextMeshProUGUI>().text = "x" + children[9].GetComponent<RocketLauncherClass>().rocketCount.ToString();
                }
                if (children[9].gameObject.tag == "spring")
                {
                    Spring(mechanicFace[2], children[9]);
                    PickFace.slotUseCount_STATIC[2].GetComponentInChildren<TextMeshProUGUI>().text = "x" + children[9].GetComponent<SpringClass>().springCount.ToString();
                }
                if (children[9].gameObject.tag == "freezegun")
                {
                    FreezeGun(mechanicFace[2], children[9]);
                    PickFace.slotUseCount_STATIC[2].GetComponentInChildren<TextMeshProUGUI>().text = "x" + children[9].GetComponent<FreezeGunClass>().freezeCount.ToString();
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Alpha4) && PickFace.play)
        {
            if (children[11] != null)
            {
                if (children[11].gameObject.tag == "rocketlauncher")
                {
                    RocketLauncher(mechanicFace[3], children[11]);
                    PickFace.slotUseCount_STATIC[3].GetComponentInChildren<TextMeshProUGUI>().text = "x" + children[11].GetComponent<RocketLauncherClass>().rocketCount.ToString();
                }
                if (children[11].gameObject.tag == "spring")
                {
                    Spring(mechanicFace[3], children[11]);
                    PickFace.slotUseCount_STATIC[3].GetComponentInChildren<TextMeshProUGUI>().text = "x" + children[11].GetComponent<SpringClass>().springCount.ToString();
                }
                if (children[11].gameObject.tag == "freezegun")
                {
                    FreezeGun(mechanicFace[3], children[11]);
                    PickFace.slotUseCount_STATIC[3].GetComponentInChildren<TextMeshProUGUI>().text = "x" + children[11].GetComponent<FreezeGunClass>().freezeCount.ToString();
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Alpha5) && PickFace.play)
        {
            if (children[13] != null)
            {
                if (children[13].gameObject.tag == "rocketlauncher")
                {
                    RocketLauncher(mechanicFace[4], children[13]);
                    PickFace.slotUseCount_STATIC[4].GetComponentInChildren<TextMeshProUGUI>().text = "x" + children[13].GetComponent<RocketLauncherClass>().rocketCount.ToString();
                }
                if (children[13].gameObject.tag == "spring")
                {
                    Spring(mechanicFace[4], children[13]);
                    PickFace.slotUseCount_STATIC[4].GetComponentInChildren<TextMeshProUGUI>().text = "x" + children[13].GetComponent<SpringClass>().springCount.ToString();
                }
                if (children[13].gameObject.tag == "freezegun")
                {
                    FreezeGun(mechanicFace[4], children[13]);
                    PickFace.slotUseCount_STATIC[4].GetComponentInChildren<TextMeshProUGUI>().text = "x" + children[13].GetComponent<FreezeGunClass>().freezeCount.ToString();
                }
            }
        }
    }

    void RocketLauncher(Vector3 face, Transform rl)
    {
        if (rl.gameObject.GetComponent<RocketLauncherClass>().rocketCount > 0)
        {
            Vector3 direction;
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.Euler(Vector3.zero);

            if (face == new Vector3(0f, 0f, 1f)) // right wall
            {
                if(!ZeroGravity.inField)
                {
                    direction = new Vector3(0f, 90f, 0f);
                    position = new Vector3(rb.transform.position.x, rb.transform.position.y, rb.transform.position.z + 1f);
                    rotation = Quaternion.Euler(direction);
                }
                if (ZeroGravity.inField)
                {
                    direction = new Vector3(0f, -90f, 0f);
                    position = new Vector3(rb.transform.position.x, rb.transform.position.y, rb.transform.position.z - 1f);
                    rotation = Quaternion.Euler(direction);
                }
            }
            if (face == new Vector3(-1f, 0f, 0f)) // left wall
            {
                direction = new Vector3(0f, 0f, 0f);
                position = new Vector3(rb.transform.position.x - 1f, rb.transform.position.y, rb.transform.position.z);
                rotation = Quaternion.Euler(direction);
            }
            if (face == new Vector3(1f, 0f, 0f)) // right open
            {
                direction = new Vector3(0f, 180f, 0f);
                position = new Vector3(rb.transform.position.x + 1f, rb.transform.position.y, rb.transform.position.z);
                rotation = Quaternion.Euler(direction);
            }
            if (face == new Vector3(0f, 0f, -1f)) // left open
            {
                if(!ZeroGravity.inField)
                {
                    direction = new Vector3(0f, -90f, 0f);
                    position = new Vector3(rb.transform.position.x, rb.transform.position.y, rb.transform.position.z - 1f);
                    rotation = Quaternion.Euler(direction);
                }
                if (ZeroGravity.inField)
                {
                    direction = new Vector3(0f, 90f, 0f);
                    position = new Vector3(rb.transform.position.x, rb.transform.position.y, rb.transform.position.z + 1f);
                    rotation = Quaternion.Euler(direction);
                }
            }

            GameObject g = Instantiate(rocket, position, rotation);
            rl.gameObject.GetComponent<RocketLauncherClass>().rocketCount -= 1;
            rl.GetComponentInChildren<ParticleSystem>().Play();
            SFX.clip = rocketlauncher_SFX;
            SFX.Play();
            facialExpressions.SetTrigger("rl");

            if (!ZeroGravity.inField)
            {
                g.GetComponent<Rigidbody>().AddForce(face * 250f); // adding force to rocket
                rb.AddForce(-face * 200f); // adding force to player in opposite direction of rocket fire
                rb.AddForce(new Vector3(0f, 1f, 0f) * 100f); // adding upward force to player for effect
            }
            if(ZeroGravity.inField)
            {
                if (face == new Vector3(0f, 0f, 1f) || face == new Vector3(0f, 0f, -1f))
                {
                    g.GetComponent<Rigidbody>().AddForce(-face * 250f); // adding force to rocket
                    rb.AddForce(face * 200f); // adding force to player in opposite direction of rocket fire
                }
                else
                {
                    g.GetComponent<Rigidbody>().AddForce(face * 250f); // adding force to rocket
                    rb.AddForce(-face * 200f); // adding force to player in opposite direction of rocket fire
                }
                rb.AddForce(new Vector3(0f, -1f, 0f) * 100f);  // adding downward force to player for effect
            }
        }
    }

    void Spring(Vector3 face, Transform spr)
    {
        if (spr.gameObject.GetComponent<SpringClass>().springCount > 0)
        {
            spr.GetComponentInChildren<ParticleSystem>().Play();
            SFX.clip = spring_SFX;
            SFX.Play();
            facialExpressions.SetTrigger("s");

            if (ZeroGravity.inField) // checking if we are in field because upward forces need to be flipped
            {
                if (face == new Vector3(0f, 1f, 0f))
                {
                    rb.AddForce(-face * 625); // upward force when spring is on bottom
                } 
                else if (face == new Vector3(0f, 0f, 1f) || face == new Vector3(0f, 0f, -1f))
                {
                    rb.AddForce(new Vector3(0f, -1f, 0f) * 225); // adding donward force to player for effect
                    rb.AddForce(face * 600f);
                }
                else
                {
                    rb.AddForce(new Vector3(0f, -1f, 0f) * 225); // adding upward force to player for effect
                    rb.AddForce(-face * 600f); 
                }   
            }

            if (!ZeroGravity.inField)
            {
                if (face == new Vector3(0f, 1f, 0f)) rb.AddForce(face * 625); // downward force when spring is on bottom and in gravity field
                else
                {
                    rb.AddForce(new Vector3(0f, 1f, 0f) * 225);
                    rb.AddForce(-face * 600f);
                }    
            }
            spr.gameObject.GetComponent<SpringClass>().springCount -= 1;
        }
    }

    void FreezeGun(Vector3 face, Transform fg)
    {
        if (fg.gameObject.GetComponent<FreezeGunClass>().freezeCount > 0)
        {
            Vector3 direction;
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.Euler(Vector3.zero);

            if (face == new Vector3(0f, 0f, 1f)) // right wall
            {
                if (!ZeroGravity.inField)
                {
                    direction = new Vector3(0f, 90f, 90f);
                    position = new Vector3(rb.transform.position.x, rb.transform.position.y, rb.transform.position.z + 1.3f);
                    rotation = Quaternion.Euler(direction);
                }
                if (ZeroGravity.inField)
                {
                    direction = new Vector3(0f, -90f, 90f);
                    position = new Vector3(rb.transform.position.x, rb.transform.position.y, rb.transform.position.z - 1.3f);
                    rotation = Quaternion.Euler(direction);
                }
            }
            if (face == new Vector3(-1f, 0f, 0f)) // left wall
            {
                direction = new Vector3(0f, 0f, 90f);
                position = new Vector3(rb.transform.position.x - 1.3f, rb.transform.position.y, rb.transform.position.z);
                rotation = Quaternion.Euler(direction);
            }
            if (face == new Vector3(1f, 0f, 0f)) // right open
            {
                direction = new Vector3(0f, 180f, 90f);
                position = new Vector3(rb.transform.position.x + 1.3f, rb.transform.position.y, rb.transform.position.z);
                rotation = Quaternion.Euler(direction);
            }
            if (face == new Vector3(0f, 0f, -1f)) // left open
            {
                if (!ZeroGravity.inField)
                {
                    direction = new Vector3(0f, -90f, 90f);
                    position = new Vector3(rb.transform.position.x, rb.transform.position.y, rb.transform.position.z - 1.3f);
                    rotation = Quaternion.Euler(direction);
                }
                if (ZeroGravity.inField)
                {
                    direction = new Vector3(0f, 90f, 90f);
                    position = new Vector3(rb.transform.position.x, rb.transform.position.y, rb.transform.position.z + 1.3f);
                    rotation = Quaternion.Euler(direction);
                }
            }
            GameObject g = Instantiate(ice_bullet, position, rotation);
            fg.gameObject.GetComponent<FreezeGunClass>().freezeCount -= 1;
            SFX.clip = freeze_SFX;
            SFX.Play();
            facialExpressions.SetTrigger("fg");

            if (!ZeroGravity.inField)
            {
                g.GetComponent<Rigidbody>().AddForce(face * 250f); // adding force to rocket
            }
            if (ZeroGravity.inField)
            {
                if (face == new Vector3(0f, 0f, 1f) || face == new Vector3(0f, 0f, -1f))
                {
                    g.GetComponent<Rigidbody>().AddForce(-face * 250f); // adding force to rocket
                }
                else
                {
                    g.GetComponent<Rigidbody>().AddForce(face * 250f); // adding force to rocket
                }
            }

        }
    }
}