using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    Rigidbody2D _rigidbody;
    BoxCollider2D _collider;

    public float jumpforce = 4f;
    public float moveSpeed = 0f;
    //슬라이딩 지속 시간
    public float slideTime = 1f;
    //체력 설정
    public int maxHealth = 9;
    private int currentHealth;

    public bool isHurt = false;

    bool isJump = false;
    bool isSliding = false;
    private bool isGrounded = false;

    public bool godMode = false;

    Vector2 originalSize;
    Vector2 originalOffset;
    Vector2 slideSize = new Vector2(2f, 0.8f);
    Vector2 slideOffset = new Vector2(0.05f, -0.35f);


    //피격 쿨타임 설정(초기 쿨타임을 위한 -999f
    private float hurtCooldown = 1.0f;
    private float lastHurtTime = -999f;

    // Start is called before the first frame update
    void Start()
    {
        //체력 초기화
        currentHealth = maxHealth;
        //컴포넌트 가져옴
        //에니메이터는 하위에 있는 오브젝트이기에 코드가 다름
        animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();

        originalSize = _collider.size;
        originalOffset = _collider.offset;
        GameManager.instance.uiManager.UpdateLifeDisplay(currentHealth);


        //컴포넌트를 찾지 못했다면 에러
        if (animator == null)
            Debug.LogError("Not Founded Animator");

        if (_rigidbody == null)
            Debug.LogError("Not Founded Rigidbody");

        if (_collider == null)
            Debug.LogError("Not Founded Collider");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isGrounded)
         {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            isJump = true;
         }
        
        if (!isHurt)
        {
            if (!isSliding && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(1)))
            {
                StartCoroutine(DoSlide());
            }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("Deadzone"))
        {
            Die();
        }
        else if (collision.gameObject.CompareTag("HealItem"))
        {
            Heal(1);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("TakeDamageItem"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    public void TakeDamage(int damage)
    {
        //아직 피격중이면 데미지를 받지않음
        if (godMode || Time.time - lastHurtTime < hurtCooldown)
            return;

        lastHurtTime = Time.time;
        currentHealth -= damage;
        isHurt = true;
        if(isSliding)
        {
            StopCoroutine(DoSlide());
            EndSlide();
        }
        animator.SetTrigger("Hurt");

        Debug.Log("HP:" + currentHealth);
        
        if (currentHealth <= 0)
        {
            GameManager.instance.GameOver();
        }

        StartCoroutine(HurtCooldown());
        GameManager.instance.uiManager.UpdateLifeDisplay(currentHealth);
    }

    IEnumerator HurtCooldown()
    {
        yield return new WaitForSeconds(hurtCooldown);
        isHurt = false;
    }

    void Die()
    {
        Debug.Log("You Died");
        GameManager.instance.GameOver();
    }

    IEnumerator DoSlide()
    {
        isSliding = true;
        animator.SetTrigger("Slide");

        //콜라이더 크기 변경
        _collider.size = slideSize;
        _collider.offset = slideOffset;

        yield return new WaitForSeconds(slideTime);

        EndSlide();
    }

    void EndSlide()
    {
        isSliding = false;

        //클라이더 복구
        _collider.size = originalSize;
        _collider.offset = originalOffset;

    }
    
    public void Heal(int healing)
    {
        currentHealth += healing;
        //초과 회복방지
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log($"Healing now HP:{currentHealth}");
        GameManager.instance.uiManager.UpdateLifeDisplay(currentHealth);
    }

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

}
