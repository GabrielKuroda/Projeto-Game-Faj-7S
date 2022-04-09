using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGController : IPersistentSingleton<RPGController>
{

    public float speed = 5f;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private Vector2 _movement = Vector2.zero;
    private static readonly int InputXHash = Animator.StringToHash("InputX");
    private static readonly int InputYHash = Animator.StringToHash("InputY");

    public Vector3 bottomLeftLimit;
    public Vector3 topRightLimit;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
    
        float inputY = Input.GetAxisRaw("Vertical");
 
        _movement = new Vector2(inputX, inputY) * speed;
        if(_movement.sqrMagnitude > 0.1f)
        {
            _spriteRenderer.flipX = (inputX > 0.1f);
        }
        _animator.SetFloat(InputXHash, _movement.x);
        _animator.SetFloat(InputYHash, _movement.y);
    }

    private void FixedUpdate()
    {
        if (_movement.sqrMagnitude > 0.01f)
        {
            Vector2 velocity = _movement.normalized * speed;
            _rigidbody.velocity = velocity;
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),
                                         Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y),
                                         transform.position.z);

    }

    public void SetBounds(Vector3 botLeft, Vector3 topRight)
    {
        bottomLeftLimit = botLeft + new Vector3(.5f, 1f, 0f);
        topRightLimit = topRight + new Vector3(-.5f, -1f, 0f);
    }

}
