using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _attackRange;
    [SerializeField] private int _damage;
    [field: SerializeField] public int _currentHP { get; set; }
    [field: SerializeField] public int _maxHP { get; set; }

    private bool _isAttacking;
    private Rigidbody _rigidbody;
    private Animator _animator;
    
    private static readonly int MovingForward = Animator.StringToHash("Moving Forward");
    private static readonly int MovingBackward = Animator.StringToHash("Moving Back");
    private static readonly int MovingLeft = Animator.StringToHash("Moving Left");
    private static readonly int MovingRight = Animator.StringToHash("Moving Right");
    private static readonly int JumpAnimation = Animator.StringToHash("Jump");
    private static readonly int Die = Animator.StringToHash("Death");
    private static readonly int PlayerAttack = Animator.StringToHash("Attack");

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetButtonDown("Fire1") && !_isAttacking)
        {
            Attack();
        }

        if (!_isAttacking)
        {
            UpdateAnimator();
        }
    }

    private void Jump()
    {
        if (CanJump())
        {
            _animator.SetTrigger(JumpAnimation);
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    private bool CanJump()
    {
        var ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 0.1f))
        {
            return hit.collider != null;
        }

        return false;
    }

    private void Move()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var direction = transform.right * x + transform.forward * z;
        direction *= _movementSpeed;
        direction.y = _rigidbody.velocity.y;
        
        _rigidbody.velocity = direction;
    }

    public void TakeDamage(int damageTaken)
    {
        _currentHP -= damageTaken;
        
        HealthBarUI.Instance.UpdateFill(_currentHP, _maxHP);
        
        if (_currentHP <= 0)
        {
            _animator.SetBool(Die, true);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Attack()
    {
        _isAttacking = true;

        _animator.SetTrigger(PlayerAttack);
        
        Invoke(nameof(TryDamage), 0.7f);
        Invoke(nameof(DisableAttacking), 1.5f);
    }

    private void TryDamage()
    {
        var ray = new Ray(transform.position + transform.forward, transform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, _attackRange, _attackRange, 1 << 8);
        
        foreach (var enemy in hits)
        {
            enemy.collider.GetComponent<Enemy>().TakeDamage(_damage);
        }
    }

    private void DisableAttacking() => _isAttacking = false;

    private void UpdateAnimator()
    {
        _animator.SetBool(MovingForward, false);
        _animator.SetBool(MovingBackward, false);
        _animator.SetBool(MovingLeft, false);
        _animator.SetBool(MovingRight, false);

        var localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);

        if (localVelocity.z > 0.1f)
        {
            _animator.SetBool(MovingForward, true);
        }
        else if (localVelocity.z < -0.1f)
        {
            _animator.SetBool(MovingBackward, true);
        }
        else if (localVelocity.x > 0.1f)
        {
            _animator.SetBool(MovingRight, true);
        }
        else if (localVelocity.x < -0.1f)
        {
            _animator.SetBool(MovingLeft, true);
        }
    }

}
