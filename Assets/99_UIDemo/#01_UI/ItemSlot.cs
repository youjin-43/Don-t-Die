using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private Image itemImage;
    private bool  isActivated = false;

    private void Awake()
    {
        itemImage = transform.Find("ItemImage").GetComponent<Image>();
    }

    void Start()
    {

    }

    void Update()
    {
        if(isActivated)
        {

        }
    }

    public void Activate()
    {
        isActivated     = true;
        itemImage.color = Color.green;
    }
    public void Deactivate()
    {
        isActivated = false;
        itemImage.color = Color.white;
    }
}
