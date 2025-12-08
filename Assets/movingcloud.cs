using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // 구름이 왕복할 최대 거리 (Inspector에서 설정 가능)
    public float moveDistance = 5f;

    // 이동 속도 (Inspector에서 설정 가능)
    public float moveSpeed = 1f;

    private Vector3 startPosition; // 구름의 초기 위치를 저장할 변수

    void Start()
    {
        // 게임 시작 시 구름의 현재 위치를 저장
        startPosition = transform.position;
    }

    void Update()
    {
        // Mathf.PingPong: Time.time * moveSpeed 값이 0과 moveDistance 사이를 왕복합니다.
        float pingPongValue = Mathf.PingPong(Time.time * moveSpeed, moveDistance);

        // 새 위치 계산: 초기 위치(startPosition)에 왕복 값을 더하여 X축으로 움직임
        Vector3 newPosition = startPosition;
        newPosition.x += pingPongValue;

        // 구름 위치 적용
        transform.position = newPosition;
    }
}