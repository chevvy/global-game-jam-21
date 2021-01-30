using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScores : MonoBehaviour {
    public TextMeshProUGUI p0Score;
    public TextMeshProUGUI p1Score;
    public TextMeshProUGUI p2Score;
    public TextMeshProUGUI p3Score;
    public int numberOfPlayers = 4;

    private List<TextMeshProUGUI> playerScores;

    private void Start() {
        playerScores = new List<TextMeshProUGUI>();
        playerScores.Add(p0Score);
        playerScores.Add(p1Score);
        playerScores.Add(p2Score);
        playerScores.Add(p3Score);
    }

    public void SetPlayerScore(int playerNumber, int score) {
        playerScores[playerNumber].text = score.ToString();
    }
}
