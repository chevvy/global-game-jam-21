using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Podium : MonoBehaviour {
    [FormerlySerializedAs("_podiumSteps")] [SerializeField] private PodiumStep[] podiumSteps;

    private List<GameObject> playersReference;

    public void SetPlayersReference(List<GameObject> players) {
        playersReference = players;
    }

    public void EndGameSetup(List<int> winningOrder) {
        for (int i = 0; i < playersReference.Count; i++) {
            podiumSteps[i].SetupPodiumStep(playersReference[winningOrder[i]]);
        }
    }
}
