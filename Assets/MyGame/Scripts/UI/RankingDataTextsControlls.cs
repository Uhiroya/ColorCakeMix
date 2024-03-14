using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingDataTextsControlls : MonoBehaviour
{
    [SerializeField] private Text _rank;
    [SerializeField] private Text _name;
    [SerializeField] private Text _score;
    [SerializeField] private Text _date;


    public void SetTexts(string rank, string name, string score, string date)
    {
        _rank.text = rank;
        _name.text = name;
        _score.text = score;
        _date.text = date;
    }
}
