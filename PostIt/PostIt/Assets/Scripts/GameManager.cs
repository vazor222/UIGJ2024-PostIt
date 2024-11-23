using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ISingleton<GameManager>
{
    private static GameManager _inst;
    public static GameManager Instance
    {
        get
        {
            if (_inst == null)
            {
                _inst = GameObject.FindObjectOfType<GameManager>();
            }
            return _inst;
        }
    }

    public int currentRound = 0;
    Dictionary<int, int> roundNumToNumPackages = new Dictionary<int, int>{
        { 1,60 },
        { 2,120 },
        { 3,180 },
    };

    public playerData player1Data = new playerData
    {
        secretMissionPackages = 0,
        misdeliveredPackages = 0,
        correctlyDeliveredPackages = 0
    };
}

public struct playerData
{
    public int secretMissionPackages;
    public int misdeliveredPackages;
    public int correctlyDeliveredPackages;
}
