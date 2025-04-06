using UnityEngine;

public class Fire : MonoBehaviour
{
    public float lifetime = 5f;   // Время жизни фаера (в секундах)

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Darkness"))
        {
            Darkness darkness = other.GetComponent<Darkness>();
            if (darkness != null)
            {
                darkness.StopTemporarily();
            }
        }
    }
}
