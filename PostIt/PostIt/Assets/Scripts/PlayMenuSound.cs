using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenuSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickPlaySfx(AudioClip audioClip)
    {
        AudioManager a = FindObjectOfType<AudioManager>();
        if (a == null)
            return;

        a.PlaySfx(audioClip);
    }
}
