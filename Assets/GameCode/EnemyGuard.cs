using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyGuard : MonoBehaviour
{
    [SerializeField] private float _flyAwaySpeed = 10.0f;           //  날라가는 속도
    [SerializeField] private Vector3 _flyVector;                    //  날라가는 방향벡터
    [SerializeField] private int _guardLevel;                       //  해당 Enemy Level
    [SerializeField] private Text _txtguardLevel;                   //  해당 Enemy Level Text
    [SerializeField] private bool _isBoss;                         //  보스 이벤트용 bool 값
    private Animator _enemyAnimator;
    private NavMeshObstacle _navMeshObstacle;
    private Transform _playerTransform;

    private Rigidbody _enemyRigidbody;
    [SerializeField] GameManager _gameManager;

    #region Property
    public int guardLevel => _guardLevel;

    #endregion
    void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        _enemyRigidbody = GetComponent<Rigidbody>();
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Update()
    {
        if (_isBoss)
        {
            // 보스일 경우 플레이어를 계속 바라보도록 호출
            LookAtPlayer();
        }
    }

    // EnemyGuard의 레벨 텍스트를 업데이트함
    private void UpdateGuardLevelText()
    {
        _txtguardLevel.text = _guardLevel.ToString();
    }

    // EnemyGuard의 레벨 텍스트 색상을 변경함
    public void SetGuardLevelColor(Color color)
    {
        _txtguardLevel.color = color;
    }
    private void LookAtPlayer()
    {
        if (_playerTransform != null)
        {
            // 플레이어 방향으로 회전
            Vector3 playerDirection = _playerTransform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Zone") && _gameManager.redCount >= _guardLevel)
        {
            FlyAway();
        }
    }

    private void FlyAway()
    {
        Debug.Log("옴?");
        // NavMeshAgent를 비활성화하여 이동 중지
        _navMeshObstacle.enabled = false;

        // Rigidbody를 활성화하여 물리 효과 적용
        _enemyRigidbody.isKinematic = false;
        _enemyRigidbody.useGravity = true;

        // 날아가는 방향과 힘을 설정하여 날아가는 동작 구현
        Vector3 flyDirection = transform.up + _flyVector.normalized;
        _enemyRigidbody.AddForce(flyDirection * _flyAwaySpeed, ForceMode.Impulse);

    }
}
