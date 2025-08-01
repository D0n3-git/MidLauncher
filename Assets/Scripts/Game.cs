using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Game : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string Name;
    public string desc;
    public Texture2D icon;
    public string path;
    public bool isDaily;
    //запускается на старте программы
    void Start()
    {
        transform.GetChild(1).GetComponent<TextMeshPro>().text = Name;
        transform.GetChild(2).GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", icon);
        transform.GetChild(3).GetComponent<TextMeshPro>().text = desc;
    }
}
