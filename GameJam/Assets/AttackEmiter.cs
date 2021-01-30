using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEmiter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            other.GetComponent<PlayerController.PlayerController>().GetsAttacked(transform.position);
        }
    }
}
