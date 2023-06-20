using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float _moveSpeed = 5f;                //   이동 속도
    [SerializeField] private float _rotationSpeed = 5f;            //   이동 속도
    private float _horizontalAxis;                                  //  수평 입력 값
    private float _verticalAxis;                                    //  수직 입력 값
    private bool _isRun;                                            //  달리기 bool 값
    private Vector3 _moveVector;                                    //  moveVector 저장 
    private Animator _playerAnimator;                               //  Animator 저장
    private Rigidbody _playerRigidbody;                             //  Rigidbody 저장
    [SerializeField] private GameObject _reinforcements;            //  지원군 프리팹

    [Header("Raycast Settings")]
    [SerializeField] private float _radius = 5f;
    [SerializeField] private LayerMask _targetLayer;


    void Start()
    {
        _playerAnimator = GetComponent<Animator>();
        _playerRigidbody = GetComponent<Rigidbody>();

    }

    void Update()
    {
        PlayerInput();
        PlayerMove();
        PlayerTurn();

        PlayerRaycast();
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
        transform.position += _moveVector * _moveSpeed * Time.deltaTime;
        _playerAnimator.SetFloat("isRun", _isRun ? 1f : 0f);
    }

    // 플레이어 방향
    private void PlayerTurn()
    {
        if (_moveVector != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void PlayerRaycast()
    {
        // 스피어 레이캐스트 발사
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius, _targetLayer);

        // 충돌한 객체들을 확인
        foreach (Collider hit in hits)
        {
            Debug.Log("타겟 들어옴");

            // 타겟이 들어왔을 때 해당 애니메이션을 트리거
            _playerAnimator.SetTrigger("doAttack");
        }
    }

    // 레이 Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    // // 지원군 생성 함수
    // private void SpawnReinforcements()
    // {
    //     int _reinforcementsCount = 1;                           //  생성할 지원군의 개수
    //     float _angleStep = 360f / (_reinforcementsCount * 5);          //  지원군이 생성될 각도 간격

    //     for (int i = 0; i < _reinforcementsCount; i++)
    //     {
    //         // 지원군을 생성할 각도 계산
    //         float _angle = i * _angleStep;
    //         Quaternion _rotation = Quaternion.Euler(0f, _angle, 0f);

    //         // 플레이어 주변에 생성할 위치 계산
    //         Vector3 _offset = _rotation * Vector3.forward * (_radius - 1.5f);      //  레이 원안 으로 넣으려고 보정
    //         Vector3 _spawnPosition = transform.position + _offset;

    //         // 지원군 생성 및 플레이어의 자식으로 설정
    //         GameObject reinforcements = Instantiate(_reinforcements, _spawnPosition, _rotation);
    //         reinforcements.transform.SetParent(transform);
    //     }
    // }

    // private void OnTriggerEnter(Collider collider)
    // {
    //     if (collider.CompareTag("Red"))
    //     {
    //         SpawnReinforcements();
    //         Destroy(collider.gameObject);
    //     }
    // }
}