using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : Interactable
{
    Animator animator;
    OpenChest openChest;

    public Transform playerStandingPosition;
    public GameObject[] itemSpawners;
    public WeaponItem[] itemsInChest;
    public GameObject[] consumableSpawners;
    public ConsumableItem[] consumableInChest;
    public GameObject[] helmetSpawners;
    public HelmetEquipment[] helmetsInChest;
    public float consumableChance;
    public float weaponChance;
    public float helmetChance;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        openChest = GetComponent<OpenChest>();
    }
    
    public override void Interact(PlayerManager playerManager)
    {
        Vector3 rotationDirection = transform.position - playerManager.transform.position;
        rotationDirection.y = 0;
        rotationDirection.Normalize();

        Quaternion tr = Quaternion.LookRotation(rotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
        playerManager.transform.rotation = targetRotation;

        playerManager.OpenChestInteraction(playerStandingPosition);
        animator.Play("Chest Open");
        int favChance = Random.Range(0, 100);

        if (weaponChance >= favChance)
        {
            StartCoroutine(SpawnItemInChest());

            for (int i = 0; i < itemSpawners.Length; i++)
            {
                WeaponPickup[] weaponPickups = itemSpawners[i].GetComponents<WeaponPickup>();

                if (weaponPickups[i] != null)
                {
                    weaponPickups[i].weapon = itemsInChest[i];
                    return;
                } 
            }
        }
        else if (helmetChance >= favChance)
        {
            StartCoroutine(SpawnHelmetInChest());

            for (int i = 0; i < helmetSpawners.Length; i++)
            {
                HeadPickup[] headPickups = helmetSpawners[i].GetComponents<HeadPickup>();

                if (headPickups[i] != null)
                {
                    headPickups[i].helmet = helmetsInChest[i];
                    return;
                } 
            }
        }
        else if (consumableChance >= favChance)
        {
            StartCoroutine(SpawnConsumableInChest());

            for (int i = 0; i < consumableSpawners.Length - 1; i++)
            {
                ConsumablePickup[] consumablePickups = consumableSpawners[i].GetComponents<ConsumablePickup>();

                if (consumablePickups[i] != null)
                {
                    consumablePickups[i].potion = consumableInChest[i];
                    return;
                } 
            }
        }
    }

    private IEnumerator SpawnItemInChest()
    {
        int itemIndex = Random.Range(0, itemSpawners.Length);
        GameObject itemToSpawn = itemSpawners[itemIndex];
        
        yield return new WaitForSeconds(1f);
        Instantiate(itemToSpawn, transform);
        Destroy(openChest);
    }

    private IEnumerator SpawnConsumableInChest()
    {
        int consumableIndex = Random.Range(0, consumableSpawners.Length);
        GameObject consumableToSpawn = consumableSpawners[consumableIndex];
        
        yield return new WaitForSeconds(1f);
        Instantiate(consumableToSpawn, transform);
        Destroy(openChest);
    }

    private IEnumerator SpawnHelmetInChest()
    {
        int helmetIndex = Random.Range(0, helmetSpawners.Length);
        GameObject helmetToSpawn = helmetSpawners[helmetIndex];

        yield return new WaitForSeconds(1f);
        Instantiate(helmetToSpawn, transform);
        Destroy(openChest);
    }
}
