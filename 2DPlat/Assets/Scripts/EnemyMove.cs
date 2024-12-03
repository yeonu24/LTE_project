using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public int nextMove;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 5);

    }
    void FixedUpdate()
    {
        /* 몹 이동 */
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        /* 플랫폼 판정 */
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Debug.Log("대학원 생이 되는 쉽고 빠른 길");
            Turn();
        }
    }

    /*재귀 함수 : 자신을 스스로 호출하는 함수 */
    void Think()
    {
        /* Random.Range : 난수 발생 함수 Random.Range(범위최소, 범위최대+1) 형식으로 작성 */
        nextMove = Random.Range(-1, 2);

        /* 스프라이트 애니메이션 */
        anim.SetInteger("walkSpeed", nextMove);

        /* 스프라이트 방향전환 */
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }


        /* 딜레이 없이 쓰는 재귀함수는 디도스와 같다. */
        /* Invoke : 함수를 딜레이 시킬때 사용. Invoke("함수명", 시간) 형식으로 사용함 */
        /* 재귀 함수는  맨 밑에 써주는 것이 매너 */
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        /* CancleInvoke() : 현재 작동중인 Invoke 함수를 취소 시키는 함수 */
        CancelInvoke();
        Invoke("Think", 3);
    }
}