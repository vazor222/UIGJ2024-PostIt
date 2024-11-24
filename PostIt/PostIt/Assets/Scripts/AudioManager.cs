using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public const float FADE_TOTAL_TIME = 3f;

    public AudioClip mainMenuTheme;
    public AudioClip missionSelectTheme;
    public AudioClip mailRoomTheme;
    public AudioClip endOfRoundTheme;
    public AudioClip loyalistWinScreenTheme;
    public AudioClip doubleAgentWinScreenTheme;

    public AudioClip mailRoomLoyalistStem;
    public AudioClip mailRoomDoubleAgentStem;

    public AudioClip mailRoomFanBackgroundAmbiance;

    public AudioClip packageSlideSfx;
    public AudioClip paperSlideSfx;
    public AudioClip paperFoldingSfx;
    public AudioClip countdownSfx;
    public AudioClip bigExplosionSfx;
    public AudioClip packageGlass1Sfx;
    public AudioClip packageGlass2Sfx;
    public AudioClip packageDrop1Sfx;
    public AudioClip packageDrop2Sfx;
    public AudioClip stampSfx;
    public AudioClip arrakisSfx;

    public AudioSource sfxAudioSource;
    public AudioSource backgroundAudioSource;
    public AudioSource stemLoyalistAudioSource;
    public AudioSource stemDoubleAgentAudioSource;
    public AudioSource backgroundAmbianceAudioSource;

    private enum StemState
    {
        NoStem,
        StemLoyalistFadingIn,
        StemDoubleAgentFadingIn,
        StemLoyalistPlaying,
        StemDoubleAgentPlaying,
        StemLoyalistFadingOut,
        StemDoubleAgentFadingOut,
    }

    private StemState stemState = StemState.NoStem;
    private bool loyalistStemQueued = false;
    private bool doubleAgentStemQueued = false;
    private float fadeTimer = FADE_TOTAL_TIME;

    void Start()
    {
        PlayBGMusic(mainMenuTheme);
        //PlayBGMusicWithStems(mailRoomTheme, mailRoomLoyalistStem, mailRoomDoubleAgentStem);  // TODO: use this in the main gameplay 
    }

    void Update()
    {
        if( loyalistStemQueued )
        {
            if( stemState == StemState.NoStem )
            {
                loyalistStemQueued = false;
                doLoyalistStemFadeIn();
            }
            else if (stemState == StemState.StemDoubleAgentPlaying)
            {
                doDoubleAgentStemFadeOut();
            }
        }
        else if( doubleAgentStemQueued )
        {
            if( stemState == StemState.NoStem )
            {
                doubleAgentStemQueued = false;
                doDoubleAgentStemFadeIn();
            }
            else if (stemState == StemState.StemLoyalistPlaying)
            {
                doLoyalistStemFadeOut();
            }
        }

        if( fadeTimer < FADE_TOTAL_TIME )
        {
            fadeTimer += Time.deltaTime;
            switch (stemState)
            {
                case StemState.StemDoubleAgentFadingIn:
                    if (fadeTimer < FADE_TOTAL_TIME)
                    {
                        float newVolume = Mathf.Lerp(0f, 1f, fadeTimer / FADE_TOTAL_TIME);
                        stemDoubleAgentAudioSource.volume = newVolume;
                    }
                    else
                    {
                        // done fading in
                        stemDoubleAgentAudioSource.volume = 1f;
                        stemState = StemState.StemDoubleAgentPlaying;
                    }
                    break;
                case StemState.StemDoubleAgentFadingOut:
                    if (fadeTimer < FADE_TOTAL_TIME)
                    {
                        float newVolume = Mathf.Lerp(1f, 0f, fadeTimer / FADE_TOTAL_TIME);
                        stemDoubleAgentAudioSource.volume = newVolume;
                    }
                    else
                    {
                        // done fading out
                        stemDoubleAgentAudioSource.volume = 0f;
                        stemState = StemState.NoStem;
                    }
                    break;
                case StemState.StemLoyalistFadingIn:
                    if (fadeTimer < FADE_TOTAL_TIME)
                    {
                        float newVolume = Mathf.Lerp(0f, 1f, fadeTimer / FADE_TOTAL_TIME);
                        stemLoyalistAudioSource.volume = newVolume;
                    }
                    else
                    {
                        // done fading in
                        stemLoyalistAudioSource.volume = 1f;
                        stemState = StemState.StemLoyalistPlaying;
                    }
                    break;
                case StemState.StemLoyalistFadingOut:
                    if (fadeTimer < FADE_TOTAL_TIME)
                    {
                        float newVolume = Mathf.Lerp(1f, 0f, fadeTimer / FADE_TOTAL_TIME);
                        stemLoyalistAudioSource.volume = newVolume;
                    }
                    else
                    {
                        // done fading out
                        stemLoyalistAudioSource.volume = 0f;
                        stemState = StemState.NoStem;
                    }
                    break;
            }
        }
    }

    public void PlaySfx(AudioClip sfx)
    {
        sfxAudioSource.PlayOneShot(sfx);
    }

    public void PlayBGAmbiance(AudioClip ambiance)
    {
        backgroundAmbianceAudioSource.clip = ambiance;
        backgroundAmbianceAudioSource.loop = true;
        backgroundAmbianceAudioSource.Play();
    }

    public void StopBGAmbiance()
    {
        backgroundAmbianceAudioSource.Stop();
    }

    public void PlayBGMusic(AudioClip theme)
    {
        backgroundAudioSource.clip = theme;
        backgroundAudioSource.loop = true;
        backgroundAudioSource.Play();
    }

    public void PlayBGMusicWithStems(AudioClip theme, AudioClip stemLoyalist, AudioClip stemDoubleAgent)
    {
        stemState = StemState.NoStem;
        loyalistStemQueued = false;
        doubleAgentStemQueued = false;
        backgroundAudioSource.clip = theme;
        backgroundAudioSource.loop = true;
        stemLoyalistAudioSource.clip = stemLoyalist;
        stemLoyalistAudioSource.loop = true;
        stemLoyalistAudioSource.volume = 0f;
        stemDoubleAgentAudioSource.clip = stemDoubleAgent;
        stemDoubleAgentAudioSource.loop = true;
        stemDoubleAgentAudioSource.volume = 0f;

        double startMusicTogetherTime = AudioSettings.dspTime + 2.0f;
        backgroundAudioSource.PlayScheduled(startMusicTogetherTime);
        stemLoyalistAudioSource.PlayScheduled(startMusicTogetherTime);
        stemDoubleAgentAudioSource.PlayScheduled(startMusicTogetherTime);
    }

    public void StartLoyalistStem()
    {
        if( stemState == StemState.StemLoyalistFadingIn || stemState == StemState.StemLoyalistPlaying )
        {
            return;
        }

        if( stemState == StemState.NoStem )
        {
            doLoyalistStemFadeIn();
        }
        else
        {
            loyalistStemQueued = true;
            doubleAgentStemQueued = false;
        }
    }

    public void StartDoubleAgentStem()
    {
        if (stemState == StemState.StemDoubleAgentFadingIn || stemState == StemState.StemDoubleAgentPlaying)
        {
            return;
        }

        if (stemState == StemState.NoStem)
        {
            doDoubleAgentStemFadeIn();
        }
        else
        {
            doubleAgentStemQueued = true;
            loyalistStemQueued = false;
        }
    }

    public void StopStem()
    {
        if( stemState == StemState.StemDoubleAgentPlaying )
        {
            doDoubleAgentStemFadeOut();
        }
        else if( stemState == StemState.StemLoyalistPlaying )
        {
            doLoyalistStemFadeOut();
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

    private void doLoyalistStemFadeOut()
    {
        fadeTimer = 0f;
        stemState = StemState.StemLoyalistFadingOut;
    }

    private void doDoubleAgentStemFadeOut()
    {
        fadeTimer = 0f;
        stemState = StemState.StemDoubleAgentFadingOut;
    }
}
