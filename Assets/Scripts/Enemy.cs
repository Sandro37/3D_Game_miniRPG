using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float totalHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float attackDamage;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float colliderRadius;

    [Header("movement zone")]
    [SerializeField] private float lookRadius;
    [SerializeField] private Transform target;
    [SerializeField] private float speedRotation;

    [Header("health bar")]
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject healhtBarCanvas;


    [Header("Path")]
    [SerializeField] private List<Transform> pathPoints = new List<Transform>();
    [SerializeField] private int currentPathIndex = 0;
    [SerializeField] private float distancePoint;

    private Animator anim;
    private NavMeshAgent agent;
    private CapsuleCollider col;
    private bool isAttack;
    private bool isWalking;
    private bool isDamage;
    private bool isDead;
    private bool isReady;
    private bool isHiting;

    private bool isPlayerDead = false;
    private void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = totalHealth;
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        Move();
    }

    void MoveToNextPoint()
    {
        if(pathPoints.Count > 0)
        {
            float distance = Vector3.Distance(pathPoints[currentPathIndex].position, transform.position);
            agent.destination = pathPoints[currentPathIndex].position;

            if(distance <= distancePoint)
            {
                currentPathIndex = Random.Range(0, pathPoints.Count);
                currentPathIndex %= pathPoints.Count;
            }
        }
    }
    void Move()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius && !isPlayerDead)
        {
            agent.isStopped = false;
            if (!isAttack && !isDamage)
            {
                anim.SetInteger("transition", 1);
                agent.SetDestination(target.position);
                isWalking = true;
            }
            

            if (distance <= agent.stoppingDistance && !isDamage)
            {
                //atacar
                StartCoroutine("TimeAttack");
                Looktarget();
            }
        }
        else
        {
            if (!isDead)
            {
                anim.SetInteger("transition", 1);
                isWalking = true;
                MoveToNextPoint();
            }
            
            isAttack = false;
            isWalking = false;
        }
    }
    void GetEnemy()
    {
        foreach (Collider col in Physics.OverlapSphere((transform.position + transform.forward *  colliderRadius), colliderRadius))
        {

            if (col.gameObject.CompareTag("Player"))
            {
                col.GetComponent<Player>().GetHit(attackDamage);
                if (col.GetComponent<Player>().IsDead)
                {
                    isPlayerDead = col.GetComponent<Player>().IsDead;
                }
            }
        }
    }
    IEnumerator TimeAttack()
    {
        if (!isReady && !isDamage && !isDead && !isHiting)
        {
            isReady = true;
            isAttack = true;
            isWalking = false;
            anim.SetInteger("transition", 2);
            yield return new WaitForSeconds(1.28f);
            GetEnemy();
            yield return new WaitForSeconds(1.3835f);
            isAttack = false;
            isReady = false;
        }
        

    }
    void Looktarget()
    {
        Vector3 direction = (target.position - target.position).normalized;
        Quaternion looRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,looRotation, Time.deltaTime * speedRotation);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position + transform.forward, colliderRadius);
    }


    public void GetHit(float damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / totalHealth;
        Die(); 
    }

    IEnumerator RecoveryFromHit()
    {
        yield return new WaitForSeconds(0.9f);
        isDamage = false;
        isHiting = false;
        anim.SetInteger("transition", 0);
        isReady = false;
    }

    void Die()
    {
        if(currentHealth <= 0)
        {
            col.enabled = false;
            lookRadius = 0f;
            isDead = true;
            healhtBarCanvas.gameObject.SetActive(false);
            anim.SetInteger("transition", 4);
            StopCoroutine("TimeAttack");
        }
        else
        {
            isHiting = true;
            isDamage = true;
            StopCoroutine("TimeAttack");
            anim.SetInteger("transition", 3);
            StartCoroutine(RecoveryFromHit());
        }
    }
}
