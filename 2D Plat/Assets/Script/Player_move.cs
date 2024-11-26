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

        // jump = space�ٸ� �Է��ϸ� ������ �νĵ�

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

        // �ȴ� �ִϸ��̼� ���
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
        {
            // SetBool :  bool�� true / false �� ������
            anim.SetBool("Walking", false);
        }
        else
        {
            anim.SetBool("Walking", true);
        }
    }

    void FixedUpdate()
    {
        /* Horizontal : X�� �̵� 
           Vertical : y�� �̵�  */
        float height = Input.GetAxisRaw("Horizontal"); //�ӵ� �Է�
        // Debug.Log("height �Է�:" + height);

        /* AddForce : ���� ������Ʈ�� �̵��ϰų�, �̵��ӵ� �Ǵ� ������ �����Ҷ� ����ϴ� �Լ� */

        rigid.AddForce(Vector2.right * height, ForceMode2D.Impulse);

        if (rigid.velocity.x > Maxspeed)
        {
            rigid.velocity = new Vector2(Maxspeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < Maxspeed * (-1))
        {
            rigid.velocity = new Vector2(Maxspeed *(-1), rigid.velocity.y);
        }
        // Math.Abs : ������ ���ϴ� �Լ�

        //����
        if(rigid.velocity.y < 0)
        {
            //RayCast : ������Ʈ �˻��� ���ؼ� Ray�� ��� ���
            // DrawRay = �����ͻ󿡼��� Ray�� �׸��� �Լ�
            Debug.DrawRay(rigid.position,Vector3.down, new Color(8, 43, 58));
            // RaycastHit : Ray�� ���� ������Ʈ
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
