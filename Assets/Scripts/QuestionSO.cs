using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Question", menuName = "Quiz Question", order = 0)]
public class QuestionSO : ScriptableObject {
    [TextArea(2,6)]
    [SerializeField] string question = "Enter new question here";
    [SerializeField] string[] answer = new string[4];
    [SerializeField] int correctAnswer;

    public string GetQuestion() {
        return question;
    }

    public int GetCorrectAnswerIndex() {
        return correctAnswer;
    }

    public string GetAnswer(int index) {
        return answer[index];
    }

}

