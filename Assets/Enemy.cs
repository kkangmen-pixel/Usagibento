using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    // [Inspector에서 설정] 이동 속도
    public float moveSpeed = 1.5f;

    // [Inspector에서 연결] 쏠 불덩이 프리팹 
    public GameObject projectilePrefab;

    //  [Inspector에서 설정] 발사 간격 (초) 
    public float shootInterval = 2f;

    //  [Inspector에서 설정] 불덩이가 날아가는 속도 
    public float projectileSpeed = 5f;

    private float nextShootTime; // 다음 발사 가능 시간
    private Transform player; // 고양이 캐릭터의 위치를 저장할 변수 (추적 목표)


    void Start()
    {
        //  추가: 고양이 캐릭터(PlayerController가 붙은 오브젝트)를 찾습니다. 
        player = GameObject.FindObjectOfType<PlayerController>()?.transform;
        nextShootTime = Time.time + shootInterval; // 첫 발사 시간 설정
    }

    void Update()
    {
        // 1. 몹을 오른쪽으로 계속 이동시킵니다. (기존 이동 로직 유지)
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        // 2. 몹의 스프라이트 방향을 조정합니다. (기존 방향 설정 로직 유지)
        if (moveSpeed > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveSpeed < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // ⭐️ 추가: 발사 로직 ⭐️
        if (player != null && Time.time >= nextShootTime)
        {
            ShootProjectile();
            nextShootTime = Time.time + shootInterval; // 다음 발사 시간 갱신
        }
    }

    // ⭐️ 추가: 불덩이를 발사하는 함수 ⭐️
    void ShootProjectile()
    {
        // 프리팹이나 플레이어가 없으면 발사하지 않습니다.
        if (projectilePrefab == null || player == null) return;

        // 1. 불덩이 생성: 강아지의 위치에서 발사
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // 2. 방향 계산: 강아지 위치에서 플레이어를 향하는 방향을 정규화합니다.
        Vector2 direction = (player.position - transform.position).normalized;

        // 3. 투사체 이동: Rigidbody2D에 속도를 적용합니다.
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // 투사체가 플레이어를 향해 날아가도록 속도를 설정합니다.
            rb.linearVelocity = direction * projectileSpeed;
        }
    }
}