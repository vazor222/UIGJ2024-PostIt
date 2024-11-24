using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundEndController : MonoBehaviour
{
    TMPro.TextMeshProUGUI textLoyalist;
    TMPro.TextMeshProUGUI textDoubleAgent;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager a = FindObjectOfType<AudioManager>();
        if( a != null )
        {
            a.StopBGAmbiance();
            a.PlayBGMusic(a.endOfRoundTheme);
        }

        textDoubleAgent.text = "Double Agent score - correctlyDeliveredPackages:" + GameplaySceneManager.Instance.playerDataDict[PlayerType.Keyboard].correctlyDeliveredPackages +
            "\nsecretMissionPackages:" + GameplaySceneManager.Instance.playerDataDict[PlayerType.Keyboard].secretMissionPackages +
        "\nmisdeliveredPackages:" + GameplaySceneManager.Instance.playerDataDict[PlayerType.Keyboard].misdeliveredPackages;
        textLoyalist.text = "Loyalist score - correctlyDeliveredPackages:" + GameplaySceneManager.Instance.playerDataDict[PlayerType.Mouse].correctlyDeliveredPackages +
            "\nsecretMissionPackages:" + GameplaySceneManager.Instance.playerDataDict[PlayerType.Mouse].secretMissionPackages +
        "\nmisdeliveredPackages:" + GameplaySceneManager.Instance.playerDataDict[PlayerType.Mouse].misdeliveredPackages;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadRoundStartScene()
    {
        SceneManager.LoadScene("RoundStart");
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene("WinScreen");
    }
}
