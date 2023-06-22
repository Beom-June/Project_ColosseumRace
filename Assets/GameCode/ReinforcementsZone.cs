using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforcementsZone : MonoBehaviour
{
    [Header("Reinforcements Settings")]
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _reinforcements;                             //  지원군 프리팹
    [SerializeField] private int _spawnedReinforcementsCount;                        // 생성된 지원군 개수
    [SerializeField] private List<GameObject> _spawnedReinforcements;                // 생성된 애들을 저장할 리스트             
    [SerializeField] private ParticleSystem _bloodParticle;
    [SerializeField] private Vector3 _particleCorrectPos;                                    // 파티클 위치 보정값

    [Header("Camp Settings")]
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _angleStep = 72f;                //  각도 증가량

    [Header("Boss Event Settings")]
    [SerializeField] private Animator _enemyBoss;
    [SerializeField] private float _AttackCheckInterval;
    private bool _canCheckAttack;

    public int spawnedReinforcementsCount
    {
        get { return _spawnedReinforcementsCount; }
        set { value = _spawnedReinforcementsCount; }
    }
    void Update()
    {
        if (_target != null)
        {
            // _target의 위치를 따라가도록 해당 오브젝트의 위치를 업데이트
            transform.position = _target.transform.position;
        }

        StartCoroutine(CheckBossAttack());
    }
    private void ControllParticle()
    {
        // 애들 위치에서 _bloodParticle 파티클 생성
        GameObject reinforcementToRemove = _spawnedReinforcements[spawnedReinforcementsCount];
        Transform reinforcementTransform = reinforcementToRemove.transform;
        ParticleSystem bloodParticleInstance = Instantiate(_bloodParticle, reinforcementTransform.position + _particleCorrectPos, reinforcementTransform.rotation);

        // _bloodParticle 재생
        bloodParticleInstance.Play();
    }
    private void SpawnReinforcements()
    {
        // 시계 방향으로 생성될 각도 계산
        float _angle = _angleStep;
        Quaternion _rotation = Quaternion.Euler(0f, _angle, 0f);

        // 생성된 지원군 개수 증가
        _spawnedReinforcementsCount++;

        // 플레이어 주변에 생성할 위치 계산
        Vector3 _offset = Quaternion.Euler(0f, _angle, 0f) * (Vector3.forward * _radius);
        Vector3 _spawnPosition = transform.position + _offset;

        // 지원군 생성 및 플레이어의 자식으로 설정
        GameObject reinforcements = Instantiate(_reinforcements, _spawnPosition, _rotation);
        reinforcements.transform.SetParent(transform);

        _spawnedReinforcements.Add(reinforcements); // 생성된 애들을 리스트에 추가

        // _angle 값을 증가 (위치 보정하기 위해서)
        _angleStep += _angleStep;
    }

    //  보스 공격 체크 코루틴
    private IEnumerator CheckBossAttack()
    {
        _canCheckAttack = false;
        yield return new WaitForSeconds(_AttackCheckInterval);

        bool _doBossAttack = _enemyBoss.GetBool("doAttack");

        if (_doBossAttack)
        {
            _spawnedReinforcementsCount--;
            ControllParticle();

            GameObject reinforcementToRemove = _spawnedReinforcements[_spawnedReinforcementsCount];
            _spawnedReinforcements.RemoveAt(_spawnedReinforcementsCount);
            Destroy(reinforcementToRemove);

            if (_spawnedReinforcementsCount == 0)
            {
                Debug.Log("Game Over");
            }
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Red"))
        {
            SpawnReinforcements();
            Destroy(collider.gameObject);
        }
        if (collider.CompareTag("Knight"))
        {
            if (_spawnedReinforcementsCount > 0)
            {
                _spawnedReinforcementsCount--;
                GameObject reinforcementToRemove = _spawnedReinforcements[_spawnedReinforcementsCount];
                _spawnedReinforcements.RemoveAt(_spawnedReinforcementsCount);
                Destroy(reinforcementToRemove);
            }
        }
    }
}
