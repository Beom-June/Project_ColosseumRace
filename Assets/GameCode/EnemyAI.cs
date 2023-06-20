using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float _moveSpeed = 20f;                //  이동 속도
    [SerializeField] private float _rotationSpeed = 20f;            //  회전 속도
    [SerializeField] private List<Transform> _wayPoints;            //  이동할 웨이 포인트
    private Vector3 _moveVector;                                    //  moveVector 저장 
    private Animator _enemyAnimator;                                //  Animator 저장
    private Rigidbody _enemyRigidbody;                              //  Rigidbody 저장
    private Transform _currentWayPoint;                             //  현재 이동할 웨이 포인트
    private List<Transform> _visitedWayPoints;

    void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        _enemyRigidbody = GetComponent<Rigidbody>();
        _visitedWayPoints = new List<Transform>();


        // 시작 시 랜덤한 웨이 포인트를 선택
        if (_wayPoints.Count > 0)
        {
            SetRandomWayPoint();
        }
    }

    void Update()
    {
        EnemyMove();
        EnemyTurn();
    }

    // 적 이동 함수
    private void EnemyMove()
    {
        if (_currentWayPoint != null)
        {
            _moveVector = (_currentWayPoint.position - transform.position).normalized;
            transform.position += _moveVector * _moveSpeed * Time.deltaTime;

            // 적이 움직일 때 "isRun" 파라미터에 1 설정
            _enemyAnimator.SetFloat("isRun", 1f);
        }
        else
        {
            // 적이 멈출 때 "isRun" 파라미터에 0 설정
            _enemyAnimator.SetFloat("isRun", 0f);
        }
    }

    // 적 보는 방향
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

        // 이미 방문한 웨이 포인트 제외
        _availableWayPoints.RemoveAll(wp => _visitedWayPoints.Contains(wp));

        if (_availableWayPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, _availableWayPoints.Count);
            _currentWayPoint = _availableWayPoints[randomIndex];
        }
        else
        {
            // 모든 웨이 포인트를 방문한 경우, 모든 웨이 포인트를 재방문할 수 있도록 초기화
            _visitedWayPoints.Clear();
            int _randIndex = Random.Range(0, _wayPoints.Count);
            _currentWayPoint = _wayPoints[_randIndex];
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("WayPoint"))
        {
            // 현재 웨이 포인트를 방문한 것으로 기록
            _visitedWayPoints.Add(_currentWayPoint);
            SetRandomWayPoint();
        }
    }
}
