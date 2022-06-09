using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadPickup : Interactable
{
    public HelmetEquipment helmet;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        PickUpItem(playerManager);
    }

    private void PickUpItem(PlayerManager playerManager)
    {
        PlayerInventoryManager playerInventory;
        PlayerLocomotionManager playerLocomotion;
        PlayerAnimatorManager animatorManager;

        playerInventory = playerManager.GetComponent<PlayerInventoryManager>();
        playerLocomotion = playerManager.GetComponent<PlayerLocomotionManager>();
        animatorManager = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

        playerLocomotion.rigidbody.velocity = Vector3.zero; //Pára o jogador quando este apanha um item
        animatorManager.PlayTargetAnimation("Pick Up Item", true); //Começar a animação de apanhar o item
        playerInventory.headEquipmentInventory.Add(helmet);
        playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = helmet.itemName;
        playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = helmet.itemIcon.texture;
        playerManager.itemInteractableGameObject.SetActive(true);
        Destroy(gameObject);
    }
}
