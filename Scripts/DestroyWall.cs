using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    public GameObject particles;
    Vector3 wallPos;

    AudioSource SFX;
    public AudioClip wallBreak_SFX;

    void Start()
    {
        GameObject g = GameObject.FindGameObjectWithTag("SFX");
        SFX = g.GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "rocket")
        {
            wallPos = transform.position;
            Destroy(gameObject);
            Destroy(collision.gameObject);
            Instantiate(particles, wallPos, Quaternion.identity);
            SFX.clip = wallBreak_SFX;
            SFX.Play();
        }
    }
}
