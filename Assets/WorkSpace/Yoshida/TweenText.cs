using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TweenText : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField] float _moveTime;
    [SerializeField] float _moveRange;

    private void Start()
    {
        _text.rectTransform.DOLocalMoveY(_moveRange, _moveTime).SetLoops(-1, LoopType.Yoyo);
    }
}
