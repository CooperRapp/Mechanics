using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    RaycastHit hit;
    Ray ray;

    public List<GameObject> levels = new List<GameObject>();
    public List<GameObject> lvl = new List<GameObject>();

    int index;
    float time;
    bool change;

    void Start()
    {
        if (EndPoint.levelNumber != 7) index = EndPoint.levelNumber;
        else index = 0;

        GameObject g = Instantiate(levels[index], new Vector3(0f, -2f, 0f), Quaternion.Euler(new Vector3(0f, 270f, 90f)));
        lvl.Add(g);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q)) Application.Quit();

        if (Input.GetMouseButtonDown(0) && !change)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "left_level")
                {
                    index--;

                    if (index < 0) index = (levels.Count - 1);

                    GameObject g = Instantiate(levels[index], new Vector3(0f, 0.25f, 0f), Quaternion.Euler(new Vector3(0f, 270f, 90f)));
                    g.GetComponent<Animator>().SetTrigger("right_in");
                    g.transform.Find("Canvas").transform.Find("title").GetComponent<Animator>().SetTrigger("text_in");
                    lvl.Add(g);

                    lvl[0].GetComponent<Animator>().SetTrigger("left_out");
                    lvl[0].transform.Find("Canvas").transform.Find("title").GetComponent<Animator>().SetTrigger("text_out");
                    time = Time.time;
                    change = true;
                }
                if (hit.collider.tag == "right_level")
                {
                    index++;

                    if (index > levels.Count - 1) index = 0;

                    GameObject g = Instantiate(levels[index], new Vector3(0f, 0.25f, 0f), Quaternion.Euler(new Vector3(0f, 270f, 90f)));
                    g.GetComponent<Animator>().SetTrigger("left_in");
                    g.transform.Find("Canvas").transform.Find("title").GetComponent<Animator>().SetTrigger("text_in");
                    lvl.Add(g);

                    lvl[0].GetComponent<Animator>().SetTrigger("right_out");
                    lvl[0].transform.Find("Canvas").transform.Find("title").GetComponent<Animator>().SetTrigger("text_out");
                    time = Time.time;
                    change = true;
                }
                if(hit.collider.tag == "play")
                {
                    string s = "level" + (index + 1);
                    SceneManager.LoadScene(s);
                }
            }
        }
        if(change && Time.time > time + 2f)
        {
            Destroy(lvl[0]);
            lvl.RemoveAt(0);
            change = false;
        }
    }
}
