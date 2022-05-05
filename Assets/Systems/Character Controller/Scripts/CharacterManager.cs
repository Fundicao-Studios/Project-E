using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Transform Para Lock On")]
    public Transform lockOnTransform;

    [Header("Colliders De Combate")]
    public BoxCollider backStabBoxCollider;
    public BackStabCollider backStabCollider;

    //O dano será feito durante o evento da animação
    //Usado nas animações de backstab e riposte 
    public int pendingCriticalDamage;
}
