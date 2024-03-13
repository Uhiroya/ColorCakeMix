using System.Threading;
using UnityEngine;

/// <summary>
/// ゲームステートのベースクラス
/// </summary>
public abstract class GameStateBase : MonoBehaviour
{
    /// <summary>
    /// 初期化処理、開始処理
    /// </summary>
    /// <param name="ct"></param>
    public abstract void OnEnter(CancellationToken ct);

    /// <summary>
    /// マニュアルアップデート　※イベント関数のUpdateは利用しないように。
    /// </summary>
    public abstract void OnUpdate(float deltaTime);

    /// <summary>
    /// 終了処理
    /// </summary>
    public abstract void OnExit();

}
