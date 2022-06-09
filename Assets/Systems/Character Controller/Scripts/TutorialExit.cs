using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialExit : MonoBehaviour
{
    public int sceneNumberToLoad;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            SceneManager.LoadScene(sceneNumberToLoad);
        }
    }
}
