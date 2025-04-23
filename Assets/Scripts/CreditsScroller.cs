using UnityEngine;
using UnityEngine.UI;

public class CreditsScroller : MonoBehaviour
{
    public RectTransform creditsPanel; // El panel que contiene todos los textos de créditos
    public float scrollSpeed = 50f;
    public float delayBeforeStart = 2f;
    public GameObject companyLogo; // El logo que aparecerá al final

    private float startTime;
    private float panelHeight;
    private float screenHeight;
    private bool scrollingDone = false;

    void Start()
    {
        startTime = Time.time + delayBeforeStart;
        panelHeight = creditsPanel.rect.height;
        screenHeight = ((RectTransform)creditsPanel.parent).rect.height;

        if (companyLogo != null)
            companyLogo.SetActive(false);
    }

    void Update()
    {
        if (Time.time < startTime || scrollingDone)
            return;

        creditsPanel.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (creditsPanel.anchoredPosition.y >= panelHeight - screenHeight)
        {
            scrollingDone = true;
            if (companyLogo != null)
                companyLogo.SetActive(true);
        }
    }
}
