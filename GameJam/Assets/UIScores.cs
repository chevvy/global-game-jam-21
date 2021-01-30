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
    public TextMeshPro winnerText;

    private List<TextMeshProUGUI> playerScores;

    private void Start() {
        playerScores = new List<TextMeshProUGUI> {p0Score, p1Score, p2Score, p3Score};
    }

    public void SetPlayerScore(int playerNumber, int score) {
        playerScores[playerNumber].text = score.ToString();
    }

    public void OnEndGame(int winner) {
        winnerText.text = "P" + winner;
    }
}
