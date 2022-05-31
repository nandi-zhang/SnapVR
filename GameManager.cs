using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    void Awake() 
    {
        instance = this;
    }
    #endregion

    [SerializeField]
    public HandPresence rightHand;
    public HandPresence leftHand;
    public HandPresence rightHandMat;
    public HandPresence leftHandMat;

    public Rigidbody Target;

    public ActivateVoice voiceActivator;

    public GameObject Mark;
    List<GameObject> Marks;
    public GameObject TargetPlacement;
    public GameObject StartingPoint;

    public TrajectoryPlanner planner;

    public Text text1;
    public Text text2;

    Vector3 startPosition;
    Vector3 initialPosition;
    Vector3 endPosition;
    bool marking = false;

    Vector3 positionA;
    Vector3 positionB;
    float period = 0f;

    List<HandPresence> virtualHands;

    void Start() {
        UpdateGameState(GameState.Intro);
        Marks = new List<GameObject>();
        virtualHands = new List<HandPresence>();
        positionA = new Vector3(-0.27f, 0.64f, 0.1f);
        positionB = new Vector3(0.216f, 0.64f, 0.187f);
    }

    public GameState GetGameState() {
        return State;
    }

    void Update()
    {
        if(marking || State == GameState.StartDemo){
            if(period > 0.5f){
                Marks.Add(Instantiate(Mark, Target.transform.position, Quaternion.identity));
                period = 0f;
            }
            period += UnityEngine.Time.deltaTime;
        }
    }
    
    public void UpdateGameState(GameState newState) {
        State = newState;

        switch (newState) {
            case GameState.Intro:
                PlayIntro();
                break;
            case GameState.Intro1:
                PlayIntro1();
                break;
            case GameState.Intro2:
                PlayIntro2();
                break;
            case GameState.Intro3:
                PlayIntro3();
                break;
            case GameState.HumanAction:
                HumanAct();
                break;
            case GameState.Snapshot:
                TakeSnapshot();
                break;
            case GameState.StartDemo:
                StartDemo();
                break;
            case GameState.EndDemo:
                EndDemo();
                break;
            case GameState.RobotPractice:
                RobotDemo();
                break;
            case GameState.RobotContact:
                RobotContact();
                break;
            case GameState.Collaboration:
                Collaborate();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public void ResolveCommand(string[] values){
        var value = values[0];
        // var value1 = value[1];
        Debug.Log(value);
        // Debug.Log(value1);

        if(State == GameState.Intro1){
            switch(value) {
                case "Start a demonstration":
                    text2.text = "Start learning the demonstration...";
                    marking = true;
                    return;
                case "Start the demonstration":
                    text2.text = "Start learning the demonstration...";
                    marking = true;
                    return;
                case "End the demonstration":
                    text2.text = "Stop learning the demonstration.";
                    marking = false;
                    return;
                case "trial":
                    text2.text = "Learning completed. Start a trial.";
                    Target.transform.position = positionA;
                    TargetPlacement.transform.position = positionB;
                    StartingPoint.transform.position = positionA;
                    endPosition = Target.transform.position;
                    Target.transform.rotation = Quaternion.identity;
                    planner.PublishJoints();
                    return;
                case "practice":
                    text2.text = "Learning completed. Start a trial.";
                    Target.transform.position = positionA;
                    TargetPlacement.transform.position = positionB;
                    StartingPoint.transform.position = positionA;
                    endPosition = Target.transform.position;
                    Target.transform.rotation = Quaternion.identity;
                    planner.PublishJoints();
                    return;
            }
        }

        if(State == GameState.Intro2){
            switch(value) {
                case "a snapshot":
                    TakeSnapshot();
                    text2.text = "Snapshot taken! ";
                    return;
                case "Start a demonstration":
                    text2.text = "Start learning the demonstration...";
                    marking = true;
                    return;
                case "Start the demonstration":
                    text2.text = "Start learning the demonstration...";
                    marking = true;
                    return;
                case "End the demonstration":
                    text2.text = "Stop learning the demonstration.";
                    marking = false;
                    return;
                case "trial":
                    text2.text = "Learning completed. Start a trial.";
                    Target.transform.position = positionA + new Vector3(0f, 0.03f, 0f);
                    StartingPoint.transform.position = positionA + new Vector3(0f, 0.03f, 0f);
                    Target.transform.rotation = Quaternion.identity;
                    planner.PublishJoints();
                    return;
                case "practice":
                    text2.text = "Learning completed. Start a trial.";
                    Target.transform.position = positionA + new Vector3(0f, 0.03f, 0f);
                    StartingPoint.transform.position = positionA + new Vector3(0f, 0.03f, 0f);
                    Target.transform.rotation = Quaternion.identity;
                    planner.PublishJoints();
                    return;
            }
        }

        if(State == GameState.Intro3){
            switch(value) {
                case "collaboration":
                    text2.text = "Learning completed. Start collaboration.";
                    Target.transform.position = positionA + new Vector3(0f, 0.03f, 0f);
                    StartingPoint.transform.position = positionA + new Vector3(0f, 0.03f, 0f);
                    startPosition = StartingPoint.transform.position + new Vector3(0f, 0.03f, 0f);
                    // Target.transform.rotation = Quaternion.identity;
                    planner.PublishJointsWithPlacementOnSite(startPosition);
                    // planner.PublishJoints();
                    return;
            }
        }

        switch(value) {
            case "a snapshot":
                UpdateGameState(GameState.Snapshot);
                break;
            case "action":
                UpdateGameState(GameState.HumanAction);
                break;
            case "Start a demonstration":
                UpdateGameState(GameState.StartDemo);
                break;
            case "Start the demonstration":
                UpdateGameState(GameState.StartDemo);
                break;
            case "End the demonstration":
                UpdateGameState(GameState.EndDemo);
                break;
            case "trial":
                UpdateGameState(GameState.RobotPractice);
                break;
            case "practice":
                UpdateGameState(GameState.RobotPractice);
                break;
            case "collaboration":
                UpdateGameState(GameState.Collaboration);
                break;
            default:
                text2.text = "Sorry, can you say that again?";
                break;
        }
    }
    
    private void PlayIntro() {
        text1.text = "Hi! I am Niryo One. Welcome to SnapVR.\nHere you can teach me to perform simple tasks by demonstration.";
    }

    private void PlayIntro1() {
        initialPosition = Target.transform.position;
        text1.text = "Let's start with a pick-and-place task.\n" +
                     "Learning task: Moving the cube to the red area.\n\n" +
                     "Say \"Start a demonstration\" before you pick up.\n" +
                     "After moving it to the red area, say \"End the demonstration\". \n" +
                     "You can say \"Start robot trial\" to check my learning outcome.";

        Target.transform.position = positionA;
        StartingPoint.transform.position = positionA;
        TargetPlacement.transform.position = positionB;
    }

    private void PlayIntro2() {
        foreach(GameObject t in Marks) {
                Destroy(t.gameObject);
        }
        text1.text = "Now try holding the cube up before demonstrating.\n" +
                     "Learning task: Starting from mid-air, moving the cube to the red area..\n\n" +
                     "Say \"Take a snapshot\" after you hold up the cube.\n" +
                     "You can now demonstrate with the \"Start/End the demonstration\" commands";
        initialPosition = Target.transform.position;
        Target.transform.position = positionA + new Vector3(0f, 0.03f, 0f);
        StartingPoint.transform.position = positionA + new Vector3(0f, 0.03f, 0f);
        TargetPlacement.transform.position = positionB;
    }

    private void PlayIntro3() {
        foreach(GameObject t in Marks) {
                Destroy(t.gameObject);
        }
        text1.text = "Let's try doing it together!\n" +
                     "Learning task: Taking the cube from the user's hand and move it to the red area...\n\n" +
                     "Say \"Try collaboration\" to start.\n";

        Target.transform.position = positionA + new Vector3(0f, 0.03f, 0f);
        StartingPoint.transform.position = positionA + new Vector3(0f, 0.03f, 0f);
        TargetPlacement.transform.position = positionB;
    }

    private void HumanAct() {
        foreach(GameObject t in Marks) {
                Destroy(t.gameObject);
        }
        for (int i = virtualHands.Count - 1; i >= 0; i--)
        {
            // Transform[] allChildren = virtualHands[i].GetComponentsInChildren<Transform>();  // collect entire sub-graph into array
            // foreach(Transform t in allChildren) {
            //     Destroy(t.gameObject);
            // }
            // Destroy(virtualHands[i]);
            virtualHands[i].transform.position = new Vector3(0f,-1f,0f);
        }
        initialPosition = Target.transform.position;
        Target.transform.position = positionA;
        StartingPoint.transform.position = positionA;
        TargetPlacement.transform.position = positionB;
        text1.text = "Congrats! You have learned how to teach me to collaborate with you in a task.\n"+
                     "Now try out your own task!";
        text2.text = "Suggested commands:\n"+
                     "Take a snapshot.        How to demonstrate?         Reset the scene.\n" +
                     "          How to take a snapshot?         What is a snapshot?";
    }

    private void TakeSnapshot() {
        text2.text = "Snapshot taken!\n" + "Suggested commands:\n"+
                     "Start a demonstration.       Start the demonstration.\n" +
                     "          How to demonstrate?";
        float gripValue = rightHand.GetHandGrip();
        float triggerValue = rightHand.GetHandTrigger();

        HandPresence virtualRightHand = Instantiate(rightHandMat, rightHand.transform.position, rightHand.transform.rotation);
        virtualRightHand.handAnimator.SetFloat("Grip", gripValue);
        virtualRightHand.handAnimator.SetFloat("Trigger", triggerValue);
        // virtualRightHand.setGrip(gripValue, triggerValue);

        // virtualHands.Add(Instantiate(rightHandMat, rightHand.transform.position, rightHand.transform.rotation));

        // virtualHands[virtualHands.Count - 1].handAnimator.SetFloat("Grip", gripValue);
        // virtualHands[virtualHands.Count - 1].handAnimator.SetFloat("Trigger", triggerValue);

        // Animator handAnimator = virtualRightHand.GetComponent<Animator>();

        // handAnimator.SetFloat("Grip", gripValue);
        // handAnimator.SetFloat("Trigger", triggerValue);

        gripValue = leftHand.GetHandGrip();
        triggerValue = leftHand.GetHandTrigger();
        HandPresence virtualLeftHand = Instantiate(leftHandMat, leftHand.transform.position, leftHand.transform.rotation);
        virtualLeftHand.handAnimator.SetFloat("Grip", gripValue);
        virtualLeftHand.handAnimator.SetFloat("Trigger", triggerValue);

        virtualHands.Add(virtualRightHand);
        virtualHands.Add(virtualLeftHand);

        Target.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        Target.velocity = Vector3.zero;
        Target.angularVelocity = Vector3.zero;
        // voiceActivator.WitActivate();
    }

    private void StartDemo() {
        text1.text = "Start learning the demonstration...";
        text2.text = "Suggested commands:\n"+
                     "End the demonstration.        How to demonstrate?.\n" +
                     "Clear all the marks.";
        startPosition = Target.transform.position;
        StartingPoint.transform.position = startPosition;
    }

    private void EndDemo() {
        text1.text = "Stop learning the demonstration.";
        text2.text = "Suggested commands:\n"+
                     "Start robot trial.        Start robot practice.\n" +
                     "          What is robot trial?      Why the robot is not moving?" +
                     "End the demonstration.       Start a demonstration.";
        endPosition = Target.transform.position;
        TargetPlacement.transform.position = endPosition;
    }

    private void RobotDemo() {
        text1.text = "Learning completed. Start a trial.";
        text2.text = "Suggested commands:\n"+
                     "Try collaboration.        Start collaboration.\n" +
                     "          Why the robot is not moving?      Take a snapshot.\n" +
                     "Why does the task fail?       Start a demonstration.";
        Target.transform.position = startPosition;
        Target.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        Target.velocity = Vector3.zero;
        Target.angularVelocity = Vector3.zero;

        planner.PublishJoints();
    }

    private void RobotContact() {
        Target.constraints = RigidbodyConstraints.None;
    }

    private void Collaborate() {
        foreach(GameObject t in Marks) {
                Destroy(t.gameObject);
        }
        text1.text = "Try collaboration.";
        text2.text = "Suggested commands:\n"+
                     "Why the robot is not moving?      Take a snapshot.\n" +
                     "              Why does the task fail?       Start a demonstration.";
        Target.transform.position = initialPosition;

        bool targetOnSite = false;
        bool placementOnSite = false;

        float targetDistance = (Target.transform.position - startPosition).magnitude;
        float placementDistance = (TargetPlacement.transform.position - endPosition).magnitude;

        Vector3 idleOffset = new Vector3(0f, 0.02f, 0f);

        for (int i = virtualHands.Count - 1; i >= 0; i--)
        {
            // Transform[] allChildren = virtualHands[i].GetComponentsInChildren<Transform>();  // collect entire sub-graph into array
            // foreach(Transform t in allChildren) {
            //     Destroy(t.gameObject);
            // }
            // Destroy(virtualHands[i]);
            virtualHands[i].transform.position = new Vector3(0f,-1f,0f);
        }
 
        virtualHands.Clear();

        if(targetDistance < 0.01f)
        {
            Debug.Log("Target On Site");
            targetOnSite = true;
        }
        if(placementDistance < 0.01f)
        {
            Debug.Log("Placement On Site");
            placementOnSite = true;
        }

        if(targetOnSite && placementOnSite)
        {
            planner.PublishJoints();
            return;
        }

        if(targetOnSite)
        {
            planner.PublishJointsWithTargetOnSite(endPosition);
            return;
        }
        if(placementOnSite)
        {
            planner.PublishJointsWithPlacementOnSite(startPosition);
            return;
        }
    }
}


public enum GameState {
    Intro,
    Intro1,
    Intro2,
    Intro3,
    HumanAction,
    Snapshot,
    StartDemo,
    EndDemo,
    RobotPractice,
    RobotContact,
    Collaboration
}