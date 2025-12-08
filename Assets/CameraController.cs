using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;

    // 카메라의 Z축 값 (카메라가 화면에서 얼마나 떨어져 있는지를 결정)
    // 이 값은 보통 변하지 않으므로 Start 함수 이전에 고정합니다.
    private float cameraZ;

    void Start()
    {
        this.player = GameObject.Find("cat_0");

        // 카메라의 원래 Z축 위치를 저장해 둡니다.
        cameraZ = transform.position.z;
    }

    void Update()
    {
        // 1. 고양이 캐릭터의 현재 위치를 가져옵니다.
        Vector3 playerPos = this.player.transform.position;

        // 2. 카메라의 위치를 고양이의 X, Y 좌표로 업데이트하고,
        //    Z 좌표는 변경하지 않고 유지합니다.
        transform.position = new Vector3(
            playerPos.x,  // 👈 고양이의 X 위치를 따라갑니다.
            playerPos.y,  // 👈 고양이의 Y 위치를 따라갑니다.
            cameraZ       // 👈 카메라의 원래 깊이(Z축)를 유지합니다.
        );
    }
}