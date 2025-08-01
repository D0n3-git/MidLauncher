using System.IO;
using UnityEngine;
using System.IO.Compression;

public class DailyGames : MonoBehaviour
{
    FileInfo currentDailyGameZip;
    public GameObject gamePrefab;
    DirectoryInfo currentDailyGameDir;
    //запускается на старте программы
    public void UpdateDGame()
    {
        Debug.Log("UwU");
        if (File.Exists(Application.persistentDataPath + "/daily.zip"))
        {
            currentDailyGameZip = new FileInfo(Application.persistentDataPath + "/daily.zip");  
            Debug.Log(currentDailyGameZip.FullName);
            if (Directory.Exists(Application.persistentDataPath + "/DGame"))
            {
                Directory.Delete(Application.persistentDataPath + "/DGame", true);
                Debug.Log("DeletedOldDir");
            }
            ZipFile.ExtractToDirectory(currentDailyGameZip.FullName, Application.persistentDataPath + "/DGame");
            currentDailyGameDir = new DirectoryInfo(Application.persistentDataPath + "/DGame");
            if (File.Exists(currentDailyGameDir.GetDirectories()[0].FullName + "/data.json"))
            {
                GameObject cartridge = Instantiate(gamePrefab, transform);
                cartridge.transform.position = transform.position;
                foreach (FileInfo f in currentDailyGameDir.GetDirectories()[0].GetFiles())
                {
                    if (f.Name == "data.json")
                    {
                        Debug.Log("found json in there: " + f.FullName);
                        string json = File.ReadAllText(f.FullName);
                        JsonGame data = JsonUtility.FromJson<JsonGame>(json);
                        Debug.Log(data.name);
                        cartridge.GetComponent<Game>().Name = data.name;
                        cartridge.name = data.name;
                        cartridge.GetComponent<Game>().path = Application.persistentDataPath + "/DGame/" + data.path;
                        cartridge.GetComponent<Game>().desc = data.description;
                        cartridge.GetComponent<Game>().isDaily = true;
                    }
                    else
                    {
                        Debug.Log("!found json in there: " + f.FullName);
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
}
