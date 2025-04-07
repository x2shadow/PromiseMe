using UnityEngine;

public class JorikFollower : MonoBehaviour
{
    public Transform girl;  // Ссылка на девочку
    public Vector3 offset;  // Желаемый смещённый вектор (в мировых координатах)

    public Transform player;  // Ссылка на объект игрока
    public float rotationSpeed = 5f;

    void LateUpdate()
    {
        if(girl != null)
        {
            transform.position = girl.position + offset;
        }

        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        
        // Если направление не нулевое, вычисляем целевой поворот и плавно приближаемся к нему
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
