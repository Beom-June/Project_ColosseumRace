using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyGuard : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] private float _flyAwaySpeed = 10.0f;           //  날라가는 속도
    [SerializeField] private Vector3 _flyVector;                    //  날라가는 방향벡터
    [SerializeField] private int _guardLevel;                       //  해당 Enemy Level
    [SerializeField] private Text _txtguardLevel;                   //  해당 Enemy Level Text

    [Header("Boss Setings")]
    [SerializeField] private bool _isBoss;                         //  보스 이벤트용 bool 값
    [SerializeField] private float _radius = 5f;
    [SerializeField] private LayerMask _targetPlayer;
    [SerializeField] private float _attackDelayTime = 3.0f;      // 공격 딜레이 시간
    [SerializeField] private bool _isAttackDelay = false;        // 공격 딜레이 bool 값
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private float _jumpAttackCheckInterval = 1f;   // JumpAttack 체크 간격 설정 (1초마다 체크)
    [SerializeField] private GameObject _endUI;
    private bool _canCheckJumpAttack = true;   // JumpAttack 체크 가능한 상태인지 나타내는 변수


    private Animator _enemyAnimator;
    private NavMeshObstacle _navMeshObstacle;
    private Transform _playerTransform;

    private Rigidbody _enemyRigidbody;

    #region Property
    public int guardLevel => _guardLevel;

    #endregion
    void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        _enemyRigidbody = GetComponent<Rigidbody>();
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // 플레이어의 Animator 컴포넌트 가져오기
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            _playerAnimator = _player.GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (_isBoss)
        {
            LookAtPlayer();
            RaycastToPlayer();

            // JumpAttack 체크 간격 타이머
            if (_canCheckJumpAttack)
            {
                StartCoroutine(CheckJumpAttack());
            }
        }
        UpdateGuardLevelText();
    }

    // EnemyGuard의 레벨 텍스트를 업데이트함
    private void UpdateGuardLevelText()
    {
        _txtguardLevel.text = "Lv. " + (_guardLevel * 10).ToString();
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

    private void RaycastToPlayer()
    {
        // 스피어 레이캐스트 발사
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius, _targetPlayer);

        // 충돌한 객체들을 확인
        foreach (Collider hit in hits)
        {
            Debug.Log("플레이어 들어옴");

            if (!_isAttackDelay)
            {
                // 타겟이 들어왔을 때 해당 애니메이션을 트리거
                _enemyAnimator.SetTrigger("doAttack");

                // 시작하여 일정 시간이 지난 후 공격 딜레이를 리셋
                _isAttackDelay = true;
                StartCoroutine(ResetAttackDelay(_attackDelayTime));
            }
        }
    }
    private void FlyAway()
    {
        // NavMeshAgent를 비활성화하여 이동 중지
        _navMeshObstacle.enabled = false;

        // Rigidbody를 활성화하여 물리 효과 적용
        _enemyRigidbody.isKinematic = false;
        _enemyRigidbody.useGravity = true;

        // 날아가는 방향과 힘을 설정하여 날아가는 동작 구현
        Vector3 flyDirection = transform.up + _flyVector.normalized;
        _enemyRigidbody.AddForce(flyDirection * _flyAwaySpeed, ForceMode.Impulse);

    }
    //  공격 딜레이 코루틴
    private IEnumerator ResetAttackDelay(float time)
    {
        yield return new WaitForSeconds(time);
        _isAttackDelay = false;
    }
    // JumpAttack 체크 간격 타이머 코루틴
    private IEnumerator CheckJumpAttack()
    {
        _canCheckJumpAttack = false;

        yield return new WaitForSeconds(_jumpAttackCheckInterval);

        // JumpAttack 체크
        bool _doJumpAttack = _playerAnimator.GetBool("doStandingJumpAttack");

        // JumpAttack이면 _guardLevel 감소 및 날아가기 동작 처리 (플레이어 레벨이 더 높아야 작동하게 함)
        if (_doJumpAttack && _gameManager.redCount >= _guardLevel)
        {
                _guardLevel--;

            // _guardLevel이 0이 되면 날아가기 동작 수행
            if (_guardLevel <= 0)
            {
                FlyAway();
                _endUI.SetActive(true);
                _playerAnimator.SetTrigger("doVictory");
            }
        }
        _canCheckJumpAttack = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Zone") && _gameManager.redCount >= _guardLevel)
        {
            // 보스가 아니면 바로 날려버림
            if (!_isBoss)
            {
                FlyAway();
            }
        }
    }
}
