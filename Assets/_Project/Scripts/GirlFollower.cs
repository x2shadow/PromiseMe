using UnityEngine;
using UnityEngine.UI;

public class GirlFollower : MonoBehaviour
{
    [Header("Настройки слежения")]
    public Transform player;             // Ссылка на объект игрока.
    public float followDistance = 2f;      // Идеальное расстояние от игрока (для расчёта целевой позиции).
    public float moveSpeed = 3f;           // Скорость движения девочки.
    public float minFollowDistance = 1f;   // Если девочка ближе этого расстояния, она не двигается.
    public float maxFollowDistance = 5f;   // Если девочка дальше этого расстояния, она тоже не двигается.
    public float maxTiltAngle = 20f;

    [Header("Настройки энергии")]
    public float maxEnergy = 100f;         // Максимальное количество энергии.
    public float energy = 100f;            // Текущее значение энергии.
    public float energyDecreaseRate = 20f; // Скорость расхода энергии при движении.
    public float energyRecoveryRate = 10f; // Скорость восстановления энергии, когда девочка стоит.

    public Slider energySlider;          // UI Slider для отображения уровня энергии.

    public DialogueUI dialogueUI;

    Animator animator;

    bool hasSaidWait = false;

    bool isResting = false;
    public bool isDead = false;
    float restTimer = 0f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        // Инициализация слайдера
        if (energySlider != null)
        {
            energySlider.minValue = 0f;
            energySlider.maxValue = maxEnergy;
            energySlider.value = energy;
        }
    }

    void Update()
    {
        if (isDead) return;
        // Всегда поворачиваемся лицом к игроку
        RotateTowardsPlayer2();

        // Расстояние до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Если энергия есть и не в режиме отдыха, двигаемся только если игрок в нужном диапазоне
        if (energy > 0f && !isResting)
        {
            if (distanceToPlayer >= minFollowDistance && distanceToPlayer <= maxFollowDistance)
            {
                FollowPlayer();
                hasSaidWait = false;
            }
            else if (distanceToPlayer > maxFollowDistance)
            {
                // Если игрок слишком далеко
                if (!hasSaidWait)
                {
                    //dialogueUI.ShowGirlDialogue("Я не вижу тебя!");
                    hasSaidWait = true;
                }
                RecoverEnergy();
            }
            else
            {
                // Если игрок слишком близко или слишком далеко – девочка не движется, а энергия восстанавливается
                RecoverEnergy();
                hasSaidWait = false;
            }
        }
        else
        {
            // Если энергии нет или включен режим отдыха, начинаем восстановление энергии
            isResting = true;
            ProcessRest();
            RecoverEnergy();
        }

        // Обновляем значение слайдера
        if (energySlider != null)
        {
            energySlider.value = energy;
        }
    }

    // Двигает девочку к целевой позиции, вычисляемой относительно позиции игрока
    private void FollowPlayer()
    {
        // Целевая позиция – позади игрока на расстоянии followDistance
        Vector3 targetPosition = player.position - player.forward * followDistance;
        Vector3 direction = targetPosition - transform.position;

        if (direction.magnitude > 0.1f)
        {
            // Расход энергии происходит только при движении
            energy -= energyDecreaseRate * Time.deltaTime;
            energy = Mathf.Clamp(energy, 0f, maxEnergy);

            Vector3 move = direction.normalized * moveSpeed * Time.deltaTime;
            move = new Vector3(move.x, 0f, move.z);
            transform.position += move;

            animator.SetBool("isWalking", true);
        }
        else
        {
            // Если цель достигнута, энергия восстанавливается
            RecoverEnergy();
        }
    }

    // Восстанавливает энергию
    private void RecoverEnergy()
    {
        animator.SetBool("isWalking", false);

        energy += energyRecoveryRate * Time.deltaTime;
        energy = Mathf.Clamp(energy, 0f, maxEnergy);
    }

    // Обрабатывает режим отдыха: когда энергия полностью исчерпана, девочка отдыхает 3 секунды,
    // после чего можно снова двигаться (при условии, что энергия восстановлена выше порога).
    private void ProcessRest()
    {
        animator.SetBool("isWalking", false);

        if (restTimer <= 0f)
        {
            // Запускаем таймер отдыха при полном исчерпании энергии
            restTimer = 3f;
        }
        else
        {
            restTimer -= Time.deltaTime;
            if (restTimer <= 0f && energy > 25f)
            {
                // Выходим из режима отдыха, если энергия восстановилась выше 25
                isResting = false;
            }
        }
    }

    // Всегда поворачивает девочку в сторону игрока
    private void RotateTowardsPlayer()
    {
        Vector3 lookDirection = player.position - transform.position;
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
        }
    }

    private void RotateTowardsPlayer2()
    {
        Vector3 lookDirection = player.position - transform.position;
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            // Получаем углы поворота
            Vector3 targetEuler = targetRotation.eulerAngles;
            
            // Преобразуем угол X из [0,360) в диапазон [-180,180]
            if (targetEuler.x > 180f)
                targetEuler.x -= 360f;
            
            // Ограничиваем наклон по оси X (например, maxTiltAngle = 20 градусов)
            targetEuler.x = Mathf.Clamp(targetEuler.x, -maxTiltAngle, maxTiltAngle);
            
            // Формируем конечный кватернион
            targetRotation = Quaternion.Euler(targetEuler);

            // Плавный переход
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
        }
    }

}
