using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundEndController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textLoyalist;
    public TMPro.TextMeshProUGUI textDoubleAgent;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager a = FindObjectOfType<AudioManager>();
        if( a != null )
        {
            a.StopBGAmbiance();
            a.PlayBGMusic(a.endOfRoundTheme);
        }

        textDoubleAgent.text = "Double Agent score - correctlyDeliveredPackages:" + GameManager.Instance.playerDataDict[PlayerType.Keyboard].correctlyDeliveredPackages +
            "\nsecretMissionPackages:" + GameManager.Instance.playerDataDict[PlayerType.Keyboard].secretMissionPackages +
        "\nmisdeliveredPackages:" + GameManager.Instance.playerDataDict[PlayerType.Keyboard].misdeliveredPackages;
        textLoyalist.text = "Loyalist score - correctlyDeliveredPackages:" + GameManager.Instance.playerDataDict[PlayerType.Mouse].correctlyDeliveredPackages +
            "\nsecretMissionPackages:" + GameManager.Instance.playerDataDict[PlayerType.Mouse].secretMissionPackages +
        "\nmisdeliveredPackages:" + GameManager.Instance.playerDataDict[PlayerType.Mouse].misdeliveredPackages;
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
