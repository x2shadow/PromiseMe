using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimationTrigger : MonoBehaviour
{
    [SerializeField] Animator girlAnimator;

    bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            girlAnimator.SetTrigger("DEATH");
        }
    }
}
