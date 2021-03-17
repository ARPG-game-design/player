using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    public void Awake()
    {
        Physics2D.IgnoreLayerCollision(9,10);
        playerRGB2D = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    internal Rigidbody2D playerRGB2D;
    internal Animator ani;
    internal AnimatorStateInfo state;
    internal bool lockMove = false;

    public float speedX = 180f;
    public float runSpeed = 125f;

    [Header("垂直向上推力")]
    public float yForce;

    [Header("感應地板的距離")]
    [Range(0, 0.5f)]
    public float distance;

    [Header("偵測地板的左射線起點")]
    public Transform groundCheckL;

    [Header("偵測地板的右射線起點")]
    public Transform groundCheckR;

    [Header("地面圖層")]
    public LayerMask groundLayer;

    [Header("感應上方的距離")]
    [Range(0, 0.5f)]
    public float Cdistance;

    [Header("偵測左上方的射線起點")]
    public Transform ceilingCheckL;

    [Header("偵測右上方的射線起點")]
    public Transform ceilingCheckR;

    [Header("地面圖層")]
    public LayerMask CgroundLayer;

    public bool Cgrounded;
    /*
    ---------設定鍵值---------
    */
    public bool JumpKey
    {
        get
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }

    public bool KUKey
    {
        get
        {
            if (!CIsGroundL && !CIsGroundR)
            {
                return Input.GetKeyUp(KeyCode.LeftControl);
            }
            else
                return false;

        }
    }

    public bool LKey
    {
        get
        {
            return Input.GetKey(KeyCode.LeftArrow);
        }
    }

    public bool RKey
    {
        get
        {
            return Input.GetKey(KeyCode.RightArrow);
        }
    }
    //----------------------

    /*
    ---------跳躍---------
    */
    void TryJump()
    {

        if (IsGrounded && (!CIsGroundL && !CIsGroundR) && Input.GetKeyDown(KeyCode.Space))
        {
            playerRGB2D.AddForce(Vector2.up * yForce);
            ani.SetTrigger("Jump");
        }
    }
    //----------------------

    /*
    ---------蹲下---------
    */
    void TryCrouch()
    {
        if (((!CIsGroundR && CIsGroundL) || (CIsGroundR && !CIsGroundL) || (!CIsGroundR && !CIsGroundL)) && (IsGroundL || IsGroundR) && Input.GetKeyDown(KeyCode.LeftControl))
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            speedX = 150f;
        }

        if ((!IsGroundL && IsGroundR) || (IsGroundL && !IsGroundR) || KUKey)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            speedX = 180f;
        }

        else if ((!CIsGroundR && !CIsGroundL) && (LKey || RKey))
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            speedX = 180f;
        }
    }
    //----------------------

    /*
    ---------判斷是否在地面---------
    */
    public bool groundedL;
    bool IsGroundL
    {
        get
        {
            Vector2 start = groundCheckL.position;
            Vector2 end = new Vector2(start.x, start.y - distance);

            Debug.DrawLine(start, end, Color.yellow);
            groundedL = Physics2D.Linecast(start, end, groundLayer);
            return groundedL;
        }
    }

    public bool groundedR;
    bool IsGroundR
    {
        get
        {
            Vector2 start = groundCheckR.position;
            Vector2 end = new Vector2(start.x, start.y - distance);

            Debug.DrawLine(start, end, Color.yellow);
            groundedR = Physics2D.Linecast(start, end, groundLayer);
            return groundedR;
        }
    }

    public bool IsGrounded
    {
        get
        {
            if (IsGroundR || IsGroundL)
            {
                return true;
            }
            else
                return false;
        }
    }


    //----------------------

    /*
    ---------判斷上方圖層是不是地面---------
    */
    bool CIsGroundL
    {
        get
        {
            Vector2 Cstart = ceilingCheckL.position;
            Vector2 Cend = new Vector2(Cstart.x, Cstart.y + Cdistance);

            Debug.DrawLine(Cstart, Cend, Color.yellow);
            Cgrounded = Physics2D.Linecast(Cstart, Cend, CgroundLayer);
            return Cgrounded;
        }
    }

    bool CIsGroundR
    {
        get
        {
            Vector2 Cstart = ceilingCheckR.position;
            Vector2 Cend = new Vector2(Cstart.x, Cstart.y + Cdistance);

            Debug.DrawLine(Cstart, Cend, Color.yellow);
            Cgrounded = Physics2D.Linecast(Cstart, Cend, CgroundLayer);
            return Cgrounded;
        }
    }
    //----------------------

    /*
    ---------移動,轉向---------
    */
    public virtual void MovementX()
    {
        bool upKey = Input.GetKeyUp(KeyCode.RightArrow) ||Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.LeftShift);
        if (Input.GetKey(KeyCode.RightArrow)&& Input.GetKey(KeyCode.LeftShift))
        {
            Move(2);
            Direction(0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(1);
            Direction(0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftShift))
        {
            Move(-2);
            Direction(1);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(-1);
            Direction(1);
        }
        if (upKey)
        {
            Move(0);
        }
    }

    public virtual void Move(int i)
    {       
        if (Mathf.Abs(i)==1)
        {
            playerRGB2D.velocity = new Vector2(i * speedX * Time.deltaTime, playerRGB2D.velocity.y);
        }
        else if (Mathf.Abs(i) == 2)
        {
            playerRGB2D.velocity = new Vector2(i * runSpeed * Time.deltaTime, playerRGB2D.velocity.y);
        }
        else if (Mathf.Abs(i) == 0)
        {
            playerRGB2D.velocity = new Vector2(0f, playerRGB2D.velocity.y);
        }
    }

    public virtual void Direction(int i)
    {
        transform.eulerAngles = new Vector3(0, 180f * i, 0);
    }

    //----------------------

    /*
    ---------動畫---------
    */
    public virtual void MoveAni()
    {
        bool upKey = Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.LeftShift);
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftShift))
        {
            ani.SetFloat("Move",2);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            ani.SetFloat("Move",1);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftShift))
        {
            ani.SetFloat("Move", 2);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            ani.SetFloat("Move", 1);
        }
        if (upKey)
        {
            ani.SetFloat("Move", 0);
        }
    }

    public void setAni()
    {
        ani.SetBool("Ground", IsGrounded);
        ani.SetFloat("SpeedY", playerRGB2D.velocity.y);
        if(gameObject.GetComponent<BoxCollider2D>().enabled == false){
            ani.SetBool("Crouch", true);
        }
        else if (gameObject.GetComponent<BoxCollider2D>().enabled == true)
        {
            ani.SetBool("Crouch", false);
        }
        state = ani.GetCurrentAnimatorStateInfo(0);
    }
    //----------------------

    private void FixedUpdate()
    {
        MovementX();
    }

    void Update()
    {
        MoveAni();
        setAni();
        TryJump();
        TryCrouch();
    }
}
