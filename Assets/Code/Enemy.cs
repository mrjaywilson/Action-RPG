using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    [SerializeField] private Player _player;
    [SerializeField] private float _attackDistance;
    [SerializeField] private int _damage;
    [SerializeField] private int _health;

    private bool _isAttacking;
    private bool _isDead;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Attacking = Animator.StringToHash("Attack");
    private static readonly int Run = Animator.StringToHash("Run");

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        if (_isDead)
        {
            return;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) <= _attackDistance)
        {
            _navMeshAgent.isStopped = true;

            if (!_isAttacking)
            {
                Attack();
            }
        }
        else
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_player.transform.position);
            _animator.SetBool(Run, true);
        }
    }

    private void Attack()
    {
        _isAttacking = true;
        
        _animator.SetBool(Run, false);
        _animator.SetTrigger(Attacking);

        Invoke(nameof(TryDamage), 1.3f);
        Invoke(nameof(DisableIsAttacking), 2.66f);
    }

    private void TryDamage()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) <= _attackDistance)
        {
            _player.TakeDamage(_damage);
        }
    }

    private void DisableIsAttacking() => _isAttacking = false;

    public void TakeDamage(int damageTaken)
    {
        _health -= damageTaken;

        if (_health <= 0)
        {
            _isDead = true;
            _navMeshAgent.isStopped = true;
            
            _animator.SetTrigger(Die);

            GetComponent<Collider>().enabled = false;
        }
    }
}
