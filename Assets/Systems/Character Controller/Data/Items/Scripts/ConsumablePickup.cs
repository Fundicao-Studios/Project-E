using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumablePickup : Interactable
{
    public ConsumableItem potion;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        PickUpItem(playerManager);
    }

    private void PickUpItem(PlayerManager playerManager)
    {
        PlayerInventory playerInventory;
        PlayerLocomotion playerLocomotion;
        PlayerAnimatorManager animatorManager;

        playerInventory = playerManager.GetComponent<PlayerInventory>();
        playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
        animatorManager = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

        playerLocomotion.rigidbody.velocity = Vector3.zero; //Para o jogador quando este apanha um item
        animatorManager.PlayTargetAnimation("Pick Up Item", true); //Começar a animação de apanhar o item
        playerInventory.consumablesInventory.Add(potion);
        playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = potion.itemName;
        playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = potion.itemIcon.texture;
        playerManager.itemInteractableGameObject.SetActive(true);
        Destroy(gameObject);
    }
}
