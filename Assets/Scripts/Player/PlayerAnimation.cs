using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _renderer;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        _playerMovement.Idled += OnIdle;
        _playerMovement.Ran += OnRun;
        _playerMovement.Jumped += OnJump;
        _playerMovement.TurnedLeft += OnTurnLeft;
        _playerMovement.TurnedRight += OnTurnRight;
    }

    private void OnDisable()
    {
        _playerMovement.Idled -= OnIdle;
        _playerMovement.Ran -= OnRun;
        _playerMovement.Jumped -= OnJump;
        _playerMovement.TurnedLeft -= OnTurnLeft;
        _playerMovement.TurnedRight -= OnTurnRight;
    }

    private void OnIdle()
    {
        _animator.Play("Idle");
    }

    private void OnRun()
    {
        _animator.Play("Run");
    }

    private void OnJump()
    {
        _animator.Play("Jump");
    }

    private void OnTurnLeft()
    {
        _renderer.flipX = true;
    }

    private void OnTurnRight()
    {
        _renderer.flipX = false;
    }
}
