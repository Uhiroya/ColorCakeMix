using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using Cysharp.Threading.Tasks;
using UniRx.Triggers;
using Unity.VisualScripting;
///UniTask , UniRx , DoTween の動作チェック
public class LibraryTest : MonoBehaviour
{
    // Start is called before the first frame update
    async UniTaskVoid Start()
    {
        this.UpdateAsObservable()
            .Where(_ => this.transform.position.z > 1f)
            .DistinctUntilChanged()
            .Subscribe(_ => Debug.Log("success"))
            .AddTo(this);
        await UniTask.Delay(1000);
        transform.DOMove(Vector3.forward * 100f ,5f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
