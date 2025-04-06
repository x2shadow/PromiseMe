using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending2Trigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ending#2");
        SceneManager.LoadScene("Ending#2");
    }
}
