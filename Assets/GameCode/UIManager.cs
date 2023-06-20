using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _textRedLevel;                     //  레드 레벨
    [SerializeField] private Text _textBlueLevel;                   //  블루 레벨

    private Camera _mainCamera;                          // 메인 카메라
    private GameManager _gameManager;                    // 게임 매니저

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
        //_textBlueLevel.transform.rotation = _mainCamera.transform.rotation;
    }

        // _textRedLevel 텍스트 업데이트
    public void UpdateRedLevelText(int redLevel)
    {
        _textRedLevel.text = "Lv. " + (redLevel * 10).ToString();
    }
}
