using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public float displayTime = 2f;  // Time to display the text in seconds
    public bool isDisplayed = false;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator ShowTextForTime(float time)
    {
        AudioManager a = FindObjectOfType<AudioManager>();
        if (a != null)
        {
            a.PlaySfx(a.stampSfx);
        }
        isDisplayed = true;
        gameObject.SetActive(true);

        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
        isDisplayed = false;
    }
}
