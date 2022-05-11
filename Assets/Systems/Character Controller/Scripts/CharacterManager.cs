using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Transform Para Lock On")]
    public Transform lockOnTransform;

    [Header("Colliders De Combate")]
    public CriticalDamageCollider backStabCollider;
    public CriticalDamageCollider riposteCollider;

    [Header("Flags De Combate")]
    public bool canBeRiposted;
    public bool canBeParried;
    public bool isParrying;
    public bool isBlocking;

    //O dano será feito durante o evento da animação
    //Usado nas animações de backstab e riposte 
    public int pendingCriticalDamage;
}
