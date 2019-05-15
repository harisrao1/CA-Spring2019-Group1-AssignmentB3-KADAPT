using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;

public class MyBehaviorTree : MonoBehaviour
{
    // Characters
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


    public GameObject Door;
    bool open;
    // Use this for initialization
    void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
        open = false;
       

    }
	
	// Update is called once per frame
	void Update ()
	{
        

        //  if (open == true )
        //   {
        //      Door.transform.Translate(new Vector3(0f, -10f, 0f) );
        //  }
    }

	protected Node ST_ApproachAndWait(Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( dan.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
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
    protected Node ST_OpenDoor( InteractionObject button, Transform posdan, Transform posjake)
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
            rob.GetComponent<BehaviorMecanim>().Node_StartInteraction(FullBodyBipedEffector.RightHand, b) ;
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
        Val<Vector3> lookatbat = Val.V(() => batobj.transform.position);
        Val<Vector3> lookatdan = Val.V(() => dan.transform.position);
        Val<Vector3> lookatrob = Val.V(() => robber.transform.position);
        Val<Vector3> lookatbut = Val.V(() => buttonobj.transform.position);
        Val<Vector3> gohere = Val.V(() => buttonWaypoint2.transform.position);
        Val<Vector3> lookatwall = Val.V(() => Wall.transform.position);

        int num = RandomNumber(1, 3);
        Debug.Log("NUM :" + num);
        if (num == 1)
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
                        ),
                            // Dan story

                            this.ST_shakeHands(dan, jake, this.DanMeetsJake),
                            new LeafWait(500),
                            this.ST_GoSit(this.danSeat, this.jakeSeat, table),
                            new LeafWait(500),
                            new SequenceParallel(
                            this.ST_Talk(),
                            new LeafWait(100),
                            robber.GetComponent<BehaviorMecanim>().Node_OrientTowards(lookatbat),
                            robber.GetComponent<BehaviorMecanim>().Node_HeadLook(lookatbat),
                            PickBat(robber, bat, batobj)
                            ),
                            // robber.GetComponent<BehaviorMecanim>().Node_OrientTowards(lookattab),
                            // robber.GetComponent<BehaviorMecanim>().Node_HeadLook(lookattab),

                            this.ST_RobberEntry(RobberEntry),
                            robber.GetComponent<BehaviorMecanim>().Node_OrientTowards(lookatdan),
                            robber.GetComponent<BehaviorMecanim>().Node_HeadLook(lookatdan),
                            new LeafWait(500),
                            dan.GetComponent<BehaviorMecanim>().Node_HeadLook(lookatrob),
                            jake.GetComponent<BehaviorMecanim>().Node_HeadLook(lookatrob),
                            new LeafWait(10),
                            new SequenceParallel(

                                robber.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("pistolaim", 1400),
                                dan.GetComponent<BehaviorMecanim>().Node_Stand(),
                                jake.GetComponent<BehaviorMecanim>().Node_Stand()

                            ),
                            dan.GetComponent<BehaviorMecanim>().Node_HeadLook(lookatbut),
                            jake.GetComponent<BehaviorMecanim>().Node_HeadLook(lookatbut),
                            new SequenceParallel(
                            robber.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("pistolaim", 7000),
                            this.ST_OpenDoor(button, buttonWaypointdan, buttonWaypointjake)
                            ),
                            new SequenceParallel(
                            dan.GetComponent<BehaviorMecanim>().Node_HeadLook(lookatwall),
                            jake.GetComponent<BehaviorMecanim>().Node_HeadLook(lookatwall),
                            dan.GetComponent<BehaviorMecanim>().Node_GoTo(gohere),
                            jake.GetComponent<BehaviorMecanim>().Node_GoTo(gohere)
                            ),

                            new LeafWait(500),
                            this.ST_Escape(escape),

                            new LeafWait(100),
                            dan.GetComponent<BehaviorMecanim>().Node_HeadLook(npcLookAt.transform.position),
                            jake.GetComponent<BehaviorMecanim>().Node_HeadLook(npcLookAt.transform.position),
                            new LeafWait(100),

                            robber.GetComponent<BehaviorMecanim>().Node_HeadLookStop(),
                            new LeafWait(200),
                             robber.GetComponent<BehaviorMecanim>().Node_HeadLook(npc4.transform.position),

                             new SequenceParallel(
                            robber.GetComponent<BehaviorMecanim>().Node_GoTo(robbertheaterwaypoint.transform.position),
                            dan.GetComponent<BehaviorMecanim>().Node_GoTo(exit.transform.position),
                            jake.GetComponent<BehaviorMecanim>().Node_GoTo(exit.transform.position)
                            ),

                            new SequenceParallel(
                            robber.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("pistolaim", 1400),
                            new Sequence(
                            new SequenceParallel(
                        npc1.GetComponent<BehaviorMecanim>().Node_Stand(),
                         npc2.GetComponent<BehaviorMecanim>().Node_Stand()
                         ),
                          npc3.GetComponent<BehaviorMecanim>().Node_Stand(),
                          new SequenceParallel(
                           npc4.GetComponent<BehaviorMecanim>().Node_Stand(),
                            npc5.GetComponent<BehaviorMecanim>().Node_Stand()
                        ),
                          new LeafWait(300),
                          npc1.GetComponent<BehaviorMecanim>().Node_OrientTowards(robbertheaterwaypoint.transform.position),
                         npc2.GetComponent<BehaviorMecanim>().Node_OrientTowards(robbertheaterwaypoint.transform.position),
                          npc3.GetComponent<BehaviorMecanim>().Node_OrientTowards(robbertheaterwaypoint.transform.position),
                         new SequenceParallel(
                           npc4.GetComponent<BehaviorMecanim>().Node_OrientTowards(robbertheaterwaypoint.transform.position),
                            npc5.GetComponent<BehaviorMecanim>().Node_OrientTowards(robbertheaterwaypoint.transform.position)
                        )
                        )
                        ),
                            
                         new SequenceParallel(
                            robber.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("pistolaim", 18500),
                            npc1.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("HANDSUP", 18500),
                            npc2.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("HANDSUP", 18500),
                            npc3.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("HANDSUP", 18500),
                            npc4.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("HANDSUP", 18500),
                            npc5.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("HANDSUP", 15500)
                            )


                );
            return Story;
        }
        else
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
                        ),


                             this.ST_shakeHands(dan, jake, this.DanMeetsJake),
                             new LeafWait(500),
                             //  this.ST_GoSit(this.danSeat, this.jakeSeat, table),
                             //  new LeafWait(500),

                             //  this.ST_Talk(),
                             //  new LeafWait(100),

                             dan.GetComponent<BehaviorMecanim>().Node_OrientTowards(danfindsrobber.transform.position),
                             jake.GetComponent<BehaviorMecanim>().Node_OrientTowards(danfindsrobber.transform.position),
                             new SequenceParallel(
                             dan.GetComponent<BehaviorMecanim>().Node_GoTo(danfindsrobber.transform.position),
                             jake.GetComponent<BehaviorMecanim>().Node_GoTo(danfindsrobber.transform.position)),

                             new LeafWait(100),
                             new SequenceParallel(
                             dan.GetComponent<BehaviorMecanim>().Node_HeadLook(robber.transform.position),
                             jake.GetComponent<BehaviorMecanim>().Node_HeadLook(robber.transform.position)),

                             dan.GetComponent<BehaviorMecanim>().Node_OrientTowards(lookatbut),
                             jake.GetComponent<BehaviorMecanim>().Node_OrientTowards(lookatbut),

                              dan.GetComponent<BehaviorMecanim>().Node_HeadLook(lookatbut),
                             jake.GetComponent<BehaviorMecanim>().Node_HeadLook(lookatbut),

                             new LeafWait(100),
                                this.ST_OpenDoor(button, buttonWaypointdan, buttonWaypointjake),

                             new SequenceParallel(
                             dan.GetComponent<BehaviorMecanim>().Node_HeadLook(lookatwall),
                             jake.GetComponent<BehaviorMecanim>().Node_HeadLook(lookatwall),
                             dan.GetComponent<BehaviorMecanim>().Node_GoTo(gohere),
                             jake.GetComponent<BehaviorMecanim>().Node_GoTo(gohere)
                             ),

                             new LeafWait(500),
                             this.ST_Escape(escape),
                             new LeafWait(100),

                             new SequenceParallel(
                                 new Sequence (
                             dan.GetComponent<BehaviorMecanim>().Node_GoTo(robbertheaterwaypoint.transform.position),
                             dan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CROWDPUMP", 1500)),
                             jake.GetComponent<BehaviorMecanim>().Node_GoTo(exit.transform.position)),

                             dan.GetComponent<BehaviorMecanim>().Node_HeadLook(npc4Seat.transform.position),

                           new SequenceParallel(
                            dan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CROWDPUMP", 4500),

                            // Npc getting up
                            new Sequence(
                            npc1.GetComponent<BehaviorMecanim>().Node_HeadLook(robbertheaterwaypoint.transform.position),
                         npc2.GetComponent<BehaviorMecanim>().Node_HeadLook(robbertheaterwaypoint.transform.position),
                          npc3.GetComponent<BehaviorMecanim>().Node_HeadLook(robbertheaterwaypoint.transform.position),
                         new SequenceParallel(
                           npc4.GetComponent<BehaviorMecanim>().Node_HeadLook(robbertheaterwaypoint.transform.position),
                            npc5.GetComponent<BehaviorMecanim>().Node_HeadLook(robbertheaterwaypoint.transform.position)
                        ),

                          new SequenceParallel(
                        npc1.GetComponent<BehaviorMecanim>().Node_Stand(),
                         npc2.GetComponent<BehaviorMecanim>().Node_Stand()
                         ),
                          npc3.GetComponent<BehaviorMecanim>().Node_Stand(),
                          new SequenceParallel(
                           npc4.GetComponent<BehaviorMecanim>().Node_Stand(),
                            npc5.GetComponent<BehaviorMecanim>().Node_Stand()
                        )
                          )), // sequence end

                          npc1.GetComponent<BehaviorMecanim>().Node_OrientTowards(robbertheaterwaypoint.transform.position),
                         npc2.GetComponent<BehaviorMecanim>().Node_OrientTowards(robbertheaterwaypoint.transform.position),
                          npc3.GetComponent<BehaviorMecanim>().Node_OrientTowards(robbertheaterwaypoint.transform.position),
                         new SequenceParallel(
                           npc4.GetComponent<BehaviorMecanim>().Node_OrientTowards(robbertheaterwaypoint.transform.position),
                            npc5.GetComponent<BehaviorMecanim>().Node_OrientTowards(robbertheaterwaypoint.transform.position)
                        ),

                         new SequenceParallel(
                        npc1.GetComponent<BehaviorMecanim>().Node_GoTo(npc1.transform.position),
                         npc2.GetComponent<BehaviorMecanim>().Node_GoTo(npc2.transform.position),
                          npc3.GetComponent<BehaviorMecanim>().Node_GoTo(npc3.transform.position),
                           npc4.GetComponent<BehaviorMecanim>().Node_GoTo(npc4.transform.position),
                            npc5.GetComponent<BehaviorMecanim>().Node_GoTo(npc5.transform.position),
                            dan.GetComponent<BehaviorMecanim>().Node_GoTo(exit.transform.position)
                        ),

            // robber coming
                             robber.GetComponent<BehaviorMecanim>().Node_OrientTowards(lookatbat),
                             robber.GetComponent<BehaviorMecanim>().Node_HeadLook(lookatbat),
                             PickBat(robber, bat, batobj),

                             // robber.GetComponent<BehaviorMecanim>().Node_OrientTowards(lookattab),
                             // robber.GetComponent<BehaviorMecanim>().Node_HeadLook(lookattab),

                             this.ST_RobberEntry(RobberEntry),
                             robber.GetComponent<BehaviorMecanim>().Node_OrientTowards(table.transform.position),

                             robber.GetComponent<BehaviorMecanim>().Node_OrientTowards(robbertheaterwaypoint.transform.position),
                             robber.GetComponent<BehaviorMecanim>().Node_GoTo(robbertheaterwaypoint.transform.position),
                             robber.GetComponent<BehaviorMecanim>().Node_HeadLook(TV.transform.position),
                             new LeafWait(2000),
                              robber.GetComponent<BehaviorMecanim>().Node_GoTo(npc4Seat.transform.position),
                               robber.GetComponent<BehaviorMecanim>().Node_OrientTowards(npcLookAt.transform.position),
                               robber.GetComponent<BehaviorMecanim>().Node_Sit(),


                               new LeafWait(15000)



                 );
            return Story;
        }
	}

    public int RandomNumber(int min, int max)
    {
        System.Random random = new System.Random();
        return random.Next(min, max);
    }
}
