using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : MonoBehaviour
{
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
    }

    void Update()
    {
        if (Time.time > time + 5.5f) Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "wall") Destroy(gameObject);
    }
}
