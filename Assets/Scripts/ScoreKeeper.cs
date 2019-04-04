using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

    public static int score;
    private Text myText;

    void Start()
    {
        myText = GetComponent<Text>();
        Reset();
    }

    public void Score(int points)
    {
        score += points;
        myText.text = score.ToString(); // change the text to the score
    }

    public static void Reset()
    {
        score = 0; // resets score to 0
    }

}
