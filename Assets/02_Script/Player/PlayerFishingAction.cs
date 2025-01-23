using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFishingAction : MonoBehaviour
{
    [SerializeField] Transform fishingTip;      // 낚싯대 끝부분
    [SerializeField] GameObject bobberPrefab;   // 찌 원본 프리팹
    Bobber bobber;                          // 지금 던져진 찌

    bool isPressed;     // 낚시 버튼 누르고 있는지
    bool isBobberThrown;   // 찌를 던졌는지
    bool isPulling;     // 물고기가 물었는지

    Vector3 fishingPoint;
    float watingTimer;          // 찌를 던지고 나서 흐른 시간
    float targetWaitingTime;    // 찌를 던지고 얼마가 지나야 물고기가 찌를 무는지

    float pullingTimer;
    float maxPullingTime = 2f;  // maxPullingTime 전에 찌를 빼야 물고기를 낚음

    float castingTimer;             // 낚시 버튼을 누르고 얼마나 흘렀는지 ( 찌를 얼마나 멀리 보낼지 체크 )
    float maxCastingTimer = 2f;
    float maxBobberDistance = 8f;   // 찌와 플레이어 사이의 최대 거리

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        isBobberThrown = false;
        watingTimer = 0;
        targetWaitingTime = Random.Range(5f, 10f);
    }
    
    void Update()
    {
        // if (도구 낀 상태로 앞에 물이 있으면)
        if (!isBobberThrown && !isPulling)
        {
            if ((Input.GetMouseButtonDown(0)))
            {
                isPressed = true;
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 playerPos = GameManager.Instance.GetPlayerPos();
                fishingPoint = (mousePos - playerPos).normalized;

                if (Mathf.Abs(fishingPoint.x) > Mathf.Abs(fishingPoint.y))    // 찌 좌우로 던지기
                {
                    fishingPoint.y = 0;
                }
                else                                        // 찌 상하로 던지기
                {
                    fishingPoint.x = 0;
                }
            }
            else if (isPressed && Input.GetMouseButtonUp(0))
            {
                ThrowBobber();
            }
        }

        if (isPressed && castingTimer < maxCastingTimer)
        {
            castingTimer += Time.deltaTime;
        }

        if (isBobberThrown)
        {
            watingTimer += Time.deltaTime;

            bobber.SetLineRenderer(fishingTip.position);

            if (Input.GetMouseButtonDown(0))
            {
                bobber.gameObject.SetActive(false);
                isBobberThrown = false;
                watingTimer = 0;
                targetWaitingTime = Random.Range(5f, 10f);
            }

            if (watingTimer >= targetWaitingTime)
            {
                isBobberThrown = false;
                isPulling = true;
                watingTimer = 0;
                targetWaitingTime = Random.Range(5f, 10f);
            }
        }

        if (isPulling)
        {
            pullingTimer += Time.deltaTime;

            bobber.SetLineRenderer(fishingTip.position);

            if (Input.GetMouseButtonDown(0))
            {
                isPulling = false;
                pullingTimer = 0;
                bobber.gameObject.SetActive(false);
                DebugController.Log("물고기를 획득했습니다.");
            }

            if (pullingTimer >= maxPullingTime)
            {
                isPulling = false;
                pullingTimer = 0;
                bobber.gameObject.SetActive(false);
                DebugController.Log("물고기를 놓쳤습니다.");
            }
        }
    }

    void ThrowBobber()
    {
        if (bobber == null)
        {
            bobber = Instantiate(bobberPrefab).GetOrAddComponent<Bobber>();
        }

        bobber.gameObject.SetActive(true);
        bobber.transform.position = transform.position;

        float distMultiflier = maxBobberDistance * (castingTimer / maxCastingTimer);
        fishingPoint *= distMultiflier;
        fishingPoint += transform.position;

        bobber.Throw(transform.position, fishingPoint);

        isPressed = false;
        castingTimer = 0f;
        isBobberThrown = true;
    }
}
