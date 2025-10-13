// using UnityEngine;

// public class PlayerController : MonoBehaviour
// {
//     public float moveSpeed = 5.0f;
    
//     // Animator 컴포넌트 참조 (private - Inspector에 안 보임)
//     private Animator animator;
    
//     void Start()
//     {
//         // 게임 시작 시 한 번만 - Animator 컴포넌트 찾아서 저장
//         animator = GetComponent<Animator>();
        
//         // 디버그: 제대로 찾았  는지 확인
//         // if (animator != null)
//         // {
//         //     Debug.Log("Animator 컴포넌트를 찾았습니다!");
//         // }
//         // else
//         // {
//         //     Debug.LogError("Animator 컴포넌트가 없습니다!");
//         // }
//     }

//     void Update()
//     {
//         // 이동 벡터 계산
//         Vector3 movement = Vector3.zero;

//         if (Input.GetKey(KeyCode.A))
//         {
//             // transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
//             transform.localScale = new Vector3(-1, 1, 1); // X축 뒤집기
//             // isMoving = true;
//             movement += Vector3.left;
//         }

//         if (Input.GetKey(KeyCode.D))
//         {
//             transform.localScale = new Vector3(1, 1, 1); // 원래 크기
//             movement += Vector3.right;
//         }

        

        

    
//         // 속도 계산: 이동 중이면 moveSpeed, 아니면 0
//         float currentMoveSpeed = movement != Vector3.zero ? moveSpeed : 0f;

//         //달리기
//         if (Input.GetKey(KeyCode.LeftShift))
//         {
//             currentMoveSpeed = moveSpeed * 2f;
//             //달리기 모드 활성화
//             transform.Translate(movement * currentMoveSpeed * Time.deltaTime);
//         }

//         //점프
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             if (animator != null)
//             {
//                 animator.SetBool("isJumping", true);
//                 transform.position += Vector3.up * Time.deltaTime;
//             }
        
            
//         }
        
//         if (Input.GetKeyUp(KeyCode.Space))
//         {
//             if (animator != null)
//             {
//                 animator.SetBool("isJumping", false);
                
//             }


//         }
    
//         // 실제 이동 적용
//         if (movement != Vector3.zero)
//         {
//             transform.Translate(movement * moveSpeed * Time.deltaTime);
//         }

//         //Animator에 속도 전달
//         if (animator != null)
//         {
//             animator.SetFloat("Speed", currentMoveSpeed);
//             // animator.SetBool("isMoving");

//         }
//     }

// }
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5.0f;
    [Header("점프 설정")]  // 새로 추가!
    public float jumpForce = 10.0f;  // 점프 힘

    private Animator animator;
    private Rigidbody2D rb;

    private float moveInput = 0f;
    private bool isGrounded = false;  // 바닥에 닿아있는지 여부
    private int score = 0;  // 점수 추가
    private Vector3 startPosition; //리스폰시작위치
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D가 없습니다! Player 오브젝트에 추가하세요.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator가 없습니다! Player 오브젝트에 추가하세요.");
        }

        // 게임 시작 시 위치를 저장 - 새로 추가!
        startPosition = transform.position;
        Debug.Log("시작 위치 저장: " + startPosition);
    }

    void Update()
    {
        // 입력 감지
        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            moveInput = 0f;
        }

        // 애니메이션 속도 설정
        animator.SetFloat("Speed", Mathf.Abs(moveInput * moveSpeed));

         // 점프 입력 처리 (새로 추가!)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            Debug.Log("점프!");
        }
            
    }

    void FixedUpdate()
    {
        // Rigidbody2D에 속도 적용
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 "Ground" Tag를 가지고 있는지 확인
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("바닥에 착지!");
            isGrounded = true;
        }

        //적,장애물 충돌기능
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("⚠️ 적과 충돌! 시작 지점으로 돌아갑니다.");
        
            // 시작 위치로 순간이동
            transform.position = startPosition;
        
            // 속도 초기화 (안 하면 계속 날아감)
            rb.linearVelocity = new Vector2(0,0);
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("⚠️ 장애물과 충돌! 시작 지점으로 돌아갑니다.");
        
            // 시작 위치로 순간이동
            transform.position = startPosition;
        
            // 속도 초기화 (안 하면 계속 날아감)
            rb.linearVelocity = new Vector2(0,0);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("바닥에서 떨어짐");
            isGrounded = false;
        }
    }
    // 아이템 수집 감지 (Trigger)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            score++;  // 점수 증가
            Debug.Log("코인 획득! 현재 점수: " + score);
            Destroy(other.gameObject);  // 코인 제거
        }
        if (other.CompareTag("Enemy"))
        {
            score++;  // 점수 증가
            Debug.Log("적 처치! 현재 점수: " + score);
            Destroy(other.gameObject);  // 적 제거
        }
        if (other.CompareTag("Finish"))
        {
            Debug.Log("🎉🎉🎉 게임 클리어! 🎉🎉🎉");
            Debug.Log("최종 점수: " + score + "점");
        
        // 캐릭터 조작 비활성화
            enabled = false;
        }
    }
    
}
