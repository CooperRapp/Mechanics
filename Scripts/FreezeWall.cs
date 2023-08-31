using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeWall : MonoBehaviour
{
    BoxCollider bc;
    MeshRenderer mr;
    public PhysicMaterial ice_surface;
    public Material ice_material;

    public Transform player;
    public bool special;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider>();
        mr = GetComponent<MeshRenderer>();
    }

    void OnParticleCollision(GameObject other)
    {
        bc.material = ice_surface;
        mr.material = ice_material;
    }

    void OnCollisionEnter(Collision collision)
    {
        if((collision.collider.tag == "spring" || collision.collider.tag == "cube") && special)
        {
            Debug.Log("YES");
            player.localPosition = new Vector3(-8.42701f, player.position.y, -3.33786e-06f);
            special = false;
        }
    }
}
