using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;


public enum Destination
{
    none,
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
public struct DestinationPair
{
    public Destination destination;
    public Destination secretDestination;
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
    public Dictionary<Destination, List<Package>> MailInSlots = new Dictionary<Destination, List<Package>>();

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

    public DestinationPair GetNextDestinationPair()
    {
        Destination normalDest = GetRandomValidDestination();
        DestinationPair result = new DestinationPair
        {
            destination = normalDest,
            secretDestination = Destination.none
        };
        switch (normalDest) { 
            case Destination.NorthPole:
                result.secretDestination = Destination.notNorthKorea;
                break;
            default:
                result.secretDestination = Destination.none;
                break;
        }
        return result;
    }

    private Destination GetRandomValidDestination()
    {
        // Create a list of valid destinations (excluding 'none' and 'Trash')
        List<Destination> validDestinations = new List<Destination>
        {
            Destination.Hell,
            Destination.notNorthKorea,
            Destination.Deathstar,
            Destination.Arrakis,
            Destination.TheDump,
            Destination.BuildABear,
            Destination.Pentagon,
            Destination.DonutElem,
            Destination.FortBragg,
            Destination.Hospital,
            Destination.CityHall,
            Destination.FarmerJo,
            Destination.BagEnd,
            Destination.NorthPole,
            Destination.YourMom
        };

        // Randomly pick an index from the validDestinations list
        int randomIndex = UnityEngine.Random.Range(0, validDestinations.Count);

        // Return the random destination
        return validDestinations[randomIndex];
    }

    internal int GetDestinationCol(Destination destination)
    {
        switch (destination)
        {
            case Destination.Hell:
            case Destination.BuildABear:
            case Destination.CityHall:
                return 0;
            case Destination.notNorthKorea:
            case Destination.Pentagon:
            case Destination.FarmerJo:
                return 1;
            case Destination.Deathstar:
            case Destination.DonutElem:
            case Destination.BagEnd:
                return 2;
            case Destination.Arrakis:
            case Destination.FortBragg:
            case Destination.NorthPole:
                return 3;
            case Destination.TheDump:
            case Destination.Hospital:
            case Destination.YourMom:
                return 4;
            case Destination.Trash:
                Debug.LogError("trash is not a valid destination for package color");
                return 0;
        }
        Debug.LogError(" destination not found for package color");
        return 0;
    }

    internal int GetDestinationRow(Destination destination)
    {
        switch (destination)
        {
            case Destination.Hell:
            case Destination.notNorthKorea:
            case Destination.Deathstar:
            case Destination.Arrakis:
            case Destination.TheDump:
                return 0;
            case Destination.BuildABear:
            case Destination.Pentagon:
            case Destination.DonutElem:
            case Destination.FortBragg:
            case Destination.Hospital:
                return 1;
            case Destination.CityHall:
            case Destination.FarmerJo:
            case Destination.BagEnd:
            case Destination.NorthPole:
            case Destination.YourMom:
                return 2;
            case Destination.Trash:
                Debug.LogError("trash is not a valid destination for package symbol location");
                break;
        }
        Debug.LogError(" destination not found for package symbol location");
        return 2;
    }

    //Table
    //Package spawner
    //delivery bins
}
