using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraRotationPoint;
    float moveSpeed = 2.5f;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (Input.GetAxis("Mouse X") > 0)
            {
                cameraRotationPoint.rotation = Quaternion.Slerp(cameraRotationPoint.rotation, Quaternion.Euler(0f, 45f, 0f), moveSpeed * Time.deltaTime);
            }
            if (Input.GetAxis("Mouse X") < 0)
            {
                cameraRotationPoint.rotation = Quaternion.Slerp(cameraRotationPoint.rotation, Quaternion.Euler(0f, -45f, 0f), moveSpeed * Time.deltaTime);
            }
        }
    }
}
