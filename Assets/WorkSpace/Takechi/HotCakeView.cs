using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ホットケーキの見た目を管理するクラス
/// </summary>
public class HotCakeView : MonoBehaviour
{
    [SerializeField, Tooltip("本体、渦")] private Image _circle;
    [SerializeField, Tooltip("焼き目")] private Image _grilledSurface;
    [SerializeField, Tooltip("回転方向")] private FloatReactiveProperty _rotateAngle;
    [SerializeField, Tooltip("回転速度")] private FloatReactiveProperty _rotateSpeed;

    [SerializeField, Tooltip("渦の大きさ、ノイズの大きさ")]
    private FloatReactiveProperty _noiseScale;

    [SerializeField] private ColorReactiveProperty _color1;
    [SerializeField] private ColorReactiveProperty _color2;
    [SerializeField] private ColorReactiveProperty _color3;

    [SerializeField, Tooltip("進行状況(0~1)"), Range(0, 1)]
    private float _progress;

    private Material _vortexMaterial;

    public float RotateSpeed
    {
        get => _rotateSpeed.Value;
        set => _rotateSpeed.Value = value;
    }

    public float RotateAngle
    {
        get => _rotateAngle.Value;
        set => _rotateAngle.Value = value;
    }

    public Color Color1
    {
        set => _color1.Value = value;
    }
    public Color Color2
    {
        set => _color2.Value = value;
    }
    public Color Color3
    {
        set => _color3.Value = value;
    }
    /// <summary>進行状況(0~1)</summary>
    public float Progress
    {
        set => _progress = Mathf.Clamp(value, 0, 1);
    }

    private void Awake()
    {
        //  _circleに付いているmaterialを元に新しくインスタンスを作り割り当てる。
        _vortexMaterial = new(_circle.material);
        _circle.material = _vortexMaterial;
        
        _rotateAngle.Subscribe(f => _vortexMaterial.SetFloat("_Vortex_Strength", f)).AddTo(this);
        _rotateSpeed.Subscribe(f => _vortexMaterial.SetFloat("_Vortex_Speed", f)).AddTo(this);
        _noiseScale.Subscribe(f => _vortexMaterial.SetFloat("_Noise_Scale", f)).AddTo(this);
        _color1.Subscribe(c => _vortexMaterial.SetColor("_Color_1", c)).AddTo(this);
        _color2.Subscribe(c => _vortexMaterial.SetColor("_Color_2", c)).AddTo(this);
        _color3.Subscribe(c => _vortexMaterial.SetColor("_Color_3", c)).AddTo(this);
        this.ObserveEveryValueChanged(view => view._progress)
            .Subscribe(f => _vortexMaterial.SetFloat("_Progress", f))
            .AddTo(this);
    }
}
