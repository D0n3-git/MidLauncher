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
    void Start()
    {
        onStart = true;
        transform.position =points[0].position;
        transform.rotation = points[0].rotation;
        Cursor.lockState = CursorLockMode.Confined;

    }
    public void MoveTo(int index)
    {
        transform.position = points[index].position;
        transform.rotation = points[index].rotation;
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
            HelpTMP.text = help[0];
        }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("TV"))
                    {            
                        HelpTMP.text = help[1];
                        transform.position = points[1].position;
                        transform.rotation = points[1].rotation;
                        onStart = false;
                    }
                    if (hit.collider.CompareTag("DeliveryBox"))
                    {
                        HelpTMP.text = help[2];
                        transform.position = points[2].position;
                        transform.rotation = points[2].rotation;
                        onStart = false;
                    }
                    if (hit.collider.CompareTag("Games"))
                    {
                        HelpTMP.text = help[3];
                        transform.position = points[3].position;
                        transform.rotation = points[3].rotation;
                        onStart = false;
                    }
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
        
    }
}
