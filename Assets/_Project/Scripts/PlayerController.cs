using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public DialogueUI dialogueUI;

    [Header("Настройки движения")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 100f;

    private Vector2 moveInput;
    private Vector2 lookInput;
    
    private CharacterController characterController;
    private Camera playerCamera;
    private float xRotation = 0f;
    
    // Ссылка на сгенерированный класс Input Actions
    [HideInInspector]
    public PlayerInputActions inputActions;

    [Header("Настройки фаеров")]
    public GameObject firePrefab;         // Префаб фаера (не забудьте установить тег "Fire")
    public Transform fireSpawnPoint;      // Точка, из которой будет появляться фаер
    public int maxFires = 222;              // Общее количество фаеров, доступных игроку
    public float throwForce = 10f;

    [Header("Кулдаун фаера")]
    public float fireCooldown = 5f; // длительность кулдауна
    private float lastFireTime = -Mathf.Infinity; // время последнего броска

    [Header("Настройки мини игры")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask girlLayer; // слой, в котором находится девочка

    [Header("Мини-игра QTE")]
    [SerializeField] private QTEBarMinigame qteMinigame;

    [Header("Диалог")]
    public bool isDialogueActive = false;
    public Transform dialogueTarget;
    public float dialogueRotationSpeed = 10f;

    [Header("Пауза")]
    [SerializeField] private GameObject pauseCanvas; 
    private bool isPaused = false;

    private bool isInputBlocked = false;

    bool firstDialogueHappened  = false;
    bool secondPart1DialogueHappened = false;
    bool secondPart2DialogueHappened = false;
    bool thirdDialogueHappened = false;
    public bool fourthDialogueHappened = false;
    public bool exitedEnding1 = false;

    private void Awake()
    {
        // Инициализируем Input Actions
        inputActions = new PlayerInputActions();
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    void Start()
    {
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 100f);
        mouseSensitivity = savedSensitivity;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        // Подписка на события для экшенов
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;
        inputActions.Player.Fire.performed += OnFire;
        inputActions.Player.Interact.performed += OnInteract;
        inputActions.Player.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        // Отписка от событий
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Look.canceled -= OnLook;
        inputActions.Player.Fire.performed -= OnFire;
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.Pause.performed -= OnPause;
    }

    private void Update()
    {
        if (isDialogueActive)
        {
            SetInputBlocked(true);
            RotateTowardsDialogueTarget();
            return;
        }

        if (isInputBlocked) return;

        // Движение персонажа
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.Move(move * moveSpeed * Time.deltaTime);
        
        // Обработка обзора (поворот камеры)
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void EndDialogue(int index)
    {
        isDialogueActive = false;
        SetInputBlocked(false);
        moveInput = Vector2.zero;

        //if (index == 0) qteMinigame.StartQTE(inputActions.Player.Spam);

        if (index == 1) 
        {
            firstDialogueHappened = true; 
            dialogueUI.ShowTutorialDialogue("Нажми F чтобы кинуть фаер, отгоняющий тьму");
        }

        if (index == 21) 
        {
            secondPart1DialogueHappened = true; 
            dialogueUI.ShowTutorialDialogue("Нажми E чтобы сыграть в игру");
        }

        if (index == 22) { secondPart2DialogueHappened = true; maxFires = 1; }

        if (index == 3)  { thirdDialogueHappened = true; maxFires = 1; }

        if (index == 4)
        {
            fourthDialogueHappened = true; 
            qteMinigame.StartQTE(inputActions.Player.Spam);
        }
    }

    private void RotateTowardsDialogueTarget()
    {
        if (dialogueTarget == null) return;

        Vector3 direction = dialogueTarget.position - transform.position;
        direction.y = 0f; // игнорируем вертикальную составляющую
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, dialogueRotationSpeed * Time.deltaTime);
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (isInputBlocked) return;
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        if (isInputBlocked) return;
        lookInput = context.ReadValue<Vector2>();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        pauseCanvas.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;  // Останавливаем время
            SetInputBlocked(true); // Блокируем управление
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;  // Возвращаем время
            SetInputBlocked(false); // Возвращаем управление
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isInputBlocked) return;
        if (secondPart1DialogueHappened == false) return;        

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            var minigame = hit.collider.GetComponent<BreathingMinigame>();
            if (minigame != null)
            {
                minigame.StartMinigame(inputActions.Player.Breath);
            }
        }
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if (isInputBlocked) return;
        if (firstDialogueHappened == false) return;

        if (Time.time - lastFireTime < fireCooldown)
        {
            Debug.Log("Фаер еще не готов!");
            return;
        }

        if ( maxFires > 0)
        { 
            ThrowFire(); maxFires--; lastFireTime = Time.time;
            if (thirdDialogueHappened) return;
            if (secondPart2DialogueHappened) { dialogueUI.ShowPlayerDialogue("Этот фаер был последний..."); }
        }
    }

    void ThrowFire()
    {
        Debug.Log("Throw Fire");
        GameObject fireInstance = Instantiate(firePrefab, fireSpawnPoint.position, Quaternion.identity);

        Rigidbody rb = fireInstance.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Добавляем силу в направлении взгляда точки спавна
            rb.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse);
        }
    }

    public void SetInputBlocked(bool blocked)
    {
        isInputBlocked = blocked;
    }
}
