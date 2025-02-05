using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerGetWaterAction : MonoBehaviour
{
    [SerializeField] ToolItemData currentTool;

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

    public float RayDistance = 2f;
    private LayerMask TargetLayer;

    void Start()
    {
        // 장비 변경 이벤트 구독
        EquipmentManager.Instance.OnEquipChanged += HandleEquipChanged;
    }

        Vector2 PlayerDir;

    public void GetWater()
    {
        Vector2 PlayerPos = GameManager.Instance.GetPlayerPos();

        if (GameManager.Instance.PlayerDir != Vector2.zero)
        {
            PlayerDir = GameManager.Instance.PlayerDir;
        }

        Debug.DrawRay(PlayerPos, PlayerDir * RayDistance, Color.red);

        if (currentTool != null && currentTool.Type == ToolType.Bottle)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RaycastHit2D hit = Physics2D.Raycast(PlayerPos, PlayerDir, 1f, LayerMask.GetMask("Water"));

                if (hit.collider != null)
                {
                    // 여기서 물병의 내구도를 최대로 올리면 될듯?

                    EquipmentManager.Instance.AddDurability();
                }
            }
        }
    }
}
