using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : SingletonMonoBehavior<InGameManager>
{
    [SerializeField] private float _timeLimit;
    [SerializeField] private Text _timerText;

    private bool _isInGame;
    private float _inGameTimer;
    private int _currentTotalScore;
    private Color _currentColor;
    private List<CakeData> _finishedCakeDatas = new List<CakeData>();
    public List<CakeData> FinishedCakeDatas => _finishedCakeDatas;
    
    /// <summary> ゲーム開始時の初期化 </summary>
    public void InitializeToStart()
    {
        _isInGame = true;
        _currentTotalScore = 0;
        _inGameTimer = _timeLimit;
        _finishedCakeDatas.Clear();
    }

    private void Update()
    {
        if (!_isInGame) return;
        
        _inGameTimer -= Time.deltaTime;
        if (_timerText) _timerText.text = ((int)_inGameTimer).ToString();

        if (_inGameTimer <= 0)
        {
            _isInGame = false;
            _inGameTimer = 0;
            GameStateMachine.Instance.ChangeNextState(GamePhase.EndGame);
            Debug.Log("time over");
        }
    }

    public void AddCookedScore(int score)
    {
        _currentTotalScore += score;
        Debug.Log("add cooked score");
    }

    public void AddColorScore(int score, Color cakeColor)
    {
        _currentTotalScore = score;
        _currentColor = cakeColor;
        Debug.Log("add color score");
    }

    /// <summary> judge時にケーキデータを確定、保存 </summary>
    public CakeData SaveFinishedCake()
    {
        _finishedCakeDatas.Add(new CakeData(_currentColor, _currentTotalScore));
        return _finishedCakeDatas[_finishedCakeDatas.Count - 1];
    }

    public class CakeData
    {
        public Color Color;
        public int Price;

        public CakeData(Color color, int price)
        {
            this.Color = color;
            Price = price;
        }
    }
}
