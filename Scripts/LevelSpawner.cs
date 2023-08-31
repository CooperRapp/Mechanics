using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public List<GameObject> Levels = new List<GameObject>(); // covert to Resources.LoadAll when more levels
    public Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 location = Vector3.zero;
        int row = 0;
        int column = 0;
        for (int i = 0; i < Levels.Count; i++)
        {
            if (column == 5)
            {
                row++;
                if (row == 1) location = new Vector3(0f, -15f, 0f);
                if (row == 2) location = new Vector3(0f, -30f, 0f);
                column = 0;
            }

            GameObject g = Instantiate(Levels[i], location, Quaternion.Euler(0f, -90f, 90f));
            //Debug.Log("(" + row + ", " + column + "): " + location);
            g.transform.localScale = g.transform.localScale / 3;
            g.transform.parent = parent;
            location = new Vector3(location.x - 20f, location.y + 9f, location.z);
            column++;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}