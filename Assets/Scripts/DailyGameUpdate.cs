using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;
using static UnityEngine.Rendering.DebugUI.Table;

public class DailyGameUpdate : MonoBehaviour
{
    private string downloadUrl = "http://194.87.213.90/down.php";
    private string persistentPath;
    private string zipPath;
    private string tempZipPath;
    Process launcherProcess = Process.GetCurrentProcess();
    CancellationTokenSource cts;
    public TMP_Text DownloadProgress;
    public GameObject UI;
    StreamWriter sw;
    StreamReader sr;
    //запускаетс€ на старте программы
    void Start()
    {
        persistentPath = Application.persistentDataPath;
        if (!File.Exists(persistentPath + "/timeInfo.txt"))
        {
            Debug.Log("-_0");
            File.Create(persistentPath + "/timeInfo.txt").Close();
        }
        sr = new StreamReader(persistentPath + "/timeInfo.txt");
        
        UI.SetActive(false);
        cts = new CancellationTokenSource();
        //File.Create(persistentPath + "/daily_game_temp.zip");
        zipPath = Path.Combine(persistentPath, "daily.zip");
        tempZipPath = Path.Combine(persistentPath, "daily_game_temp.zip");
        Debug.Log("[DailyGameUpdate] Persistent data path: " + persistentPath);
        Debug.Log("[DailyGameUpdate] Target ZIP path: " + zipPath);
        Debug.Log("[DailyGameUpdate] Temp ZIP path: " + tempZipPath);
        if(sr.ReadLine()!= DateTime.UtcNow.Date.ToString())
        {
            sr.Close();
            File.Delete(zipPath);
            CheckAndDownloadDailyGame(cts.Token);
        }
        else
        {
            gameObject.GetComponent<DailyGames>().UpdateDGame();
        }
    }
    void OnApplicationQuit()
    {
        
        Debug.Log("Download canceled");
        cts.Cancel();
    }
    void OnDestroy()
    {
        if (cts != null)
        {
            cts.Dispose();
        }
    }
    //загрузка игры дн€ 
    async void CheckAndDownloadDailyGame(CancellationToken token)
    {
        Debug.Log("[DailyGameUpdate] Start game update proverk...");
        if (File.Exists(tempZipPath))
        {
            Debug.Log("[DailyGameUpdate] Remove old temp ZIP file...");
            try
            {
                File.Delete(tempZipPath);
                Debug.Log("[DailyGameUpdate] Old  ZIP file removed.");
            }
            catch (Exception ex)
            {
                Debug.LogError("[DailyGameUpdate] Fail to delte temp file: " + ex.Message);
                return;
            }
        }

        Debug.Log("[DailyGameUpdate] Start download: " + downloadUrl);
        UI.SetActive(true);

        using (HttpClient client = new HttpClient())
        {
            try
            {
                Debug.Log("send head req.");
                var headRequest = new HttpRequestMessage(HttpMethod.Head, downloadUrl);
                var headResponse = await client.SendAsync(headRequest, token);
                if (headResponse.Content.Headers.ContentLength.HasValue)
                {
                    long totalBytes = headResponse.Content.Headers.ContentLength.Value;
                    Debug.Log($"[DailyGameUpdate] Total file size: {totalBytes} bytes");
                }
                else
                {
                    Debug.LogWarning("Server  not return lehtn cont.");
                }
                Debug.Log("Starting download with prog track");
                using (var response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead, token))
                {
                    response.EnsureSuccessStatusCode();

                    var totalLength = response.Content.Headers.ContentLength ?? -1L;

                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(tempZipPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        byte[] buffer = new byte[81920];
                        int bytesRead;
                        long totalRead = 0;

                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            token.ThrowIfCancellationRequested();

                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            totalRead += bytesRead;

                            if (totalLength > 0)
                            {
                                float progress = (float)totalRead / totalLength * 100f;
                                Debug.Log($"  Download progress {progress:F2}% ({totalRead}/{totalLength} bytes)");
                                DownloadProgress.text = (int)progress + "";
                            }
                            else
                            {
                                Debug.Log($"]Downloaded: {totalRead}bytes so far...");
                            }
                        }
                    }
                    
                    Debug.Log("File download completed.");
                    if(File.Exists(Application.persistentDataPath + "/timeInfo.txt"))
                    {
                        using (var sw = new StreamWriter(Application.persistentDataPath + "/timeInfo.txt"))
                        {
                            sw.WriteLine(DateTime.UtcNow.Date.ToString());
                            sw.Close();
                        }
                    }
                    UI.SetActive(false);
                }
                if (File.Exists(zipPath)) File.Delete(zipPath);
                File.Move(tempZipPath, zipPath);
                Debug.Log(" Daily game updated successfully.");
                gameObject.GetComponent<DailyGames>().UpdateDGame();
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("Download was cancelled.");
            }
            catch (Exception ex)
            {
                Debug.LogError("Error during download: " + ex.Message);
            }

        }
    }
        //повтор€етс€ в течении работы программы
void Update()
    {
        TimeSpan now = DateTime.UtcNow.TimeOfDay;
        if (now.Hours == 0 && now.Minutes == 0 && now.Seconds == 0)
        {
            Debug.Log("Midnight");
            CheckAndDownloadDailyGame(cts.Token);
        }
    }
}
