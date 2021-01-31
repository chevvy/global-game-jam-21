using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Podium : MonoBehaviour {
    [SerializeField] private PodiumStep[] _podiumSteps;

    private List<GameObject> playersReference;

    public void SetPlayersReference(List<GameObject> players) {
        playersReference = players;
    }

    public void EndGameSetup(List<int> winningOrder) {
        for (int i = 0; i < playersReference.Count; i++) {
            _podiumSteps[i].SetupPodiumStep(playersReference[winningOrder[i]]);
        }
    }
}
