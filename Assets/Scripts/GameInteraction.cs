    using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameInteraction : MonoBehaviour
{
    public GameObject GameViewPoint;
    public GameObject DailyGameViewPoint;
    //повторяется в течении работы программы
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameObject.GetComponent<CameraScroller>().GetPoint()!=0)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {

                Debug.Log("pew!");
                if (hit.collider.CompareTag("Game"))
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    GameObject currentGame = hit.collider.gameObject;
                    currentGame.GetComponent<BoxCollider>().enabled = false;
                    currentGame.GetComponent<Rigidbody>().useGravity = false;
                    if (currentGame.GetComponent<Game>().isDaily)
                    {
                        currentGame.transform.position = DailyGameViewPoint.transform.position;
                    }
                    else
                    {
                        currentGame.transform.position = GameViewPoint.transform.position;
                    }
                    gameObject.GetComponent<GameRotation>().enabled = true;
                    gameObject.GetComponent<GameRotation>().SetCurrentGame(currentGame);
                    gameObject.GetComponent<CameraScroller>().enabled = false;
                    gameObject.GetComponent<GameInteraction>().enabled = false;                     
                }
            }           
        }
    } 
}

