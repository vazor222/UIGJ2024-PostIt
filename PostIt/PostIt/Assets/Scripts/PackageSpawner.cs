using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ItemType
{
    none,
    Letter,
    BigEnvelope,
    LightPackage,
    MedPackage,
    HeavyPackage,
    Weapon
}

[Serializable]
public struct SpawnableObject
{
    public ItemType type;
    public Package prefab;
    public List<Sprite> Colors;
    public List<Sprite> Symbols;
}

public class PackageSpawner : MonoBehaviour
{
    [SerializeField]
    private int packagesToSpawn = 5;
    public int CooldownTime = 100;
    private int coolDownRemaining = 0;
    public Collider2D spawnZone;
    [SerializeField]
    private int spawnedZPos = 0;

    private Dictionary<int, ItemType> weightedSpawnList;
    public List<SpawnableObject> SpawnableObjects;

    public int maxNumPackagesOnTable = 20;

    void Start()
    {

    }

    private void FixedUpdate()
    {
        coolDownRemaining -= 1;
        if (IsReadyToSpawn())
        {
            SpawnPackage();
            packagesToSpawn -= 1;
            if (CooldownTime > 30) {
                CooldownTime -= 1;
            }
            coolDownRemaining = CooldownTime;
        }
    }

    private bool IsReadyToSpawn()
    {
        if (packagesToSpawn <= 0)
        {
            return false;
        }
        if (coolDownRemaining > 0){//spawn is on cooldown
            return false;
        }
        int currentNumOnTable = GameplaySceneManager.Instance.PackagesOnTable().Count;
        if (currentNumOnTable >= maxNumPackagesOnTable) { 
            return false;
        }
        return true;
    }

    public void SpawnPackage()
    {
        if (spawnZone is null)
        {
            Debug.LogError("PackageSpawner:spawnPackage Failed -> spawnZone is null");
            return;
        }
        Vector3 spawnPoint = GetRandomSpawnPoint(spawnZone);
        Package packageToSpawn = null;
        Sprite colorSprite = null;
        Sprite symbolSprite = null;
        DestinationPair dest = new DestinationPair { destination = Destination.none, secretDestination = Destination.none };
        int row = 0;
        int col = 0;
        ItemType type = PickItem();
        foreach (SpawnableObject spawnableObject in SpawnableObjects) {
            if (spawnableObject.type == type)
            {
                packageToSpawn = spawnableObject.prefab;

                symbolSprite = spawnableObject.Symbols[0];
                dest = GameplaySceneManager.Instance.GetNextDestinationPair();
                if (dest.destination == Destination.none) { 
                    Debug.LogError("PackageSpawner:spawnPackage Failed to find next destination");
                }
                row = GameplaySceneManager.Instance.GetDestinationRow(dest.destination);
                col = GameplaySceneManager.Instance.GetDestinationCol(dest.destination);
                colorSprite = spawnableObject.Colors[col];
                if (dest.secretDestination == Destination.none)
                {
                    symbolSprite = spawnableObject.Symbols[0];
                }
                else
                {
                    symbolSprite = spawnableObject.Symbols[1];//TODO set symbol based on specific secret dest
                }
                break;
            }
        }
        if (packageToSpawn is null) {
            Debug.LogError("PackageSpawner:spawnPackage Failed to find game object of type : " 
                + type + "in SpawnableObjects.");
            return;
        }
        if (colorSprite is null)
        {
            Debug.LogError("PackageSpawner:spawnPackage Failed to find colorSprite of type : "
                + type + "in SpawnableObjects.");
            return;
        }
        if (symbolSprite is null)
        {
            Debug.LogError("PackageSpawner:spawnPackage Failed to find symbolSprite of type : "
                + type + "in SpawnableObjects.");
            return;
        }

        Debug.Log("PackageSpawner:spawnPackage spawning " + type + " -> at " + spawnPoint);
        Package entity = Instantiate(packageToSpawn.gameObject, spawnPoint, Quaternion.identity).GetComponent<Package>();
        entity.SetIntendedDestination(colorSprite, symbolSprite, row, dest.destination, dest.secretDestination);
        GameplaySceneManager.Instance.AddSpawnedPackage(entity);
    }

    Vector3 GetRandomSpawnPoint(Collider2D zone)
    {
        Bounds bounds = zone.bounds;

        float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(randomX, randomY, spawnedZPos);
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
