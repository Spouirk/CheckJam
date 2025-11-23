using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [Header("Réglages Scrolling")]
    public float speed;

    [SerializeField]
    private Renderer bgRenderer;

    [Header("Réglages Couleurs")]
    public bool useDynamicColor = true;
    public float colorSpeed = 0.5f;
    [Range(0, 1)] public float saturation = 0.8f;
    [Range(0, 1)] public float brightness = 1f;

    void Start()
    {

    }

    void Update()
    {
        // 1. Scrolling
        bgRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);

        // 2. Couleur (Version compatible URP)
        if (useDynamicColor && bgRenderer != null)
        {
            float hue = Mathf.Repeat(Time.time * colorSpeed, 1f);
            Color newColor = Color.HSVToRGB(hue, saturation, brightness);

            // CORRECTION ICI : On utilise SetColor avec "_BaseColor"
            bgRenderer.material.SetColor("_BaseColor", newColor);
        }
    }
}