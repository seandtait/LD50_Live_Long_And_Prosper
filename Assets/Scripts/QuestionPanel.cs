using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class QuestionPanel : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject ResultPanel;

    [SerializeField] private TMPro.TextMeshProUGUI AnswerText;
    [SerializeField] private TMPro.TextMeshProUGUI Message;
    [SerializeField] private TMPro.TextMeshProUGUI Changes;

    [SerializeField] private Image Border;
    [SerializeField] private TMPro.TextMeshProUGUI questionText;
    [SerializeField] private List<GameObject> AnswerButtons = new List<GameObject>();

    public static event Action<int> AnswerClicked;
    public static event Action OnHideResult;


    public void SetNumberOfAnswers(int _num)
    {
        HideAnswerButtons();
        for (int i = 0; i < _num; i++)
        {
            AnswerButtons[i].SetActive(true);
        }
    }

    void HideAnswerButtons()
    {
        for (int i = 0; i < AnswerButtons.Count; i++)
        {
            AnswerButtons[i].SetActive(false);
        }
    }

    void SetAnswerText(GameObject _button, string _text)
    {
        _button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = _text;
    }

    void SetQuestionText(string _text)
    {
        questionText.text = _text;
    }

    public void ShowQuestion(GameObject _question)
    {
        int numberOfAnswers = _question.transform.childCount;
        SetNumberOfAnswers(numberOfAnswers);

        SetQuestionText(_question.name);
        SetAnswers(_question);

        Panel.SetActive(true);
    }

    public void ShowResult(GameObject _answer)
    {
        AnswerText.text = $"[ {_answer.name} ]";
        Message.text = _answer.GetComponent<ResultMessage>().value;
        Changes.text = _answer.GetComponent<ResultChanges>().value;
        ResultPanel.SetActive(true);
    }

    void SetAnswers(GameObject _question)
    {
        int numberOfAnswers = _question.transform.childCount;
        for (int i = 0; i < numberOfAnswers; i++)
        {
            GameObject _answer = _question.transform.GetChild(i).gameObject;
            SetAnswerText(AnswerButtons[i], _answer.name);
            SetAnswerInteractable(AnswerButtons[i], _answer);
        }
    }

    void SetAnswerInteractable(GameObject _buttonObj, GameObject _answer)
    {
        Button b = _buttonObj.GetComponent<Button>();
        b.interactable = true;
        AnswerStatRequirement asr = _answer.GetComponent<AnswerStatRequirement>();
        if (!asr)
        {
            Debug.Log("No asr");
            return;
        }

        if (asr.MoneyRequired > 0)
        {
            if (player.money < asr.MoneyRequired)
            {
                Debug.Log("Not enough money");
                b.interactable = false;
                return;
            }
        }

        if (asr.HealthRequired > 0)
        {
            if (player.health < asr.HealthRequired)
            {
                b.interactable = false;
                return;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Panel.SetActive(false);

        QuestionManager.ShowQuestion += ShowQuestion;
        QuestionManager.HideQuestion += HideQuestion;
        Game.OnGameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        QuestionManager.ShowQuestion -= ShowQuestion;
        QuestionManager.HideQuestion -= HideQuestion;
        Game.OnGameOver -= OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        if (ResultPanel.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // left click
                HideResult();
            }
        }
    }

    void HideQuestion(GameObject _answer)
    {
        Panel.SetActive(false);
        ShowResult(_answer);
    }

    void HideResult()
    {
        ResultPanel.SetActive(false);
        OnHideResult?.Invoke();
    }

    public void ClickedAnswer()
    {
        // Get the index of the button that was clicked
        GameObject answerButton = EventSystem.current.currentSelectedGameObject;
        int index = AnswerButtons.IndexOf(answerButton);
        AnswerClicked?.Invoke(index);
    }

    void OnGameOver()
    {
        Panel.SetActive(false);
        ResultPanel.SetActive(false);
    }

}
