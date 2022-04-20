using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 0.6f;
    public float centerX = 0f;
    public float centerY = 0f;
    public float centerZ = 0f;
    public string interactableText;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + new Vector3(centerX, centerY, centerZ), radius);
    }

    public virtual void Interact(PlayerManager playerManager)
    {
        Debug.Log("Interagiste com um objeto!");
    }
}
