using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    private Transform player;
    private ObjectPool objectPool;

    [SerializeField] private GameObject goose;
    [Header("Chances to spawn")]


    [SerializeField] private float jackBoxChance;
    [SerializeField] private float trapBoxChance;
    [SerializeField] private float chanceToSpawnFeather;

    [Header("Position controll")]
    [SerializeField] private float maxPlayerDistance;
    [SerializeField] private float defaultCooldown = 0.5f;
    [SerializeField] private float standByCooldown = 4;
                     private float cooldown = 0.5f;


    [SerializeField] private Transform[] respawnTransform;
                     private List<Transform> listOfTransforms;






    private int milestonePosition = 20;
    [HideInInspector] public int playersMilestone = -7;

    private void Start()
    {

        objectPool = ObjectPool.instance;
        player = GameObject.Find("Player").transform;
        listOfTransforms = new List<Transform>();
        AddPositionsToList();

        StartCoroutine(WaitAndGenerate());

    }

    private IEnumerator WaitAndGenerate()
    {
        yield return new WaitForSeconds(cooldown);
        GenerateCube();
    }

    private void GenerateItem()
    {
        if (Random.Range(0, 100) < 2)
        {
            GameObject newGoose = Instantiate(goose);
        }
    }

    private void ChoosBoxAndSpawn(Transform transform)
    {
        bool jackBoxRoll = Random.Range(0, 100) < jackBoxChance;
        bool trapBoxRoll = Random.Range(0, 100) < trapBoxChance;
        bool featherLuckyRoll = Random.Range(0, 100) < chanceToSpawnFeather;

        if (trapBoxRoll)
        {
            GameObject newTrapBox = objectPool.SpawnFromPool("BoxTrap", transform.position, Quaternion.identity);
        }
        else if (jackBoxRoll)
        {
            GameObject newJackTrap = objectPool.SpawnFromPool("JackTrap", transform.position, Quaternion.identity);
        }
        else if (!jackBoxRoll && !trapBoxRoll)
        {
            CheckForMilestone();

            GameObject newCage = objectPool.SpawnFromPool("Cage",transform.position, Quaternion.identity);
            newCage.GetComponent<FallingCage>().SetupCage(featherLuckyRoll, playersMilestone);
        }
    }

    private void GenerateCube()
    {
        GenerateItem();

        if (listOfTransforms.Count > 0)
        {
            int randomPosition = Random.Range(0, listOfTransforms.Count);

            ChoosBoxAndSpawn(listOfTransforms[randomPosition]);
            CheckForChangeTransform();

            listOfTransforms.Remove(listOfTransforms[randomPosition]);
        }
        else
        {
            AddPositionsToList();
        }

        StartCoroutine(WaitAndGenerate());
    }

    private void CheckForChangeTransform()
    {
        if (Vector2.Distance(transform.position, player.transform.position) > maxPlayerDistance)
        {
            cooldown = standByCooldown;
        }
        else
        {
            cooldown = defaultCooldown;
            transform.position = new Vector2(transform.position.x, transform.position.y + 2f);
        }

    }


    private void AddPositionsToList()
    {
        for (int i = 0; i < respawnTransform.Length; i++)
        {
            listOfTransforms.Add(respawnTransform[i]);
        }
    }

    private void CheckForMilestone()
    {
        if (playersMilestone >= 0)
            return;


        if (player.transform.position.y > milestonePosition)
        {
            milestonePosition += 1;
            playersMilestone++;
        }
    }
}

