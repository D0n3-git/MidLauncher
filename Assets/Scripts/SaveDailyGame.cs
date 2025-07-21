using System.IO;
using UnityEngine;

public class SaveDailyGame : MonoBehaviour
{
    DirectoryInfo currentDailyGameDir;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentDailyGameDir = new DirectoryInfo(Application.persistentDataPath + "/DGame");
    }

    // Update is called once per frame
    public void SaveGame(Game game)
    {
        foreach (FileInfo f in currentDailyGameDir.GetDirectories()[0].GetFiles())
        {
            if (f.Name == "data.json")
            {
                string json = File.ReadAllText(f.FullName);
                JsonGame data = JsonUtility.FromJson<JsonGame>(json);
                bool isSaved = false;
                foreach (DirectoryInfo d in new DirectoryInfo(Application.persistentDataPath + "/Saved Games").GetDirectories())
                {
                    if (d.Name == f.Directory.Name)
                    {
                        isSaved = true;
                        game.GetComponent<Game>().path = Application.persistentDataPath + "/Saved Games/" + data.path;
                        Debug.Log("Allready Saved");
                    }
                }
                if (!isSaved)
                {
                    new DirectoryInfo(currentDailyGameDir.GetDirectories()[0].FullName).MoveTo(Application.persistentDataPath + "/Saved Games/" + data.path);
                    gameObject.GetComponent<GameRotation>().gameSpawner.GetComponent<SavedGames>().UpdateGameList();
                }
            }
            break;
        }
    }

}
