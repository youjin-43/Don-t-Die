using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFishingAction : MonoBehaviour
{
    [SerializeField] ToolItemData currentTool;

    [SerializeField] Transform fishingTip;      // 낚싯대 끝부분
    [SerializeField] GameObject bobberPrefab;   // 찌 원본 프리팹
    Bobber bobber;                          // 지금 던져진 찌

    LineRenderer lineRenderer;              // 낚시줄
    Animator animator;

    Vector3 fishingPoint;                   // 찌가 던져질 위치

    bool isPressed;     // 낚시 버튼 누르고 있는지
    bool isBobberThrown;   // 찌를 던졌는지
    bool isPulling;     // 물고기가 물었는지

    float watingTimer;          // 찌를 던지고 나서 흐른 시간
    float targetWaitingTime;    // 찌를 던지고 얼마가 지나야 물고기가 찌를 무는지

    float pullingTimer;
    float maxPullingTime = 2f;  // maxPullingTime 전에 찌를 빼야 물고기를 낚음

    float castingTimer;             // 낚시 버튼을 누르고 얼마나 흘렀는지 ( 찌를 얼마나 멀리 보낼지 체크 )
    float maxCastingTimer = 2f;
    float maxBobberDistance = 10f;   // 찌와 플레이어 사이의 최대 거리

    const string TRIGGER_CAST = "Fish_Cast";
    const string TRIGGER_WAIT = "Fish_Wait";
    const string TRIGGER_PULL = "Fish_Pull";
    const string TRIGGER_CATCH = "Fish_Catch";


    #region 장비 변경 이벤트

    private void OnDestroy()
    {
        // 이벤트 해제 (메모리 누수 방지)
        EquipmentManager.Instance.OnEquipChanged -= HandleEquipChanged;
    }

    /// <summary>
    /// 장비가 변경될 때 호출되는 함수
    /// </summary>
    private void HandleEquipChanged(ItemData itemData, EquipmentSlot slot)
    {
        if (slot == EquipmentSlot.Hand) currentTool = EquipmentManager.Instance.GetCurrentTool();
    }

    #endregion

    void Start()
    {
        // 장비 변경 이벤트 구독
        EquipmentManager.Instance.OnEquipChanged += HandleEquipChanged;

        animator = GetComponent<Animator>();
        lineRenderer = fishingTip.GetComponent<LineRenderer>();
        isBobberThrown = false;
        watingTimer = 0;
        targetWaitingTime = Random.Range(5f, 10f);
    }
    
    public void Fishing()
    {
        if (currentTool != null && currentTool.Type == ToolType.Rod && !isBobberThrown && !isPulling)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isPressed = true;
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 playerPos = GameManager.Instance.GetPlayerPos();
                fishingPoint = (mousePos - playerPos).normalized;
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

            if (Input.GetMouseButtonDown(0))
            {
                DebugController.Log("낚시 취소");
                CatchBobber();
            }

            if (watingTimer >= targetWaitingTime)
            {
                animator.SetTrigger(TRIGGER_PULL);
                isBobberThrown = false;
                isPulling = true;
                watingTimer = 0;
                targetWaitingTime = Random.Range(5f, 10f);
            }
        }

        if (isPulling)
        {
            pullingTimer += Time.deltaTime;

            if (Input.GetMouseButtonDown(0))
            {
                DebugController.Log("물고기를 획득했습니다.");
                CatchBobber();

                //GetFish 또는 몬스터 스폰 중 랜덤하게 함수를 호출. 추후 수정
                CatchBobber(true);
            }

            if (pullingTimer >= maxPullingTime)
            {
                DebugController.Log("물고기를 놓쳤습니다.");
                CatchBobber();
            }
        }

        if (bobber != null && bobber.gameObject.activeSelf)
        {
            SetFishingLine();
        }
    }

    /// <summary>
    /// 낚싯대 끝과 찌를 연결하는 line을 렌더링한다.
    /// </summary>
    void SetFishingLine()
    {
        lineRenderer.SetPosition(0, fishingTip.position);
        lineRenderer.SetPosition(1, bobber.transform.position);
    }

    void ThrowBobber()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Casting")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) { return; }
        lineRenderer.enabled = true;

        // 현재 생성된 찌가 없으면 만듦
        if (bobber == null)
        {
            bobber = Instantiate(bobberPrefab).GetOrAddComponent<Bobber>();
            bobber.player = this;
        }

        // 던지는 애니메이션
        animator.SetTrigger(TRIGGER_CAST);

        // 찌 던질 위치를 정하고 그곳으로 던진다
        bobber.gameObject.SetActive(true);
        bobber.transform.position = transform.position;

        float distMultiflier = Mathf.Max(3f, maxBobberDistance * (castingTimer / maxCastingTimer));
        fishingPoint *= distMultiflier;
        fishingPoint += transform.position;

        bobber.Throw(transform.position, fishingPoint);

        isPressed = false;
        castingTimer = 0f;
        isBobberThrown = true;
    }

    public void WaitingAnimation()
    {   // 낚싯대 던지는 애니메이션이 끝나면 대기하는 애니메이션으로 전환하려고 만든 함수.
        animator.SetTrigger(TRIGGER_WAIT);
    }

    public void DeactivateLineRenderer()
    {
        lineRenderer.enabled = false;
    }

    public void ResetCatchTrigger()
    {
        animator.ResetTrigger(TRIGGER_CAST);
        animator.ResetTrigger(TRIGGER_PULL);
        animator.ResetTrigger(TRIGGER_WAIT);
        animator.ResetTrigger(TRIGGER_CATCH);
    }

    /// <summary>
    /// 찌를 뺀다.
    /// </summary>
    public void CatchBobber(bool success = false)
    {
        isBobberThrown = false;
        isPulling = false;
        pullingTimer = 0;
        watingTimer = 0;
        targetWaitingTime = Random.Range(5f, 10f);
        animator.SetTrigger(TRIGGER_CATCH);
        bobber.Throw(fishingPoint, transform.position, true, success);      // success 가 true면 물고기 획득.
    }
}
