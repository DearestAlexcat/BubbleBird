using System.Collections;
using UnityEngine;

public class BirdScripts : MonoBehaviour
{
    public static BirdScripts instance = null;

    [SerializeField]  Vector2 startPosition;
    [SerializeField]  SpriteRenderer birdSprite;
    [SerializeField]  Rigidbody2D rigidbody2d;
    [SerializeField]  Animator animator;
    [SerializeField]  float bounceSpeed = 4.0f;
    [SerializeField]  float speedZ = 7.0f;

    public static event System.Action<int> OnSetPoints;
    public static event System.Action OnSaveScore;
    public static event System.Func<Color> OnGetRandomColor;
    public static event System.Action<bool> OnColoredWallSetActive;

    bool didFlap;
    public bool isAlive;

    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip flap, died, addedPoints;
    [SerializeField]
    GameObject ConfettiFX;
    [SerializeField]
    float timeLifeConfetti;
    [SerializeField]
    float borderSpeed;

    enum State { Idle, Flap, Die }

    public int pointCount { get; private set; }

    public event System.Action addSpeedColorWall;

    public void Play()
    {
        pointCount = 0;       
        isAlive = true;
        
        BirdSetActive(true);
        
        birdSprite.color = OnGetRandomColor();

        OnColoredWallSetActive.Invoke(true);

        birdSprite.transform.position = startPosition;       
        birdSprite.transform.rotation = Quaternion.identity;
    }

    void BirdSetActive(bool value)
    {
        birdSprite.enabled = value;
        rigidbody2d.isKinematic = !value;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        //isAlive = true;
        //CameraSetPosX();
    }

    private void FixedUpdate()
    {
        if(isAlive)
        {
            #region version 1
  
            //Vector3 temp = transform.position;
            //temp.x += forwardSpeed * Time.deltaTime;
            //transform.position = temp;

            ////Debug.Log(rigidbody2d.velocity);
            
            //if(didFlap)
            //{
            //    didFlap = false;
            //    rigidbody2d.velocity = new Vector2(0, bounceSpeed);
            //    animator.SetTrigger("Flap");
            //}

            //if(rigidbody2d.velocity.y >= 0) // Если был рывок
            //{
            //    transform.rotation = Quaternion.Euler(0, 0, 0);
            //}
            //else
            //{
            //    float angle;
            //    angle = Mathf.Lerp(0, -90, -rigidbody2d.velocity.y / 7.0f);
            //    transform.rotation = Quaternion.Euler(0, 0, angle);
            //}
            #endregion

            if (didFlap)
            {
                didFlap = false;
                
                rigidbody2d.velocity = new Vector2(0.0f, bounceSpeed);
                transform.rotation = Quaternion.Euler(0, 0, 0);  // reset
                
                animator.SetInteger("State", (int)State.Flap);

                Vector3 correctPos = transform.position;
                correctPos.z = -1.0f;

                //GameObject ob = Instantiate(ConfettiFX, correctPos, ConfettiFX.transform.rotation);
                Destroy(Instantiate(ConfettiFX, correctPos, ConfettiFX.transform.rotation), timeLifeConfetti);
            }
            else
            {
                animator.SetInteger("State", (int)State.Idle);
                //float angle = Mathf.Lerp(0, -90, -rigidbody2d.velocity.y / speedZ);
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, -90, -rigidbody2d.velocity.y / speedZ));
            }
        }
    }

    //void CameraSetPosX()
    //{
    //    CameraFollow.offset = (Camera.main.transform.position.x - transform.position.x);
    //}

    public float GetXPosition()
    {
        return transform.position.x;
    }

    public void OnFlapBird()
    {
        if (!isAlive) return;
        
        didFlap = true;
        
        source.PlayOneShot(flap);
    }

    IEnumerator Die()
    {
        animator.SetInteger("State", (int)State.Die);
        source.PlayOneShot(died);

        //isAlive = false;
        yield return new WaitForSeconds(2.7f);

        rigidbody2d.velocity = new Vector2(0.0f, 0.0f);

        transform.rotation = Quaternion.Euler(0, 0, 0);

        BirdSetActive(false);
        
        OnSaveScore();

        animator.SetInteger("State", (int)State.Idle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAlive == false) return;

        if(collision.collider.CompareTag("Border"))
        {
            isAlive = false;
            StartCoroutine(Die());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isAlive == true) return;

        if (collision.collider.CompareTag("Border") && collision.collider.transform.position.y < 0)
        {
            Vector2 pos = transform.position;
            pos.x += borderSpeed * Time.deltaTime;
            transform.position = pos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAlive == false) return;

        if (collision.CompareTag("Base"))
        {
            if (collision.GetComponent<SpriteRenderer>().color.GetHashCode() == birdSprite.color.GetHashCode())
            {
                pointCount++;
                OnSetPoints(pointCount);
                source.PlayOneShot(addedPoints);
                birdSprite.color = OnGetRandomColor();
                OnColoredWallSetActive(false);
                
                addSpeedColorWall.Invoke();
            }
            else
            {
                isAlive = false;

                StartCoroutine(Die());
            }
        }
    }
}
