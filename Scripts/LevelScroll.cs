using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScroll : MonoBehaviour
{
    Transform camera;
    Vector3 positon;

    public Vector3 topCappedPosition;
    public Vector3 bottomCappedPosition;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Transform>();
        positon = camera.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0f) // forward
        {
            float futureYPos = positon.y + (Input.GetAxisRaw("Mouse ScrollWheel") * 10f);

            if (futureYPos <= topCappedPosition.y && futureYPos >= bottomCappedPosition.y)
            {
                positon.y = futureYPos;
                camera.position = positon;
            }
        }
    }
}