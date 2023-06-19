using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameManager : MonoBehaviour
{

    [Header("Point Check")]
    [SerializeField] private int _pointRed;                             //  레드 포인트 저장용 변수
    [SerializeField] private int _pointBlue;                            //  블루 포인트 저장용 변수
    private List<ColorState> _currentColorStates = new List<ColorState>(); // 현재 씬에 있는 오브젝트의 색깔 상태를 저장하는 리스트
    private void Update()
    {
        // 빨간색과 파란색의 개수를 _currentColorStates에서 계산합니다.
        int redCount = _currentColorStates.FindAll(colorState => colorState == ColorState.Red).Count;
        int blueCount = _currentColorStates.FindAll(colorState => colorState == ColorState.Blue).Count;

        _pointRed = redCount;
        _pointBlue = blueCount;
    }

    // Add this method to update the _currentColorStates list
    public void UpdateColorState(ColorState colorState)
    {
        _currentColorStates.Add(colorState);
    }
}
