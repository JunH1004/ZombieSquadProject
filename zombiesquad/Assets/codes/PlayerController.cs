using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1f; // 공격 쿨다운 (초 단위)
    [SerializeField] private float moveCooldown = 1f;   // 이동 쿨다운 (1초마다 이동)
    [SerializeField] private Weapon currentWeapon;     // 현재 장착된 무기

    [SerializeField] private float health = 100f;

    private Vector3 tileCenter = new Vector3(0, 0, 0);
    public Vector2 tileBounds = new Vector2(6f, 3f);
    private Vector3 targetPosition;
    private float lastAttackTime;
    private float lastMoveTime; // 마지막 이동 시간
    private SpriteRenderer spriteRenderer; // 스프라이트 플립용
    private Transform nearestZombie; // 가장 가까운 좀비 추적

    void Start()
    {
        targetPosition = transform.position;
        tileCenter = transform.position;
        currentWeapon = new Pistol(); // 초기 무기로 권총 설정
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 가져오기
        lastMoveTime = Time.time; // 초기화
    }

    void Update()
    {
        MoveInTile();
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        AttackNearestZombie();
        UpdateZOrder(); // Z축 조정
    }

    void MoveInTile()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange * 2);
        nearestZombie = null;
        float minDistance = float.MaxValue;

        Vector3 previousPosition = transform.position; // 이동 방향 계산용 (플립에는 사용 안 함)

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Zombie"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestZombie = hit.transform;
                }
            }
        }

        // 이동 쿨다운 체크
        if (Time.time - lastMoveTime < moveCooldown) 
        {
            FlipSprite(); // 이동 없어도 방향 업데이트
            return;
        }

        if (nearestZombie == null)
        {
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Vector2 slightOffset = Random.insideUnitCircle * 0.2f;
                targetPosition = tileCenter + new Vector3(slightOffset.x, slightOffset.y, 0);
                targetPosition = ClampToTile(targetPosition);
                lastMoveTime = Time.time; // 이동 시각 갱신
            }
        }
        else
        {
            float distanceToZombie = Vector3.Distance(transform.position, nearestZombie.position);
            Vector3 directionToZombie = (nearestZombie.position - transform.position).normalized;

            if (distanceToZombie < attackRange * 0.5f)
            {
                targetPosition = transform.position - directionToZombie * (attackRange * 0.75f);
            }
            else if (distanceToZombie > attackRange)
            {
                targetPosition = transform.position + directionToZombie * (attackRange * 0.75f);
            }
            else
            {
                Vector3 perpendicular = new Vector3(-directionToZombie.y, directionToZombie.x, 0).normalized;
                targetPosition = transform.position + perpendicular * 0.5f;
            }
            targetPosition = ClampToTile(targetPosition);
            lastMoveTime = Time.time; // 이동 시각 갱신
        }

        FlipSprite(); // 방향 업데이트
    }

    void AttackNearestZombie()
    {
        if (Time.time - lastAttackTime < attackCooldown || currentWeapon == null) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        Transform attackTarget = null;
        float minDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Zombie"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    attackTarget = hit.transform;
                }
            }
        }

        if (attackTarget != null)
        {
            currentWeapon.Attack(attackTarget.gameObject, this);
            lastAttackTime = Time.time;
        }
    }

    Vector3 ClampToTile(Vector3 pos)
    {
        Vector3 relativePos = pos - tileCenter;
        float halfWidth = tileBounds.x / 2;
        float halfHeight = tileBounds.y / 2;

        relativePos.x = Mathf.Clamp(relativePos.x, -halfWidth, halfWidth);
        float xAbs = Mathf.Abs(relativePos.x);
        float yMax = halfHeight * (1 - xAbs / halfWidth);
        float yMin = -halfHeight * (1 - xAbs / halfWidth);
        relativePos.y = Mathf.Clamp(relativePos.y, yMin, yMax);

        return tileCenter + relativePos;
    }

    void UpdateZOrder()
    {
        // Y축에 따라 Z값 조정 (낮을수록 앞에 렌더링)
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y * 0.01f);
    }

    void FlipSprite()
    {
        if (nearestZombie != null)
        {
            // 항상 가장 가까운 좀비 방향을 바라봄
            Vector3 attackDirection = (nearestZombie.position - transform.position).normalized;
            if (attackDirection.x > 0) // 오른쪽
            {
                spriteRenderer.flipX = false; // 기본 방향 (오른쪽)
            }
            else if (attackDirection.x < 0) // 왼쪽
            {
                spriteRenderer.flipX = true; // 좌로 플립
            }
        }
        // 좀비가 없으면 기본 방향 (오른쪽) 유지
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        float halfWidth = tileBounds.x / 2;
        float halfHeight = tileBounds.y / 2;

        Vector3 top = tileCenter + new Vector3(0, halfHeight, 0);
        Vector3 bottom = tileCenter - new Vector3(0, halfHeight, 0);
        Vector3 left = tileCenter - new Vector3(halfWidth, 0, 0);
        Vector3 right = tileCenter + new Vector3(halfWidth, 0, 0);

        Gizmos.DrawLine(top, right);
        Gizmos.DrawLine(right, bottom);
        Gizmos.DrawLine(bottom, left);
        Gizmos.DrawLine(left, top);
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
        Debug.Log("Player died!");
    }

    public bool IsDead() => health <= 0;
}