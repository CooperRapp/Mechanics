using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public Transform player;
    public Rigidbody playerRB;

    public Transform northPoint, southPoint, eastPoint, westPoint;
    public bool north, south, east, west;

    bool onBelt, stopMoving;
    public static bool inAir;
    public float time;

    [SerializeField] float speed;

    public bool extra;

    void Start()
    {
        //west = true;
    }

    void Update()
    {
        // ----- CONVEYOR BELT MOVING -----
        if (onBelt)
        {
            if (north)
            {
                if (!(Vector3.Distance(player.position, northPoint.position) <= 1))
                {
                    player.position = Vector3.MoveTowards(player.position, northPoint.position, speed * Time.deltaTime);
                }
                else
                {
                    north = false;
                    east = true;
                }
            }
            if (south)
            {
                if (!(Vector3.Distance(player.position, southPoint.position) <= 1))
                {
                    player.position = Vector3.MoveTowards(player.position, southPoint.position, speed * Time.deltaTime);
                }
                else
                {
                    south = false;
                    west = true;
                }
            }
            if (east)
            {
                if (!(Vector3.Distance(player.position, eastPoint.position) <= 1))
                {
                    player.position = Vector3.MoveTowards(player.position, eastPoint.position, speed * Time.deltaTime);
                }
                else
                {
                    east = false;
                    south = true;
                }
            }
            if (west)
            {
                if (!(Vector3.Distance(player.position, westPoint.position) <= 1))
                {
                    player.position = Vector3.MoveTowards(player.position, westPoint.position, speed * Time.deltaTime);
                }
                else
                {
                    west = false;
                    north = true;
                }
            }
        }


        // ----- CHECKING IF PLAYER IN AIR & ADDING MOMENTUM -----
        RaycastHit hitFloor;
        Ray down = new Ray(player.position, new Vector3(0f, -1f, 0f));
        if (Physics.Raycast(down, out hitFloor)) // 100f
        {
            //Debug.DrawRay(player.position, new Vector3(0f, -1f, 0f), Color.green);
            //Debug.Log(hitFloor.distance);

            if (hitFloor.distance > 1f && PickFace.springOnBottom && extra)
            {
                if (north && inAir)
                {
                    playerRB.AddForce(new Vector3(0f, 0f, -1f) * 75f);
                    inAir = false;
                    Debug.Log("SHOOT1");
                }
                if (south && inAir)
                {
                    playerRB.AddForce(new Vector3(0f, 0f, 1f) * 75f);
                    inAir = false;
                    Debug.Log("SHOOT2");
                }
                if (east && inAir)
                {
                    playerRB.AddForce(new Vector3(-1f, 0f, 0f) * 75f);
                    inAir = false;
                    Debug.Log("SHOOT3");
                }
                if (west && inAir)
                {
                    playerRB.AddForce(new Vector3(1f, 0f, 0f) * 75f);
                    inAir = false;
                    Debug.Log("SHOOT4");
                }
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "cube")
        {
            onBelt = true;
        }
        if(collision.collider.tag == "spring")
        {
            onBelt = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        onBelt = false;
        inAir = true;
    }
}
