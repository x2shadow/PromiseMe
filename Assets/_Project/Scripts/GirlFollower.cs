using UnityEngine;
using UnityEngine.UI;

public class GirlFollower : MonoBehaviour
{
    [Header("Настройки слежения")]
    public Transform player;           // Ссылка на объект игрока.
    public float followDistance = 2f;    // Расстояние, на котором девочка будет следовать за игроком.
    public float moveSpeed = 3f;         // Скорость движения девочки.

    [Header("Настройки энергии")]
    public float maxEnergy = 100f;             // Максимальное количество энергии.
    public float energy = 100f;                // Текущее значение энергии.
    public float energyDecreaseRate = 20f;     // Скорость расхода энергии при движении.
    public float energyRecoveryRate = 10f;     // Скорость восстановления энергии, когда девочка стоит.

    public Slider energySlider;

    bool isResting = false;

    void Start()
    {
        // Инициализация слайдера
        if (energySlider != null)
        {
            energySlider.minValue = 0f;
            energySlider.maxValue = maxEnergy;
            energySlider.value = energy;
        }
    }

    private void Update()
    {
        // Если энергия больше нуля, девочка пытается следовать за игроком.
        if (energy > 0f && !isResting)
        {
            FollowPlayer();
        }
        else
        {
            // Если энергия 0, девочка стоит, и энергия восстанавливается.
            isResting = true;
            RecoverEnergy();
            if (energy > 25f) isResting = false;
        }

        // Обновляем значение слайдера
        if (energySlider != null)
        {
            energySlider.value = energy;
        }
    }

    private void FollowPlayer()
    {
        // Целевая позиция немного позади игрока, чтобы создать ощущение "следования"
        Vector3 targetPosition = player.position + player.forward * followDistance;
        Vector3 direction = targetPosition - transform.position;

        // Если расстояние больше заданного порога, двигаемся к цели.
        if (direction.magnitude > 0.1f)
        {
            // Расход энергии при движении (на основе времени).
            energy -= energyDecreaseRate * Time.deltaTime;
            energy = Mathf.Clamp(energy, 0f, maxEnergy);

            Vector3 move = direction.normalized * moveSpeed * Time.deltaTime;
            transform.position += move;
            // Дополнительно можно добавить поворот девочки в сторону игрока:
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), 0.1f);
        }
        else RecoverEnergy();
    }

    private void RecoverEnergy()
    {
        energy += energyRecoveryRate * Time.deltaTime;
        energy = Mathf.Clamp(energy, 0f, maxEnergy);
    }
}
