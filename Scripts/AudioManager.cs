using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sound Effects")]
    public AudioSource SFX;

    [Header("Background Music")]
    public AudioSource BK_Music;

    float time;
    bool flag;

    private static GameObject instance;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null) instance = gameObject;
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        flag = false;
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > time + 5f && !flag)
        {
            BK_Music.Play();
            flag = true;
        }

        if(Input.GetKeyDown(KeyCode.M)) BK_Music.mute = !BK_Music.mute;

    }
}
