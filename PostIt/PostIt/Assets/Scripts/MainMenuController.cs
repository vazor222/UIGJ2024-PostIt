using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager a = FindObjectOfType<AudioManager>();
        a.PlayBGMusic(a.mainMenuTheme);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadStartMissionScene()
    {
        SceneManager.LoadScene("RoundStart");
    }
}
