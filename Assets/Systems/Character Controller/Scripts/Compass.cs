using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public GameObject iconPrefab;
    List<QuestMarker> questMarkers = new List<QuestMarker>();

    public RawImage compassImage;
    [SerializeField] GameObject playerCamera;

    float compassUnit;

    [SerializeField] QuestMarker[] altars;

    private void Awake()
    {
        playerCamera = GameObject.Find("Camera Holder");
        altars = FindObjectsOfType<QuestMarker>();
    }

    private void Start()
    {
        compassUnit = compassImage.rectTransform.rect.width / 360f;

        for (int i = 0; i < altars.Length; i++)
        {
            AddQuestMarker(altars[i]);
        }
    }

    private void Update()
    {
        compassImage.uvRect = new Rect (playerCamera.transform.localEulerAngles.y / 360f, 0f, 1f, 1f);

        foreach (QuestMarker marker in questMarkers)
        {
            marker.image.rectTransform.anchoredPosition = GetPosOnCompass(marker);
        }
    }

    public void AddQuestMarker (QuestMarker marker)
    {
        GameObject newMarker = Instantiate(iconPrefab, compassImage.transform);
        marker.image = newMarker.GetComponent<Image>();
        marker.image.sprite = marker.icon;

        questMarkers.Add(marker);
    }

    Vector2 GetPosOnCompass (QuestMarker marker)
    {
        Vector2 playerCameraPos = new Vector2(playerCamera.transform.position.x, playerCamera.transform.position.z);
        Vector2 playerCameraFwd = new Vector2(playerCamera.transform.forward.x, playerCamera.transform.forward.z);

        float angle = Vector2.SignedAngle(marker.position - playerCameraPos, playerCameraFwd);

        return new Vector2(compassUnit * angle, 0f);
    }
}
