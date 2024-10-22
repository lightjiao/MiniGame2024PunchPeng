using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public enum BehaviorState
    {
        Idle,
        Walk,
        Run
    }

    private BehaviorState currentState;
    private float stateDuration;
    private float timer;
    private Vector3 randomDirection;

    private void Start()
    {
        RandomizeState();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        switch (currentState)
        {
            case BehaviorState.Idle:
                IdleBehavior();
                break;
            case BehaviorState.Walk:
                WalkBehavior();
                break;
            case BehaviorState.Run:
                RunBehavior();
                break;
        }

        if (timer >= stateDuration)
        {
            RandomizeState();
        }
    }

    private void IdleBehavior()
    {
        Debug.Log("Character is idle.");
    }

    private void WalkBehavior()
    {
        transform.position += randomDirection.normalized * Time.deltaTime * 2f; // 行走速度
        Debug.Log("Character is walking in direction: " + randomDirection);
    }

    private void RunBehavior()
    {
        transform.position += randomDirection.normalized * Time.deltaTime * 5f; // 奔跑速度
        Debug.Log("Character is running in direction: " + randomDirection);
    }

    private void RandomizeState()
    {
        System.Random random = new System.Random(System.DateTime.Now.Millisecond);
        currentState = (BehaviorState)random.Next(0, 3);
        stateDuration = random.Next(2, 10);
        timer = 0;

        if (currentState == BehaviorState.Walk || currentState == BehaviorState.Run)
        {
            randomDirection = new Vector3(random.Next(-10, 10), 0, random.Next(-10, 10));
        }
    }
}