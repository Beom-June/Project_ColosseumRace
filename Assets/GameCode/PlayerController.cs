using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float _moveSpeed = 5f;                //   이동 속도
    [SerializeField] private float _rotationSpeed = 5f;            //   회전 속도
    private float _horizontalAxis;                                  //  수평 입력 값
    private float _verticalAxis;                                    //  수직 입력 값
    private bool _isRun;                                            //  달리기 bool 값
    private Vector3 _moveVector;                                    //  moveVector 저장 
    private Animator _playerAnimator;                               //  Animator 저장
    private Rigidbody _playerRigidbody;                             //  Rigidbody 저장
    private NavMeshAgent _playerNavMeshAgent;                       // NavMeshAgent 저장

    [Header("Raycast Settings")]
    [SerializeField] private float _radius = 5f;
    [SerializeField] private LayerMask _targetComputer;                //  Computer 레이어 찾기용
    [SerializeField] private LayerMask _targetKnight;                //  Kinght 레이어 찾기용

    [Header("Camera Settings")]
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _eventCameraPoint;
    [SerializeField] private GameObject _returnCameraPoint;
    [SerializeField] private float _cameraMoveTime;
    private Transform _savePosition;

    [Header("Event Settings")]
    [SerializeField] private GameObject _eventInOut;                //  안에서 통로로 나갈때
    [SerializeField] private GameObject _eventOutInt;               //  통로에서 안으로 들어올 때
    [SerializeField] private float _changTime;
    [SerializeField] private bool _isChecking;                      //  체크용 bool 값

    [Header("Attack Action Settings")]
    [SerializeField] private float _attackDelayTime = 3.0f;      // 공격 딜레이 시간
    [SerializeField] private bool _isAttackDelay = false;        // 공격 딜레이 bool 값

    void Start()
    {
        _playerAnimator = GetComponent<Animator>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerNavMeshAgent = GetComponent<NavMeshAgent>();

    }

    void Update()
    {
        PlayerInput();
        PlayerMove();
        PlayerTurn();

        RaycastToComputer();
        RaycastToKnight();
    }

    // Input
    private void PlayerInput()
    {
        _horizontalAxis = Input.GetAxisRaw("Horizontal");
        _verticalAxis = Input.GetAxisRaw("Vertical");
        // 입력 값이 있는 경우에만 _isRun을 true로 설정
        _isRun = (_horizontalAxis != 0f || _verticalAxis != 0f);
    }
    // 플레이어 이동
    private void PlayerMove()
    {
        _moveVector = new Vector3(_horizontalAxis, 0, _verticalAxis).normalized;
        Vector3 moveDirection = _moveVector * _playerNavMeshAgent.speed;
        _playerNavMeshAgent.Move(moveDirection * Time.deltaTime);
        _playerAnimator.SetFloat("isRun", _isRun ? 1f : 0f);
    }

    // 플레이어 보는 방향
    private void PlayerTurn()
    {
        if (_moveVector != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    //  레이어를 Computer 한 것 찾기
    private void RaycastToComputer()
    {
        // 스피어 레이캐스트 발사
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius, _targetComputer);

        // 충돌한 객체들을 확인
        foreach (Collider hit in hits)
        {
            Debug.Log("타겟 들어옴");

            if (!_isAttackDelay)
            {
                // 타겟이 들어왔을 때 해당 애니메이션을 트리거
                _playerAnimator.SetTrigger("doAttack");

                // 시작하여 일정 시간이 지난 후 공격 딜레이를 리셋
                _isAttackDelay = true;
                StartCoroutine(ResetAttackDelay(_attackDelayTime));
            }
        }
    }
    //  레이어를 Knight 한 것 찾기
    private void RaycastToKnight()
    {
        // 스피어 레이캐스트 발사
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius, _targetKnight);

        // 충돌한 객체들을 확인
        foreach (Collider hit in hits)
        {

            if (!_isAttackDelay)
            {
                // 타겟이 들어왔을 때 해당 애니메이션을 트리거
                _playerAnimator.SetTrigger("doStandingJumpAttack");
            }
        }
    }

    // 레이 Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    //  공격 딜레이 코루틴
    private IEnumerator ResetAttackDelay(float time)
    {
        yield return new WaitForSeconds(time);
        _isAttackDelay = false;
    }

    // 카메라 움직임 코루틴
    IEnumerator MoveCameraSmoothly(Transform start, Transform end, float duration, GameObject targetObject)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector3 startPosition = start.position;
            Vector3 endPosition = end.position;

            Quaternion startRotation = start.rotation;
            Quaternion endRotation = end.rotation;

            elapsedTime += Time.deltaTime;
            // 계산된 위치와 회전값을 적용
            targetObject.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            targetObject.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);


            yield return null;
        }

        // 이동이 완료되었을 때 마지막 위치를 저장
        _savePosition = end;
    }
    private IEnumerator TriggerEvent01(float time)
    {
        _isChecking = true;
        yield return new WaitForSeconds(time);

        if (_isChecking)
        {
            _eventInOut.SetActive(false);
            _eventOutInt.SetActive(true);
        }

        _isChecking = false;
    }

    private IEnumerator TriggerEvent02(float time)
    {
        _isChecking = true;
        yield return new WaitForSeconds(time);

        if (_isChecking)
        {
            _eventInOut.SetActive(true);
            _eventOutInt.SetActive(false);
        }

        _isChecking = false;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("EventPoint01"))
        {
            StartCoroutine(TriggerEvent01(_changTime));
            StartCoroutine(MoveCameraSmoothly(_mainCamera.transform, _eventCameraPoint.transform, _cameraMoveTime, _mainCamera));
        }
        else if (collider.CompareTag("EventPoint02"))
        {
            StartCoroutine(TriggerEvent02(_changTime));
            StartCoroutine(MoveCameraSmoothly(_eventCameraPoint.transform, _returnCameraPoint.transform, _cameraMoveTime, _mainCamera));
        }

        if (collider.CompareTag("ReturnPoint"))
        {
            StartCoroutine(MoveCameraSmoothly(_eventCameraPoint.transform, _returnCameraPoint.transform, _cameraMoveTime, _mainCamera));
        }
    }
}