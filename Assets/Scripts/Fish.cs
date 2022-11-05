using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{

    Rigidbody2D _rb; 
    
    int angle;
    int maxAngle = 20;
    int minAngle = -60;
    [SerializeField]
    private float _speed;
    public Score score;
    bool touchedGround; 
    public GameManager gameManager;
    public Sprite fishDied;
    SpriteRenderer sp;
    Animator anim;
    public ObstacleSpawner obstacleSpawner;
    [SerializeField] private AudioSource swim, hit, point; 

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        FishSwim();               
    }

    private void FixedUpdate()
    {
        FishRotation(); // rotation yavaşladı ve yumuşak oldu.tüm cihazlarda aynı sonuç almak için.
    }

    void FishSwim()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.gameOver == false)
        {
                    swim.Play();
            if(GameManager.gameStarted == false)
            {
                _rb.gravityScale = 4f;
                _rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(_rb.velocity.x, _speed);
                gameManager.GameHasStarted();
            }
            else
            {
                _rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(_rb.velocity.x, _speed);
            }
        }
    }

    void FishRotation()
    {
        if(_rb.velocity.y > 0) 
        {
            if(angle <= maxAngle)
            {
                angle = angle + 4;
            }
        }
        else if (_rb.velocity.y < -1.2)
        {
            if(angle > minAngle)
            {
                angle = angle - 2;
            }
        }
        if (touchedGround == false)
        {
            transform.rotation = Quaternion.Euler(0,0,angle); //açısal dönüş
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            //Debug.Log("Scored!..");
            score.Scored();
            point.Play();
        }
        else if(collision.CompareTag("Column") && GameManager.gameOver == false)
        {
            //gameover
            FishDiedEffect();
            gameManager.GameOver();
            
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            if(GameManager.gameOver == false)
            {
                //gameover
                FishDiedEffect();
                gameManager.GameOver(); // bu oyunu durduran game
                GameOver();
            }
            else
            {
                GameOver(); //gameover(fish)
            }
            
        }
       
    }
    void GameOver() // gameover olduğunda fish in görünüm ve animasyonu
    {
        touchedGround = true;
        transform.rotation = Quaternion.Euler(0,0,-90);
        sp.sprite = fishDied;
        anim.enabled = false;
    }
    void FishDiedEffect()
    {
        hit.Play();
    }

    

}
