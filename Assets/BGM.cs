using UnityEngine;

public class BGM : MonoBehaviour
{
    // ⭐️ 이 BGM 오브젝트의 유일한 인스턴스를 저장하는 정적 변수 ⭐️
    private static BGM instance = null;

    // Start() 대신 Awake()를 사용하여 오브젝트가 생성되자마자 실행되게 합니다.
    void Awake()
    {
        // 1. 이미 BGM 인스턴스가 존재하고, 그 인스턴스가 '이' 오브젝트가 아니라면,
        //    (즉, 다른 씬에서 이미 BGM이 넘어왔다면)
        if (instance != null && instance != this)
        {
            // ⭐️ 자신을 파괴하여 중복 재생을 막습니다. ⭐️
            Destroy(this.gameObject);
            return;
        }

        // 2. 현재 오브젝트가 유일한 인스턴스이므로, 자신을 인스턴스로 지정합니다.
        instance = this;

        // 3. ⭐️ 씬이 바뀌어도 파괴되지 않도록 설정합니다. ⭐️
        DontDestroyOnLoad(this.gameObject);

        // 4. AudioSource 컴포넌트를 가져와서 설정을 확인하거나 여기서 재생할 수도 있습니다.
        // 하지만 일반적으로 AudioSource 컴포넌트의 Play On Awake 옵션을 켜둡니다.
    }

    // Start()와 Update()는 기능이 없으므로 비워둡니다.
    void Start()
    {

    }

    void Update()
    {

    }
}