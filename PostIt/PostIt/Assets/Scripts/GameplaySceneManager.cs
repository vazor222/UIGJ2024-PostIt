using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    List<Package> packageEnties = new List<Package>();
    public List<Package> PackagesOnTable() { 
        return packageEnties;//TODO implement
    }
    //Table
    //Package spawner
    //delivery bins
}
