using System.Linq;
using UnityEngine;

public class PlayerAutoInteract : MonoBehaviour
{
    // 에디터에서 값이 변경될 때마다 호출
    private void OnValidate()
    {
        // 변경된 값 확인 
        DebugController.Log("OnValidate called, detectionRange: " + detectionRange);
    }

    public PlayerAnimator playerAnimator;
    float moveSpeed =0f;

    [SerializeField] bool isAutoInteracting = false; // 자동 상호작용 중인지(디버그용)
    [Space(10f)]
    [SerializeField] float detectionRange = 5f; // 탐색 반경
    [SerializeField] float InteractionRange = 1f; // 상호작용 반경
    [SerializeField] Collider2D[] colliders; // 주변에 상호작용 가능한 물체들 (디버그용)
    [SerializeField] Transform autoInteractTargetTransform; // 자동 상호작용 대상


    private void Start()
    {
        //playerAnimator = GetComponent<PlayerAnimator>();
        moveSpeed = GetComponent<PlayerMove>().moveSpeed;
    }

    /// <summary>
    /// 스페이스 바를 누르면 근처 오브젝트와 자동 상호작용
    /// </summary>
    public void AutoInteract()
    {
        //스페이스 바를 처음 누르게되면 주변에서 상호작용 가능한 가장 가까운 오브젝트를 찾음 
        if (Input.GetKeyDown(KeyCode.Space)) FindClosestInteractableObj();

        //스페이스바를 누르는 동안 타겟으로 이동 
        if (Input.GetKey(KeyCode.Space) && autoInteractTargetTransform != null) MoveTowardsTargetObj();

        //스페이스 바에서 손을 떼면 상호작용 중지 
        if (Input.GetKeyUp(KeyCode.Space)) StopAutoInteraction();

    }

    /// <summary>
    /// 상호작용 가능한 가장 가까운 오브젝트 탐색 
    /// </summary>
    void FindClosestInteractableObj()
    {
        //DebugController.Log("FindClosestInteractableObj 실행 ");

        // 탐색 반경 내에 있는 Interactable 물체 탐지
        colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange, LayerMask.GetMask("Interactable"));

        // 주변에 Interactable한 오브젝트가 있다면 가장 가까운 것으로 이동 타겟 설정 
        if (colliders.Length > 0)
        {
            autoInteractTargetTransform = colliders // Collider2D 배열을 
                .Select(collider => collider.transform) //transform 배열로 바꿔주고 (using System.Linq; 필요)
                .OrderBy(t => Vector2.Distance(transform.position, t.position)) //Distance 기준 오름차순으로 정렬 
                .FirstOrDefault(); // 첫번째 혹은 null 반환 ->  탐지된 물체가 없을 때도 오류 없이 처리가능

            if (autoInteractTargetTransform != null)
            {
                isAutoInteracting = true;

                playerAnimator.SetWalkAnimaion(); //애니메이션 적용

                // 이동 방향에 따라 좌우 바라보게 하기          
                Vector3 direction = autoInteractTargetTransform.position - transform.position;
                if (direction.x < 0) playerAnimator.LookLeft();
                if (direction.x > 0) playerAnimator.LookRight();
            }
        }
        else
        {
            //DebugController.Log("상호작용 가능한 오브젝트가 없습니다 ");
            autoInteractTargetTransform = null;
        }
    }

    /// <summary>
    /// 가장 가까운 상호작용 가능한 오브젝트로 이동 
    /// </summary>
    void MoveTowardsTargetObj()
    {
        if (autoInteractTargetTransform != null)
        {
            Vector3 direction = (autoInteractTargetTransform.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        // 타겟 근처에 도달하면 상호작용 //TODO : 아 이거 Distance 안쓰고 싶은데 나중에 최적화 ㄱㄱ 
        if (Vector2.Distance(transform.position, autoInteractTargetTransform.position) < InteractionRange) InteractWithTarget();
    }

    void InteractWithTarget()
    {
        //DebugController.Log("InteractWithTarget 실행됨");
        if (autoInteractTargetTransform == null) return;
      
        switch (autoInteractTargetTransform.tag)
        {
            case "Item":
                GetItem(); //아이템습득 
                break;
            //case "Monster":
                //공격
                //Attack();
                //break;
            // TODO : 태그 회의 후 추가 
            default:
                break;
        }

        // TODO : 지금은 아이템밖에 없어서 한번만 상호작용하면 되지만 나중에 나무캐기나 공격같은거 하면 여러번 해야하니까 이후 수정 필요 
        
        autoInteractTargetTransform = null; //상호작용 완료한 타겟은 없애고
        FindClosestInteractableObj();//새로운 타겟 탐색
        if (autoInteractTargetTransform == null) StopAutoInteraction();
    }

    void StopAutoInteraction()
    {
        DebugController.Log("StopAutoInteraction");
        isAutoInteracting = false;
        playerAnimator.SetIdleAnimaion();
        autoInteractTargetTransform = null;
    }

    void GetItem()
    {
        //DebugController.Log("GetItem 함수 실행됨");
        Item gotItem = autoInteractTargetTransform.GetComponent<Item>();

        // 필드의 아이템을 인벤토리에 추가했다면
        if (gotItem != null && InventoryManager.Instance.AddItem(gotItem.ItemData))
        {
            Destroy(autoInteractTargetTransform.gameObject); // 필드의 아이템은 지움 //TODO : 아이템들도 오브젝트 풀 써야할까? 
        }
    }

    // 탐색 반경 디버그용 표시
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.DrawWireSphere(transform.position, InteractionRange);
    }

}

