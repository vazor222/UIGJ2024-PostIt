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
        if( GameManager.Instance.player1Data.secretMissionPackages > GameManager.Instance.player1Data.correctlyDeliveredPackages )
        {
            if( a != null )
                a.PlayBGMusic(a.doubleAgentWinScreenTheme);
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
