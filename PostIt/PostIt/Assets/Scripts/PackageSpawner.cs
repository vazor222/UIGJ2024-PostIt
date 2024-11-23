using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Letter,
    BigEnvelope,
    LightPackage,
    MedPackage,
    HeavyPackage,
    Weapon
}

public class PackageSpawner : MonoBehaviour
{
    [SerializeField]
    private int packagesToSpawn = 5;
    public int CooldownTime = 600;
    private int coolDownRemaining = 0;
    public Collider spawnZone;

    private Dictionary<int, ItemType> weightedSpawnList;
    [SerializeField]
    public Dictionary<ItemType, GameObject> SpawnableObjects;

    public int maxNumPackagesOnTable = 20;

    void Start()
    {

    }

    private void FixedUpdate()
    {
        coolDownRemaining -= 1;
        if (isReadyToSpawn())
        {
            spawnPackage();
            packagesToSpawn -= 1;
            coolDownRemaining = CooldownTime;
        }
    }

    private bool isReadyToSpawn()
    {
        if (coolDownRemaining > 0){//spawn is on cooldown
            return false;
        }
        int currentNumOnTable = GameplaySceneManager.Instance.PackagesOnTable().Count;
        if (currentNumOnTable >= maxNumPackagesOnTable) { 
            return false;
        }
        return true;
    }

    public void spawnPackage()
    {
        if (spawnZone is null)
        {
            Debug.LogError("PackageSpawner:spawnPackage Failed -> spawnZone is null");
            return;
        }
        Vector3 spawnPoint = GetRandomSpawnPoint(spawnZone);
        GameObject packageToSpawn = null;
        ItemType type = PickItem();
        if (!SpawnableObjects.TryGetValue(PickItem(), out packageToSpawn)) {
            Debug.LogError("PackageSpawner:spawnPackage Failed to find game object of type : " 
                + type + "in SpawnableObjects.");
            return;
        }
        
        GameObject entity = Instantiate(packageToSpawn, spawnPoint, Quaternion.identity);
        //TODO Add pkg to list
        //GameplaySceneManager.Instance.AddEntity(entity);
    }

    Vector3 GetRandomSpawnPoint(Collider zone)
    {
        Bounds bounds = zone.bounds;

        float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = UnityEngine.Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, randomY, randomZ);
    }

    public ItemType PickItem()
    {
        if (weightedSpawnList is null || weightedSpawnList.Count < 1) {
            Debug.LogWarning("PackageSpawner:PickItem weightedSpawnList not found, defaulting to Letter");
            return ItemType.Letter;
        }
        int totalWeight = 0;
        foreach (var weight in weightedSpawnList.Keys)
        {
            totalWeight += weight;
        }
        int randomNumber = UnityEngine.Random.Range(0, totalWeight);

        int cumulativeWeight = 0;
        foreach (var kvp in weightedSpawnList)
        {
            cumulativeWeight += kvp.Key;
            if (randomNumber < cumulativeWeight)
            {
                return kvp.Value;
            }
        }
        Debug.LogError("PackageSpawner:PickItem Failed to resolve weighted probability with randomNumber : " 
            + randomNumber + " and totalWeight : " + totalWeight);
        return ItemType.Letter;
    }
}
