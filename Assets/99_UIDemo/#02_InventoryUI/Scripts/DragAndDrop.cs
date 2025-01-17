using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour
{
    #region SINGLETON
    private static DragAndDrop instance;
    public  static DragAndDrop Instance
    {
        get
        {
            return instance;
        }
    }

    void SingletonInitialize()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion 

    [SerializeField] Canvas        canvas;
    [SerializeField] RectTransform dragItem;

    UI_ItemSlot   originalSlot;
    ItemData      draggedItemData;

    bool _isDragging = false;

    private void Awake()
    {
        SingletonInitialize();
    }

    void Start()
    {
        dragItem.gameObject.SetActive(false);
        dragItem.GetComponent<Image>().raycastTarget = false;
    }

    void Update()
    {
        if (_isDragging == true)
        {
            dragItem.position = Input.mousePosition;
        }
    }

    public void BeginDrag(UI_ItemSlot slot)
    {
        if(slot.IsEmpty() == true)
        {
            return;
        }

        originalSlot     = slot;
        draggedItemData  = slot.GetItemData();
        dragItem.GetComponent<Image>().sprite = draggedItemData.Image;
        dragItem.gameObject.SetActive(true);

        slot.ClearSlot();

        _isDragging = true;
    }

    public void EndDrag(UI_ItemSlot dropSlot)
    {
        if(dropSlot != null && dropSlot.IsEmpty() == true)
        {
            dropSlot.AddItem(draggedItemData);
        }
        else
        {
            ItemData dropSlotItemData = dropSlot.GetItemData();

            dropSlot.AddItem(draggedItemData);

            originalSlot.AddItem(dropSlotItemData);
        }

        draggedItemData = null;
        dragItem.gameObject.SetActive(false);

        _isDragging = false;
    }
}
