using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트의 색깔 상태
public enum ColorState
{
    Red,
    Blue,
    None
}
public class ObjectColorChanger : MonoBehaviour
{
    [SerializeField] private ColorState _colorState = ColorState.None;
    [Header("Material Settings")]
    [SerializeField] private Material _matMine;                                     //  내 메테리얼
    [SerializeField] private Material _matOpponent;                                 //  상대 메테리얼
    private Renderer _objectRenderer;                                       //  해당 오브젝트 렌더러

    [Header("Particle Settings")]
    [SerializeField] private ParticleSystem _particleMine;                      //  내 파티클
    [SerializeField] private ParticleSystem _particleOpponent;                      //  상대 파티클

    private GameManager _gameManager;
    private void Start()
    {
        _objectRenderer = GetComponent<Renderer>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            _colorState = ColorState.Red;
            _objectRenderer.material = _matMine;    // 색 변경
            _particleMine.Play();

        }
        else if (collider.CompareTag("Computer"))
        {
            _colorState = ColorState.Blue;
            _objectRenderer.material = _matOpponent;    // 색 변경
            _particleOpponent.Play();

        }
    }
}
