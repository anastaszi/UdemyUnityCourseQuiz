using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Quiz : MonoBehaviour
{
    [Header("Question UI")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answer UI")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnswered = true;
    [Header("Button Sprites")]
    [SerializeField] Sprite defaultButtonSprite;
    [SerializeField] Sprite correctButtonSprite;
    [Header("Timer UI")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Score UI")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Progress Bar")]
    [SerializeField] Slider progressBar;
    public bool isComplete = false;


    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadNextQuestion) {
            if (progressBar.value == progressBar.maxValue) {
                isComplete = true;
                return;
            }
            timer.loadNextQuestion = false;
            hasAnswered = false;
            GetNextQuestion();
        } else if (!timer.isAnsweringQuestion && !hasAnswered) {
            hasAnswered = true;
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    void GetNextQuestion() {
        if (questions.Count == 0) {
            Debug.Log("No more questions");
            return;
        }
        GetRandomQuestion();
        SetButtonState(true);
        SetDefaultButtonSprites();
        DisplayQuestion();
        scoreKeeper.IncrementQuestionsSeen();
        progressBar.value++;
    }

    void GetRandomQuestion() {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];
        if (questions.Contains(currentQuestion)) {
            questions.Remove(currentQuestion);
        }
    }
    void DisplayQuestion() {
        questionText.text = currentQuestion.GetQuestion();
        correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
        for (int i = 0; i < answerButtons.Length; i++) {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }

    void SetButtonState(bool state) {
        for (int i = 0; i < answerButtons.Length; i++) {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprites() {
        for (int i = 0; i < answerButtons.Length; i++) {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultButtonSprite;
        }
    }

    public void OnAnswerSelected (int index) {
        hasAnswered = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer(); 
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
    }

    void DisplayAnswer(int index) {
        Debug.Log("Selected Answer" + index + "Correct Answer" + correctAnswerIndex);
        Image buttonImage;
        if (index == correctAnswerIndex) {
            questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctButtonSprite;
            scoreKeeper.IncrementCorrectAnswers();
        } else {
            questionText.text = "Wrong! The correct answer is: \n" + currentQuestion.GetAnswer(correctAnswerIndex);
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctButtonSprite;
        }
    }
    
}
