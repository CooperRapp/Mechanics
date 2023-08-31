using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    public Rigidbody player;
    public Animator facialExpressions;
    public GameObject fallDeathTextBox;
    public GameObject fallDeathParticles;

    bool once;
    float time;
    GameObject fdTB;
    GameObject fdPS;

    public static bool died;

    void Start()
    {
        once = true;
        
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "cube")
        {
            if (once)
            {
                time = Time.time;
                once = false;
            }

            if (Time.time < time + 0.001f)
            {
                player.constraints = RigidbodyConstraints.FreezeAll;
                facialExpressions.SetTrigger("fall");

                // particle system
                fdPS = Instantiate(fallDeathParticles,
                            new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z),
                            Quaternion.Euler(Vector3.zero));
                fdPS.SetActive(false);

                // text box
                fdTB = Instantiate(fallDeathTextBox,
                            new Vector3(player.transform.position.x, player.transform.position.y + 2f, player.transform.position.z),
                            Quaternion.Euler(new Vector3(90f, -45f, 0f)));
                fdTB.SetActive(true);
            }

            if(Time.time > time + 1f && Time.time < time + 1.05f)
            {
                player.constraints = RigidbodyConstraints.None;
                fdPS.SetActive(true);
            }

            if (Time.time > time + 1.1f && Time.time < time + 1.15f)
            {
                fdPS.SetActive(true);
            }

            if (Time.time > time + 2.9f && Time.time < time + 3f)
            {
                died = true;
            }
        }
    }
}
