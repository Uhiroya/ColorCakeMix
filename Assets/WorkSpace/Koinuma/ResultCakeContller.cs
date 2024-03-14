using UnityEngine;
using UnityEngine.UI;

public class ResultCakeContller : MonoBehaviour
{
    [SerializeField] private Image _cakeImage;
    [SerializeField] private Text _priceText;

    public void InitializeCake(Color cakeColor, int cakePrice)
    {
        _cakeImage.color = cakeColor;
        _priceText.text = cakePrice + "円";
    }
}
