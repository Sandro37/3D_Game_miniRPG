using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float colliderRadius;
    [SerializeField] private float enemyDamage;
    private float rotation;
    Vector3 moveDirection;
    CharacterController controller;
    Animator anim;
    List<Transform> enemiesList = new List<Transform>();

    [Header("LIFE")]
    [SerializeField] private float totalHealth;
    [SerializeField] private float currentHealth;

    [Header("LIFE BAR - CANVAS")]
    [SerializeField] private Image healthBar;

    [Header("GameController")]
    [SerializeField] private GameController gameController;
    private bool isDamage;
    private bool isDead = false;
    private bool isReady;
    private bool isClick;

    public bool IsDead
    {
        get => isDead;
        set => isDead = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        currentHealth = totalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        isClick = EventSystem.current.currentSelectedGameObject;

        if (!isDead)
        {
            if (!isClick)
            {
                Move();
                GetMouseInput();
            }
        }
    }

    private void Move()
    {
        //if (controller.isGrounded)
        //{
            if (Input.GetKey(KeyCode.W) && !anim.GetBool("attacking") && !anim.GetBool("hiting") && !isClick)
            {
                anim.SetBool("walking", true);
                anim.SetInteger("transition", 1);
                moveDirection = Vector3.forward * speed;
                moveDirection = transform.TransformDirection(moveDirection);
            }
            if (Input.GetKeyUp(KeyCode.W) && !anim.GetBool("attacking") && !anim.GetBool("hiting") && !isClick)
            {
                anim.SetBool("walking", false);
                anim.SetInteger("transition", 0);
                moveDirection = Vector3.zero;
            }

            if (Input.GetKey(KeyCode.S) && !anim.GetBool("attacking") && !anim.GetBool("hiting") && !isClick) 
            {
                anim.SetBool("walking", true);
                anim.SetInteger("transition", 1);
                moveDirection = new Vector3(0, 0, -1) * speed;
                moveDirection = transform.TransformDirection(moveDirection);
            }

            if (Input.GetKeyUp(KeyCode.S) && !anim.GetBool("attacking") && !anim.GetBool("hiting") && !isClick)
            {
                anim.SetBool("walking", false);
                anim.SetInteger("transition", 0);
                moveDirection = Vector3.zero;
            }
       // }

        rotation += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rotation, 0);

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void GetMouseInput()
    {
        if (controller.isGrounded)
        {
            if (Input.GetMouseButtonDown(0) && !anim.GetBool("attacking") && !anim.GetBool("hiting") && !gameController.IsInventoryEnable && !isClick)
            {
                if (anim.GetBool("walking"))
                {
                    anim.SetBool("walking", false);
                    anim.SetInteger("transition", 0);
                    moveDirection = Vector3.zero;
                }

                if (!anim.GetBool("walking"))
                {
                    StartCoroutine("Attack");
                    
                }
            }
        }
    }

    IEnumerator Attack()
    {
        if (!isReady)
        {
            isReady = true;

            anim.SetInteger("transition", 2);
            anim.SetBool("attacking", true);
            yield return new WaitForSeconds(0.5f);
            GetEnemiesRange();

            foreach (Transform enemy in enemiesList)
            {
                //executar acao dano enemy
                Enemy _enemy = enemy.GetComponent<Enemy>();

                if (_enemy != null)
                {
                    _enemy.GetHit(enemyDamage);
                }
            }
            yield return new WaitForSeconds(0.8f);


            anim.SetBool("attacking", false);
            anim.SetInteger("transition", 0);
            isReady = false;

        }
    }


    void GetEnemiesRange()
    {
        enemiesList.Clear();
        foreach (Collider col in Physics.OverlapSphere(transform.position + transform.forward * colliderRadius,colliderRadius))
        {
            if(col.gameObject.CompareTag("Enemy")){
                enemiesList.Add(col.transform);
            }
        }
    }


    public void GetHit(float damage)
    {
        currentHealth -= damage;
        Die();
    }

    private void Die()
    {
        healthBar.fillAmount = currentHealth / totalHealth;

        if (currentHealth <= 0)
        {
            isDead = true;
            anim.SetInteger("transition", 4);
            StopCoroutine("Attack");
        }
        else
        {
            
            isDamage = true;
            anim.SetInteger("transition", 3);
            StopCoroutine("Attack");
            anim.SetBool("hiting", true);
            isReady = false;
            anim.SetBool("attacking", false);
            StartCoroutine(RecoveryFromHit());
        }
    }

    IEnumerator RecoveryFromHit()
    {
        yield return new WaitForSeconds(1.333f);
        isDamage = false;
        anim.SetInteger("transition", 0);
        anim.SetBool("hiting", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, colliderRadius);
    }

    public void Increasestats(float health, float increaseSpeed)
    {
        currentHealth += health;
        speed += increaseSpeed;
    }

    public void DecreaseStats(float health, float increaseSpeed)
    {
        currentHealth -= health;
        speed -= increaseSpeed;
    }

}
