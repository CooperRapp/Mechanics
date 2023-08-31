using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    RaycastHit hit;
    Ray ray;
    Vector3 mousePos;

    [Header("Arrow Stuff")]
    public Animator animT;
    public Animator animD;
    public Transform camera;
    public List<Vector3> cameraJumpPositions = new List<Vector3>();
    int index;
    bool moveCamera;

    [Header("Scroll Stuff")]
    public Vector3 topCappedPosition;
    public Vector3 bottomCappedPosition;
    public Vector3 position;
    public bool clickedTop, clickedBottom;

    static int static_index;

    void Start()
    {
        index = 0;

        if(cameraJumpPositions[static_index] != null)
        {
            camera.position = cameraJumpPositions[static_index];
        }
        position = camera.position;

        if (LevelManager.levelsUnlocked > 14)
        {
            topCappedPosition = new Vector3(-59.6f, 225f, -59.5f);
            cameraJumpPositions.Add(topCappedPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(LevelManager.clickedLevel)
        {
            Debug.Log("yes");
            WhatPosition(position);
            LevelManager.clickedLevel = false;
        }

        // BUTTON HOVERING, ANIMATIONS, CLICKING -------------------
        mousePos = Input.mousePosition;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            // top arrow
            if (hit.collider.tag == "up_arrow")
            {
                animT.SetBool("hoverUp", true);

                if(Input.GetMouseButtonDown(0) && !clickedTop) // if they click on top arrow
                {
                    WhatPosition(position); // see what level we are closest
                    clickedTop = true;
                }
            }
            else animT.SetBool("hoverUp", false);

            // bottom arrow
            if (hit.collider.tag == "down_arrow")
            {     
                animD.SetBool("hoverDown", true);

                if (Input.GetMouseButtonDown(0) && !clickedBottom) // if they click on bottom arrow
                {
                    WhatPosition(position); // see what level we are closest 
                    clickedBottom = true;
                }
            }
            else animD.SetBool("hoverDown", false);
        }
        else
        {
            animT.SetBool("hoverUp", false);
            animD.SetBool("hoverDown", false);
        }

        // clicked the top arrow
        if(clickedTop)
        {
            if (index != cameraJumpPositions.Count - 1)
            {
                if (position.y != cameraJumpPositions[index + 1].y)
                {
                    position.y = Mathf.MoveTowards(position.y, cameraJumpPositions[index + 1].y, 100 * Time.deltaTime);
                    camera.position = position; // move to the one above it
                }
                if (position.y >= cameraJumpPositions[index + 1].y) clickedTop = false;
            }
            else clickedTop = false;
        }

        // clicked the bottom arrow
        if (clickedBottom)
        {
            if (index != 0)
            {
                if (position.y != cameraJumpPositions[index - 1].y)
                {
                    position.y = Mathf.MoveTowards(position.y, cameraJumpPositions[index - 1].y, 100f * Time.deltaTime);
                    camera.position = position; // move to the one above it
                }
                if (position.y <= cameraJumpPositions[index - 1].y) clickedBottom = false;
            }
            else clickedBottom = false;
        }
        // BUTTON HOVERING, ANIMATIONS, CLICKING -------------------


        // SCROLL ACTIONS -------------------
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0f) // forward
        {
            float futureYPos = position.y + (Input.GetAxisRaw("Mouse ScrollWheel") * 5f);

            if (futureYPos <= topCappedPosition.y && futureYPos >= bottomCappedPosition.y)
            {
                position.y = futureYPos;
                camera.position = position;
            }
        }
        // SCROLL ACTIONS -------------------
    }

    void WhatPosition(Vector3 pos)
    {
        index = 0;
        float closest = Mathf.Abs(cameraJumpPositions[0].y - position.y);

        for (int i = 1; i < cameraJumpPositions.Count; i++)
        {
            float check = Mathf.Abs(cameraJumpPositions[i].y - position.y);
            if (check < closest)
            {
                closest = check;
                index = i;
                static_index = i;
            }
        }
    }
}