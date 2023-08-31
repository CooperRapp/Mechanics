using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravity : MonoBehaviour
{
    public Transform player;
    float time;

    public static bool inField;

    Animator anim;

    bool upsideDown;
    bool rightsideUp;
    float xRotation;

    void Start()
    {
        anim = player.gameObject.GetComponent<Animator>();
        xRotation = 0;
    }

    void Update()
    {
        if (inField) Debug.Log("YAH");

        if(upsideDown)
        {
            // rotate x 180
            xRotation = Mathf.MoveTowards(xRotation, 180f, 300f * Time.deltaTime);
            player.rotation = Quaternion.Euler(new Vector3(xRotation, player.rotation.y, player.rotation.z));

            if (xRotation >= 180f) upsideDown = false; 
        }

        if (rightsideUp)
        {
            // rotate x 0
            xRotation = Mathf.MoveTowards(xRotation, 0f, 300f * Time.deltaTime);
            player.rotation = Quaternion.Euler(new Vector3(xRotation, player.rotation.y, player.rotation.z));

            if (xRotation <= 0f) rightsideUp = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        time = Time.time;

        if (other.tag == "cube")
        {
            inField = true;
            upsideDown = true;
            rightsideUp = false;
            Physics.gravity = new Vector3(0f, 9.8f, 0f);
        }

        if (other.tag == "ice_bullet")
        {
            Debug.Log("ICE IN");
            ParticleSystem.MainModule ps = other.transform.gameObject.GetComponentInChildren<ParticleSystem>().main;
            ps.gravityModifierMultiplier = 1;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "cube")
        {
            inField = true;
            upsideDown = true;
            rightsideUp = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "cube")
        {
            upsideDown = false;
            rightsideUp = true;
            Physics.gravity = new Vector3(0f, -9.8f, 0f);
            inField = false;
        }

        if (other.tag == "ice_bullet")
        {
            Debug.Log("ICE OUT");
            ParticleSystem.MainModule ps = other.transform.gameObject.GetComponentInChildren<ParticleSystem>().main;
            ps.gravityModifierMultiplier = -1;
        }
    }
}