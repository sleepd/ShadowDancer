using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
public class PlayerController : MonoBehaviour
{
    public int hp {get => _currentHp;}
    [SerializeField] private int _maxHp;
    private int _currentHp;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public int maxJumpCount = 2;
    public LayerMask groundLayer;
    public VisualEffect landingEffect;
    public float maxFallSpeed = -20f;
    public int faceing = 1;
    public Vector2 velocity { get => _velocity;}

    [HideInInspector] public bool onGround {get => _onGround;}
    [SerializeField] float footCheckDistance;
    // sound effects
    [SerializeField]
    AudioSource sfx;
    [SerializeField]
    AudioClip sfx_landing;

    private Rigidbody2D _rb;
    private GameObject _sprite;
    private Animator _animator;
    
    
    // temporary velocity per frame
    private Vector2 _velocity;
    // use to check if the player is starting to fall
    private Vector2 _preVelocity;
    // double jump count, support triple jump
    private int _jumpCount = 0;
    public float _jumpCooldown = 0.1f;
    private float _jumpCooldownTimer = 0f;
    private bool _jumpPressed = false;

    // the boolean to record if the player is on the ground
    private bool _onGround = true;
    // the boolean to record if the player is on the ground in the previous frame
    // for landing animation
    private bool _preOnGround = true;

    private Vector2 _lastOnGroundPosition;
    private bool _invincible = false;

    // dash properties
    [Header("Dash")]
    private bool _dashPressed = false;
    private int _dashCount = 0;
    [SerializeField] private int _maxDashCount = 1;
    [SerializeField] private float _dashSpeed = 10f;
    [SerializeField] private float _dashDuration = 0.5f;
    [SerializeField] private float _dashCooldown = 1f;
    private float _dashCooldownTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = transform.Find("Sprite").gameObject;
        _animator = _sprite.GetComponent<Animator>();
        _currentHp = _maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        _velocity = _rb.linearVelocity;
        OnGroundCheck();
        LandingCheck();

        // input check
        ButtonCheck();
        
        

        


        // fall
        if (!_onGround)
        {
            if ( _preVelocity.y >= 0 && _velocity.y < 0f)
            {
                _animator.SetTrigger("FallStart");
                Debug.Log("Start Falling");
            }
            
            else if (_preOnGround && _velocity.y < 0f)
            {
                _animator.SetTrigger("FallStart");
                Debug.Log("Start Falling");
                // if the player is falling, set the jumpCount to 1
                _jumpCount = 1;
            }
        }

        float inputX = Input.GetAxisRaw("Horizontal");
        inputX = Mathf.Abs(inputX) < 0.1f ? 0f : Mathf.Sign(inputX);
        float inputY = Input.GetAxisRaw("Vertical");



        _velocity = new Vector2(inputX * moveSpeed, _velocity.y);

        if(inputX != 0)
        {
            _animator.SetBool("Running", true);
            if(inputX < 0)
            {
                _sprite.transform.localScale = new Vector2(1, 1);
                faceing = -1;
            }
            if(inputX > 0)
            {
                _sprite.transform.localScale = new Vector2(-1, 1);
                faceing = 1;
            }
        }
        else
        {
            _animator.SetBool("Running", false);
        }
        _preVelocity = _velocity;
        _preOnGround = _onGround;
    }

    void FixedUpdate()
    {
        if (_velocity.y < maxFallSpeed) _velocity.y = maxFallSpeed;
        _rb.linearVelocity = _velocity;

        if (_jumpPressed)
        {
            Jump();
            _jumpPressed = false;
        }

        if (_dashPressed)
        {
            Dash();
            _dashPressed = false;
        }
    }



    void Jump()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
        _animator.ResetTrigger("Landing");
        _animator.SetTrigger("JumpStart");
        _jumpCooldownTimer = _jumpCooldown;
    }

    void Dash()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x + _dashSpeed * faceing, 0);
        _animator.ResetTrigger("Landing");
        
    }




    void OnGroundCheck()
    {
        _preOnGround = _onGround;
        Vector3 leftFeetPosition = transform.position;
        Vector3 rightFeetPositon = transform.position;
        leftFeetPosition.x -= footCheckDistance;
        rightFeetPositon.x += footCheckDistance;

        RaycastHit2D leftFeetCheck = Physics2D.Raycast(leftFeetPosition, Vector2.down, 0.3f, groundLayer);
        RaycastHit2D rightFeetCheck = Physics2D.Raycast(rightFeetPositon, Vector2.down, 0.3f, groundLayer);

        Debug.DrawLine(leftFeetPosition, new Vector3(leftFeetPosition.x, leftFeetPosition.y - 0.3f, leftFeetPosition.z),Color.red);
        Debug.DrawLine(rightFeetPositon, new Vector3(rightFeetPositon.x, rightFeetPositon.y - 0.3f, rightFeetPositon.z),Color.red);

        if (leftFeetCheck.collider != null || rightFeetCheck.collider != null)
        {
            _onGround = true;
            _lastOnGroundPosition = transform.position;
        }
        else
        {
            _onGround = false;
        }
    }

    void LandingCheck()
    {
        if (!_preOnGround && _onGround)
        {
            _animator.SetTrigger("Landing");
            Debug.Log("Landing");
            _jumpCount = 0;
            _dashCount = 0;
            landingEffect.SendEvent("PlayerLanding");
            sfx.Play();
        }
    }

    public void OnLandingAnimationEnd()
    {
        _animator.ResetTrigger("Landing");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DeadZone"))
        {
            Vector2 safePosition = FindLastSafeGroundPosition();
            transform.position = safePosition;
            TakeFallDamage();
        }
    }

    Vector2 FindLastSafeGroundPosition()
    {
        Vector2 safePosition = _lastOnGroundPosition;
        float checkStep = footCheckDistance; // footCheckDistance
        float maxCheckDistance = 1f; // maxCheckDistance
        float checkWidth = 0.5f; // checkWidth
        
        for (float distance = checkStep; distance <= maxCheckDistance; distance += checkStep)
        {
            // leftBasePosition
            Vector3 leftBasePosition = safePosition;
            leftBasePosition.x -= distance;
            
            // leftPoint1
            Vector3 leftPoint1 = leftBasePosition;
            leftPoint1.x -= checkWidth/2;
            
            // leftPoint2
            Vector3 leftPoint2 = leftBasePosition;
            leftPoint2.x += checkWidth/2;
            
            RaycastHit2D leftCheck1 = Physics2D.Raycast(leftPoint1, Vector2.down, 0.3f, groundLayer);
            RaycastHit2D leftCheck2 = Physics2D.Raycast(leftPoint2, Vector2.down, 0.3f, groundLayer);
            
            // rightBasePosition
            Vector3 rightBasePosition = safePosition;
            rightBasePosition.x += distance;
            
            // rightPoint1
            Vector3 rightPoint1 = rightBasePosition;
            rightPoint1.x -= checkWidth/2;
            
            // rightPoint2
            Vector3 rightPoint2 = rightBasePosition;
            rightPoint2.x += checkWidth/2;
            
            RaycastHit2D rightCheck1 = Physics2D.Raycast(rightPoint1, Vector2.down, 0.3f, groundLayer);
            RaycastHit2D rightCheck2 = Physics2D.Raycast(rightPoint2, Vector2.down, 0.3f, groundLayer);
            
            // if both leftCheck1 and leftCheck2 are on the ground
            if (leftCheck1.collider != null && leftCheck2.collider != null)
            {
                safePosition.x = leftBasePosition.x;
                break;
            }
            else if (rightCheck1.collider != null && rightCheck2.collider != null)
            {
                safePosition.x = rightBasePosition.x;
                break;
            }
            
            if (distance >= maxCheckDistance)
            {
                Debug.LogWarning("No safe position found within range");
                // should restart game here
            }
        }
        
        return safePosition;
    }

    void TakeFallDamage()
    {
        int damage = _maxHp / 5;
        _currentHp -= damage;
        Debug.Log($"Take {damage} damage");
        Debug.Log($"Current HP: {_currentHp}");
        SetInvincible(1f);

    }

    void SetInvincible(float duration)
    {
        _invincible = true;
        Invoke("ResetInvincible", duration);
        _animator.SetBool("Twinkling", true);
    }

    void ResetInvincible()
    {
        _invincible = false;
        _animator.SetBool("Twinkling", false);
    }

    void ButtonCheck()
    {
        // jump
        if (Input.GetButtonDown("Jump") && _jumpCooldownTimer <= 0)
        {
            if (_onGround)
            {
                _jumpPressed = true;
                _jumpCount++;
                Debug.Log("Jump");
            }

            else if (_jumpCount < maxJumpCount)
            {
                _jumpPressed = true;
                _jumpCount++;
                Debug.Log("Double Jump");
            }

        }
        if (_jumpCooldownTimer > 0)
        {
            _jumpCooldownTimer -= Time.deltaTime;
        }

        // dash
        if (Input.GetButtonDown("Dash"))
        {
            if (_dashCooldownTimer <= 0 && _dashCount < _maxDashCount)
            {
                _dashPressed = true;
                _dashCount++;
                _dashCooldownTimer = _dashCooldown;
                Debug.Log("Dash");
            }
        }
        if (_dashCooldownTimer > 0)
        {
            _dashCooldownTimer -= Time.deltaTime;
        }
    }

    public float GetHpPercentage()
    {
        return (float)_currentHp / _maxHp;
    }
        
}
