using TMPro;
using UnityEngine;

public class CameraScroller : MonoBehaviour
{
    public Transform[] points;
    public float speed = 1;
    public bool onStart;
    public string[] help;
    public TMP_Text HelpTMP;
    public GameObject UI;
    public GameObject HelpLamp;
    public float Mtime = 1;
    public float Rspeed = 1;
    int index;
    Vector3 targetPosition;
    Quaternion targetRotation;
    Vector3 moveVelocity;
    bool isMoving;
    void Start()
    {
        onStart = true;
        MoveTo(0);
        Cursor.lockState = CursorLockMode.Confined;

    }
    public void MoveTo(int index)
    {
        //transform.position = points[index].position;
        targetPosition = points[index].position;
        targetRotation = points[index].rotation;
        isMoving = true;
        this.index = index;
        HelpTMP.text = help[index];
        if (index != 0)
        {
            onStart = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (onStart)
        {
            index = 0;
            HelpTMP.text = help[0];
        }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("TV"))
                    {
                        index = 1;
                    }
                    if (hit.collider.CompareTag("DeliveryBox"))
                    {
                        index = 2;
                    }
                    if (hit.collider.CompareTag("Games"))
                    {
                        index = 3;
                    }
                    MoveTo(index);
                    if (hit.collider.CompareTag("HelpLamp"))
                    {
                        Debug.Log("Lampew!");
                        UI.SetActive(!UI.activeSelf);
                        HelpLamp.SetActive(!HelpLamp.activeSelf);
                    }
                }
            }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Start();
        }
        if(isMoving)
        {
            transform.position = Vector3.SmoothDamp(transform.position,targetPosition,ref moveVelocity,Mtime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime*Rspeed);
        }
    }
    public void Teleport(int index)
    {
        transform.position = points[index].position;
        transform.rotation = points[index].rotation;
        this.index = index;
        HelpTMP.text = help[index];
    }
    public int GetPoint()
    {
        return index;
    }
}
