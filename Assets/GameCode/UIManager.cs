using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _getPointRed;                     //  레드 포인트 텍스트
    [SerializeField] private Text _getPointBlue;                    //  블루 포인트 텍스트
    private static UIManager _instance;                             //  인스턴스 선언
    public static UIManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
    }

    // 포인트 텍스트 업데이트 메서드
    public void UpdatePointText(int pointRed, int pointBlue)
    {
        Debug.Log("UI");
        _getPointRed.text = pointRed.ToString();
        _getPointBlue.text = pointBlue.ToString();
    }
}
