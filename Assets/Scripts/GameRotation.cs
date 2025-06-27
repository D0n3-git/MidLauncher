using System.Diagnostics;
using UnityEngine;
using System.Collections;

public class GameRotation : MonoBehaviour
{
    GameObject currentGame;
    public GameObject gameSpawner;
    public GameObject DailygameSpawner;
    float rotationX = 0f;
    float rotationY = 0f;
    public GameObject TVlight;

    void Start()
    {
        Vector3 startAngles = currentGame.transform.eulerAngles;
        rotationX = startAngles.y;
        rotationY = startAngles.x;

    }

    public void SetCurrentGame(GameObject game)
    {
        currentGame = game;
    }

    void Update()
    {
        if (!gameObject.GetComponent<CameraScroller>().onStart)
        {
            float mouseX = Input.GetAxis("Mouse X") * 180 * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * 160 * Time.deltaTime;
            rotationX += mouseX;
            rotationY += mouseY;
            currentGame.transform.rotation = Quaternion.Euler(0f, rotationX, rotationY);

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Cursor.visible = true;

                var cameraScroller = gameObject.GetComponent<CameraScroller>();
                cameraScroller.enabled = true;
                cameraScroller.MoveTo(1);
                cameraScroller.enabled = false;
                StartCoroutine(LaunchGameSequence());
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                currentGame.GetComponentInParent<SavedGames>().DeleteGame(currentGame);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            if(currentGame.GetComponent<Game>().isDaily)
            {
                currentGame.transform.position = DailygameSpawner.transform.position;
                currentGame.transform.rotation = DailygameSpawner.transform.rotation;
            }
            else
            {
                currentGame.transform.position = gameSpawner.transform.position;
                currentGame.transform.rotation = gameSpawner.transform.rotation;
            }
            currentGame.GetComponent<BoxCollider>().enabled = true;
            currentGame.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<GameInteraction>().enabled = true;
            gameObject.GetComponent<CameraScroller>().enabled = true;
            enabled = false;
        }
    }

    IEnumerator LaunchGameSequence()
    {
        TVlight.SetActive(true);
        yield return new WaitForSeconds(3f);

        if (currentGame.GetComponent<Game>().isDaily)
        {
            currentGame.transform.position = DailygameSpawner.transform.position;
            currentGame.transform.rotation = DailygameSpawner.transform.rotation;
        }
        else
        {
            currentGame.transform.position = gameSpawner.transform.position;
            currentGame.transform.rotation = gameSpawner.transform.rotation;
        }

        Process game = new Process();
        game.StartInfo.FileName = currentGame.GetComponent<Game>().path;

        try
        {
            game.Start();

            while (!game.HasExited)
            {
                yield return null;
            }

            TVlight.SetActive(false);
            currentGame.GetComponent<BoxCollider>().enabled = true;
            currentGame.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<GameInteraction>().enabled = true;
            gameObject.GetComponent<CameraScroller>().enabled = true;
        }
        finally
        {
            game.Dispose();
        }

        enabled = false;
    }
}
