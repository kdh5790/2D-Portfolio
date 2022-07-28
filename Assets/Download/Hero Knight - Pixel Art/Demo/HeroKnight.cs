using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f; // 이동 속도
    [SerializeField] float m_jumpForce = 7.5f; // 점프 속도
    [SerializeField] float m_rollForce = 6.0f; // 구르기 속도
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust; // 먼지 이펙트

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_groundSensor; // 착지 확인용 센서
    private Sensor_HeroKnight m_wallSensorR1; // 벽 슬라이딩 확인용 센서 (오른쪽 아래)
    private Sensor_HeroKnight m_wallSensorR2; // 벽 슬라이딩 확인용 센서 (오른쪽 위)
    private Sensor_HeroKnight m_wallSensorL1; // 벽 슬라이딩 확인용 센서 (왼쪽 아래)
    private Sensor_HeroKnight m_wallSensorL2; // 벽 슬라이딩 확인용 센서 (왼쪽 위)
    private bool m_isWallSliding = false; // 벽 슬라이딩 여부
    private bool m_grounded = false; // 땅 착지 여부
    private bool m_rolling = false; // 구르기 여부
    private bool m_isAttack = false; // 공격 여부
    private bool m_isGuard = false; // 방어 여부
    private bool m_isMove = false; // 움직임 여부
    private int m_facingDirection = 1; // 스프라이트 회전값
    private int m_currentAttack = 0; // 콤보 공격
    private float m_timeSinceAttack = 0.0f; // 콤보 공격 제한시간
    private float m_delayToIdle = 0.0f; // idle 깜빡거림 방지용 시간 변수
    private float m_rollDuration = 8.0f / 14.0f; // 구르기 지속시간
    private float m_rollCurrentTime; // 구르기 시작후 시간

    public Transform m_wallChk;
    public float m_wallChkDistance;
    public LayerMask m_layerMask;
    public float m_slidingSpeed;
    public float m_wallJumpPower;

    private bool m_isWallJump;
    private bool m_isWall;

    float inputX;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    void Update()
    {
        // 콤보 공격 시간
        m_timeSinceAttack += Time.deltaTime;

        // 구르기 지속시간 시간
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // 구르기 지속시간 넘어갈 시 구르기 취소
        if (m_rollCurrentTime > m_rollDuration)
        {
            m_rolling = false;
        }

        // 착지 여부 확인
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // 이동 및 스프라이트 회전 용 변수 저장
        if (!m_isWallJump)
            inputX = Input.GetAxis("Horizontal");

        if (!m_isWallSliding || !m_isWallJump)
        {
            // 스프라이트 회전
            if (inputX > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                m_facingDirection = 1;
            }

            else if (inputX < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                m_facingDirection = -1;
            }

            else
                m_isMove = false;
        }

        // 이동
        if (!m_rolling && !m_isWallSliding)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        // AirSpeed 설정 (점프 중 올라가는지 내려오는지 확인용)
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);


        m_isWall = Physics2D.Raycast(m_wallChk.position, Vector2.right * m_facingDirection, m_wallChkDistance, m_layerMask);
        m_animator.SetBool("WallSlide", m_isWall);

        if (m_isWall)
        {
            m_isWallJump = false;
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_body2d.velocity.y * m_slidingSpeed);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_isWallJump = true;
                Invoke("FreezeX", 0.3f);
                m_body2d.velocity = new Vector2(m_facingDirection * m_wallJumpPower, 0.9f * m_wallJumpPower);
                GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            }
        }

        void FreezeX()
        {
            m_isWallJump = false;
        }


        // 벽 슬라이딩 애니메이션

        // 원래 벽 슬라이딩

        //m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        //m_animator.SetBool("WallSlide", m_isWallSliding);


        // 공격
        if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling && m_grounded)
        {
            m_currentAttack++;

            // 콤보 공격 1번(처음)으로 변경
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // 일정시간 지나면 콤보공격 초기화
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // 공격 애니메이션 재생
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // 콤보 공격 타이머 초기화
            m_timeSinceAttack = 0.0f;
        }

        // 방어
        else if (Input.GetMouseButtonDown(1) && !m_rolling && m_grounded && !m_isMove)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);

        // 구르기 (수정 필요)
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding && m_grounded)
        {
            //m_rolling = true;
            //m_animator.SetTrigger("Roll");
            //m_body2d.velocity = new Vector2((inputX * m_speed) * m_rollForce, m_body2d.velocity.y);

            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }


        // 점프
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        // 달리기 
        // Epsilon : 0에 가까운 소수점
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            m_isMove = true;
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        // idle
        else
        {
            // idle 중 깜빡거림 방지
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
    }


    // 벽 슬라이딩
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;

            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}
