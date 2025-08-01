using TMPro;
using UnityEngine;

public class CameraScroller : MonoBehaviour
{
    public Transform[] points;
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
    //запускается на старте программы
    void Start()
    {
        MoveTo(0);
        Cursor.lockState = CursorLockMode.Confined;
    }
    //задание точки для плавного перемещение
    public void MoveTo(int index)
    {
        targetPosition = points[index].position;
        targetRotation = points[index].rotation;
        isMoving = true;
        this.index = index;
        HelpTMP.text = help[index];
    }
    //повторяется в течении работы программы
    void Update()
    {
        if (index==0)
        {
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
    //мнговенное перемещение в точку
    public void Teleport(int index)
    {
        transform.position = points[index].position;
        transform.rotation = points[index].rotation;
        this.index = index;
        HelpTMP.text = help[index];
    }
    //получение текщего положения пользователя
    public int GetPoint()
    {
        return index;
    }
}
