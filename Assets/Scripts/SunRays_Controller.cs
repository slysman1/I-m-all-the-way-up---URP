using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRays_Controller : MonoBehaviour
{
    [SerializeField] private GameObject[] decor;
    private Player player;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        player = Player.instance.GetComponent<Player>();
        InvokeRepeating("CheckIfRaysOff", 0, 1f);
    }


    private void CheckIfRaysOff()
    {
        bool playerNearBy = Vector2.Distance(player.transform.position, transform.position) < 20;

        meshRenderer.enabled = playerNearBy;

        for (int i = 0; i < decor.Length; i++)
        {
            decor[i].SetActive(playerNearBy);
        }
       
    }
}
