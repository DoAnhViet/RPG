using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class SpinPopup : MonoBehaviour
{
    public float floatSpeed = 30f; // tốc độ di chuyển lên
    public float fadeTime = 1f;  // thời gian fade out

    CanvasGroup cg;
    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        // di chuyển lên
        transform.Translate(Vector3.up * floatSpeed * Time.unscaledDeltaTime);
        // fade dần
        cg.alpha -= Time.unscaledDeltaTime / fadeTime;
        if (cg.alpha <= 0f)
            Destroy(gameObject);
    }
}