using UnityEngine;
using UnityEngine.UI;

public class UI_CraftListItemSlot : MonoBehaviour
{
    public enum Type
    {
        ItemSlot,
        Image_Equal,
        Image_Plus
    }

    private Image  _image;
    private string _imageName;
    private Type   _type;

    void Start()
    {
        _image = transform.Find("ItemImage").GetComponent<Image>();
    }

    public void SetType(Type type, string imageName)
    {
        _imageName = imageName;
        _type = type;
    }

    private void Update()
    {
        if(DataManager.Instance.IconImageData.Count > 0 )
        {
            _image.sprite = DataManager.Instance.IconImageData[_imageName];
        }
        if (_type != Type.ItemSlot)
        {
            transform.Find("Shade").gameObject.SetActive(false);
        }
    }
}