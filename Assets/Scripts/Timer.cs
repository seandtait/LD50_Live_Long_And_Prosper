using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool isActive = false;

    public static event Action AgeIncrease;
    public static event Action<int> PercentTimerUpdate;

    [SerializeField] float counter = 0;
    float timePerYearOfLife = 100;

    int savedAge = -1;

    public void Pause()
    {
        SetActive(false);
    }

    public void Unpause()
    {
        SetActive(true);
    }

    void SetActive(bool _state)
    {
        isActive = _state;
    }

    public float GetTimerRatio()
    {
        return counter / timePerYearOfLife;
    }

    private void FixedUpdate()
    {
        if (!isActive) { return; }

        counter += Time.deltaTime;

        int percent = Mathf.RoundToInt((GetTimerRatio() * 100));
        if (savedAge != percent)
        {
            savedAge = percent;
            // Age changed
            AgeIncrease?.Invoke();
            PercentTimerUpdate?.Invoke(percent);
        }
        
    }

    private void Start()
    {
        Game.OnGameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        Game.OnGameOver -= OnGameOver;
    }

    void OnGameOver()
    {
        Pause();
    }

}
