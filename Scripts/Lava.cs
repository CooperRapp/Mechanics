using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public Rigidbody player;
    public Animator facialExpressions;
    public GameObject lavaDeathTextBox;
    public GameObject lavaDeathParticles;

    bool once;
    float time;
    GameObject ldTB;

    public static bool died;

    void Start()
    {
        once = true;
    }

    void OnTriggerStay(Collider collision)
    {
        if(collision.tag == "cube")
        {  
            if(once)
            {
                time = Time.time;
                once = false;
            }

            if (Time.time < time + 0.001f)
            {
                player.constraints = RigidbodyConstraints.FreezeAll;
                facialExpressions.SetTrigger("lava");

                // particle system
                Instantiate(lavaDeathParticles, 
                            new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z),
                            Quaternion.Euler(Vector3.zero));
                lavaDeathParticles.SetActive(true);
                lavaDeathParticles.GetComponentInChildren<ParticleSystem>().Play();

                // text box
                ldTB = Instantiate(lavaDeathTextBox,
                            new Vector3(player.transform.position.x, player.transform.position.y - 2f, player.transform.position.z),
                            Quaternion.Euler(new Vector3(90f, -45f, 0f)));
                ldTB.SetActive(true);
            }

            if (Time.time > time + 0.001f && Time.time < time + 0.5f)
            {
                Vector3 target = new Vector3(player.transform.position.x, player.transform.position.y + 2f, player.transform.position.z);
                ldTB.transform.position = Vector3.MoveTowards(ldTB.transform.position, target, 0.2f);
            }

            if (Time.time > time + 2f && Time.time < time + 2.05f)
            {
                player.constraints = RigidbodyConstraints.None;
            }

            if (Time.time > time + 2.5f && Time.time < time + 2.6f)
            {
                died = true;
            }
        }
    }
}
