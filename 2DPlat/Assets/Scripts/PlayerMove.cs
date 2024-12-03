using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/* 진행순서 : Awake > Start > Update > FixedUpdate > LateUpdate */

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    /* 단타 or 판정 같은 곳에서 사용 */
    void Update()
    {
        /* JUMP :  space 바 누르면 점프라고 인식됨 ㅅㄱ */
        if(Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
        // 버튼에서 키 땔 떄(정지)
        if(Input.GetButtonUp("Horizontal"))
        {
            /*normalized : 벡터 크기를 1로 만듦(단위벡터 상태)*/
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y); 
        } 

        // 방향 전환
        if(Input.GetButtonDown("Horizontal"))
        {
            /* Flip : 스프라이트를 뒤집기 */
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        /* 걷는 애니메이션 재생 */
        if(Mathf.Abs(rigid.velocity.x) < 0.3)
        {
            /* SetBool : bool의 true / false 를 설정함 */
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
    }

/* 1초에 60번 정도 출력 */
    void FixedUpdate()
    {
        /* Horizontal : 가로 이동(2D 기준 X축) */
        /* Vertical : 세로 이동 (2D 기준 Y축) */
        float height = Input.GetAxisRaw("Horizontal"); // 속도 입력
        // Debug.Log("height 입력 : " + height);

        /* AddForce : 물리 오브젝트를 이동하거나, 이동속도 또는 방향을 변경할 때 사용하는 함수 */
        rigid.AddForce(Vector2.right * height, ForceMode2D.Impulse);

        /* velocity : Rigidbody의 현재 속도. 자료형은 Vector */
        if(rigid.velocity.x > maxSpeed) // 오른쪽 최대속도 도달시
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if(rigid.velocity.x < -maxSpeed) // 왼쪽 최대 속도 도달시
        {
            rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);
        }

        /* Mathf.Abs : 절댓값을 구하는 함수. */
        Debug.Log("현재 속도 : " + Mathf.Abs(rigid.velocity.x));

        /* 착지 */
        if(rigid.velocity.y < 0)
        {
            /* RayCast : 오브젝트 검색을 위해서 Ray를 쏘는 방식 */
            /* DrawRay : 에디터 상에서 만 Ray를 그리는 함수. */
            Debug.DrawRay(rigid.position,Vector3.down, new Color(0, 1, 0));
            /* RayCastHit : Ray에 닿은 오브젝트 */ 
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if(rayHit.collider != null)
            {
                if(rayHit.distance < 0.5f)
                {
                    anim.SetBool("isJumping", false);
                }
                Debug.Log(rayHit.collider.name);
            }
        }        
    }
}