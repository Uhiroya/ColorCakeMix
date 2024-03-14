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
        new Color(215, 69, 57),
        new Color(120, 185, 35),
        new Color(91, 165, 212),
        new Color(254, 215, 98),
        new Color(240, 137, 0),
        new Color(239, 239, 239),
        new Color(57, 50, 45),
    };

    public Color OrderColor { get; private set; }
    public List<Color> SelectMaterials { get; private set; } = new();
    
    public void DecisionOrderColor()
    {
        var orderList = _colorList;
        var r = 0f;
        var g = 0f;
        var b = 0f;
        for (var i = 0; i < _orderColorCount; i++)
        {
            var random = Random.Range(0, orderList.Count - 1);
            var co = orderList[random];
            Debug.Log($"AnsIndex {random}");
            r += co.r;
            g += co.g;
            b += co.b;
            orderList.RemoveAt(random);
        }

        var col = new Color((r / 3 / 255), (g / 3 / 255), (b / 3 / 255), 1);
        OrderColor = col;
        _image.color = col;
    }

    public void SelectMaterial(int number)
    {
        SelectMaterials.Add(_colorList[number]);
    }
}