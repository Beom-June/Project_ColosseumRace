using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObejctSettings : MonoBehaviour
{
    [SerializeField] private GameObject _targetObject;
    [SerializeField] private float _rotSpeed = 60.0f;                   //  오브젝트 회전속도
    [SerializeField] private ParticleSystem _particlePrefab;                //  파티클 프리팹
    private ParticleSystem _destroyParticle;                            //  파티클 삭제하기 위함


    private void Start()
    {
    }

    void Update()
    {
        RotateObject();
    }

    private void RotateObject()
    {
        Quaternion _currentRotation = _targetObject.transform.rotation;
        Quaternion _newRotation = Quaternion.Euler(_currentRotation.eulerAngles.x, _currentRotation.eulerAngles.y + (_rotSpeed * Time.deltaTime), _currentRotation.eulerAngles.z);
        _targetObject.transform.rotation = _newRotation;
    }

    private void OnDestroy()
    {
        PlayAndDestroyParticle();
    }

    private void PlayAndDestroyParticle()
    {
        _destroyParticle = Instantiate(_particlePrefab, transform.position, Quaternion.identity);
        _destroyParticle.Play();

        Destroy(_destroyParticle.gameObject, _destroyParticle.main.duration);
    }
}
