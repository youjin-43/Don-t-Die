using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
    private GameObject _shade;
    private GameObject      _highlight;
    private Image      _itemImage;

    private bool       _isActivated = false;

    private Queue<Item> _items = new Queue<Item>();

    private void Awake()
    {
        _shade     = transform.Find("Shade").gameObject; 
        _highlight = transform.Find("Highlight").gameObject;
        _itemImage = transform.Find("ItemImage").GetComponent<Image>();
    }

    void Start()
    {

    }

    void Update()
    {
        if(IsEmpty() == true)
        {
            _shade.gameObject.SetActive(false);
        }
        else
        {
            _shade.gameObject.SetActive(true);
        }
    }

    public void Activate()
    {
        _isActivated = true;
        _highlight.SetActive(true);
    }
    public void Deactivate()
    {
        _isActivated = false;
        _highlight.SetActive(false);
    }

    public void AddItem(Item item)
    {
        _items.Enqueue(item);

        if(_itemImage.sprite == null)
        {
            _itemImage.sprite = DataManager.Instance.IconImageData[item.name];
        }
    }
    public Item GetItem()
    {
        return _items.Peek();
    }
    public string GetHavingItemName()
    {
        return _items.Peek().name;
    }
    public bool IsEmpty()
    {
        return _items.Count == 0;
    }
    public bool IsFull()
    {
        return _items.Count == 64;
    }
}
