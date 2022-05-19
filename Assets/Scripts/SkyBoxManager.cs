using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxManager : MonoBehaviour
{
    public static SkyBoxManager instance;


    public float cooldown;
    [SerializeField] private Material[] skyBox;
    private int skyBoxId = 0;

    public bool sunRaysOn;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InvokeRepeating("ChangeSky", 0, cooldown);
    }

    private void ChangeSky()
    {
        Debug.Log(skyBoxId);
        RenderSettings.skybox = skyBox[skyBoxId];

        if (skyBoxId < 1)
            sunRaysOn = true;
        else
            sunRaysOn = false;


        skyBoxId++;

        if (skyBoxId >= skyBox.Length)
        {
            skyBoxId = 0;
        }          
    }
}
