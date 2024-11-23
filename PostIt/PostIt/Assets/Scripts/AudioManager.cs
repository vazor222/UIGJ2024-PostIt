using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip mainMenuTheme;
    public AudioClip missionSelectTheme;
    public AudioClip mailRoomTheme;
    public AudioClip endOfRoundTheme;
    public AudioClip loyalistWinScreenTheme;
    public AudioClip doubleAgentWinScreenTheme;

    public AudioClip mailRoomLoyalistStem;
    public AudioClip mailRoomDoubleAgentStem;

    public AudioClip packageIncomingSfx;
    public AudioClip packagePickupSfx;
    public AudioClip packageDropSfx;
    public AudioClip packageSortedSfx;
    public AudioClip stampSfx;

    private AudioSource sfxAudioSource;
    private AudioSource backgroundAudioSource;
    private AudioSource stemLoyalistAudioSource;
    private AudioSource stemDoubleAgentAudioSource;

    private enum StemState
    {
        NoStem,
        StemLoyalistFadingIn,
        StemDoubleAgentFadingIn,
        StemLoyalistPlaying,
        StemDoubleAgentPlaying,
        StemLoyaistFadingOut,
        StemDoubleAgentFadingOut,
    }

    private StemState stemState = StemState.NoStem;
    private bool loyalistStemQueued = false;
    private bool doubleAgentStemQueued = false;
    private float fadeStartTime;
    private float fadeTimer;
    public const float FADE_TOTAL_TIME = 3f;

    void Start()
    {
        PlayBGMusic(mainMenuTheme);
    }

    void Update()
    {
        if( loyalistStemQueued )
        {
            if( stemState == StemState.NoStem )
            {
                doLoyalistStemFadeIn();
            }
        }
        else if( doubleAgentStemQueued )
        {
            if( stemState == StemState.NoStem )
            {
                doDoubleAgentStemFadeIn();
            }
        }

        fadeTimer += Time.deltaTime;
        switch(stemState)
        {
            case StemState.StemDoubleAgentFadingIn:
                if (fadeTimer < FADE_TOTAL_TIME)
                {
                    float newVolume = Mathf.Lerp(stemDoubleAgentAudioSource.volume, 1f, fadeTimer / FADE_TOTAL_TIME);
                    stemDoubleAgentAudioSource.volume = newVolume;
                }
                else
                {
                    // done fading
                    stemDoubleAgentAudioSource.volume = 1f;
                    stemState = StemState.StemLoyalistPlaying;
                }
                break;
        }
    }

    public void PlayBGMusic(AudioClip theme)
    {
        backgroundAudioSource.clip = theme;
        backgroundAudioSource.Play();
    }

    public void PlayBGMusicWithStems(AudioClip theme, AudioClip stemLoyalist, AudioClip stemDoubleAgent)
    {
        stemState = StemState.NoStem;
        loyalistStemQueued = false;
        doubleAgentStemQueued = false;
        backgroundAudioSource.clip = theme;
        stemLoyalistAudioSource.clip = stemLoyalist;
        stemLoyalistAudioSource.volume = 0f;
        stemDoubleAgentAudioSource.clip = stemDoubleAgent;
        stemDoubleAgentAudioSource.volume = 0f;

        double startMusicTogetherTime = AudioSettings.dspTime + 2.0f;
        backgroundAudioSource.PlayScheduled(startMusicTogetherTime);
        stemLoyalistAudioSource.PlayScheduled(startMusicTogetherTime);
        stemDoubleAgentAudioSource.PlayScheduled(startMusicTogetherTime);
}

    public void StartLoyalistStem()
    {
        if( stemState == StemState.NoStem )
        {
            doLoyalistStemFadeIn();
        }
        else
        {
            loyalistStemQueued = true;
        }
    }

    private void doLoyalistStemFadeIn()
    {
        fadeTimer = 0f;
        stemState = StemState.StemLoyalistFadingIn;
    }

    private void doDoubleAgentStemFadeIn()
    {
        fadeTimer = 0f;
        stemState = StemState.StemDoubleAgentFadingIn;
    }
}
