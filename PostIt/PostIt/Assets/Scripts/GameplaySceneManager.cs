using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;


public enum Destination
{
    Hell,
    notNorthKorea,
    Deathstar,
    Arrakis,
    TheDump,
    BuildABear,
    Pentagon,
    DonutElem,
    FortBragg,
    Hospital,
    CityHall,
    FarmerJo,
    BagEnd,
    NorthPole,
    YourMom,
    Trash
}

[Serializable]
public struct MailSlotMarker
{
    public Destination type;
    public GameObject slot;
}

public class GameplaySceneManager : MonoBehaviour, ISingleton<GameplaySceneManager>
{
    private static GameplaySceneManager _inst;
    public static GameplaySceneManager Instance
    {
        get
        {
            if (_inst == null)
            {
                _inst = GameObject.FindObjectOfType<GameplaySceneManager>();
            }
            return _inst;
        }
    }

    public Collider2D TableCollider { get; internal set; }
    public List<MailSlotMarker> mailSlotMarkers;
    public Dictionary<Destination, List<Package>> MailInSlots;

    int currentRound = 0;
    private float roundStart;
    float roundLen;
    private float roundEnd;
    private List<Package> packageEnties = new List<Package>();

    private void Start()
    {/*
        roundStart = Time.time;
        currentRound = GameManager.Instance.currentRound;
        if (!GameManager.Instance.roundNumToRoundLen.TryGetValue(currentRound,out roundLen)){
            Debug.LogWarning("GameplaySceneManager:Start roundLen not found, defaulting to 180 seconds");
            roundLen = 180;
        }
        if (roundLen <= 0) {
            Debug.LogWarning("GameplaySceneManager:Start roundLen has an invalid value of "+ roundLen + 
                ", defaulting to 180 seconds");
            roundLen = 180;
        }
        roundEnd = roundStart + roundLen;*/
    }

    public void AddSpawnedPackage(Package spawnedPackage) 
    {
        packageEnties.Add(spawnedPackage);
    }
    public void DestroySpawnedPackage(Package spawnedPackage) {
        if (packageEnties.Count > 0 && packageEnties.Contains(spawnedPackage))
        {
            packageEnties.Remove(spawnedPackage);
        }
        Destroy(spawnedPackage.gameObject);
    }
    public List<Package> PackagesOnTable() { 
        List<Package> result = new List<Package>();
        foreach (Package package in packageEnties) {
            if (package is null)
            {
                continue;
            }
            if (package.isOnTable()) {
                result.Add(package);
            }
        }
        return result;
    }

    public float GetTablePos()
    {
        Bounds bounds = TableCollider.bounds;
        return UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
    }

    public void HandleMailPlacedInSlot(Destination type, Package package) {
        if (type == Destination.Trash) {
            DestroySpawnedPackage(package);
            return;
        }
        List<Package> packageList;
        if (!MailInSlots.TryGetValue(type,out packageList)) {
            packageList = new List<Package>();
            MailInSlots.Add(type, packageList);
        }
        foreach (Package oldPackage in packageList) {
            oldPackage.ReduceSortOrder();
        }
        packageList.Add(package);
    }

    //Table
    //Package spawner
    //delivery bins
}
