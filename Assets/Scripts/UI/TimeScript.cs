using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour
{
    private Automate _automate = null;

    private Text _timerText;

    private void Awake()
    {
        _automate = FindObjectOfType<Automate>();
        _timerText = this.GetComponentInChildren<Text>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        GetTimeDisplay();
    }
    
    // Update is called once per frame
    void Update()
    {
        GetTimeDisplay();
    }

    private void GetTimeDisplay()
    {
        float time =_automate.RotationCycle - _automate.Timer;
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        float fraction = time * 1000;
        fraction %= 1000;

        _timerText.text = String.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
    }

    public void StopTimer()
    {
        if (_automate.Stop)
            _automate.Stop = false;
        else
            _automate.Stop = true;
    }
    

}
