using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; //ネットワーク用のネームスペース


public class RankingDataController : MonoBehaviour
{
    [SerializeField] private Transform _rankingParent;
    [SerializeField] private GameObject _rankingTextPrefab;
    [SerializeField] Button dataButton;　//追加
    [SerializeField] InputField nameField; //追加

    private const string URL = "https://docs.google.com/spreadsheets/d/1StuGiDa9z08HEDhna7L1oTaUmi1LGXtgf9mnA3RUhV8/gviz/tq?tqx=out:csv&sheet=test";
    private const string GasUrl = "https://script.google.com/macros/s/AKfycbw2P3Ia7tHq9dbtaDZhpqeoVGJw7bpFxTxj9lHGE1lWWBieShhU7KNRhUIN-z-8F0Fadw/exec";  //追加（最初は空っぽ）

    private List<SendData> _rankingList;

    #region Unity公開箇所
    public void ResultEnter()
    {
        dataButton.onClick.AddListener(RegisterLister);
    }
    public void ResultExit()
    {
        dataButton.onClick.RemoveListener(RegisterLister);
    }
    void RegisterLister()
    {
        StartCoroutine(PostData());
    }
    public void MakeRanking()
    {
        _ = GetData();
    }
    #endregion

    public async UniTask GetData()
    {
        using (UnityWebRequest req = UnityWebRequest.Get(URL)) //UnityWebRequest型オブジェクト
        {
            _rankingList = new List<SendData>();
            await req.SendWebRequest(); //URLにリクエストを送る
            if (IsWebRequestSuccessful(req)) //成功した場合
            {
                var csvData = req.downloadHandler.text.Replace("\"", "");
                var data = csvData.Split('\n');
                for (int i = 0; i < data.Length; i++)
                {
                    if (i != 0)
                    {
                        var sData = data[i].Split(',');
                        _rankingList.Add(new SendData( sData[0] , sData[1] , sData[2] ));
                    }
                }
                _rankingList = _rankingList.OrderByDescending(x => int.Parse(x.Score)).ToList();
                //順位挿入用
                int rank = 1;
                foreach (var sData in _rankingList)
                {
                    RankingDataTextsControlls _rankText = Instantiate(_rankingTextPrefab , _rankingParent).GetComponent<RankingDataTextsControlls>();
                    _rankText.SetTexts(rank.ToString(), sData.PlayerName , sData.Score , sData.Date);
                    rank++;
                }
            }
            else //失敗した場合
            {
                Debug.Log("データ獲得に失敗しました");
            }
        }
    }
    

    /*略*/
   
    //送信ボタンをクリックされたときの処理
    IEnumerator PostData()
    {
        //WWWForm型のインスタンスを生成
        WWWForm form = new WWWForm();
        
        //それぞれのInputFieldから情報を取得
        string nameText = nameField.text;
        int score = 1000;
        string sendTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        //値が空の場合は処理を中断
        if(string.IsNullOrEmpty(nameText))
        {
            Debug.Log("empty!");
            yield break;
        }
        
        //それぞれの値をカンマ区切りでcombinedText変数に代入
        string combinedText = string.Join(",", nameText, score , sendTime);
        
        //formにPostする情報をvalというキー、値はcombinedTextで追加する
        form.AddField("val", combinedText);
        
        //UnityWebRequestを使ってGoogle Apps Script用URLにform情報をPost送信する
        using (UnityWebRequest req = UnityWebRequest.Post(GasUrl, form))
        {
            //情報を送信
            yield return req.SendWebRequest();
            
            //リクエストが成功したかどうかの判定
            Debug.Log(IsWebRequestSuccessful(req) ? "データ登録に成功しました" : "データ登録に失敗しました");
        }
    }
    //リクエストが成功したかどうか判定する関数
    bool IsWebRequestSuccessful(UnityWebRequest req)
    {
        /*プロトコルエラーとコネクトエラーではない場合はtrueを返す*/
        if (req.result == UnityWebRequest.Result.ProtocolError)
        {
            print("protocol error");
            return false;
        }
        if(req.result == UnityWebRequest.Result.ConnectionError)
        {
            print("Connection error");
            return false;
        }
        return true;
    }
}

[System.Serializable]
public class SendData
{
    public string PlayerName;
    public string Score;
    //TODO 日時比較も行うならDateTimeを利用する
    public string Date;

    public SendData(string playerName, string score, string date)
    {
        PlayerName = playerName;
        Score = score;
        Date = date;
    }
}
