using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float _moveSpeed = 20f;                //  �̵� �ӵ�
    [SerializeField] private float _rotationSpeed = 20f;            //  ȸ�� �ӵ�
    [SerializeField] private List<Transform> _wayPoints;            //  �̵��� ���� ����Ʈ
    private Vector3 _moveVector;                                    //  moveVector ���� 
    private Animator _enemyAnimator;                                //  Animator ����
    private Rigidbody _enemyRigidbody;                              //  Rigidbody ����
    private Transform _currentWayPoint;                             //  ���� �̵��� ���� ����Ʈ
    private List<Transform> _visitedWayPoints;

    void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        _enemyRigidbody = GetComponent<Rigidbody>();
        _visitedWayPoints = new List<Transform>();


        // ���� �� ������ ���� ����Ʈ�� ����
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

    // �� �̵� �Լ�
    private void EnemyMove()
    {
        if (_currentWayPoint != null)
        {
            _moveVector = (_currentWayPoint.position - transform.position).normalized;
            transform.position += _moveVector * _moveSpeed * Time.deltaTime;

            // ���� ������ �� "isRun" �Ķ���Ϳ� 1 ����
            _enemyAnimator.SetFloat("isRun", 1f);
        }
        else
        {
            // ���� ���� �� "isRun" �Ķ���Ϳ� 0 ����
            _enemyAnimator.SetFloat("isRun", 0f);
        }
    }

    // �� ���� ����
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

        // �̹� �湮�� ���� ����Ʈ ����
        _availableWayPoints.RemoveAll(wp => _visitedWayPoints.Contains(wp));

        if (_availableWayPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, _availableWayPoints.Count);
            _currentWayPoint = _availableWayPoints[randomIndex];
        }
        else
        {
            // ��� ���� ����Ʈ�� �湮�� ���, ��� ���� ����Ʈ�� ��湮�� �� �ֵ��� �ʱ�ȭ
            _visitedWayPoints.Clear();
            int _randIndex = Random.Range(0, _wayPoints.Count);
            _currentWayPoint = _wayPoints[_randIndex];
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("WayPoint"))
        {
            // ���� ���� ����Ʈ�� �湮�� ������ ���
            _visitedWayPoints.Add(_currentWayPoint);
            SetRandomWayPoint();
        }
    }
}
