using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableDataNode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerController.PlayerController>().LootDataNode();
            Destroy(gameObject);
        }
    }
}
