using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject GameOverPanel;
    
    [SerializeField] private Timer timer;
    [SerializeField] private Player player;

    public static event Action OnStartGame;
    public static event Action OnGameOver;

    public AudioSource DeathSFX;

    bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;
        MenuPanel.SetActive(true);
        GameOverPanel.SetActive(false);

        Player.HealthReachedZero += GameOver;
        Player.ReachedMaxAge += GameOver;
    }

    

    private void OnDestroy()
    {
        Player.HealthReachedZero -= GameOver;
        Player.ReachedMaxAge -= GameOver;
    }

    public void StartGame()
    {
        OnStartGame?.Invoke();
        MenuPanel.SetActive(false);
        player.StartGame();
        timer.Unpause();
    }

    public void GameOver()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;
        timer.Pause();
        DeathSFX.Play();
        GameOverPanel.SetActive(true);
        OnGameOver?.Invoke();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }

    private void FixedUpdate()
    {
        if (isGameOver && timer.isActive)
        {
            timer.Pause();
        }
        
    }

}
