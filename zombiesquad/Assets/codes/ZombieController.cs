using UnityEngine;
using MoreMountains.Feedbacks;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float health = 50f;
    [SerializeField] private GameObject zombieDeathEffectPrefab;
    [SerializeField] private MMF_Player actionFeedback;

    private Transform target;
    private SpriteRenderer spriteRenderer;
    private float lastAttackTime;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (actionFeedback == null)
        {
            Debug.LogError("MMF_Player 'actionFeedback'가 할당되지 않았습니다!");
        }
    }

    void Update()
    {
        UpdateTarget();
        MoveToTarget();
        AttackTarget();
        UpdateZOrder();
    }

    void UpdateTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform nearestTarget = null;
        float minDistance = float.MaxValue;

        foreach (var player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                PlayerController pc = player.GetComponent<PlayerController>();
                if (pc != null && !pc.IsDead())
                {
                    minDistance = distance;
                    nearestTarget = player.transform;
                }
            }
        }
        target = nearestTarget;
    }

    void MoveToTarget()
    {
        if (target != null)
        {
            Vector3 previousPosition = transform.position;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget > attackRange)
            {
                if (!actionFeedback.IsPlaying)
                    actionFeedback.PlayFeedbacks(); // 이동 시작 시 애니메이션
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            }
            FlipSprite(previousPosition);
        }
    }

    void UpdateRotationFeedback(Vector3 moveDirection)
    {
        if (actionFeedback != null)
        {
            // MMF_Player의 FeedbacksList에서 MMF_Rotation을 찾음
            MMF_Rotation[] rotationFeedbacks = new MMF_Rotation[2];
            int rotationIndex = 0;

            foreach (MMF_Feedback feedback in actionFeedback.FeedbacksList)
            {
                if (feedback is MMF_Rotation rotationFeedback && rotationIndex < 2)
                {
                    rotationFeedbacks[rotationIndex] = rotationFeedback;
                    rotationIndex++;
                }
            }

            float rotationAmount = 15f; // 기본 회전 각도
            float firstRotation = 0f;

            // 첫 번째 회전 피드백
            if (rotationFeedbacks[0] != null)
            {
                if (moveDirection.x <= 0) // 오른쪽 이동 → 반시계 회전
                {
                    rotationFeedbacks[0].RemapCurveZero = 0f;
                    rotationFeedbacks[0].RemapCurveOne = -rotationAmount; // -15도
                    firstRotation = -rotationAmount;
                }
                else if (moveDirection.x > 0) // 왼쪽 이동 → 시계 회전
                {
                    rotationFeedbacks[0].RemapCurveZero = 0f;
                    rotationFeedbacks[0].RemapCurveOne = rotationAmount; // +15도
                    firstRotation = rotationAmount;
                }
                rotationFeedbacks[0].AnimateRotationTarget = gameObject.transform;
            }

            // 두 번째 회전 피드백 (첫 번째와 반대 방향)
            if (rotationFeedbacks[1] != null)
            {
                rotationFeedbacks[1].RemapCurveZero = 0f;
                rotationFeedbacks[1].RemapCurveOne = -firstRotation; // 첫 번째와 반대
                rotationFeedbacks[1].AnimateRotationTarget = gameObject.transform;
            }
        }
    }

    void AttackTarget()
    {
        if (target == null || Time.time - lastAttackTime < attackCooldown) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= attackRange)
        {
            PlayerController player = target.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
                lastAttackTime = Time.time;
                if (!actionFeedback.IsPlaying)
                {
                    actionFeedback.PlayFeedbacks();
                }
                Debug.Log($"{gameObject.name} attacked player for {damage} damage!");
            }
        }
    }

    void UpdateZOrder()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y * 0.01f);
    }

    void FlipSprite(Vector3 previousPosition)
    {
        Vector3 moveDirection = transform.position - previousPosition;
        if (moveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        UpdateRotationFeedback(moveDirection);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(zombieDeathEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Debug.Log($"{gameObject.name} died!");
    }
}