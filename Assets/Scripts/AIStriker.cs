using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class AIStriker : MonoBehaviour
{
    public static AIStriker Instance { get; private set; }

    public static event System.Action OnStrike;
    public Transform[] carromPieces; // Array of all the carromPieces on the board

    [SerializeField] private Turn turnPlayer;
    [SerializeField] Vector3 initialPosition = new Vector2(0, 8);


    private Transform targetPiece;
    private Rigidbody2D strikerRigidbody;
    private CircleCollider2D circleCollider2D;
    Coroutine InitializeCoroutine;
    Coroutine ExitCoroutine;
    bool canStrike = true;
    private void Awake()
    {
        strikerRigidbody = GetComponent<Rigidbody2D>();
        Instance = this;
    }

    enum AIState
    {
        Enter, Find, Exit
    }

    private AIState state;
    private void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.enabled = false;

    }


    private void Update()
    {



        if (GameManager.Instance.CurrentTurn == turnPlayer)
        {

            if (canStrike)
            {//Basic state_machine for handling AI_Movement and Attack
                switch (state)
                {
                    case AIState.Enter:

                        if (InitializeCoroutine == null)
                        {
                            float moveDuration = 1f;
                            InitializeCoroutine = StartCoroutine(Initialize(moveDuration));
                        }
                        break;
                    case AIState.Find:
                        targetPiece = FindClosedPieceToCenter();
                        state = AIState.Exit;
                        break;
                    case AIState.Exit:
                        if (ExitCoroutine == null)
                            ExitCoroutine = StartCoroutine(MoveAndExit());
                        break;
                }
            }
        }
    }

    #region StateMachine Methods
    IEnumerator Initialize(float duration)
    {
        transform.position = new Vector3(0, 8);
        yield return new WaitForSeconds(0.8f);
        Vector2 minMaxPositionX = new Vector2(-6.4f, 6.4f);
        Vector2 targetPos = new Vector2(Random.Range(minMaxPositionX.x, minMaxPositionX.y), 8);
        Vector2 currentPos = transform.position;
        float timer = 0;
        float percent = 0;
        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / duration;
            transform.position = Vector2.Lerp(currentPos, targetPos, percent);
            yield return null;
        }
        InitializeCoroutine = null;
        state = AIState.Find;
    }

    IEnumerator MoveAndExit()
    {
        yield return new WaitForSeconds(1);
        Vector2 direction = (targetPiece.position - transform.position).normalized;
        Vector2 minMaxForceAmount = new Vector2(100, 200);

        strikerRigidbody.AddForce(direction * Random.Range(minMaxForceAmount.x, minMaxForceAmount.y), ForceMode2D.Impulse);

        canStrike = false;
        OnStrike?.Invoke();
        ExitCoroutine = null;

    }
    #endregion

    //extremely simple method to find a carromPiece
    private Transform FindClosedPieceToCenter()
    {
        Transform closedPiece = null;
        float closestDistance = Mathf.Infinity;
        Vector3 centerPocketPosition = Vector3.zero;

        foreach (Transform carromPiece in carromPieces)
        {
            float distance = Vector3.Distance(carromPiece.position, centerPocketPosition);
            if (distance < closestDistance)
            {
                closedPiece = carromPiece;
                closestDistance = distance;
            }
        }

        return closedPiece;
    }

    public void ResetSide(bool canSwitch)
    {


        strikerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        StartCoroutine(MoveToInitial(0.5f, canSwitch));


    }

    //Method to configure all the setting in striker 
    public void Spawn()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        strikerRigidbody.constraints = RigidbodyConstraints2D.None;
        transform.eulerAngles = Vector3.zero;
        yield return StartCoroutine(ColorLerp(0.5f));
        circleCollider2D.enabled = true;
        state = AIState.Enter;


    }
    IEnumerator ColorLerp(float duration)
    {

        SpriteRenderer spriteRender = GetComponentInChildren<SpriteRenderer>();

        float percent = 0f;
        float timer = 0f;
        spriteRender.color = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, 0);

        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / duration;

            spriteRender.color = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, Mathf.Lerp(0, 1, percent));

            yield return null;

        }
        canStrike = true;


    }
    //Coroutine to move striker to initialPosition and fade it out 
    IEnumerator MoveToInitial(float duration, bool fadeColor)
    {
        GetComponent<CircleCollider2D>().enabled = false;
        SpriteRenderer spriteRender = GetComponentInChildren<SpriteRenderer>();
        Color initialColor = spriteRender.color;
        Vector3 currentPosition = (Vector2)transform.position;
        Vector3 targetPosition = initialPosition;
        float percent = 0f;
        float timer = 0f;
        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / duration;
            if (fadeColor)
                spriteRender.color = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, Mathf.Lerp(1, 0, percent));
            transform.position = Vector2.Lerp(currentPosition, targetPosition, percent);
            yield return null;
        }


        //TODO: have to refactor to a new name ,not clear enough
        if (!fadeColor)
        {
            circleCollider2D.enabled = true;
            strikerRigidbody.constraints = RigidbodyConstraints2D.None;
            canStrike = true;
            state = AIState.Enter;

        }



    }
}
