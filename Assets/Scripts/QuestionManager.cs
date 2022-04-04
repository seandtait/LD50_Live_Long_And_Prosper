using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestionManager : MonoBehaviour
{
    // Ref
    [SerializeField] private Timer timer;
    [SerializeField] private Player player;

    // Config
    const int MIN_QUESTIONS_PER_LIFE = 10;
    const int MAX_QUESTIONS_PER_LIFE = 15;

    // Paths
    const string PATH_TO_QUESTIONS = "Prefabs/Questions";

    public Dictionary<string, List<GameObject>> questions = new Dictionary<string, List<GameObject>>();

    public List<int> questionTimes = new List<int>();
    public List<GameObject> chosenQuestions = new List<GameObject>();

    public static event Action<GameObject> ShowQuestion;
    public static event Action<GameObject> HideQuestion;

    public AudioSource BamSFX;

    // Start is called before the first frame update
    void Start()
    {
        Timer.PercentTimerUpdate += CheckShowQuestion;
        QuestionPanel.AnswerClicked += OnAnswerClicked;
        QuestionPanel.OnHideResult += OnHideResult;
        Game.OnStartGame += SetQuestions;

        SpawnQuestions();
    }

    private void OnDestroy()
    {
        Timer.PercentTimerUpdate -= CheckShowQuestion;
        QuestionPanel.AnswerClicked -= OnAnswerClicked;
        QuestionPanel.OnHideResult -= OnHideResult;
        Game.OnStartGame -= SetQuestions;
    }

    void SpawnQuestions()
    {
        // Grab all questions from that age group
        GameObject[] baby = Resources.LoadAll<GameObject>($"{PATH_TO_QUESTIONS}/Baby");
        GameObject[] toddler = Resources.LoadAll<GameObject>($"{PATH_TO_QUESTIONS}/Toddler");
        GameObject[] child = Resources.LoadAll<GameObject>($"{PATH_TO_QUESTIONS}/Child");
        GameObject[] tweens = Resources.LoadAll<GameObject>($"{PATH_TO_QUESTIONS}/Tweens");
        GameObject[] teens = Resources.LoadAll<GameObject>($"{PATH_TO_QUESTIONS}/Teens");
        GameObject[] adult = Resources.LoadAll<GameObject>($"{PATH_TO_QUESTIONS}/Adult");
        GameObject[] middleAged = Resources.LoadAll<GameObject>($"{PATH_TO_QUESTIONS}/MiddleAged");
        GameObject[] elderly = Resources.LoadAll<GameObject>($"{PATH_TO_QUESTIONS}/Elderly");

        SetupAndFillDictionary("Baby", baby);
        SetupAndFillDictionary("Toddler", toddler);
        SetupAndFillDictionary("Child", child);
        SetupAndFillDictionary("Tweens", tweens);
        SetupAndFillDictionary("Teens", teens);
        SetupAndFillDictionary("Adult", adult);
        SetupAndFillDictionary("MiddleAged", middleAged);
        SetupAndFillDictionary("Elderly", elderly);

    }

    void SetupAndFillDictionary(string _key, GameObject[] _array)
    {
        questions.Add(_key, new List<GameObject>());
        for (int i = 0; i < _array.Length; i++)
        {
            GameObject q = Instantiate(_array[i]);
            q.name = _array[i].name;
            q.transform.SetParent(this.transform);
            q.transform.localPosition = Vector2.zero;
            questions[_key].Add(q);
        }
    }

    void SetQuestions()
    {
        // Clear
        questionTimes.Clear();
        chosenQuestions.Clear();

        List<GameObject> qs = questions["Baby"];
        GameObject q = qs[UnityEngine.Random.Range(0, qs.Count)];
        int time = UnityEngine.Random.Range(0, 2);
        questionTimes.Add(time);
        chosenQuestions.Add(q);
        DebugQuestion(q, time);

        qs = questions["Toddler"];
        q = qs[UnityEngine.Random.Range(0, qs.Count)];
        time = UnityEngine.Random.Range(2, 4);
        questionTimes.Add(time);
        chosenQuestions.Add(q);
        DebugQuestion(q, time);

        qs = questions["Child"];
        q = qs[UnityEngine.Random.Range(0, qs.Count)];
        time = UnityEngine.Random.Range(4, 13);
        questionTimes.Add(time);
        chosenQuestions.Add(q);
        DebugQuestion(q, time);

        qs = questions["Tweens"];
        q = qs[UnityEngine.Random.Range(0, qs.Count)];
        time = UnityEngine.Random.Range(13, 16);
        questionTimes.Add(time);
        chosenQuestions.Add(q);

        qs = questions["Teens"];
        q = qs[UnityEngine.Random.Range(0, qs.Count)];
        time = UnityEngine.Random.Range(16, 20);
        questionTimes.Add(time);
        chosenQuestions.Add(q);
        DebugQuestion(q, time);

        // Adult
        qs = questions["Adult"];
        q = qs[UnityEngine.Random.Range(0, qs.Count)];
        time = UnityEngine.Random.Range(20, 25);
        questionTimes.Add(time);
        chosenQuestions.Add(q);
        questions["Adult"].Remove(q);
        DebugQuestion(q, time);

        qs = questions["Adult"];
        q = qs[UnityEngine.Random.Range(0, qs.Count)];
        time = UnityEngine.Random.Range(25, 30);
        questionTimes.Add(time);
        chosenQuestions.Add(q);
        questions["Adult"].Remove(q);
        DebugQuestion(q, time);

        qs = questions["Adult"];
        q = qs[UnityEngine.Random.Range(0, qs.Count)];
        time = UnityEngine.Random.Range(30, 35);
        questionTimes.Add(time);
        chosenQuestions.Add(q);
        questions["Adult"].Remove(q);
        DebugQuestion(q, time);
    
        // Middle Aged
        qs = questions["MiddleAged"];
        q = qs[UnityEngine.Random.Range(0, qs.Count)];
        time = UnityEngine.Random.Range(35, 44);
        questionTimes.Add(time);
        chosenQuestions.Add(q);
        questions["MiddleAged"].Remove(q);
        DebugQuestion(q, time);

        qs = questions["MiddleAged"];
        q = qs[UnityEngine.Random.Range(0, qs.Count)];
        time = UnityEngine.Random.Range(44, 53);
        questionTimes.Add(time);
        chosenQuestions.Add(q);
        questions["MiddleAged"].Remove(q);
        DebugQuestion(q, time);

        qs = questions["MiddleAged"];
        q = qs[UnityEngine.Random.Range(0, qs.Count)];
        time = UnityEngine.Random.Range(53, 60);
        questionTimes.Add(time);
        chosenQuestions.Add(q);
        questions["MiddleAged"].Remove(q);
        DebugQuestion(q, time);

        // Elderly
        qs = questions["Elderly"];
        q = qs[UnityEngine.Random.Range(0, qs.Count)];
        time = UnityEngine.Random.Range(60, 100);
        questionTimes.Add(time);
        chosenQuestions.Add(q);
        questions["Elderly"].Remove(q);
        DebugQuestion(q, time);

    }

    void DebugQuestion(GameObject _question, int _time)
    {
        //Debug.Log($"Added the question: {_question} at time {_time}");
    }

    void CheckShowQuestion(int _percent)
    {
        if (questionTimes.Contains(_percent))
        {
            // Question found for this percent
            Debug.Log($"{_percent} - SHOW THE QUESTION!");
            BamSFX.Play();
            timer.Pause();
            ShowQuestion?.Invoke(chosenQuestions[0]);
        } else
        {
            // No question for this percent

        }
    }

    GameObject GetAnswerFromQuestion(GameObject _question, int _answerIndex)
    {
        return _question.transform.GetChild(_answerIndex).gameObject;
    }

    void OnAnswerClicked(int _answerIndex)
    {
        // Carry out the effects
        StartCoroutine(IOnAnswerClicked(GetAnswerFromQuestion(chosenQuestions[0], _answerIndex)));
    }

    IEnumerator IOnAnswerClicked(GameObject _answer)
    {
        yield return null;

        AlterStat alterStat =  _answer.GetComponent<AlterStat>();
        if (alterStat)
        {
            yield return StartCoroutine(alterStat.Perform(player));
        }
        GainAchiev gainAchiev = _answer.GetComponent<GainAchiev>();
        if (gainAchiev)
        {
            yield return StartCoroutine(gainAchiev.Perform(player));
        }

        yield return null;
        HideQuestion?.Invoke(_answer);
        
    }

    void OnHideResult()
    {
        // Erase this question
        GameObject q = chosenQuestions[0];
        chosenQuestions.RemoveAt(0);
        questionTimes.RemoveAt(0);
        DestroyImmediate(q);

        timer.Unpause();
    }

    private void Update()
    {
        
    }

}
