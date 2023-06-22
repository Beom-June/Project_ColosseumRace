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

    [Header("Bonus Arrow")]
    [SerializeField] private List<RawImage> _bonusArrow;                                //  화살표 ui
    [SerializeField] private float _movementDistance = 100f;  // 이동 거리
    [SerializeField] private float _movementSpeed = 2f;      // 이동 속도
    private bool _isMovingRight = true;  // 현재 이동 방향

    [Header("UI Settings")]
    [SerializeField] private List<Text> _noThanks;
    private bool _increasingAlpha = true;
    private float _alpha = 0f;
    private float _alphaStep = 1f;


    private void Start()
    {
        _mainCamera = Camera.main;
        _gameManager = FindObjectOfType<GameManager>();

        StartCoroutine(MoveArrow());
    }

    private void Update()
    {
        TextAlphaEffect();
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

    private void TextAlphaEffect()
    {
        // 알파값 증감 로직
        if (_increasingAlpha)
        {
            _alpha += _alphaStep;
            if (_alpha >= 255f)
            {
                _alpha = 255f;
                _increasingAlpha = false;
            }
        }
        else
        {
            _alpha -= _alphaStep;
            if (_alpha <= 0f)
            {
                _alpha = 0f;
                _increasingAlpha = true;
            }
        }
        // 텍스트 알파값 적용
        for (int i = 0; i < _noThanks.Count; i++)
        {
            Color textColor = _noThanks[i].color;
            textColor.a = _alpha / 255f;
            _noThanks[i].color = textColor;
        }
    }

    // 화살표 UI 코루틴
    private IEnumerator MoveArrow()
    {
        while (true)
        {
            // 현재 위치와 목표 위치 계산
            float startX = _isMovingRight ? -_movementDistance : _movementDistance;
            float targetX = _isMovingRight ? _movementDistance : -_movementDistance;

            // 이동 애니메이션
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * _movementSpeed;
                float newX = Mathf.Lerp(startX, targetX, t);
                for (int i = 0; i < _bonusArrow.Count; i++)
                {
                    Vector3 newPosition = _bonusArrow[i].rectTransform.localPosition;
                    newPosition.x = newX;
                    _bonusArrow[i].rectTransform.localPosition = newPosition;
                }
                yield return null;
            }

            // 이동 방향 변경
            _isMovingRight = !_isMovingRight;
        }
    }
}
