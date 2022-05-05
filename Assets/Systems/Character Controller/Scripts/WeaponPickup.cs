using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickup : Interactable
{
    public WeaponItem weapon;

    public override void Interact(PlayerManager playerManagar)
    {
        base.Interact(playerManagar);

        PickUpItem(playerManagar);
    }

    private void PickUpItem(PlayerManager playerManager)
    {
        PlayerInventory playerInventory;
        PlayerLocomotion playerLocomotion;
        AnimatorManager animatorManager;

        playerInventory = playerManager.GetComponent<PlayerInventory>();
        playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
        animatorManager = playerManager.GetComponentInChildren<AnimatorManager>();

        playerLocomotion.rigidbody.velocity = Vector3.zero; //Para o jogador quando este apanha um item
        animatorManager.PlayTargetAnimation("Pick Up Item", true); //Começar a animação de apanhar item
        playerInventory.weaponsInventory.Add(weapon);
        playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
        playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
        playerManager.itemInteractableGameObject.SetActive(true);
        Destroy(gameObject);
    }
}
