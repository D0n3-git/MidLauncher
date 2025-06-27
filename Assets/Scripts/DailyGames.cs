using System.IO;
using UnityEngine;
using System.IO.Compression;

public class DailyGames : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    FileInfo currentDailyGameZip;
    public GameObject gamePrefab;
    DirectoryInfo currentDailyGameDir;
    void Start()
    {
        currentDailyGameZip = new FileInfo(Application.persistentDataPath+"/DGame.zip");
        if (Directory.Exists(Application.persistentDataPath + "/DGame"))
        {
            Directory.Delete(Application.persistentDataPath + "/DGame",true);
            Debug.Log("DeletedOldDir");

        }
        ZipFile.ExtractToDirectory(Application.persistentDataPath + "/DGame.zip",Application.persistentDataPath + "/DGame");
        currentDailyGameDir = new DirectoryInfo(Application.persistentDataPath + "/DGame");
        //if (File.Exists(Application.persistentDataPath + "/DGame/data.json"))
        //{
            GameObject cartridge = Instantiate(gamePrefab, transform);
            cartridge.transform.position = transform.position;
            foreach (FileInfo f in currentDailyGameDir.GetDirectories()[0].GetFiles())
            {
                if (f.Name == "data.json")
                {
                    Debug.Log("found json in there: "+f.FullName);

                    string json = File.ReadAllText(f.FullName);
                    JsonGame data = JsonUtility.FromJson<JsonGame>(json);
                    Debug.Log(data.Name);
                    cartridge.GetComponent<Game>().Name = data.Name;
                    cartridge.name = data.Name;
                    cartridge.GetComponent<Game>().path = Application.persistentDataPath + "/Saved Games/" + data.Path;
                    cartridge.GetComponent<Game>().desc = data.Description;
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
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
