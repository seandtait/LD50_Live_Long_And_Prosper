using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI Reason;
    [SerializeField] private TMPro.TextMeshProUGUI Age;
    [SerializeField] private TMPro.TextMeshProUGUI Message;
    [SerializeField] private Button ShowHideButton;
    [SerializeField] private GameObject Panel;


    private void Awake()
    {
        ShowHideButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        Player.HealthReachedZero += OnHealthReachedZero;
        Player.ReachedMaxAge += OnReachedMaxAge;
        Player.RelayAgeIncrease += SetAge;
        Game.OnGameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        Player.HealthReachedZero -= OnHealthReachedZero;
        Player.ReachedMaxAge -= OnReachedMaxAge;
        Player.RelayAgeIncrease -= SetAge;
        Game.OnGameOver -= OnGameOver;
    }

    void OnGameOver()
    {
        ShowHideButton.gameObject.SetActive(true);
    }

    public void SetReason(string _reason)
    {
        Reason.text = _reason;
    }

    public void SetAge(int _age)
    {
        Age.text = _age.ToString();
        ChooseMessage(_age);
    }

    void OnHealthReachedZero()
    {
        SetReason("Your health hit rock bottom! Try to take better care of yourself.");
    }

    void OnReachedMaxAge()
    {
        SetReason("You and your body have expired. You reached your highest potential age given the life you have had.");
    }

    void ChooseMessage(int _age)
    {
        if (_age > 80)
        {
            SetMessage("Wow! It's amazing you didn't get bored halfway through! You are a superstar. Congratulations on a long and healthy life.");
        } else if (_age > 60)
        {
            SetMessage("You didn't do too bad for yourself. You looked after yourself well enough but probably made a few mistakes along the way.");
        } else if (_age > 40)
        {
            SetMessage("Hmm... Unfortunate. Often the best leave us too soon.");
        } else if (_age > 20)
        {
            SetMessage("Too young. Far too young. Look after yourself better next time.");
        } else
        {
            SetMessage("... That didn't go too well, did it? What were you even doing? I mean... do you need me to show you how to play or..? Oh screw it... Try again.");
        }
    }

    void SetMessage(string _message)
    {
        Message.text = _message;
    }

    public void ShowHide()
    {
        Panel.SetActive(!Panel.activeSelf);
    }

}
