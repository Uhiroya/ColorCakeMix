using System.Threading;
using UnityEngine;

/// <summary>
/// ゲームステートのベースクラス
/// </summary>
public abstract class GameStateBase : MonoBehaviour
{
    /// <summary>
    /// 開始処理
    /// </summary>
    /// <param name="ct">
    /// ステートが強制的に切り替わった際にキャンセルする処理、
    /// 時間制限等で強引に止める時等
    /// </param>
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
