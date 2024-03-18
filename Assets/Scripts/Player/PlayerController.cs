using System;
using Game;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action PlayerDied;

    [SerializeField] private float _jumpforce;
    [SerializeField] private int _maxJumpCount = 2;
    [SerializeField] private GameObject _playerDiedFxPrefab;
    [SerializeField] private AudioSource _moveAudio;
    [SerializeField] private AudioSource _landingAudio;

    private int _jumpCount;
    private bool _isGrounded;
    private Rigidbody2D _rigidbody2D;
    private EchoEffect _echoEffect;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _echoEffect = GetComponent<EchoEffect>();
        _jumpCount = 0;
    }

    private void Update()
    {
        var isJumpRequested = CheckJumpInput();

        if (CanJump() && isJumpRequested)
        {
            Jump();
            _echoEffect.CanShowEcho(true);
        }
    }

    public void DestroyPlayer()
    {
        Instantiate(_playerDiedFxPrefab, transform.position, Quaternion.identity);
        PlayerDied?.Invoke();

        Destroy(gameObject);
    }
    
    private void Jump()
    {
        _moveAudio.Play();
        _rigidbody2D.velocity = Vector2.up * _jumpforce * _rigidbody2D.gravityScale;
        _jumpCount--;
    }

    private bool CheckJumpInput()
    {
        var isSpaceButton = Input.GetKeyDown(KeyCode.Space);
        var isTouchInput = Input.touches.Length > 0 && Input.GetTouch(0).phase == TouchPhase.Began;

        return isSpaceButton || isTouchInput;
    }

    private bool CanJump() => _jumpCount > 0;

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.collider.CompareTag(GlobalConstants.FLOOR_TAG) && !_isGrounded)
        {
            _landingAudio.Play();
            _jumpCount = _maxJumpCount;
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision2D)
    {
        if (collision2D.collider.CompareTag(GlobalConstants.FLOOR_TAG))
        {
            _isGrounded = false;
        }
    }
}