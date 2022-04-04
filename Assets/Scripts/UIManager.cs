using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Scripts
    [SerializeField] private Timer timer;
    [SerializeField] private Image timerGauge;

    // UI
    [SerializeField] private TMPro.TextMeshProUGUI AgeDisplay;
    [SerializeField] private TMPro.TextMeshProUGUI MoneyDisplay;
    [SerializeField] private TMPro.TextMeshProUGUI HealthDisplay;

    float barWidth = 1684;

    private void Start()
    {
        Player.RelayAgeIncrease += UpdateAge;
        Player.RelayHealthChange += UpdateHealth;
        Player.RelayMoneyChange += UpdateMoney;
    }

    private void OnDestroy()
    {
        Player.RelayAgeIncrease -= UpdateAge;
        Player.RelayHealthChange -= UpdateHealth;
        Player.RelayMoneyChange -= UpdateMoney;
    }

    private void FixedUpdate()
    {
        RefreshTimer();
        
    }

    void RefreshTimer()
    {
        timerGauge.GetComponent<RectTransform>().sizeDelta = new Vector2(barWidth * timer.GetTimerRatio(), 48);
    }



    public void UpdateAge(int _age)
    {
        UpdateTextField(AgeDisplay, _age.ToString());
    }

    public void UpdateHealth(int _oldHealth, int _newHealth)
    {
        UpdateTextField(HealthDisplay, _newHealth.ToString());
    }

    public void UpdateMoney(int _oldMoney, int _newMoney)
    {
        UpdateTextField(MoneyDisplay, _newMoney.ToString());
    }

    void UpdateTextField(TMPro.TextMeshProUGUI _textField, string _text)
    {
        _textField.text = _text;
    }

}
