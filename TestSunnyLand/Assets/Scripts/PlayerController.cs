using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    
    // Animator 컴포넌트 참조 (private - Inspector에 안 보임)
    private Animator animator;
    
    void Start()
    {
        // 게임 시작 시 한 번만 - Animator 컴포넌트 찾아서 저장
        animator = GetComponent<Animator>();
        
        // 디버그: 제대로 찾았는지 확인
        // if (animator != null)
        // {
        //     Debug.Log("Animator 컴포넌트를 찾았습니다!");
        // }
        // else
        // {
        //     Debug.LogError("Animator 컴포넌트가 없습니다!");
        // }
    }

    void Update()
    {
        // 이동 벡터 계산
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            // transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            transform.localScale = new Vector3(-1, 1, 1); // X축 뒤집기
            // isMoving = true;
            movement += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.localScale = new Vector3(1, 1, 1); // 원래 크기
            movement += Vector3.right;
        }

        

        

    
        // 속도 계산: 이동 중이면 moveSpeed, 아니면 0
        float currentMoveSpeed = movement != Vector3.zero ? moveSpeed : 0f;

        //달리기
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMoveSpeed = moveSpeed * 2f;
            //달리기 모드 활성화
            transform.Translate(movement * currentMoveSpeed * Time.deltaTime);
        }

        //점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (animator != null)
            {
                animator.SetBool("isJumping", true);
                transform.position += Vector3.up * Time.deltaTime;
            }
        
            
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (animator != null)
            {
                animator.SetBool("isJumping", false);
                
            }


        }
    
        // 실제 이동 적용
        if (movement != Vector3.zero)
        {
            transform.Translate(movement * moveSpeed * Time.deltaTime);
        }

        //Animator에 속도 전달
        if (animator != null)
        {
            animator.SetFloat("Speed", currentMoveSpeed);
            // animator.SetBool("isMoving");

        }
    }

}