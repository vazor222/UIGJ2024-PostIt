using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundEndController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager a = FindObjectOfType<AudioManager>();
        if( a != null )
        {
            a.PlayBGMusic(a.endOfRoundTheme);
        }
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
        SceneManager.LoadScene("Win");
    }
}
