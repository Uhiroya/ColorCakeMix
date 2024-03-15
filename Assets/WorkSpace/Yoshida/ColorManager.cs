using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : SingletonMonoBehavior<ColorManager>
{
    [SerializeField] private Image _image;
    [SerializeField] private GameObject[] _powderPrefabs;

    private const int _orderColorCount = 3;
    
    private readonly List<Color> _colorList = new()
    {
        //  Color構造体は0~1
        new Color(215 / 255f, 69 / 255f, 57 / 255f),
        new Color(120 / 255f, 185 / 255f, 35 / 255f),
        new Color(91 / 255f, 165 / 255f, 212 / 255f),
        new Color(254 / 255f, 215 / 255f, 98 / 255f),
        new Color(240 / 255f, 137 / 255f, 0),
        new Color(239 / 255f, 239 / 255f, 239 / 255f),
        new Color(57 / 255f, 50 / 255f, 45 / 255f),
    };

    public Color OrderColor { get; private set; }
    public List<Color> SelectMaterials { get; private set; } = new();
    public GameObject[] PowderPrefabs => _powderPrefabs;
    
    public void DecisionOrderColor()
    {
        var orderList = new List<Color>(_colorList);
        var r = 0f;
        var g = 0f;
        var b = 0f;
        for (var i = 0; i < _orderColorCount; i++)
        {
            var random = Random.Range(0, orderList.Count - 1);
            var co = orderList[random];
            Debug.Log(orderList[random]);
            r += co.r;
            g += co.g;
            b += co.b;
            orderList.RemoveAt(random);
        }

        var col = new Color(r,g,b, 1) / 3f;
        col.a = 1;
        OrderColor = col;
        _image.color = col;
    }

    public void SelectMaterial(int number)
    {
        SelectMaterials.Add(_colorList[number]);
    }
}
