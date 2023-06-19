using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트의 색깔 상태
public enum ColorState
{
    Red,
    Blue
}
public class ObjectColorChanger : MonoBehaviour
{
    [SerializeField] Material _matMine;                                     //  내 메테리얼
    [SerializeField] Material _matOpponent;                                 //  상대 메테리얼
    private Renderer _objectRenderer;
    private Dictionary<int, ColorState> _objectColorStates;                 // 오브젝트의 상태를 저장하는 딕셔너리

    private void Start()
    {
        _objectRenderer = GetComponent<Renderer>();
        // _objectColorStates = new Dictionary<int, ColorState>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            _objectRenderer.material = _matMine;
        }
        else if (collider.CompareTag("Computer"))
        {
            _objectRenderer.material = _matOpponent;
        }
    }
}
