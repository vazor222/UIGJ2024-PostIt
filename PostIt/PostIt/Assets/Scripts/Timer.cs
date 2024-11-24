using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshPro timerText; 
    private float timeRemaining;
    int currentRound = 0;
    bool isCountdownStarted = false;

    void Start()
    {
        float roundLen = 180;/*
        currentRound = GameManager.Instance.currentRound;
        if (!GameManager.Instance.roundNumToRoundLen.TryGetValue(currentRound, out roundLen))
        {
            Debug.LogWarning("GameplaySceneManager:Start roundLen not found, defaulting to 180 seconds");
            roundLen = 180;
        }
        if (roundLen <= 0)
        {
            Debug.LogWarning("GameplaySceneManager:Start roundLen has an invalid value of " + roundLen +
                ", defaulting to 180 seconds");
            roundLen = 180;
        }*/
        timeRemaining = roundLen;
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        while (timeRemaining > 0)
        {
            UpdateTimerDisplay();
            yield return null;
            timeRemaining -= Time.deltaTime;
        }

        timeRemaining = 0;
        UpdateTimerDisplay();
        GameplaySceneManager.Instance.roundTimeRemaining = timeRemaining;
        Debug.Log("Timer Finished!");
    }

    void UpdateTimerDisplay()
    {
        if (!isCountdownStarted && timeRemaining <= 9) {
            AudioManager a = FindObjectOfType<AudioManager>();
            if (a != null)
            {
                a.PlaySfx(a.countdownSfx);
            }
            isCountdownStarted = true;
        }
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }
}
