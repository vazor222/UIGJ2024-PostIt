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
    private IndicatorBounce indicator;
    int keyboardPlayerSelectedPackageIndex = -1;

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
        indicator = FindObjectOfType<IndicatorBounce>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (packageEnties.Exists(m => m.IsOnTable()))
            {
                // select the next package
                do
                {
                    if (++keyboardPlayerSelectedPackageIndex >= packageEnties.Count)
                        keyboardPlayerSelectedPackageIndex = 0;
                }
                while (!packageEnties[keyboardPlayerSelectedPackageIndex].IsOnTable());
                Package p = packageEnties[keyboardPlayerSelectedPackageIndex];

                // move the indicator to the package
                indicator.transform.position = new Vector3(p.transform.position.x, p.transform.position.y+1, 1);
            }
        }

        if( Input.GetKeyDown(KeyCode.Q) )
        {
            packageEnties[keyboardPlayerSelectedPackageIndex].PlaceInSlot(mailSlotMarkers.Find(m => m.type == Destination.Hell).slot.transform.position, Destination.Hell);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            packageEnties[keyboardPlayerSelectedPackageIndex].PlaceInSlot(mailSlotMarkers.Find(m => m.type == Destination.notNorthKorea).slot.transform.position, Destination.notNorthKorea);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            packageEnties[keyboardPlayerSelectedPackageIndex].PlaceInSlot(mailSlotMarkers.Find(m => m.type == Destination.Deathstar).slot.transform.position, Destination.Deathstar);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            packageEnties[keyboardPlayerSelectedPackageIndex].PlaceInSlot(mailSlotMarkers.Find(m => m.type == Destination.Arrakis).slot.transform.position, Destination.Arrakis);
            // TODO: play sfx
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            packageEnties[keyboardPlayerSelectedPackageIndex].PlaceInSlot(mailSlotMarkers.Find(m => m.type == Destination.TheDump).slot.transform.position, Destination.TheDump);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            packageEnties[keyboardPlayerSelectedPackageIndex].PlaceInSlot(mailSlotMarkers.Find(m => m.type == Destination.BuildABear).slot.transform.position, Destination.BuildABear);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            packageEnties[keyboardPlayerSelectedPackageIndex].PlaceInSlot(mailSlotMarkers.Find(m => m.type == Destination.Pentagon).slot.transform.position, Destination.Pentagon);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            packageEnties[keyboardPlayerSelectedPackageIndex].PlaceInSlot(mailSlotMarkers.Find(m => m.type == Destination.DonutElem).slot.transform.position, Destination.DonutElem);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            packageEnties[keyboardPlayerSelectedPackageIndex].PlaceInSlot(mailSlotMarkers.Find(m => m.type == Destination.FortBragg).slot.transform.position, Destination.FortBragg);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            packageEnties[keyboardPlayerSelectedPackageIndex].PlaceInSlot(mailSlotMarkers.Find(m => m.type == Destination.Hospital).slot.transform.position, Destination.Hospital);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            packageEnties[keyboardPlayerSelectedPackageIndex].PlaceInSlot(mailSlotMarkers.Find(m => m.type == Destination.CityHall).slot.transform.position, Destination.CityHall);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            packageEnties[keyboardPlayerSelectedPackageIndex].PlaceInSlot(mailSlotMarkers.Find(m => m.type == Destination.FarmerJo).slot.transform.position, Destination.FarmerJo);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            packageEnties[keyboardPlayerSelectedPackageIndex].PlaceInSlot(mailSlotMarkers.Find(m => m.type == Destination.BagEnd).slot.transform.position, Destination.BagEnd);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            packageEnties[keyboardPlayerSelectedPackageIndex].PlaceInSlot(mailSlotMarkers.Find(m => m.type == Destination.NorthPole).slot.transform.position, Destination.NorthPole);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            packageEnties[keyboardPlayerSelectedPackageIndex].PlaceInSlot(mailSlotMarkers.Find(m => m.type == Destination.YourMom).slot.transform.position, Destination.YourMom);
        }
    }

    private void FixedUpdate()
    {
        foreach (Package package in PackagesOnTable()) {
            if (package is null)
            {
                continue;
            }
            else if(package.transform.position.y <= -10)
            {
                DestroySpawnedPackage(package);
            }
        }
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
            if (package.IsOnTable()) {
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
