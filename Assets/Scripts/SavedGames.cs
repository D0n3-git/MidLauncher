using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class JsonGame
{
    public string Name;
    public string Path;
    public string Description;
}

public class SavedGames : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    DirectoryInfo root;
    
    DirectoryInfo[] directories;
    FileInfo file;
    public GameObject gamePrefab;
    JsonGame data;

    void Start()
    {
        root = new DirectoryInfo(Application.persistentDataPath + "/Saved Games");
        root.Create();
        directories = root.GetDirectories();
        UpdateGameList();
        Debug.Log(root.FullName);
    }

    public void UpdateGameList()
    {
        foreach(DirectoryInfo d in directories)
        {
            GameObject cartridge = Instantiate(gamePrefab,transform);
            cartridge.transform.position = transform.position;
            foreach (FileInfo f in d.GetFiles())
            {
                if(f.Name == "data.json")
                {
                    string json = File.ReadAllText(f.FullName);
                    JsonGame data = JsonUtility.FromJson<JsonGame>(json);
                    Debug.Log(data.Name);
                    cartridge.GetComponent<Game>().Name = data.Name;
                    cartridge.name = data.Name;
                    cartridge.GetComponent<Game>().path = data.Path;
                    cartridge.GetComponent <Game>().desc = data.Description;
                    
                }
                if (f.Name == "Icon.png")
                {
                    byte[] imageBytes = File.ReadAllBytes(f.FullName);
                    Texture2D tx = new Texture2D(4, 3);
                    tx.LoadImage(imageBytes);

                    cartridge.GetComponent<Game>().icon = tx;
                }
            }
        }
    }
    public void DeleteGame(GameObject game)
    {
        //DirectoryInfo deleteDir = new DirectoryInfo(game.GetComponent<Game>().path.Substring(0, game.GetComponent<Game>().path.LastIndexOf("\\")));
        Directory.Delete(game.GetComponent<Game>().path.Substring(0, game.GetComponent<Game>().path.LastIndexOf("\\")),true);
        UpdateGameList();
    }
}
