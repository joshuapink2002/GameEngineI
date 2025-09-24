using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;        // 현재 입력
    private Vector2 lastMoveDir = Vector2.down; // 마지막 비영(非0) 입력 방향
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // 물리 프레임에서 속도 적용
        rb.linearVelocity = moveInput * moveSpeed;
    }

    // PlayerInput(Action Events)에서 Move에 연결: PlayerMovement -> Move(InputAction.CallbackContext)
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed || context.started)
        {
            Vector2 input = context.ReadValue<Vector2>();

            // 데드존
            if (input.magnitude < 0.1f)
                input = Vector2.zero;

            // 마지막 비영 입력 방향 저장 (0이 되기 전에)
            if (input.sqrMagnitude > 0.0001f)
                lastMoveDir = input.normalized;

            moveInput = input;
        }
        else if (context.canceled)
        {
            moveInput = Vector2.zero;
        }

        // --- Animator 갱신 ---
        if (animator)
        {
            bool isWalking = moveInput.sqrMagnitude > 0.0001f;
            animator.SetBool("isWalking", isWalking);

            // 현재 입력
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);

            // 마지막 방향 (Idle에서 바라볼 방향)
            animator.SetFloat("LastInputX", lastMoveDir.x);
            animator.SetFloat("LastInputY", lastMoveDir.y);
        }
    }
}
