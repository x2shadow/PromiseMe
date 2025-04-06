using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Darkness : MonoBehaviour
{
    public PlayerController player;
    public float speed = 1f;

    void Update()
    {
        if (!isStopped)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
        }
    }

    public void StopTemporarily()
    {
        if (!isStopped)
        {
            StartCoroutine(StopCoroutine());
        }
    }

    private IEnumerator StopCoroutine()
    {
        isStopped = true;
        speed = 0f;
        yield return new WaitForSeconds(stopDuration);
        speed = originalSpeed;
        isStopped = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Girl")) Debug.Log("Girl eaten by Darkness");
        if (other.CompareTag("Player") && player.fourthDialogueHappened && !player.exitedEnding1) { Debug.Log("Ending #1"); SceneManager.LoadScene("Ending#1");}
    }
}
