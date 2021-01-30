using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEmiter : MonoBehaviour {
    public bool isAttacking;
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            other.GetComponent<PlayerController.PlayerController>().GetsAttacked(transform.position);
        }
    }

    // public bool SetAttackingState => isAttacking != isAttacking;
    // faire que au debug de l'anim, on permettre l'attaque et à fin anim, on empèche lattaque
    // si pas dans ce frame, l'attaque n'est pas passé
}
