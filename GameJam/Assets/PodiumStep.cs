using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumStep : MonoBehaviour {
    [SerializeField] private Transform playerSpawn;
    
    private Material AssignedPlayerMaterial { get; set; }

    public void SetupPodiumStep(GameObject player) {
        PlayerController.PlayerController controller = player.GetComponent<PlayerController.PlayerController>();
        controller.isGameFinished = true; // disable move on player
        // player.GetComponent<Rigidbody>().isKinematic = true; // set kinematic -> we dont want funbky shiy
        Destroy(player.GetComponent<Rigidbody>());
        // move to spawn
        player.transform.position = playerSpawn.position;
        player.transform.rotation = playerSpawn.rotation;
        // set podium step to be same material
        AssignedPlayerMaterial = controller.GetPlayerMaterial();
        GetComponent<MeshRenderer>().material = AssignedPlayerMaterial;
    }
}
