using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenController : MonoBehaviour
{
    public GameObject loyalistWinScreenBG;
    public GameObject doubleAgentWinScreenBG;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager a = FindObjectOfType<AudioManager>();
        int doubleAgentScore = GameManager.Instance.playerDataDict[PlayerType.Keyboard].correctlyDeliveredPackages + GameManager.Instance.playerDataDict[PlayerType.Keyboard].secretMissionPackages * 5 - GameManager.Instance.playerDataDict[PlayerType.Keyboard].misdeliveredPackages;
        int loyalistScore = GameManager.Instance.playerDataDict[PlayerType.Mouse].correctlyDeliveredPackages + GameManager.Instance.playerDataDict[PlayerType.Mouse].secretMissionPackages * 5 - GameManager.Instance.playerDataDict[PlayerType.Mouse].misdeliveredPackages;
        if ( doubleAgentScore > loyalistScore )
        {
            if( a != null )
            {
                a.PlayBGMusic(a.doubleAgentWinScreenTheme);
                a.PlaySfx(a.bigExplosionSfx);
            }
            doubleAgentWinScreenBG.SetActive(true);
            loyalistWinScreenBG.SetActive(false);
        }
        else
        {
            if( a != null )
                a.PlayBGMusic(a.loyalistWinScreenTheme);
            doubleAgentWinScreenBG.SetActive(false);
            loyalistWinScreenBG.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
