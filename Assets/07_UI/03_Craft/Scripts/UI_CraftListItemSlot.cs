using UnityEngine;
using UnityEngine.UI;

public class UI_CraftListItemSlot : MonoBehaviour
{
    // 아이템을 그릴 슬롯인지, +, = 모양을 그릴 슬롯인지
    public enum Type
    {
        ItemSlot,
        Image_Equal,
        Image_Plus
    }

    private string _imageName;
    private Type   _type;

    void Start()
    {
    }

    public void SetType(Type type, string imageName)
    {
        _imageName = imageName;
        _type      = type;

        if (_type != Type.ItemSlot)
        {
            transform.Find("Shade").gameObject.SetActive(false);
        }

        if (DataManager.Instance.IconImageData.TryGetValue(_imageName, out Sprite sprite))
        {
            transform.GetChild(1).GetComponent<Image>().sprite = sprite;
        }
    }
}