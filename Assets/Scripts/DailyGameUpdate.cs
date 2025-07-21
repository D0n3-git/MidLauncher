using System;
using System.IO;
using System.Net;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DailyGameUpdate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    FileInfo currentDailyGameZip;
    string curDT;
    void Start()
    {
        curDT = DateTime.UtcNow.TimeOfDay.ToString();
        curDT = curDT.Substring(0,curDT.IndexOf('.'));
        Debug.Log(curDT);
        currentDailyGameZip = new FileInfo(Directory.GetFiles(Application.persistentDataPath, "*zip")[0]);
        using (WebClient client = new WebClient())
        {
            try
            {
                Debug.Log("Atempting to download here:" + currentDailyGameZip.Directory.FullName);
                client.DownloadFile("http://194.87.213.90/down.php", currentDailyGameZip.FullName);
                Debug.Log("Downloaded");
                currentDailyGameZip.Delete();
                Debug.Log("Old game deleted");
            }
            catch(Exception ex) 
            {
                Debug.Log(ex);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(curDT == "0:00:00")
        {
            Start();
        }
    }
}
