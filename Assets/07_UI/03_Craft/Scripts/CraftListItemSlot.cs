using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftListItemSlot : MonoBehaviour
{
    // 아이템을 그릴 슬롯인지, +, = 모양을 그릴 슬롯인지
    public enum Type
    {
        ItemSlot,
        Image_Equal,
        Image_Plus
    }
    
    private TextMeshProUGUI _currentItemCountText;
    private TextMeshProUGUI _needItemCountText;

    private string _imageName;
    private int    _currentItemCount;
    private int    _needItemCount;
    private Type   _type;

    public void SetData(Type type, string imageName, int needItemCount = 0)
    {
        if(_currentItemCountText == null && _needItemCountText == null)
        {
            _currentItemCountText = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
            _needItemCountText    = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        _imageName     = imageName;
        _needItemCount = needItemCount;
        _type          = type;

        // 아이템이 그려질 슬롯이라면
        if (_type == Type.ItemSlot)
        {
            _needItemCountText.text = needItemCount.ToString();
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
        }

        if (DataManager.Instance.IconImageData.TryGetValue(_imageName, out Sprite sprite))
        {
            transform.GetChild(1).GetComponent<Image>().color  = new Color(1, 1, 1, 1);
            transform.GetChild(1).GetComponent<Image>().sprite = sprite;
        }
    }
}