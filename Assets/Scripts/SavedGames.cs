using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class JsonGame
{
    public string name;
    public string path;
    public string description;
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
    }

    public void UpdateGameList()
    {
        foreach(DirectoryInfo d in directories)
        {
            if (File.Exists(d.FullName + "/data.json"))
            {
                GameObject cartridge = Instantiate(gamePrefab, transform);
                cartridge.transform.position = transform.position;
                foreach (FileInfo f in d.GetFiles())
                {
                
                    
                    if (f.Name == "data.json")
                    {
                        
                        string json = File.ReadAllText(f.FullName);
                        JsonGame data = JsonUtility.FromJson<JsonGame>(json);
                        cartridge.GetComponent<Game>().Name = data.name;
                        cartridge.name = data.name;
                        cartridge.GetComponent<Game>().path = Application.persistentDataPath + "/Saved Games/" + data.path;
                        cartridge.GetComponent<Game>().desc = data.description;
                        cartridge.GetComponent <Game>().isDaily = false;
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
    }
public void DeleteGame(GameObject game)
    {
        //DirectoryInfo deleteDir = new DirectoryInfo(game.GetComponent<Game>().path.Substring(0, game.GetComponent<Game>().path.LastIndexOf("\\")));
        string path = game.GetComponent<Game>().path.Substring(0, game.GetComponent<Game>().path.LastIndexOf("/"));
        Directory.Delete(path.Substring(0, path.LastIndexOf("/")),true);
        //Debug.Log(path.Substring(0, path.LastIndexOf("/")));
        UpdateGameList();
    }
}

