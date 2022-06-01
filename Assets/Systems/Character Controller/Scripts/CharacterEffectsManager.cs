using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterStatsManager characterStatsManager;

    [Header("Efeitos De Dano")]
    public GameObject bloodSplatterFX;

    [Header("Lava")]
    public GameObject defaultBurnParticleFX;
    private GameObject currentBurnParticleFX;
    public Transform buildUpTransform; //O local onde as partículas vão ser instanciadas
    public bool isBurning;
    public bool isInLava;
    public bool isLavaResistant;
    public float burnBuildup = 0; //O tempo que demora até ao jogador começar a arder
    public float burnAmount = 100; //A quantidade de tempo que o jogador ainda tem de passar até deixar de arder
    public float defaultBurnAmount; //O valor padrão ao qual o jogador tem de ter até ficar a arder
    public float burnTimer = 2; //A quantidade de tempo entre cada tick de dano de fogo
    public int burnDamage = 1;
    public float lavaTimer = 0.5f;
    public float lavaDamage = 1;
    float timer;
    float lavaCounter;

    /*
    [Header("Efeitos Da Arma")]
    public WeaponFX rightWeaponFX;
    public WeaponFX leftWeaponFX;

    public virtual void PlayWeaponFX(bool isLeft)
    {
        if (isLeft == false)
        {
            if (rightWeaponFX != null)
            {
                rightWeaponFX.PlayWeaponFX();
                Debug.Log(rightWeaponFX.normalWeaponTrail.name);
                Debug.Log("VFX ATIVADO");
            }
        }
        else
        {
            if (leftWeaponFX != null)
            {
                leftWeaponFX.PlayWeaponFX();
            }
        }
    }
    */

    protected virtual void Awake()
    {
        characterStatsManager = GetComponentInParent<CharacterStatsManager>();
    }

    public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
    {
        GameObject blood = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity); 
    }

    public virtual void HandleAllBuildUpEffects()
    {
        if (characterStatsManager.isDead)
            return;

        HandleBurnBuildup();
        HandleIsBurningEffect();
    }

    protected virtual void HandleBurnBuildup()
    {
        if (isBurning)
            return;

        if (burnBuildup > 0 && burnBuildup < 100)
        {
            burnBuildup = burnBuildup - 1 * Time.deltaTime;
        }
        else if (burnBuildup >= 100)
        {
            isBurning = true;
            burnBuildup = 0;

            if (buildUpTransform != null)
            {
                currentBurnParticleFX = Instantiate(defaultBurnParticleFX, buildUpTransform.transform);
            }
            else
            {
                currentBurnParticleFX = Instantiate(defaultBurnParticleFX, characterStatsManager.transform);
            }
        }
    }

    protected virtual void HandleIsBurningEffect()
    {
        if (isInLava)
        {
            lavaCounter += Time.deltaTime;

            if (lavaCounter >= lavaTimer)
            {
                characterStatsManager.TakeBurnDamage(burnDamage);
                lavaCounter = 0;
            }
        }

        if (isBurning)
        {
            if (burnAmount > 0)
            {
                timer += Time.deltaTime;

                if (timer >= burnTimer)
                {
                    characterStatsManager.TakeBurnDamage(burnDamage);
                    timer = 0;
                }

                burnAmount = burnAmount - 1 * Time.deltaTime;
            }
            else
            {
                isBurning = false;
                burnAmount = defaultBurnAmount;
                Destroy(currentBurnParticleFX);
            }
        }
    }
}
