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
        transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Girl")) Debug.Log("Girl eaten by Darkness");
        if (other.CompareTag("Player") && player.fourthDialogueHappened) { Debug.Log("Ending #1"); SceneManager.LoadScene("Ending#1");}
    }
}
