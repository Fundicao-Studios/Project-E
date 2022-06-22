using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Animator animator;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            animator.SetBool("isOnElevator", true);
        }
    }
}
