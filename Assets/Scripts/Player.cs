using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    Rigidbody2D _rigidbody;

    public float jumpforce = 4f;
    public float moveSpeed = 2f;
    //체력 설정
    public int maxHealth = 9;
    public bool ishurt = false;

    bool isJump = false;

    public bool godMode = false;

    private int currentHealth;

    //피격 쿨타임 설정(초기 쿨타임을 위한 -999f
    private float hurtCooldown = 1.0f;
    private float lastHurtTime = -999f;

    // Start is called before the first frame update
    void Start()
    {
        //체력 초기화
        currentHealth = maxHealth;
        //컴포넌트 가져옴
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

        //컴포넌트를 찾지 못했다면 에러
        if (animator == null)
            Debug.LogError("Not Founded Animator");

        if (_rigidbody == null)
            Debug.LogError("Not Founded Rigidbody");
    }

    // Update is called once per frame
    void Update()
    {
        if(ishurt)
            return;
        else
        {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                isJump = true;
        }
    }

    private void FixedUpdate()
    {
        Vector3 velocity = _rigidbody.velocity;
        velocity.x = moveSpeed;

        //점프의 힘만큼 점프시킴
        if (isJump)
        {
            velocity.y = jumpforce;
            isJump = false;
        }

        _rigidbody.velocity = velocity;

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("Deadzone"))
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        //아직 피격중이면 데미지를 받지않음
        if (godMode || Time.time - lastHurtTime < hurtCooldown)
            return;

        lastHurtTime = Time.time;
        currentHealth -= damage;
        ishurt = true;
        animator.SetTrigger("Hurt");

        Debug.Log("HP:" + currentHealth);
        
        if (currentHealth <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    void Die()
    {
        Debug.Log("You Died");
        GameManager.Instance.GameOver();
    }
}
