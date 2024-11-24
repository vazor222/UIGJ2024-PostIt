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
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public int currentRound = 0;
    public Dictionary<int, int> roundNumToNumPackages = new Dictionary<int, int>{
        { 1,60 },
        { 2,120 },
        { 3,180 },
    };
    //in seconds
    public Dictionary<int, float> roundNumToRoundLen = new Dictionary<int, float>{
        { 1,180.0f },
        { 2,180.0f },
        { 3,180.0f },
    };


    public Dictionary<PlayerType, PlayerData> playerDataDict = new Dictionary<PlayerType, PlayerData> {
        { PlayerType.Keyboard ,
            new PlayerData{
                secretMissionPackages = 0,
                misdeliveredPackages = 0,
                correctlyDeliveredPackages = 0
            }
        },
        { PlayerType.Mouse ,
            new PlayerData{
                secretMissionPackages = 0,
                misdeliveredPackages = 0,
                correctlyDeliveredPackages = 0
            }
        },
    };
}

public struct PlayerData
{
    public int secretMissionPackages;
    public int misdeliveredPackages;
    public int correctlyDeliveredPackages;
}
