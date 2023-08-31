using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Signature : MonoBehaviour
{
    public GameObject dot;
    public Transform area;

    public GraphicRaycaster m_Raycaster;
    public EventSystem m_EventSystem;
    PointerEventData m_PointerEventData;

    public static bool signed;

    // Start is called before the first frame update
    void Start()
    {
        signed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(UseMechanic.mechanicFace.Count > 0)
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);

            foreach (RaycastResult res in results)
            {
                if (res.gameObject.name == "SignatureArea")
                {
                    if (Input.GetMouseButton(0))
                    {
                        GameObject d = Instantiate(dot, Input.mousePosition, Quaternion.identity);
                        d.transform.parent = area;
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        signed = true;
                        enabled = false;
                    }
                }
            }
        }  
    }
}
