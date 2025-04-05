using UnityEngine;

public class Fire : MonoBehaviour
{
    public float lifetime = 3f;   // Время жизни фаера (в секундах)

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
