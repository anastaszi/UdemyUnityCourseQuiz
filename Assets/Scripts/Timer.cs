using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    
    [SerializeField] float timeToCompeteQuestion = 30f;
    [SerializeField] float timeToShowAnswer = 10f;
    public bool isAnsweringQuestion = false;
    public bool loadNextQuestion = false;
    public float fillFraction;
    float timerValue;
    void Update()
    {
        UpdateTimer();
    }
    public void CancelTimer() {
        timerValue = 0;
    }

    void UpdateTimer() {
        timerValue -= Time.deltaTime;
        if (timerValue <= 0) {
            if (isAnsweringQuestion) {
                isAnsweringQuestion = false;
                timerValue = timeToShowAnswer;
            } else {
                isAnsweringQuestion = true;
                timerValue = timeToCompeteQuestion;
                loadNextQuestion = true;
            }
        } else {
            if (isAnsweringQuestion) {
                fillFraction = timerValue / timeToCompeteQuestion;
            } else {
                fillFraction = timerValue / timeToShowAnswer;
            }
        }
    }
}
