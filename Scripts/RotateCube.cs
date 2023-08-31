using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    RaycastHit hit;
    Ray ray;

    public Transform viewport_cube;
    Quaternion left_rotate_amount;
    Quaternion right_rotate_amount;
    Quaternion down_rotate_amount;
    Quaternion rotateTo;

    bool left, right, down;
    float time;

    public MeshRenderer leftArrow;
    public MeshRenderer rightArrow;
    public Material arrowLight;
    public Material normal;

    [SerializeField] float speed;

    void Start()
    {
        left_rotate_amount = Quaternion.Euler(0f, 90f, 0f);
        right_rotate_amount = Quaternion.Euler(0f, -90f, 0f);
        down_rotate_amount = Quaternion.Euler(0f, 90f, -180f);
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) // change the material of arrows if player hovers over them
        {
            if (hit.collider.tag == "left_arrow")
            {
                leftArrow.material = arrowLight;
            }
            else leftArrow.material = normal;

            if (hit.collider.tag == "right_arrow")
            {
                rightArrow.material = arrowLight;
            }
            else rightArrow.material = normal;
        }

        if (Input.GetMouseButtonDown(0) && !left && !right) //  && !down
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "left_arrow")
                {
                    left = true;
                    time = Time.time;
                    rotateTo = viewport_cube.transform.rotation * left_rotate_amount;
                }
                if (hit.collider.tag == "right_arrow")
                {
                    right = true;
                    time = Time.time;
                    rotateTo = viewport_cube.transform.rotation * right_rotate_amount;
                }
                //if (hit.collider.tag == "down_arrow")
                //{
                //    right = true;
                //    time = Time.time;
                //    rotateTo = viewport_cube.transform.rotation * down_rotate_amount;
                //}
            }
        }

        if (left)
        {
            viewport_cube.rotation = Quaternion.Lerp(viewport_cube.rotation, rotateTo, speed * Time.deltaTime);
            if (Time.time > time + 0.8f) left = false;
        }

        if(right)
        {
            viewport_cube.rotation = Quaternion.Lerp(viewport_cube.rotation, rotateTo, speed * Time.deltaTime);
            if (Time.time > time + 0.8f) right = false;
        }

        //if (down)
        //{
        //    viewport_cube.rotation = Quaternion.Lerp(viewport_cube.rotation, rotateTo, 0.015f * Time.deltaTime);
        //    if (Time.time > time + 1.2f) down = false;
        //}
    }
}
