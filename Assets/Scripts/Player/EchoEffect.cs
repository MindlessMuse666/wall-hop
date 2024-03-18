using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    private const int ECHO_COUNT = 50;

    [SerializeField] private Transform _echoParentRoot;
    [SerializeField] private GameObject _echoPrefab;
    [SerializeField] private float _spawnDelay = 0.1f;
    [SerializeField] private float _shrinkDuration = 0.8f;

    private readonly Queue<GameObject> _echoObjectsPool = new();
    private float _nextEchoSpawnTime;
    private bool _isEchoEnabled;
    private Vector3 _initialEchoScale;

    private void Awake()
    {
        _initialEchoScale = _echoPrefab.transform.localScale;
        _isEchoEnabled = true;
        SpawnInitialEcho();
    }

    private void Update()
    {
        HandleEchoSpawn();
    }

    public void CanShowEcho(bool isVisible)
    {
        _isEchoEnabled = isVisible;
    }

    private void HandleEchoSpawn()
    {
        if (_isEchoEnabled)
        {
            ShowEcho();
        }
    }

    private void ShowEcho()
    {
        _nextEchoSpawnTime -= Time.deltaTime;

        if (_nextEchoSpawnTime <= 0 && !IsPoolEmpty())
        {
            var echo = _echoObjectsPool.Dequeue();
            InitializeEcho(echo);
            AnimateEcho(echo);
        }
    }

    private bool IsPoolEmpty()
    {
        return _echoObjectsPool.Count == 0;
    }

    private void InitializeEcho(GameObject echo)
    {
        echo.transform.position = transform.position;
        echo.SetActive(true);
        _nextEchoSpawnTime = _spawnDelay;
    }

    private void AnimateEcho(GameObject echo)
    {
        echo.transform.localScale = _initialEchoScale;
        echo.transform.DOScale(Vector3.zero, _shrinkDuration).OnComplete(() => 
        {
            echo.SetActive(false);
            _echoObjectsPool.Enqueue(echo);
        });
    }

    private void SpawnInitialEcho()
    {
        for (var i = 0; i < ECHO_COUNT; i++)
        {
            var echo = Instantiate(_echoPrefab, _echoParentRoot);
            echo.SetActive(false);
            _echoObjectsPool.Enqueue(echo);
        }
    }
}
