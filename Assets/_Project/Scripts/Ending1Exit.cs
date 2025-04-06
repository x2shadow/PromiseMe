using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending1Exit : MonoBehaviour
{
    public GameObject wall;
    public PlayerController player;

    void OnTriggerEnter(Collider other)
    {
        wall.SetActive(true);
        player.exitedEnding1 = true;
        Debug.Log("Exited Ending#1");
    }
}
