using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

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
    public List<Package> PackagesOnTable() { 
        return packageEnties;//TODO implement
    }

    public float GetTablePos()
    {
        Bounds bounds = TableCollider.bounds;
        return UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
    }


    //Table
    //Package spawner
    //delivery bins
}
