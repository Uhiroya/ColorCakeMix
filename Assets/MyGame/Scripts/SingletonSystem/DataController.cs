using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; //ネットワーク用のネームスペース


public class DataController : MonoBehaviour
{
    [SerializeField] Text _viewText;
    [SerializeField] Button dataButton;　//追加
    [SerializeField] InputField nameField; //追加

    private const string URL = "https://docs.google.com/spreadsheets/d/1StuGiDa9z08HEDhna7L1oTaUmi1LGXtgf9mnA3RUhV8/gviz/tq?tqx=out:csv&sheet=test";
    private const string GasUrl = "https://script.google.com/macros/s/AKfycbw2P3Ia7tHq9dbtaDZhpqeoVGJw7bpFxTxj9lHGE1lWWBieShhU7KNRhUIN-z-8F0Fadw/exec";  //追加（最初は空っぽ）

    private List<SendData> _rankingList = new List<SendData>();
    private List<SendData> _sendDataList = new List<SendData>();
    async void Start()
    {
        await GetData(); //データ取得用のコルーチン
        
        dataButton.onClick.AddListener(()=> StartCoroutine(PostData()));
    }

    async UniTask GetData()
    {
        using (UnityWebRequest req = UnityWebRequest.Get(URL)) //UnityWebRequest型オブジェクト
        {
            _rankingList = new List<SendData>();
            await req.SendWebRequest(); //URLにリクエストを送る
            if (IsWebRequestSuccessful(req)) //成功した場合
            {
                var csvData = req.downloadHandler.text.Replace("\"", "");
                _viewText.text = csvData;
                var data = csvData.Split('\n');
                for (int i = 0; i < data.Length; i++)
                {
                    if (i != 0)
                    {
                        var sendData = data[i].Split(',');
                        _rankingList.Add(new SendData(sendData[0] , int.Parse(sendData[1])));
                    }
                }
                _rankingList = _rankingList.OrderByDescending(x => x.Score).ToList();
            }
            else //失敗した場合
            {
                Debug.Log("error");
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
        
        //値が空の場合は処理を中断
        if(string.IsNullOrEmpty(nameText))
        {
            Debug.Log("empty!");
            yield break;
        }
        
        //それぞれの値をカンマ区切りでcombinedText変数に代入
        string combinedText = string.Join(",", nameText, score);
        
        //formにPostする情報をvalというキー、値はcombinedTextで追加する
        form.AddField("val", combinedText);
        
        //UnityWebRequestを使ってGoogle Apps Script用URLにform情報をPost送信する
        using (UnityWebRequest req = UnityWebRequest.Post(GasUrl, form))
        {
            //情報を送信
            yield return req.SendWebRequest();
            
            //リクエストが成功したかどうかの判定
            Debug.Log(IsWebRequestSuccessful(req) ? "success" : "error");
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
    public int Score;

    public SendData(string playerName, int score)
    {
        PlayerName = playerName;
        Score = score;
    }
}
