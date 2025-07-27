using UnityEngine;

public class LevelUpEffect : MonoBehaviour
{
    public GameObject effectPrefab;

    public void PlayEffect(Vector3 position)
    {
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, position, Quaternion.identity);
        }
    }
}
