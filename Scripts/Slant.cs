using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slant : MonoBehaviour
{
    [Header("Requirements")]
    public Rigidbody player;
    MeshCollider mc;
    public PhysicMaterial ice;
    public PhysicMaterial bounce;

    [Header("Option")]
    public bool extraForce;
    public bool bigSlant;
    public bool cancelVelocity;

    [Header("Direction")]
    public bool north;
    public bool south;
    public bool east;
    public bool west;

    public bool special;

    // Start is called before the first frame update
    void Start()
    {
        mc = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PickFace.springOnBottom) mc.material = bounce;
        else mc.material = ice;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "spring" || collision.collider.tag == "freezegun")
        {
            Debug.Log("HIT");
            if (cancelVelocity)
            {
                player.velocity = Vector3.zero;
                player.angularVelocity = Vector3.zero;

                if (north) player.AddForce(new Vector3(0f, 0f, -1f) * 500f);
                if (south) player.AddForce(new Vector3(0f, 0f, 1f) * 500f);
                if (east) player.AddForce(new Vector3(-1f, 0f, 0f) * 500f);
                if (west) player.AddForce(new Vector3(1f, 0f, 0f) * 500f);
                player.AddForce(new Vector3(0f, 1f, 0f) * 200f);
            }
            if (extraForce) player.AddForce(new Vector3(1f, 0f, 0f) * 150f);
            if(bigSlant)
            {
                if (north) player.AddForce(new Vector3(0f, 0f, -1f) * 375f);
                if (south) player.AddForce(new Vector3(0f, 0f, 1f) * 375f);
                if (east) player.AddForce(new Vector3(-1f, 0f, 0f) * 375f);
                if (west) player.AddForce(new Vector3(1f, 0f, 0f) * 375f);
                player.AddForce(new Vector3(0f, 1f, 0f) * 130f);
            }

            if(special)
            {
                player.velocity = Vector3.zero;
                player.angularVelocity = Vector3.zero;
                player.AddForce(new Vector3(0f, 0f, 1f) * 275f);
                player.AddForce(new Vector3(0f, 1f, 0f) * 100f);
            }
        }
    }
}
