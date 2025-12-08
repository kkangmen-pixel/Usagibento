using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;
    float jumpForce = 550.0f;
    float walkForce = 30.0f;
    float maxWalkSpeed = 2.0f;

    // ⭐️ 추가: 획득 효과음 파일 (Inspector에서 연결) ⭐️
    public AudioClip fishCollectSound;

    // ⭐️ 추가: 피격 효과음 파일 (Inspector에서 연결) ⭐️
    public AudioClip hurtSound;

    // ⭐️ 추가: 소리를 재생할 컴포넌트 ⭐️
    private AudioSource audioSource;

    // ⭐️ 추가: 물고기 획득 시 생성할 이펙트 프리팹 ⭐️
    public GameObject fishEffectPrefab;

    // 2단 점프를 위한 변수
    private int jumpCount = 0;
    public int maxJumpCount = 2;

    void Start()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();

        // ⭐️ 추가: AudioSource 컴포넌트를 가져옵니다. ⭐️
        this.audioSource = GetComponent<AudioSource>();
    }

    // FixedUpdate(): 물리 연산을 안정적으로 처리합니다.
    void FixedUpdate()
    {
        // 걷는 힘 로직
        int key = 0;
        if (Input.GetKey(KeyCode.RightArrow)) { key = 1; }
        if (Input.GetKey(KeyCode.LeftArrow)) { key = -1; }

        float speedx = Mathf.Abs(this.rigid2D.linearVelocity.x);
        if (speedx < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce);
        }
    }


    void Update()
    {
        // 점프 로직: Update에서 Input.GetKeyDown을 사용하여 점프 순간만 감지합니다.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount < maxJumpCount)
            {
                jumpCount++;

                if (rigid2D.linearVelocity.y > 0)
                {
                    rigid2D.linearVelocity = new Vector2(rigid2D.linearVelocity.x, 0f);
                }

                this.animator.SetTrigger("JumpTrigger");
                this.rigid2D.AddForce(transform.up * this.jumpForce);
            }
        }

        // 움직이는 방향에 맞춰 반전
        int key = 0;
        if (Input.GetKey(KeyCode.RightArrow)) { key = 1; }
        if (Input.GetKey(KeyCode.LeftArrow)) { key = -1; }

        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }

        // 플레이어 속도에 맞춰 애니메이션 속도를 바꾼다
        float speedx = Mathf.Abs(this.rigid2D.linearVelocity.x);
        if (this.rigid2D.linearVelocity.y == 0)
        {
            this.animator.speed = speedx / 2.0f;
        }
        else
        {
            this.animator.speed = 1.0f;
        }

        // 플레이어가 화면 밖으로 나갔다면 처음부터 (떨어졌을 때 재시작 로직)
        if (transform.position.y < -10)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // ----------------------------------------------------------------------
    // ⭐️ 충돌 처리 함수 ⭐️
    // ----------------------------------------------------------------------

    // 골 도착 및 물고기 획득 처리 (트리거 충돌)
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 물고기 획득 처리
        if (collision.gameObject.CompareTag("Fish"))
        {
            // 1. 효과음 재생
            if (audioSource != null && fishCollectSound != null)
            {
                audioSource.PlayOneShot(fishCollectSound);
            }

            // 2. 이펙트 생성
            if (fishEffectPrefab != null)
            {
                Instantiate(fishEffectPrefab, collision.transform.position, Quaternion.identity);
            }

            // 3. 물고기 오브젝트 삭제
            Destroy(collision.gameObject);
            return;
        }

        // ⭐️ 수정: 불덩이(Projectile) 충돌 시 피격음만 재생 (레벨 재시작 없음) ⭐️
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // 1. 피격음 재생
            if (audioSource != null && hurtSound != null)
            {
                audioSource.PlayOneShot(hurtSound);
            }

            // 2. 불덩이 오브젝트 삭제
            Destroy(collision.gameObject);

            // ⚠️ SceneManager.LoadScene(SceneManager.GetActiveScene().name); 코드는 제거되었습니다.

            return; // 충돌 처리 후 함수 종료
        }

        // 골 도착 (기존 코드)
        if (collision.gameObject.CompareTag("Goal"))
        {
            Debug.Log("골");
            SceneManager.LoadScene("ClearScene");
        }
    }

    // OnCollisionEnter2D 함수 하나로 통합 (적 캐릭터, 점프 횟수 초기화, 구름 부모 설정)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. 적 캐릭터와 충돌 시 레벨 재시작
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        // 2. 점프 횟수 초기화 (적과 충돌한 경우를 제외하고 실행)
        jumpCount = 0;

        // 3. 움직이는 구름에 닿았을 때 부모 설정
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(collision.transform);
        }
    }

    // 구름에서 떨어졌을 때
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(null);
        }
    }
}