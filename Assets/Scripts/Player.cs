using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int age;
    public int health;
    public int money;

    public int maxAge;

    // Config
    const int default_health = 80;
    const int default_money = 0;
    const int default_maxAge = 70;

    int maxHealth = 100;
    int MAX_AGE = 99;

    public static event Action<int> RelayAgeIncrease;
    public static event Action<int, int> RelayMoneyChange;
    public static event Action<int, int> RelayHealthChange;
    public static event Action ReachedMaxAge;

    public static event Action HealthReachedZero;
    public static event Action<int> GainedAchiev;

    public List<GameObject> achievements = new List<GameObject>();

    public AudioSource DingSFX;

    void ResetAchievs()
    {
        for (int i = 0; i < achievements.Count; i++)
        {
            achievements[i].SetActive(false);
        }
    }

    public void GetAchievement(int _id)
    {
        if (achievements[_id].activeSelf)
        {
            // already active
            return;
        }

        Debug.Log($"Activating achiev: {_id}");

        DingSFX.Play();
        achievements[_id].SetActive(true);
        GainedAchiev?.Invoke(_id);
    }

    private void Start()
    {
        // Subscribe
        Timer.AgeIncrease += AgeIncrease;
    }

    public void StartGame()
    {
        ResetAchievs();
        HealthChange(default_health);
        MoneyChange(default_money);
        age = -1;
        maxAge = default_maxAge;

        AgeIncrease();
    }

    private void OnDestroy()
    {
        // Unsubscribe
        Timer.AgeIncrease -= AgeIncrease;

    }

    void AgeIncrease()
    {
        age += 1;
        RelayAgeIncrease?.Invoke(age);

        CheckForAgeBasedAchievs();

        HealthChange(-1);

        // Check for age related death
        if (age >= maxAge)
        {
            // Died of old age
            ReachedMaxAge?.Invoke();
        }
    }

    void CheckForAgeBasedAchievs()
    {
        if (age >= 60)
        {
            GetAchievement(7);
        }
        if (age >= 35)
        {
            GetAchievement(6);
        }
        if (age >= 20)
        {
            GetAchievement(5);
        }
        if (age >= 16)
        {
            GetAchievement(4);
        }
        if (age >= 13)
        {
            GetAchievement(3);
        }
        if (age >= 4)
        {
            GetAchievement(2);
        }
        if (age >= 2)
        {
            GetAchievement(1);
        }
        if (age >= 0)
        {
            GetAchievement(0);
        }
    }

    public void HealthChange(int _value)
    {
        int tempHealth = health;
        health += _value;

        if (health > maxHealth)
        {
            health = health = maxHealth;
        }

        if (health != tempHealth)
        {
            RelayHealthChange?.Invoke(health - _value, health);
        }

        if (health <= 0)
        {
            HealthReachedZero?.Invoke();
            return;
        }
        
    }

    public void MoneyChange(int _value)
    {
        money += _value;
        RelayMoneyChange?.Invoke(money - _value, money);
    }

    public void MaxAgeChange(int _value)
    {
        maxAge += _value;
        if (maxAge > MAX_AGE)
        {
            maxAge = MAX_AGE;
        }
    }

}
