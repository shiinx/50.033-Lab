using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour {
    public float speed;
    public float upSpeed;
    public float maxSpeed;
    public float linearDrag;
    public float gravity;
    public Transform enemyLocation;
    public Text scoreText;
    public Button startButton;
    private SpriteRenderer _marioSprite;
    private Rigidbody2D _marioBody;
    private Animator _marioAnimator;
    private AudioSource _marioAudio;
    private int _score;
    private bool _countScoreState;
    private string _sceneName;

    private bool _onGroundState = true;
    private bool _faceRightState = true;
    
    // Start is called before the first frame update
    private void Start() {
        Application.targetFrameRate = 30;
        _marioBody = GetComponent<Rigidbody2D>();
        _marioSprite = GetComponent<SpriteRenderer>();
        _marioAnimator = GetComponent<Animator>();
        _marioAudio = GetComponent<AudioSource>();
        _sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    private void Update() {
        if (_sceneName == "Level1") {
            if (!_onGroundState && _countScoreState) {
                if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f) {
                    _countScoreState = false;
                    _score++;
                }
            }    
        }
    }

    private void FixedUpdate() {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var marioVelocity = _marioBody.velocity;
        MoveMario(moveHorizontal, marioVelocity);
        
        if (Input.GetKeyDown("space") && _onGroundState) {
            Jump();
            _countScoreState = true;
        }
        ModifyDrag(moveHorizontal, marioVelocity);
    }
    
    private void Jump() {
        _marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
        _onGroundState = false;
        _marioAnimator.SetBool("onGround", _onGroundState);
    }

    private void MoveMario(float moveHorizontal, Vector2 marioVelocity) {
        // move by add force
        var movement = new Vector2(moveHorizontal, 0);
        _marioBody.AddForce(movement * speed);
        
        // change sprite looking direction
        if (moveHorizontal < 0 && _faceRightState) {
            _faceRightState = false;
            _marioSprite.flipX = true;
            if (Mathf.Abs(_marioBody.velocity.x) > 1.0 && _onGroundState) {
                _marioAnimator.SetTrigger("onSkid");
            }
        }else if (moveHorizontal > 0 && !_faceRightState) {
            _faceRightState = true;
            _marioSprite.flipX = false;
            if (Mathf.Abs(_marioBody.velocity.x ) > 1.0 && _onGroundState) {
                _marioAnimator.SetTrigger("onSkid");
            }
        }
        
        // set max speed 
        if (Mathf.Abs(marioVelocity.x) > maxSpeed) {
            _marioBody.velocity = new Vector2(Mathf.Sign(marioVelocity.x) * maxSpeed, marioVelocity.y);
        }
        _marioAnimator.SetFloat("xSpeed", Mathf.Abs(moveHorizontal));
    }

    private void ModifyDrag(float moveHorizontal, Vector2 marioVelocity) {
        var changingDirections = moveHorizontal > 0 && marioVelocity.x < 0 ||  moveHorizontal < 0 && marioVelocity.x > 0;

        if(_onGroundState) {
            if(Mathf.Abs(moveHorizontal) < 0.4f || changingDirections){
                _marioBody.gravityScale = 1;
                _marioBody.drag = linearDrag;
            }else{
                _marioBody.gravityScale = 0f;
                _marioBody.drag = 0f;
            }
        }else {
            _marioBody.gravityScale = gravity;
            _marioBody.drag = linearDrag * 0.15f;
        }
        
    }
    
    private void OnCollisionEnter2D(Collision2D col) {
        Vector2 dir = col.GetContact(0).normal;
        if (dir != Vector2.up) return;
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Platform") || col.gameObject.CompareTag("Brick")) {
            _onGroundState = true;
            _marioAnimator.SetBool("onGround", _onGroundState);
            _countScoreState = false;
            scoreText.text = "Score: " + _score;
        }
            
    }
    
    private void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.CompareTag("Brick") || col.gameObject.CompareTag("Platform")) {
            _onGroundState = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Enemy")) {
            Debug.Log("Collided with Goomba!");
            Time.timeScale = 0.0f;
            startButton.gameObject.SetActive(true);
        }
    }

    private void PlayJumpSound() {
        _marioAudio.PlayOneShot(_marioAudio.clip);
    }

}