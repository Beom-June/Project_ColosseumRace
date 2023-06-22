using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WallController : MonoBehaviour
{
    private NavMeshObstacle _navMeshObstacle;
    [SerializeField] private bool _playerWall;
    [SerializeField] private bool _computerWall;
    void Start()
    {
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && _playerWall)
        {
            _navMeshObstacle.enabled = false;
        }
        if (collider.CompareTag("Computer") && _computerWall)
        {
            _navMeshObstacle.enabled = false;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player") && _playerWall)
        {
            _navMeshObstacle.enabled = true;
        }
        if (collider.CompareTag("Computer") && _computerWall)
        {
            _navMeshObstacle.enabled = true;
        }
    }
}
