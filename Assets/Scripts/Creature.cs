using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Creature : MonoBehaviour
{
    public enum AttackType { Melee, Ranged }
    public AttackType attackType = AttackType.Melee;

    public Transform attackPivot;
    public GameObject swordTrail;
    
    public float speed = 1.0f;
    public float detectionRange = 0.1f;
    public Transform raycastOriginMovement; 
    public Transform raycastOriginCombat; 

    private GameObject target;
    public float attackRange = 0.5f;
    public float attackCooldown = 5.0f;
    public int attackDamage = 20;
    public float attackTimer = 0;

    private int movementDirection;
    
    public Image attackTimerBar;
    
    public AudioClip attackSound;
    private AudioSource audioSource;

    void Start()
    {
        if (gameObject.CompareTag("Ally"))
            movementDirection = 1;
        else if (gameObject.CompareTag("Enemy"))
            movementDirection = -1;
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        swordTrail.SetActive(false);
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        UpdateAttackTimerBar();
        RotateAttackPivot();
    
        RaycastHit2D hitMove = Physics2D.Raycast(raycastOriginMovement.position, Vector2.right * movementDirection, detectionRange);
        RaycastHit2D hitAttack = Physics2D.Raycast(raycastOriginCombat.position, Vector2.right * movementDirection, attackRange);
    
        if (hitAttack.collider != null && 
            attackTimer >= attackCooldown && 
            ((movementDirection == -1 && hitAttack.collider.CompareTag("Ally")) || (movementDirection == 1 && hitAttack.collider.CompareTag("Enemy"))))
        {
            Attack(hitAttack.collider.gameObject);
            PlayAttackSound();
            StartCoroutine(ShowWaponTrail()); ;
        }
    
        if (hitMove.collider != null && (hitMove.collider.CompareTag("Ally") || hitMove.collider.CompareTag("Enemy")))
        {
            return; // Stop moving if another creature is detected
        }

        // Move the creature
        transform.Translate(Vector2.right * (speed * Time.deltaTime * movementDirection));
    }

    
    private void Attack(GameObject target)
    {
        Health enemyHealth = target.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(attackDamage);
        }
        attackTimer = 0f;
        if (attackType == AttackType.Melee)
        {
            ResetPivotRotation(); // Reset rotation for melee attack
        }
    }
    
    private void UpdateAttackTimerBar()
    {
        if (attackTimerBar != null)
            attackTimerBar.fillAmount = attackTimer / attackCooldown;
    }
    
    private void RotateAttackPivot()
    {
        if (attackPivot != null && attackType == AttackType.Melee)
        {
            float attackProgress = attackTimer / attackCooldown;
            float targetAngle = 179;
            if (attackTimer < attackCooldown)
            {
                float currentAngle = Mathf.Lerp(0, 179, attackProgress);
                attackPivot.localRotation = Quaternion.Euler(0, 0, currentAngle * movementDirection);
            }
        }
    }

    private void ResetPivotRotation()
    {
        if (attackPivot != null)
        {
            attackPivot.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
    
    private void PlayAttackSound()
    {
        if (attackSound != null && audioSource != null)
        {
            audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(attackSound);
        }
    }
    
    private IEnumerator ShowWaponTrail()
    {
        swordTrail.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        swordTrail.SetActive(false);
    }
}