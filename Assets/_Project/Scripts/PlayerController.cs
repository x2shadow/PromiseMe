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
    private PlayerInputActions inputActions;

    [Header("Настройки фаеров")]
    public GameObject firePrefab;         // Префаб фаера (не забудьте установить тег "Fire")
    public Transform fireSpawnPoint;      // Точка, из которой будет появляться фаер
    public int maxFires = 3;              // Общее количество фаеров, доступных игроку
    public float throwForce = 10f;

    [Header("Настройки мини игры")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask girlLayer; // слой, в котором находится девочка

    private bool isInputBlocked = false;



    private void Awake()
    {
        // Инициализируем Input Actions
        inputActions = new PlayerInputActions();
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    void Start()
    {
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
    }

    private void Update()
    {
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

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isInputBlocked) return;

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
        if ( maxFires > 0) { ThrowFire(); maxFires--; }
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

        dialogueUI.ShowGirlDialogue("Пипи пупу чек!");
    }

    public void SetInputBlocked(bool blocked)
    {
        isInputBlocked = blocked;
    }
}
