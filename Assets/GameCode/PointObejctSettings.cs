using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObejctSettings : MonoBehaviour
{
    [SerializeField] private GameObject _targetObject;          //  회전할 오브젝트
    [SerializeField] private float _rotSpeed = 60.0f;
    [SerializeField] private GameObject _particlePrefab;   //  파괴될 때 사용되는 파티클
    private ParticleSystem _destroyParticle;   // 생성된 파티클 시스템

    void Update()
    {

        // 현재 회전값 저장
        Quaternion currentRotation = _targetObject.transform.rotation;

        // 회전값 갱신
        Quaternion newRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y + (_rotSpeed * Time.deltaTime), currentRotation.eulerAngles.z);

        // 회전 적용
        _targetObject.transform.rotation = newRotation;
    }

    private void OnDestroy()
    {
        // 파티클 시스템을 독립적인 게임 오브젝트로 생성
        GameObject particleObject = Instantiate(_particlePrefab, transform.position, Quaternion.identity);

        // 파티클 시스템 컴포넌트 가져오기
        _destroyParticle = particleObject.GetComponent<ParticleSystem>();

        // 파티클 플레이
        _destroyParticle.Play();
    }
}
