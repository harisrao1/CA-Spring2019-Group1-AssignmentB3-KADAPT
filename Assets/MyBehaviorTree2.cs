using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;

public class MyBehaviorTree2 : MonoBehaviour
{
    public GameObject dan;
    public GameObject jake;
    public GameObject robber;
    public GameObject npc1;
    public GameObject npc2;
    public GameObject npc3;
    public GameObject npc4;
    public GameObject npc5;

    //Character Waypoints & other lookat points
    public Transform danSeat;
    public Transform jakeSeat;
    public Transform DanMeetsJake;
    public Transform RobberEntry;
    public Transform escape;
    public Transform table;
    public Transform buttonWaypointdan;
    public Transform buttonWaypointjake;
    public Transform buttonWaypoint2;
    public Transform Wall;
    public Transform npc1Seat;
    public Transform npc2Seat;
    public Transform npc3Seat;
    public Transform npc4Seat;
    public Transform npc5Seat;
    public Transform npcLookAt;
    public Transform TV;
    public Transform robbertheaterwaypoint;
    public Transform exit;
    public Transform danfindsrobber;

    //Objects
    public InteractionObject button;
    public InteractionObject bat;
    public GameObject batobj;
    public GameObject buttonobj;

    private BehaviorAgent behaviorAgent;

    Vector3 movedir = Vector3.zero;

    public GameObject Door;
    bool open;
    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
        open = false;


    }

    // Update is called once per frame
    void Update()
    {
       
    }

    protected Node ST_ApproachAndWait(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(dan.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }
    protected Node ST_Sit(Transform target, Transform LOOKAT)
    {
        Val<Vector3> position = Val.V(() => target.position);
        Val<Vector3> lookat = Val.V(() => button.transform.position);
        return new Sequence(dan.GetComponent<BehaviorMecanim>().Node_GoTo(position)
            , dan.GetComponent<BehaviorMecanim>().Node_OrientTowards(lookat)

            , new LeafWait(1000));
    }



    protected Node ST_shakeHands(GameObject dan, GameObject jake, Transform pos)
    {
        Val<Vector3> position = Val.V(() => pos.position);
        // Val<Vector3> lookat = Val.V(() => dan.transform.position);
        InteractionObject danhand = dan.transform.Find("Interactions").transform.Find("INTER_Give").GetComponent<InteractionObject>();
        InteractionObject jakehand = jake.transform.Find("Interactions").transform.Find("INTER_Give").GetComponent<InteractionObject>();
        Val<Vector3> lookat = Val.V(() => dan.transform.position);
        Val<Vector3> lookat2 = Val.V(() => jake.transform.position);
        return new Sequence(
            dan.GetComponent<BehaviorMecanim>().Node_GoTo(position),
                 new LeafWait(1000),
                jake.GetComponent<BehaviorMecanim>().Node_OrientTowards(lookat),
                dan.GetComponent<BehaviorMecanim>().Node_OrientTowards(lookat2),

                new LeafWait(1000),

            new SequenceParallel(

                dan.GetComponent<BehaviorMecanim>().Node_StartInteraction(FullBodyBipedEffector.RightHand, jakehand),
                jake.GetComponent<BehaviorMecanim>().Node_StartInteraction(FullBodyBipedEffector.RightHand, danhand)),
                new LeafWait(1500),
                dan.GetComponent<BehaviorMecanim>().Node_StopInteraction(FullBodyBipedEffector.RightHand),
                jake.GetComponent<BehaviorMecanim>().Node_StopInteraction(FullBodyBipedEffector.RightHand)
        );
    }
    protected Node ST_OpenDoor(InteractionObject button, Transform posdan, Transform posjake)
    {
        Val<Vector3> positiondan = Val.V(() => posdan.position);
        Val<Vector3> positionjake = Val.V(() => posjake.position);
        Val<Vector3> lookat = Val.V(() => buttonobj.transform.position);
        //  open = true;
        //  Door.transform.Translate(new Vector3(0f, -10f, 0f));
        return new Sequence(
            new SequenceParallel(
            dan.GetComponent<BehaviorMecanim>().Node_GoTo(positiondan),
            new LeafWait(500),
            jake.GetComponent<BehaviorMecanim>().Node_GoTo(positionjake)
            ),

           dan.GetComponent<BehaviorMecanim>().Node_StartInteraction(FullBodyBipedEffector.LeftHand, button),
           new LeafWait(500),
           dan.GetComponent<BehaviorMecanim>().Node_StopInteraction(FullBodyBipedEffector.LeftHand)

            );

    }
    protected Node ST_RobberEntry(Transform pos)
    {
        Val<Vector3> position = Val.V(() => pos.position);
        return new Sequence(
            robber.GetComponent<BehaviorMecanim>().Node_GoTo(position)
            );
    }
    protected Node ST_Escape(Transform pos)
    {
        Val<Vector3> position = Val.V(() => pos.position);
        return new Sequence(

            new SequenceParallel(
            dan.GetComponent<BehaviorMecanim>().Node_GoTo(position),
             jake.GetComponent<BehaviorMecanim>().Node_GoTo(position)
            )
            );
    }
    protected Node ST_GoSit(Transform danseat, Transform jakeseat, Transform t)
    {
        Val<Vector3> lookat = Val.V(() => t.position);
        Val<Vector3> danpos = Val.V(() => danseat.position);
        Val<Vector3> jakepos = Val.V(() => jakeseat.position);
        return new Sequence(
            new SequenceParallel(
           dan.GetComponent<BehaviorMecanim>().Node_GoTo(danpos),
           jake.GetComponent<BehaviorMecanim>().Node_GoTo(jakepos)),

            dan.GetComponent<BehaviorMecanim>().Node_OrientTowards(lookat),
            jake.GetComponent<BehaviorMecanim>().Node_OrientTowards(lookat),
            new LeafWait(200),

            dan.GetComponent<BehaviorMecanim>().Node_Sit(),
            jake.GetComponent<BehaviorMecanim>().Node_Sit()
        );

    }
    protected Node PickBat(GameObject rob, InteractionObject b, GameObject bat)
    {

        return
            rob.GetComponent<BehaviorMecanim>().Node_StartInteraction(FullBodyBipedEffector.RightHand, b);
    }
    protected Node ST_Talk()
    {
        return new Sequence(
            new SequenceParallel(
            new SequenceShuffle(
                dan.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADSHAKE", 500),
                dan.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADNOD", 500),
                dan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("YAWN", 500),
                dan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CHEER", 500)

            ),
            new SequenceShuffle(
                jake.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADSHAKE", 500),
                jake.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADNOD", 500),
                jake.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("YAWN", 500),
                jake.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CHEER", 500)

            )
            )
            );
    }

    protected Node BuildTreeRoot()
	{
        Node Story = new Sequence(


                        //NPC stuff

                        new SequenceParallel(
                        npc1.GetComponent<BehaviorMecanim>().Node_GoTo(npc1Seat.transform.position),
                         npc2.GetComponent<BehaviorMecanim>().Node_GoTo(npc2Seat.transform.position),
                          npc3.GetComponent<BehaviorMecanim>().Node_GoTo(npc3Seat.transform.position),
                           npc4.GetComponent<BehaviorMecanim>().Node_GoTo(npc4Seat.transform.position),
                            npc5.GetComponent<BehaviorMecanim>().Node_GoTo(npc5Seat.transform.position)
                        ),

                        npc1.GetComponent<BehaviorMecanim>().Node_OrientTowards(npcLookAt.transform.position),
                         npc2.GetComponent<BehaviorMecanim>().Node_OrientTowards(npcLookAt.transform.position),
                          npc3.GetComponent<BehaviorMecanim>().Node_OrientTowards(npcLookAt.transform.position),
                         new SequenceParallel(
                           npc4.GetComponent<BehaviorMecanim>().Node_OrientTowards(npcLookAt.transform.position),
                            npc5.GetComponent<BehaviorMecanim>().Node_OrientTowards(npcLookAt.transform.position)
                        ),


                         new SequenceParallel(
                        npc1.GetComponent<BehaviorMecanim>().Node_Sit(),
                         npc2.GetComponent<BehaviorMecanim>().Node_Sit()
                         ),
                          npc3.GetComponent<BehaviorMecanim>().Node_Sit(),
                          new SequenceParallel(
                           npc4.GetComponent<BehaviorMecanim>().Node_Sit(),
                            npc5.GetComponent<BehaviorMecanim>().Node_Sit()
                        ),
                         new SequenceParallel(
                        npc1.GetComponent<BehaviorMecanim>().Node_HeadLook(TV.transform.position),
                         npc2.GetComponent<BehaviorMecanim>().Node_HeadLook(TV.transform.position),
                          npc3.GetComponent<BehaviorMecanim>().Node_HeadLook(TV.transform.position),
                           npc4.GetComponent<BehaviorMecanim>().Node_HeadLook(TV.transform.position),
                            npc5.GetComponent<BehaviorMecanim>().Node_HeadLook(TV.transform.position)
                        ));
    return Story;
        
	}
}
