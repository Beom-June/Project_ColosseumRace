using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float _moveSpeed = 20f;                //  �̵� �ӵ�
    [SerializeField] private float _rotationSpeed = 20f;            //  ȸ�� �ӵ�
    [SerializeField] private List<Transform> _wayPoints;            //  �̵��� ���� ����Ʈ
    [SerializeField] private ParticleSystem _bloodParticle;         //  적 감소시 나오는 피 파티클
    [SerializeField] private Vector3 _correctPos = new Vector3(0, 0, 0);

    private Vector3 _moveVector;                                    //  moveVector ���� 
    private Animator _enemyAnimator;                                //  Animator ����
    private Rigidbody _enemyRigidbody;                              //  Rigidbody ����
    private Transform _currentWayPoint;                             //  ���� �̵��� ���� ����Ʈ
    private List<Transform> _visitedWayPoints;

    [SerializeField] private EnemyReinforcementsZone _enemyZone;

    [Header("Attack Action Settings")]
    [SerializeField] private float _stopTime;                       //  피격후 대기시간;
    [SerializeField] private bool _isKnockedDown = false;  // 피격 상태 여부
    [SerializeField] private bool _isStopped = false;      // 이동 멈춤 여부
    void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        _enemyRigidbody = GetComponent<Rigidbody>();
        _visitedWayPoints = new List<Transform>();

        if (_wayPoints.Count > 0)
        {
            SetRandomWayPoint();
        }
    }

    void Update()
    {
        if (!_isKnockedDown && !_isStopped)
        {
            EnemyMove();
            EnemyTurn();
        }
    }

    // Enemy 이동 메소드
    private void EnemyMove()
    {
        if (_currentWayPoint != null)
        {
            _moveVector = (_currentWayPoint.position - transform.position).normalized;
            transform.position += _moveVector * _moveSpeed * Time.deltaTime;

            _enemyAnimator.SetFloat("isRun", 1f);
        }
        else
        {
            _enemyAnimator.SetFloat("isRun", 0f);
        }
    }

    //  Enmy 회전 메소드
    private void EnemyTurn()
    {
        if (_moveVector != Vector3.zero)
        {
            Quaternion _targetRotation = Quaternion.LookRotation(_moveVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void SetRandomWayPoint()
    {
        List<Transform> _availableWayPoints = new List<Transform>(_wayPoints);

        _availableWayPoints.RemoveAll(wp => _visitedWayPoints.Contains(wp));

        if (_availableWayPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, _availableWayPoints.Count);
            _currentWayPoint = _availableWayPoints[randomIndex];
        }
        else
        {
            //  비워주고 다시 랜덤 재생성
            _visitedWayPoints.Clear();
            int _randIndex = Random.Range(0, _wayPoints.Count);
            _currentWayPoint = _wayPoints[_randIndex];
        }
    }
    private void StopEnemyMovement()
    {
        _isKnockedDown = true;
        _isStopped = true;

        // 이후 _stopTime 이후에 이동 재개
        Invoke("ResumeEnemyMovement", _stopTime);
    }

    private void ResumeEnemyMovement()
    {
        _isKnockedDown = false;
        _isStopped = false;

        // 다음 웨이포인트 설정
        SetRandomWayPoint();
    }
    private void ControllParticle()
    {
        // 애들 위치에서 _bloodParticle 파티클 생성
        GameObject reinforcementToRemove = _enemyZone.spawnedReinforcements[_enemyZone.spawnedReinforcementsCount];
        Transform reinforcementTransform = reinforcementToRemove.transform;
        ParticleSystem bloodParticleInstance = Instantiate(_bloodParticle, reinforcementTransform.position + _correctPos, reinforcementTransform.rotation);

        // _bloodParticle 재생
        bloodParticleInstance.Play();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("WayPoint"))
        {
            _visitedWayPoints.Add(_currentWayPoint);
            SetRandomWayPoint();
        }

        if (collider.CompareTag("Zone"))
        {
            _enemyAnimator.SetTrigger("isKnockedDown");
            if (_enemyZone.spawnedReinforcementsCount > 0)
            {
                _enemyZone.spawnedReinforcementsCount--;
                ControllParticle();

                GameObject reinforcementToRemove = _enemyZone.spawnedReinforcements[_enemyZone.spawnedReinforcementsCount];
                _enemyZone.spawnedReinforcements.RemoveAt(_enemyZone.spawnedReinforcementsCount);
                Destroy(reinforcementToRemove);

            }
            StopEnemyMovement();
        }
    }
}
