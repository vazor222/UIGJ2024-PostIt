using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundStartController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager a = FindObjectOfType<AudioManager>();
        if( a != null )
        {
            a.PlayBGMusic(a.missionSelectTheme);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGameplayMailroomScene()
    {
        SceneManager.LoadScene("MainGameplay");
        AudioManager a = FindObjectOfType<AudioManager>();
        if (a != null)
        {
            a.PlayBGAmbiance(a.mailRoomFanBackgroundAmbiance);
        }
    }
}
