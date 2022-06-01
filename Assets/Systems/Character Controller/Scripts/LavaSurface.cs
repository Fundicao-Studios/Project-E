using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSurface : MonoBehaviour
{
    public float burnBuildUpAmount = 7;

    public List<CharacterEffectsManager> charactersInsideLavaSurface;

    private void OnTriggerEnter(Collider other)
    {
        CharacterEffectsManager character = other.GetComponentInChildren<CharacterEffectsManager>();

        if (character != null)
        {
            if (character.isLavaResistant)
                return;

            charactersInsideLavaSurface.Add(character);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterEffectsManager character = other.GetComponentInChildren<CharacterEffectsManager>();

        if (character != null)
        {
            if (character.isLavaResistant)
                return;

            character.isInLava = false;
            charactersInsideLavaSurface.Remove(character);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        foreach (CharacterEffectsManager character in charactersInsideLavaSurface)
        {
            if (character.isLavaResistant)
                return;

            character.isInLava = true;

            if (character.isBurning)
                return;
                
            character.burnBuildup = character.burnBuildup + burnBuildUpAmount * Time.deltaTime;
        }
    }
}
