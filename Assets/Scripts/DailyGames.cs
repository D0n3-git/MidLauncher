using System.IO;
using UnityEngine;
using System.IO.Compression;

public class DailyGames : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    FileInfo currentDailyGame;
    void Start()
    {
        currentDailyGame = new FileInfo(Application.persistentDataPath+"/DGame.zip");
        if (Directory.Exists(Application.persistentDataPath + "/DGame"))
        {
            Directory.Delete(Application.persistentDataPath + "/DGame",true);
            Debug.Log("DeletedOldDir");

        }
        ZipFile.ExtractToDirectory(Application.persistentDataPath + "/DGame.zip",Application.persistentDataPath + "/DGame");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
