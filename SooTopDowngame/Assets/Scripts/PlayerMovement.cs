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
        if (PauseController.IsGamePaused)
        {
            // 퍼즈 상태: 이동만 멈추고 애니메이터는 건드리지 않음
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 물리 프레임에서 속도 적용
        rb.linearVelocity = moveInput * moveSpeed;
    }

    // PlayerInput(Action Events)에서 Move에 연결: PlayerMovement -> Move(InputAction.CallbackContext)
    public void Move(InputAction.CallbackContext context)
    {
        if (PauseController.IsGamePaused)
        {
            // 퍼즈 상태에서는 입력을 무시하고 애니메이터 갱신도 하지 않음
            return;
        }

        if (context.performed || context.started)
        {
            Vector2 input = context.ReadValue<Vector2>();

            if (input.magnitude < 0.1f)
                input = Vector2.zero;

            if (input.sqrMagnitude > 0.0001f)
                lastMoveDir = input.normalized; // 마지막 이동 방향 저장

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

            // Idle 상태일 때 마지막 바라보던 방향 유지
            animator.SetFloat("LastInputX", lastMoveDir.x);
            animator.SetFloat("LastInputY", lastMoveDir.y);
        }
    }
}
