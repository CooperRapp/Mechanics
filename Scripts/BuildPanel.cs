using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPanel : MonoBehaviour
{
    public GameObject viewportCube;
    public Rigidbody viewportCubeRB;

    public Transform leftArrow;
    public Transform rightArrow;
    public Transform mechanicalArm;

    Transform buildPanel;
    public Animator bpAnimator;

    bool stopper;

    public Animator playButtonCover;
    public static bool play;
    bool open;

    RaycastHit hit;
    Ray ray;
    Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        //buildPanel = GetComponent<Transform>();
        //bpAnimator = GetComponent<Animator>();

        stopper = true;
        open = false;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "playbutton" && !open)
            {
                playButtonCover.SetTrigger("open");
                open = true;
            }
            if (hit.collider.tag != "playbutton" && open)
            {
                playButtonCover.SetTrigger("close");
                open = false;
            }
            if(open && Input.GetMouseButtonDown(0))
            {
                play = true;
            }
        }

        if (play && stopper)
        {
            viewportCube.transform.parent = buildPanel;
            leftArrow.parent = buildPanel;
            rightArrow.parent = buildPanel;
            mechanicalArm.parent = buildPanel;

            viewportCubeRB.constraints = RigidbodyConstraints.FreezeAll;
            bpAnimator.SetTrigger("flip");

            stopper = false;
        }
    }
}
