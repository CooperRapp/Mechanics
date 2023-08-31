using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScale : MonoBehaviour
{
    public CanvasScaler title;

    // Start is called before the first frame update
    void Start()
    {
        if (Screen.currentResolution.width < Screen.currentResolution.height)
        {
            title.matchWidthOrHeight = 0;
        }
        else title.matchWidthOrHeight = 1;
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.currentResolution.width < Screen.currentResolution.height)
        {
            title.matchWidthOrHeight = 0;
        }
        else title.matchWidthOrHeight = 1;
    }
}
