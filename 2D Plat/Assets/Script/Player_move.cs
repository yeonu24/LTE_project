using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chracter_move : MonoBehaviour
{
    public float Maxspeed;
    public float JumpPower;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        // jump = space바를 입력하면 점프로 인식됨

        if (Input.GetButtonDown("Jump")&& !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            anim.SetBool("Jumping", true);
        }


        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }


        if (Input.GetButtonDown("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        // 걷는 애니메이션 재생
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
        {
            // SetBool :  bool의 true / false 를 설정함
            anim.SetBool("Walking", false);
        }
        else
        {
            anim.SetBool("Walking", true);
        }
    }

    void FixedUpdate()
    {
        /* Horizontal : X축 이동 
           Vertical : y축 이동  */
        float height = Input.GetAxisRaw("Horizontal"); //속도 입력
        // Debug.Log("height 입력:" + height);

        /* AddForce : 물리 오브젝트를 이동하거나, 이동속도 또는 방향을 변경할때 사용하는 함수 */

        rigid.AddForce(Vector2.right * height, ForceMode2D.Impulse);

        if (rigid.velocity.x > Maxspeed)
        {
            rigid.velocity = new Vector2(Maxspeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < Maxspeed * (-1))
        {
            rigid.velocity = new Vector2(Maxspeed *(-1), rigid.velocity.y);
        }
        // Math.Abs : 절댓값을 구하는 함수

        //착지
        if(rigid.velocity.y < 0)
        {
            //RayCast : 오브젝트 검색을 위해서 Ray를 쏘는 방식
            // DrawRay = 에디터상에서만 Ray를 그리는 함수
            Debug.DrawRay(rigid.position,Vector3.down, new Color(8, 43, 58));
            // RaycastHit : Ray에 닿은 오브젝트
            RaycastHit2D rayHit;
            rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if(rayHit.collider != null)
            {
                if(rayHit.distance < 0.5f)
                {
                    anim.SetBool("Jumping", false);
                }
                Debug.Log(rayHit.collider.name);
            }
        }
    }
}
