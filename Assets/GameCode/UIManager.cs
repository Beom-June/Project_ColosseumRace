using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _textRedLevel;                     //  레드 레벨 텍스트
    [SerializeField] private Text _textBlueLevel;                   //  블루 레벨 텍스트
    [SerializeField] private List<Text> _textEnemyLevel;             //  적 레벨 텍스트

    private Camera _mainCamera;                                     // 메인 카메라
    private GameManager _gameManager;                               // 게임 매니저

    private void Start()
    {
        _mainCamera = Camera.main;
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void LateUpdate()
    {
        RotateUI();
    }

    // Text 컴포넌트가 항상 메인 카메라를 향하도록 회전 설정
    void RotateUI()
    {
        _textRedLevel.transform.rotation = _mainCamera.transform.rotation;
        _textBlueLevel.transform.rotation = _mainCamera.transform.rotation;

        for (int i = 0; i < _textEnemyLevel.Count; i++)
        {
            _textEnemyLevel[i].transform.rotation = _mainCamera.transform.rotation;
        }
    }

    // _textRedLevel 텍스트 업데이트
    public void UpdateRedLevelText(int _redLevel)
    {
        _textRedLevel.text = "Lv. " + (_redLevel * 10).ToString();
    }
    // _textBlueLevel 텍스트 업데이트
    public void UpdateBlueLevelText(int _blueLevel)
    {
        _textBlueLevel.text = "Lv. " + (_blueLevel * 10).ToString();
    }
}
