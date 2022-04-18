using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProcGenUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI StatusDisplay;
    [SerializeField] ProcGenManager TargetManager;
    [SerializeField] GameObject loadingScreen;

    void Awake()
    {
        OnRegenerate();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRegenerate()
    {
        StartCoroutine(PerformRegeneration());
    }

    IEnumerator PerformRegeneration()
    {
        yield return TargetManager.AsyncRegenerateWorld(OnStatusReported);

        Cursor.lockState = CursorLockMode.None;
        loadingScreen.SetActive(false);

        yield return null;
    }

    void OnStatusReported(int step, int totalSteps, string status)
    {
        StatusDisplay.text = $"Fase {step} de {totalSteps}: {status}";
    }
}
