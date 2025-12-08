using UnityEngine;

public class hagang: MonoBehaviour
{
    // [Inspector에서 설정] 하강 속도 (음수로 설정해야 아래로 내려옵니다)
    public float fallSpeed = -1.0f;

    // ⭐️ 구름이 현재 내려오고 있는지 추적하는 변수 ⭐️
    private bool isFalling = false;

    // Update() 함수는 계속 실행되며, isFalling이 true일 때만 하강합니다.
    void Update()
    {
        if (isFalling)
        {
            // Vector2.up * fallSpeed * Time.deltaTime을 통해 아래로 이동
            transform.Translate(Vector2.up * fallSpeed * Time.deltaTime);

            // (선택 사항) 화면 아래로 완전히 사라졌을 때 삭제하는 로직
            if (transform.position.y < -15f)
            {
                Destroy(gameObject);
            }
        }
    }

    // ⭐️ 고양이 캐릭터(Player)가 닿았을 때 감지 ⭐️
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 🚨 중요: 고양이 오브젝트에 "Player" 태그가 붙어 있어야 합니다.
        if (collision.gameObject.CompareTag("Player"))
        {
            // 구름의 상단에 고양이가 닿았을 때만 작동하도록 높이를 체크하는 것이 더 정확할 수 있습니다.
            // 하지만 간단하게는, 닿기만 해도 하강하도록 설정합니다.

            // 밟는 순간 하강 시작!
            isFalling = true;
        }

        // *참고: 이 구름이 움직이는 구름이었다면, 여기서 기존의 부모 설정 로직도 통합해야 합니다.
    }
}