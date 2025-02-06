using UnityEngine;
using UnityEngine.UI;

public class GaugeColor : MonoBehaviour
{
    public Image gaugeImage;

    void Update()
    {
        gaugeImage.color = Color.HSVToRGB(gaugeImage.fillAmount / 3, 1.0f, 1.0f);
    }
}
