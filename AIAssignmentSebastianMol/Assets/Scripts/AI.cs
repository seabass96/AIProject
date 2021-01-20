using System.Collections.Generic;
using UnityEngine;

/*****************************************************************************************************************************
 * Write your core this code in this file here. The private variable 'agentScript' contthisns all the agents actions which are listed
 * below. Ensure your code it clear and organised and commented.
 *
 * Unity Tags
 * public static class Tags
 * public const string BlueTeam = "Blue Team";	The tag assigned to blue team members.
 * public const string RedTeam = "Red Team";	The tag assigned to red team members.
 * public const string Collectable = "Collectable";	The tag assigned to collectable items (health kit and power up).
 * public const string Flag = "Flag";	The tag assigned to the flags, blue or red.
 * 
 * Unity GameObject names
 * public static class Names
 * public const string PowerUp = "Power Up";	Power up name
 * public const string HealthKit = "Health Kit";	Health kit name.
 * public const string BlueFlag = "Blue Flag";	The blue teams flag name.
 * public const string RedFlag = "Red Flag";	The red teams flag name.
 * public const string RedBase = "Red Base";    The red teams base name.
 * public const string BlueBase = "Blue Base";  The blue teams base name.
 * public const string BlueTeamMember1 = "Blue Team Member 1";	Blue team member 1 name.
 * public const string BlueTeamMember2 = "Blue Team Member 2";	Blue team member 2 name.
 * public const string BlueTeamMember3 = "Blue Team Member 3";	Blue team member 3 name.
 * public const string RedTeamMember1 = "Red Team Member 1";	Red team member 1 name.
 * public const string RedTeamMember2 = "Red Team Member 2";	Red team member 2 name.
 * public const string RedTeamMember3 = "Red Team Member 3";	Red team member 3 name.
 * 
 * _agentData properties and public variables
 * public bool IsPoweredUp;	Have we powered up, true if we’re powered up, false otherwise.
 * public int CurrentHitPoints;	Our current hit points as an integer
 * public bool HasFriendlyFlag;	True if we have collected our own flag
 * public bool HasEnemyFlag;	True if we have collected the enemy flag
 * public GameObject FriendlyBase; The friendly base GameObject
 * public GameObject EnemyBase;    The enemy base GameObject
 * public int FriendlyScore; The friendly teams score
 * public int EnemyScore;       The enemy teams score
 * 
 * _agentActions methods
 * public bool MoveTo(GameObject target)	Move towards a target object. Takes a GameObject representing the target object as a parameter. Returns true if the location is on the navmesh, false otherwise.
 * public bool MoveTo(Vector3 target)	Move towards a target location. Takes a Vector3 representing the destination as a parameter. Returns true if the location is on the navmesh, false otherwise.
 * public bool MoveToRandomLocation()	Move to a random location on the level, returns true if the location is on the navmesh, false otherwise.
 * public void CollectItem(GameObject item)	Pick up an item from the level which is within reach of the agent and put it in the inventory. Takes a GameObject representing the item as a parameter.
 * public void DropItem(GameObject item)	Drop an inventory item or the flag at the agents’ location. Takes a GameObject representing the item as a parameter.
 * public void UseItem(GameObject item)	Use an item in the inventory (currently only health kit or power up). Takes a GameObject representing the item to use as a parameter.
 * public void AttackEnemy(GameObject enemy)	Attack the enemy if they are close enough. ). Takes a GameObject representing the enemy as a parameter.
 * public void Flee(GameObject enemy)	Move in the opposite direction to the enemy. Takes a GameObject representing the enemy as a parameter.
 * 
 * _agentSenses properties and methods
 * public List<GameObject> GetObjectsInViewByTag(string tag)	Return a list of objects with the same tag. Takes a string representing the Unity tag. Returns null if no objects with the specified tag are in view.
 * public GameObject GetObjectInViewByName(string name)	Returns a specific GameObject by name in view range. Takes a string representing the objects Unity name as a parameter. Returns null if named object is not in view.
 * public List<GameObject> GetObjectsInView()	Returns a list of objects within view range. Returns null if no objects are in view.
 * public List<GameObject> GetCollectablesInView()	Returns a list of objects with the tag Collectable within view range. Returns null if no collectable objects are in view.
 * public List<GameObject> GetFriendliesInView()	Returns a list of friendly team this agents within view range. Returns null if no friendlies are in view.
 * public List<GameObject> GetEnemiesInView()	Returns a list of enemy team this agents within view range. Returns null if no enemy are in view.
 * public bool IsItemInReach(GameObject item)	Checks if we are close enough to a specific collectible item to pick it up), returns true if the object is close enough, false otherwise.
 * public bool IsInAttackRange(GameObject target)	Check if we're with attacking range of the target), returns true if the target is in range, false otherwise.
 * 
 * _agentInventory properties and methods
 * public bool AddItem(GameObject item)	Adds an item to the inventory if there's enough room (max capacity is 'Constants.InventorySize'), returns true if the item has been successfully added to the inventory, false otherwise.
 * public GameObject GetItem(string itemName)	Retrieves an item from the inventory as a GameObject, returns null if the item is not in the inventory.
 * public bool HasItem(string itemName)	Checks if an item is stored in the inventory, returns true if the item is in the inventory, false otherwise.
 * 
 * You can use the game objects name to access a GameObject from the sensing system. Thereafter all methods require the GameObject as a parameter.
 * 
*****************************************************************************************************************************/

/// <summary>
/// Implement your this script here, you can put everything in this file, or better still, break your code up into multiple files
/// and call your script here in the Update method. This class includes all the data members you need to control your this agent
/// get information about the world, manage the this inventory and access essential information about your this.
///
/// You may use any this algorithm you like, but please try to write your code properly and professionaly and don't use code obtthisned from
/// other sources, including the labs.
///
/// See the assessment brief for more detthisls
/// </summary>
public class AI : MonoBehaviour
{
    // Gives access to important data about the this agent (see above)
    internal AgentData _agentData;
    // Gives access to the agent senses
    internal Sensing _agentSenses;
    // gives access to the agents inventory
    internal InventoryController _agentInventory;
    // This is the script contthisning the this agents actions
    // e.g. agentScript.MoveTo(enemy);
    internal AgentActions _agentActions;
    //the decision tree used by this agent
    private DecisionTree decisionTree;
    //================== bools for testing
    public bool looseHealth = false;
    public bool PickUpFlagTest = false;
    public bool droptestflag = false;
    public bool pickUpHealth = false;
    //=====================================
    //type of enemy ai e.g. commando
    public enemyType enemyType;
    //attack range for assasin decision
    internal float attackRange = 4;


    // Use this for initialization
    void Start ()
    {
        // Initialise the accessable script components
        _agentData = GetComponent<AgentData>();
        _agentActions = GetComponent<AgentActions>();
        _agentSenses = GetComponentInChildren<Sensing>();
        _agentInventory = GetComponentInChildren<InventoryController>();

        switch (enemyType)
        {
            case enemyType.COMMANDO:
                ComandoDecisions();
                break;
            case enemyType.HEALER:
                MedicDecsision();
                break;
            case enemyType.ASSASIN:
                AssasinDecision();
                break;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        decisionTree.Execute();

        //if (looseHealth)
        //{
        //    _agentData.TakeDamage(20);
        //    looseHealth = false;
        //}

        //if (PickUpFlagTest)
        //{
        //    GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
        //    _agentActions.CollectItem(flags[0]);
        //    PickUpFlagTest = false;
        //}

        //if(droptestflag)
        //{
        //    droptestflag = false;
        //}

        //if(pickUpHealth)
        //{
        //    if(_agentActions.CollectItem(GameObject.Find("Health Kit"))) Debug.Log("has picked up health");
        //    pickUpHealth = false;
        //}

        ////Debug.Log(_agentInventory.HasItem("Red Flag") + " " + gameObject.name);
        //// Debug.Log(_agentInventory.GetInventoryCount() + " " + gameObject.name);
        ////if (_agentInventory.GetItem("Power Up")) Debug.Log(gameObject.name + " has the power up");
    }

    /// <summary>
    /// the decison tree set up required for the commando class
    /// </summary>
    public void ComandoDecisions()
    {
        #region actions
        //just for test so it dosent break
        ActionNode DoNothingAction = new ActionNode(Actions.DoNothing, this);

        //go to home base
        ActionNode MoveToHomeBaseAction = new ActionNode(Actions.MoveToHomeBase, this);

        // move to health
        ActionNode MoveTowardsHealthPackAction = new ActionNode(Actions.MoveTowardsHealthKit, this);

        //pick up health
        ActionNode PickUpHealthPackAction = new ActionNode(Actions.PickUpHealthKitAndUse, this);

        //drop flag Action
        ActionNode DropEnemyFlagAction = new ActionNode(Actions.DropEnemyFlag, this);

        //drop any flag
        ActionNode DropAnyFlagAction = new ActionNode(Actions.DropAnyFlag, this);

        //move to power up
        ActionNode MoveTowardsPowerUpAction = new ActionNode(Actions.MoveTowardPowerUp, this);

        //pick up power up
        ActionNode PickUpPowerUpAction = new ActionNode(Actions.PickUpPowerUpAndUse, this);

        //move to enemy with flag
        ActionNode MoveToEnemyWithFlagAction = new ActionNode(Actions.MoveToEnemyWithFlag, this);

        //move to enemy base
        ActionNode MoveToEnemyBaseAction = new ActionNode(Actions.MoveToEnemyBase, this);

        //move to friendly with flag
        ActionNode MoveToFriendlyWithFlagAction = new ActionNode(Actions.MoveToFriendlyWithEnemyFlag, this);

        //attack closes enemy
        ActionNode AttackClosestEnemyAction = new ActionNode(Actions.AttackEnemy, this);

        //move to team flag
        ActionNode MoveTowardsTeamFlagAction = new ActionNode(Actions.MoveTowardsTeamFlag, this);

        //pick up team flag
        ActionNode PickUpTeamFlagAction = new ActionNode(Actions.PickUpFlagTeamFlag, this);

        //move to enemy flag
        ActionNode MoveToEnemyFlagAction = new ActionNode(Actions.MoveTowardsEnemyTeamFlag, this);

        //pick up enemy flag
        ActionNode PickUpEnemyFlagAction = new ActionNode(Actions.PickUpEnemyFlag, this);

        //move towards closest enemy
        ActionNode MoveToClosestEnemyAction = new ActionNode(Actions.MoveTowardsClosestEnemy, this);
        #endregion

        //========================================== get home with flag branch

        //have you got the flag
        Decision DoseAgentHaveFlagDecision = new Decision(Decisions.DoseAgentHaveFlag);
        DecisionNode DoseAgentHaveFlagNode = new DecisionNode(DoseAgentHaveFlagDecision, this);

        Decision IsAgentAtFriendlyBaseDecision = new Decision(Decisions.IsAgentOnFriendlyBase);
        DecisionNode IsAgentAtFriendlyBaseNode = new DecisionNode(IsAgentAtFriendlyBaseDecision, this);
        DoseAgentHaveFlagNode.AddYesChild(IsAgentAtFriendlyBaseNode);
        IsAgentAtFriendlyBaseNode.AddYesChild(DropAnyFlagAction);
        IsAgentAtFriendlyBaseNode.AddNoChild(MoveToHomeBaseAction);

        //========================================== health is low branch

        //is health low
        Decision IsLowOnHealthDecision = new Decision(Decisions.IsLowOnHealth);
        DecisionNode IsLowOnHealthNode = new DecisionNode(IsLowOnHealthDecision, this);
        DoseAgentHaveFlagNode.AddNoChild(IsLowOnHealthNode);//joiner


        //is health on the map
        Decision IsHealthPacOnTheMapDecision = new Decision(Decisions.IsThereAHealthKitOnMap);
        DecisionNode isHealthPackOnMapNode = new DecisionNode(IsHealthPacOnTheMapDecision, this);
        IsLowOnHealthNode.AddYesChild(isHealthPackOnMapNode);


        //can you pick up health pac
        Decision IsHealthPacInReachDecsision = new Decision(Decisions.IsHealthKitInRange);
        DecisionNode isHealthPackInReachNode = new DecisionNode(IsHealthPacInReachDecsision, this);
        isHealthPackOnMapNode.AddYesChild(isHealthPackInReachNode);
        isHealthPackInReachNode.AddNoChild(MoveTowardsHealthPackAction);
        isHealthPackInReachNode.AddYesChild(PickUpHealthPackAction);

        //====================================================================================if close to enemy attack
        Decision IsAgentCloseTooEnemyDecision = new Decision(Decisions.CanYouHitClosestEnemy);
        DecisionNode IsAgentCloseTooEnemyNode = new DecisionNode(IsAgentCloseTooEnemyDecision, this);
         IsLowOnHealthNode.AddNoChild(IsAgentCloseTooEnemyNode);//joiner
         isHealthPackOnMapNode.AddNoChild(IsAgentCloseTooEnemyNode);//joiner

        IsAgentCloseTooEnemyNode.AddYesChild(AttackClosestEnemyAction);


        //====================================================================== get flag back from enemy branch

        //dose enemy have flag
        Decision DoseEnemyHaveFlagDecision = new Decision(Decisions.DoseEnemyHaveOurFlag);
        DecisionNode DoseEnemyHaveFlagNode = new DecisionNode(DoseEnemyHaveFlagDecision, this);
        // IsPowerUpOnMapNode.AddNoChild(DoseEnemyHaveFlagNode);//joiner
        IsAgentCloseTooEnemyNode.AddNoChild(DoseEnemyHaveFlagNode); //joiner



        //can you seee enmey with flag
        Decision CanYouSeeEnemyWithFlagDecision = new Decision(Decisions.CanYouSeeEnemyWithFlag);
        DecisionNode CanYouSeeEnemyWithFlagAction = new DecisionNode(CanYouSeeEnemyWithFlagDecision, this);
        DoseEnemyHaveFlagNode.AddYesChild(CanYouSeeEnemyWithFlagAction);
        CanYouSeeEnemyWithFlagAction.AddNoChild(MoveToEnemyBaseAction);


        // add a are you in range of enemy decision
        Decision isAgentInRangeOfEnemyFlagCarrierDecision = new Decision(Decisions.CanYouAttackEnemyWithFalg);
        DecisionNode isAgentInRangeOfEnemyFlagCarrierAction = new DecisionNode(isAgentInRangeOfEnemyFlagCarrierDecision, this);
        CanYouSeeEnemyWithFlagAction.AddYesChild(isAgentInRangeOfEnemyFlagCarrierAction);
        isAgentInRangeOfEnemyFlagCarrierAction.AddNoChild(MoveToEnemyWithFlagAction);
        isAgentInRangeOfEnemyFlagCarrierAction.AddYesChild(AttackClosestEnemyAction);

        //===================================================================================================== protect flag carrier branch

        Decision DoseOurTeamHaveFlagDecsison = new Decision(Decisions.DoseOurTeamHaveEnemyFlag);
        DecisionNode DoseOurTeamHaveFlagNode = new DecisionNode(DoseOurTeamHaveFlagDecsison, this);
        DoseEnemyHaveFlagNode.AddNoChild(DoseOurTeamHaveFlagNode);//joner

        Decision DoseAgentHaveEnemyFlagDecsion = new Decision(Decisions.DoseAgentHaveFlag);
        DecisionNode DoseAgetHaveEnemyFlagNode = new DecisionNode(DoseAgentHaveEnemyFlagDecsion, this);
        DoseOurTeamHaveFlagNode.AddYesChild(DoseAgetHaveEnemyFlagNode);
        DoseAgetHaveEnemyFlagNode.AddYesChild(MoveToHomeBaseAction);

        Decision IsAgentNeerFlagCarrierDecision = new Decision(Decisions.AreYouNeerTeamFlagCarrier);
        DecisionNode IsAgentNeerFlagCarrierNode = new DecisionNode(IsAgentNeerFlagCarrierDecision, this);
        DoseAgetHaveEnemyFlagNode.AddNoChild(IsAgentNeerFlagCarrierNode);
        IsAgentNeerFlagCarrierNode.AddNoChild(MoveToFriendlyWithFlagAction);
        IsAgentNeerFlagCarrierNode.AddYesChild(AttackClosestEnemyAction);

        //================================================================================================  get flag back to base
        Decision IsMyTeamFlagNotOnOurBaseDescison = new Decision(Decisions.IsTeamFlagNotOnBase);
        DecisionNode IsMyTeamFlagNotOnOurBaseNode = new DecisionNode(IsMyTeamFlagNotOnOurBaseDescison, this);
        DoseOurTeamHaveFlagNode.AddNoChild(IsMyTeamFlagNotOnOurBaseNode);//joiner


        Decision IsAgentInRangeOfTeamFlagDecision = new Decision(Decisions.CanYouPickUpTeamFlag);
        DecisionNode IsAgentInRangeOfTeamFlagNode = new DecisionNode(IsAgentInRangeOfTeamFlagDecision, this);
        IsMyTeamFlagNotOnOurBaseNode.AddYesChild(IsAgentInRangeOfTeamFlagNode);
        IsAgentInRangeOfTeamFlagNode.AddNoChild(MoveTowardsTeamFlagAction);
        IsAgentInRangeOfTeamFlagNode.AddYesChild(PickUpTeamFlagAction);

        //============================================================================= power up branch
        ////is there a power up on map
        //Decision IsPowerUpOnMapDecision = new Decision(Decisions.IsTherPowerUpOnMap);
        //DecisionNode IsPowerUpOnMapNode = new DecisionNode(IsPowerUpOnMapDecision, this);
        //// IsAgentCloseTooEnemyNode.AddNoChild(IsPowerUpOnMapNode);//joiner
        //IsMyTeamFlagNotOnOurBaseNode.AddNoChild(IsPowerUpOnMapNode); //joiner


        ////are you neer a power up
        //Decision IsAgentNeerPowerUpDecision = new Decision(Decisions.AreYouNeerPowerUp);
        //DecisionNode IsAgentNeerPowerUpNode = new DecisionNode(IsAgentNeerPowerUpDecision, this);
        //IsPowerUpOnMapNode.AddYesChild(IsAgentNeerPowerUpNode);
        //IsAgentNeerPowerUpNode.AddNoChild(MoveTowardsPowerUpAction);
        //IsAgentNeerPowerUpNode.AddYesChild(PickUpPowerUpAction);

        //====================================================================================== defend enemy flag at base
        Decision IsEnemyFlagAtHomeBaseDecsision = new Decision(Decisions.IsEnemyFlagAtHomeBase);
        DecisionNode IsEnemyFlagAtHomeBaseNode = new DecisionNode(IsEnemyFlagAtHomeBaseDecsision, this);
        //IsPowerUpOnMapNode.AddNoChild(IsEnemyFlagAtHomeBaseNode);//joiner  
        IsMyTeamFlagNotOnOurBaseNode.AddNoChild(IsEnemyFlagAtHomeBaseNode); //joiner

        Decision IsAgentAtFriendlyBaseDecision2 = new Decision(Decisions.IsAgentOnFriendlyBase);
        DecisionNode IsAgentAtFriendlyBaseNode2 = new DecisionNode(IsAgentAtFriendlyBaseDecision2, this);
        IsAgentAtFriendlyBaseNode2.AddNoChild(MoveToHomeBaseAction);
        IsEnemyFlagAtHomeBaseNode.AddYesChild(IsAgentAtFriendlyBaseNode2);

        Decision CanYouSeeEnemyDecision = new Decision(Decisions.CanAgentSeeAnEnemy);
        DecisionNode CanYouSeeEnemyNode = new DecisionNode(CanYouSeeEnemyDecision, this);
        CanYouSeeEnemyNode.AddNoChild(DoNothingAction);
        IsAgentAtFriendlyBaseNode2.AddYesChild(CanYouSeeEnemyNode);

        Decision CanAgentHitClosesEnemyDecision = new Decision(Decisions.CanYouHitClosestEnemy);
        DecisionNode CanAgentHitClosesEnemyNode = new DecisionNode(CanAgentHitClosesEnemyDecision, this);
        CanYouSeeEnemyNode.AddYesChild(CanAgentHitClosesEnemyNode);
        CanAgentHitClosesEnemyNode.AddNoChild(MoveToClosestEnemyAction);
        CanAgentHitClosesEnemyNode.AddYesChild(AttackClosestEnemyAction);

        //========================================================================================= retrive enemy flag 

        Decision IsAgentInRangeOfEnmeyFlagDecision = new Decision(Decisions.CanYouPickUpEnemyFlag);
        DecisionNode IsAgentInRangeOfEnmeyFlagNode = new DecisionNode(IsAgentInRangeOfEnmeyFlagDecision, this);

        IsEnemyFlagAtHomeBaseNode.AddNoChild(IsAgentInRangeOfEnmeyFlagNode);
        IsAgentInRangeOfEnmeyFlagNode.AddNoChild(MoveToEnemyFlagAction);
        IsAgentInRangeOfEnmeyFlagNode.AddYesChild(PickUpEnemyFlagAction);



        decisionTree = new DecisionTree(DoseAgentHaveFlagNode);

        
    }

    /// <summary>
    /// the decison tree set up required for the medic class
    /// </summary>
    public void MedicDecsision()
    {
        #region actions
        //just for test so it dosent break
        ActionNode DoNothingAction = new ActionNode(Actions.DoNothing, this);

        // move to health
        ActionNode MoveTowardsHealthPackAction = new ActionNode(Actions.MoveTowardsHealthKit, this);

        //pick up health pac
        ActionNode PickUpHealthPackIntoInventoryAction = new ActionNode(Actions.PickUpHealthKitAndPutInInventory, this);

        //move to power up
        ActionNode MoveTowardsPowerUpAction = new ActionNode(Actions.MoveTowardPowerUp, this);

        //pick up power up and put in inventory 
        ActionNode PickUpPowerUpIntoInventoryAction = new ActionNode(Actions.PickUpPowerUpAndPutInInventory, this);

        //move to team mate with lowest health action
        ActionNode MoveToTeamMateWithLowestHealthAction = new ActionNode(Actions.MoveToTeamMateWithLowestHealth, this);

        //heal closest teammate
        ActionNode HealClosestTeamMateAction = new ActionNode(Actions.GiveHealthKitToClosestTeamMamber, this);

        //move to highest health team mate
        ActionNode MoveToHighestHealthTeammateAction = new ActionNode(Actions.MoveToTeamMateWithHighestHealth, this);

        //power up higest health team mate
        ActionNode PowerUpClosetsTeamMateAction = new ActionNode(Actions.GivePowerUpToClosestTeamMamber, this);

        //move to friendly with flag
        ActionNode MoveToFriendlyWithFlagAction = new ActionNode(Actions.MoveToFriendlyWithEnemyFlag, this);

        //attack closes enemy
        ActionNode AttackClosestEnemyAction = new ActionNode(Actions.AttackEnemy, this);

        //go to home base
        ActionNode MoveToHomeBaseAction = new ActionNode(Actions.MoveToHomeBase, this);

        //move towards closest enemy
        ActionNode MoveToClosestEnemyAction = new ActionNode(Actions.MoveTowardsClosestEnemy, this);
        #endregion

        //======================================================================= collect health kit
        //is health on the map
        Decision IsHealthPacOnTheMapDecision = new Decision(Decisions.IsThereAHealthKitOnMap);
        DecisionNode isHealthPackOnMapNode = new DecisionNode(IsHealthPacOnTheMapDecision, this);
       
        //can you pick up health pac
        Decision IsHealthPacInReachDecsision = new Decision(Decisions.IsHealthKitInRange);
        DecisionNode isHealthPackInReachNode = new DecisionNode(IsHealthPacInReachDecsision, this);
        isHealthPackOnMapNode.AddYesChild(isHealthPackInReachNode);

        isHealthPackInReachNode.AddYesChild(PickUpHealthPackIntoInventoryAction);
        isHealthPackInReachNode.AddNoChild(MoveTowardsHealthPackAction);

        //===================================================================================heal lowest team member

        Decision IsTeamMateLowOnHealthDecision = new Decision(Decisions.IsTeamMateLowOnHealth);
        DecisionNode IsTeamMateLowOnHealthNode = new DecisionNode(IsTeamMateLowOnHealthDecision, this);
        isHealthPackOnMapNode.AddNoChild(IsTeamMateLowOnHealthNode);


        Decision DoseAgentHaveHealthInInventoryDecision = new Decision(Decisions.doseAgentHaveHealthKitInInventory);
        DecisionNode DoseAgentHaveHealthInInventoryNode = new DecisionNode(DoseAgentHaveHealthInInventoryDecision, this);
        IsTeamMateLowOnHealthNode.AddYesChild(DoseAgentHaveHealthInInventoryNode);

        Decision IsAgentNeerTeammateWithLowestHealthDecision = new Decision(Decisions.isAgentNearTeamMateWithLowestHealth);
        DecisionNode IsAgentNeerTeammateWithLowestHealthNode = new DecisionNode(IsAgentNeerTeammateWithLowestHealthDecision, this);
        DoseAgentHaveHealthInInventoryNode.AddYesChild(IsAgentNeerTeammateWithLowestHealthNode);
        IsAgentNeerTeammateWithLowestHealthNode.AddNoChild(MoveToTeamMateWithLowestHealthAction);
        IsAgentNeerTeammateWithLowestHealthNode.AddYesChild(HealClosestTeamMateAction);



        //=============================================================================  collect power up

        //is there a power up on map
        Decision IsPowerUpOnMapDecision = new Decision(Decisions.IsTherPowerUpOnMap);
        DecisionNode IsPowerUpOnMapNode = new DecisionNode(IsPowerUpOnMapDecision, this);
        IsTeamMateLowOnHealthNode.AddNoChild(IsPowerUpOnMapNode);//joiner
        DoseAgentHaveHealthInInventoryNode.AddNoChild(IsPowerUpOnMapNode);//joiner

        //are you neer a power up
        Decision IsAgentNeerPowerUpDecision = new Decision(Decisions.AreYouNeerPowerUp);
        DecisionNode IsAgentNeerPowerUpNode = new DecisionNode(IsAgentNeerPowerUpDecision, this);

        IsPowerUpOnMapNode.AddYesChild(IsAgentNeerPowerUpNode);
        IsAgentNeerPowerUpNode.AddNoChild(MoveTowardsPowerUpAction);
        IsAgentNeerPowerUpNode.AddYesChild(PickUpPowerUpIntoInventoryAction);

       
        //====================================================================================== power up highest health member

        Decision DoseTeamMateWithHighestHealthHavePowerUpDecision = new Decision(Decisions.DoseAgentWithHighestHealthHaveNotPowerUp);
        DecisionNode DoseTeamMateWithHighestHealthHavePowerUpNode = new DecisionNode(DoseTeamMateWithHighestHealthHavePowerUpDecision, this);
        IsPowerUpOnMapNode.AddNoChild(DoseTeamMateWithHighestHealthHavePowerUpNode); //joiner


        Decision IsPowerUpInAgentInventoryDecision = new Decision(Decisions.DoseAgentHavePowerUpInInventory);
        DecisionNode IsPowerUpInAgentInventoryNode = new DecisionNode(IsPowerUpInAgentInventoryDecision, this);
        DoseTeamMateWithHighestHealthHavePowerUpNode.AddYesChild(IsPowerUpInAgentInventoryNode);


        Decision IsAgentNearTeammateWithHighestHealthDecision = new Decision(Decisions.IsAgentInRangeOfTeamMateWithHighestHealth);
        DecisionNode IsAgentNearTeammateWithHighestHealthNode = new DecisionNode(IsAgentNearTeammateWithHighestHealthDecision, this);
        IsPowerUpInAgentInventoryNode.AddYesChild(IsAgentNearTeammateWithHighestHealthNode);
        IsAgentNearTeammateWithHighestHealthNode.AddNoChild(MoveToHighestHealthTeammateAction);
        IsAgentNearTeammateWithHighestHealthNode.AddYesChild(PowerUpClosetsTeamMateAction);


        //====================================================================================== support flag carrier
        Decision DoseOurTeamHaveFlagDecsison = new Decision(Decisions.DoseOurTeamHaveEnemyFlag);
        DecisionNode DoseOurTeamHaveFlagNode = new DecisionNode(DoseOurTeamHaveFlagDecsison, this);
        DoseTeamMateWithHighestHealthHavePowerUpNode.AddNoChild(DoseOurTeamHaveFlagNode);//joiner
        IsPowerUpInAgentInventoryNode.AddNoChild(DoseOurTeamHaveFlagNode);//joiner

        Decision DoseAgentHaveEnemyFlagDecsion = new Decision(Decisions.DoseAgentHaveFlag);
        DecisionNode DoseAgetHaveEnemyFlagNode = new DecisionNode(DoseAgentHaveEnemyFlagDecsion, this);
        DoseOurTeamHaveFlagNode.AddYesChild(DoseAgetHaveEnemyFlagNode);

        Decision IsAgentNeerFlagCarrierDecision = new Decision(Decisions.AreYouNeerTeamFlagCarrier);
        DecisionNode IsAgentNeerFlagCarrierNode = new DecisionNode(IsAgentNeerFlagCarrierDecision, this);
        DoseAgetHaveEnemyFlagNode.AddNoChild(IsAgentNeerFlagCarrierNode);
        IsAgentNeerFlagCarrierNode.AddNoChild(MoveToFriendlyWithFlagAction);
        IsAgentNeerFlagCarrierNode.AddYesChild(AttackClosestEnemyAction);

        //====================================================================================== protect flag
        Decision IsEnemyFlagAtHomeBaseDecsision = new Decision(Decisions.IsEnemyFlagAtHomeBase);
        DecisionNode IsEnemyFlagAtHomeBaseNode = new DecisionNode(IsEnemyFlagAtHomeBaseDecsision, this);
        DoseOurTeamHaveFlagNode.AddNoChild(IsEnemyFlagAtHomeBaseNode);//joiner       

        Decision IsAgentAtFriendlyBaseDecision2 = new Decision(Decisions.IsAgentOnFriendlyBase);
        DecisionNode IsAgentAtFriendlyBaseNode2 = new DecisionNode(IsAgentAtFriendlyBaseDecision2, this);
        IsAgentAtFriendlyBaseNode2.AddNoChild(MoveToHomeBaseAction);
        IsEnemyFlagAtHomeBaseNode.AddYesChild(IsAgentAtFriendlyBaseNode2);

        Decision CanYouSeeEnemyDecision = new Decision(Decisions.CanAgentSeeAnEnemy);
        DecisionNode CanYouSeeEnemyNode = new DecisionNode(CanYouSeeEnemyDecision, this);
        CanYouSeeEnemyNode.AddNoChild(DoNothingAction);
        IsAgentAtFriendlyBaseNode2.AddYesChild(CanYouSeeEnemyNode);

        Decision CanAgentHitClosesEnemyDecision = new Decision(Decisions.CanYouHitClosestEnemy);
        DecisionNode CanAgentHitClosesEnemyNode = new DecisionNode(CanAgentHitClosesEnemyDecision, this);
        CanYouSeeEnemyNode.AddYesChild(CanAgentHitClosesEnemyNode);
        CanAgentHitClosesEnemyNode.AddNoChild(MoveToClosestEnemyAction);
        CanAgentHitClosesEnemyNode.AddYesChild(AttackClosestEnemyAction);

        //====================================================================================== attack enemy / guard home base
        Decision IsAgentCloseTooEnemyDecision = new Decision(Decisions.CanYouHitClosestEnemy);
        DecisionNode IsAgentCloseTooEnemyNode = new DecisionNode(IsAgentCloseTooEnemyDecision, this);
        IsEnemyFlagAtHomeBaseNode.AddNoChild(IsAgentCloseTooEnemyNode);//joiner
        IsAgentCloseTooEnemyNode.AddYesChild(AttackClosestEnemyAction);
        IsAgentCloseTooEnemyNode.AddNoChild(MoveToHomeBaseAction);


        decisionTree = new DecisionTree(isHealthPackOnMapNode);
    }

    /// <summary>
    /// the decison tree set up required for the assasin class
    /// </summary>
    public void AssasinDecision()
    {
        #region actions
        //just for test so it dosent break
        ActionNode DoNothingAction = new ActionNode(Actions.DoNothing, this);

        //go to home base
        ActionNode MoveToHomeBaseAction = new ActionNode(Actions.MoveToHomeBase, this);

        //drop any flag
        ActionNode DropAnyFlagAction = new ActionNode(Actions.DropAnyFlag, this);

        //use health kit on self from inventory
        ActionNode UseHealthPackFromInventoryOnSelfAction = new ActionNode(Actions.UseHealthKitFromInventoryOnSelf, this);

        //attack closes enemy
        ActionNode AttackClosestEnemyAction = new ActionNode(Actions.AttackEnemy, this);

        //move to weakest enemy
        ActionNode MoveToWeakestEnemyAction = new ActionNode(Actions.MoveToWeakestEenemy, this);

        //move to closets map corner
        ActionNode MoveToClosestMapCornerAction = new ActionNode(Actions.MoveToClostesCorner, this);

        //move to cornere with health kit
        ActionNode MoveToCornerWithHealthKitAction = new ActionNode(Actions.MoveToCornerOfMapWithHealthKit, this);

        // move to health
        ActionNode MoveTowardsHealthPackAction = new ActionNode(Actions.MoveTowardsHealthKit, this);

        //pick up health pac
        ActionNode PickUpHealthPackIntoInventoryAction = new ActionNode(Actions.PickUpHealthKitAndPutInInventory, this);

        //drop health kit 
        ActionNode DropHealthKitAction = new ActionNode(Actions.DropHealthKit, this);

        //move to enemy flag
        ActionNode MoveToEnemyFlagAction = new ActionNode(Actions.MoveTowardsEnemyTeamFlag, this);

        //pick up enemy flag
        ActionNode PickUpEnemyFlagAction = new ActionNode(Actions.PickUpEnemyFlag, this);
        #endregion

        //========================================== get home with flag branch

        Decision DoseAgentHaveFlagDecision = new Decision(Decisions.DoseAgentHaveFlag);
        DecisionNode DoseAgentHaveFlagNode = new DecisionNode(DoseAgentHaveFlagDecision, this);

        Decision IsAgentAtFriendlyBaseDecision = new Decision(Decisions.IsAgentOnFriendlyBase);
        DecisionNode IsAgentAtFriendlyBaseNode = new DecisionNode(IsAgentAtFriendlyBaseDecision, this);
        DoseAgentHaveFlagNode.AddYesChild(IsAgentAtFriendlyBaseNode);
        IsAgentAtFriendlyBaseNode.AddYesChild(DropAnyFlagAction);
        IsAgentAtFriendlyBaseNode.AddNoChild(MoveToHomeBaseAction);
        //DoseAgentHaveFlagNode.AddNoChild(DoNothingAction); // test

        //========================================= heal self if you have health pac
        //is health low
        Decision IsLowOnHealthDecision = new Decision(Decisions.IsLowOnHealth);
        DecisionNode IsLowOnHealthNode = new DecisionNode(IsLowOnHealthDecision, this);
        DoseAgentHaveFlagNode.AddNoChild(IsLowOnHealthNode); //joiner

        Decision DoseAgentHaveHealthInInventoryDecision = new Decision(Decisions.doseAgentHaveHealthKitInInventory);
        DecisionNode DoseAgentHaveHealthInInventoryNode = new DecisionNode(DoseAgentHaveHealthInInventoryDecision, this);
        IsLowOnHealthNode.AddYesChild(DoseAgentHaveHealthInInventoryNode);
        DoseAgentHaveHealthInInventoryNode.AddYesChild(UseHealthPackFromInventoryOnSelfAction);

        //====================================================== attack weakest enemy or any enemy 

        Decision IsEnemyInAttackRangeDecision = new Decision(Decisions.IsEnemyInAttackRangeAssasin);
        DecisionNode IsEnemyInAttackRangeNode = new DecisionNode(IsEnemyInAttackRangeDecision, this);
        IsLowOnHealthNode.AddNoChild(IsEnemyInAttackRangeNode);//joiner
        DoseAgentHaveHealthInInventoryNode.AddNoChild(IsEnemyInAttackRangeNode);//joiner

        Decision IsAgentCloseToWeakestEnemyDecision = new Decision(Decisions.IsAgentCloseToWeakestEnemy);
        DecisionNode IsAgentCloseToWeakestEnemyNode = new DecisionNode(IsAgentCloseToWeakestEnemyDecision, this);
        IsEnemyInAttackRangeNode.AddYesChild(IsAgentCloseToWeakestEnemyNode);
        IsAgentCloseToWeakestEnemyNode.AddNoChild(MoveToWeakestEnemyAction); 
        IsAgentCloseToWeakestEnemyNode.AddYesChild(AttackClosestEnemyAction);

        //=================================================== if health pac is in corner of map wait for ambush

        Decision IsHealthKitInCornerOfMapDecision = new Decision(Decisions.IsThereAHealthPacInTheCornerOfTheMap);
        DecisionNode IsHealthKitInCornerOfMapNode = new DecisionNode(IsHealthKitInCornerOfMapDecision, this);
        IsEnemyInAttackRangeNode.AddNoChild(IsHealthKitInCornerOfMapNode); //joiner


        Decision IsAgentInCornerOfMapwithHealthKitDecsion = new Decision(Decisions.IsAgentInCornerOfMapWithHealhKit);
        DecisionNode IsAgentInCornerOfMapWithHealthKitNode = new DecisionNode(IsAgentInCornerOfMapwithHealthKitDecsion, this);
        IsHealthKitInCornerOfMapNode.AddYesChild(IsAgentInCornerOfMapWithHealthKitNode);
        IsAgentInCornerOfMapWithHealthKitNode.AddYesChild(DoNothingAction);
        IsAgentInCornerOfMapWithHealthKitNode.AddNoChild(MoveToCornerWithHealthKitAction);

        //=================================================== go pick up health pac
        //is health on the map
        Decision IsHealthPacOnTheMapDecision = new Decision(Decisions.IsThereAHealthKitOnMap);
        DecisionNode isHealthPackOnMapNode = new DecisionNode(IsHealthPacOnTheMapDecision, this);
        IsHealthKitInCornerOfMapNode.AddNoChild(isHealthPackOnMapNode); //test joiner

        //can you pick up health pac
        Decision IsHealthPacInReachDecsision = new Decision(Decisions.IsHealthKitInRange);
        DecisionNode isHealthPackInReachNode = new DecisionNode(IsHealthPacInReachDecsision, this);
        isHealthPackOnMapNode.AddYesChild(isHealthPackInReachNode);
        isHealthPackInReachNode.AddYesChild(PickUpHealthPackIntoInventoryAction);
        isHealthPackInReachNode.AddNoChild(MoveTowardsHealthPackAction);

        // =================================================== set up trap from medic
        Decision DoseAgentHaveHealthInInventory2Decision = new Decision(Decisions.doseAgentHaveHealthKitInInventory);
        DecisionNode DoseAgentHaveHealthInInventory2Node = new DecisionNode(DoseAgentHaveHealthInInventory2Decision, this);
        isHealthPackOnMapNode.AddNoChild(DoseAgentHaveHealthInInventory2Node); //joiner

        Decision IsAgentInCornerOfMapDecision = new Decision(Decisions.IsAgentInCornerOfMap);
        DecisionNode IsAgentInCornerOfMapNode = new DecisionNode(IsAgentInCornerOfMapDecision, this);
        DoseAgentHaveHealthInInventory2Node.AddYesChild(IsAgentInCornerOfMapNode);
        IsAgentInCornerOfMapNode.AddNoChild(MoveToClosestMapCornerAction);
        IsAgentInCornerOfMapNode.AddYesChild(DropHealthKitAction);

        // ====================================================== get enemy flag if not at home base
        Decision IsEnemyFlagAtTeamBaseDecision = new Decision(Decisions.IsEnemyFlagAtHomeBase);
        DecisionNode IsEnemyFlagAtTeamBaseNode = new DecisionNode(IsEnemyFlagAtTeamBaseDecision, this);
        IsEnemyFlagAtTeamBaseNode.AddYesChild(DoNothingAction);//this shoudl never really hit 
        DoseAgentHaveHealthInInventory2Node.AddNoChild(IsEnemyFlagAtTeamBaseNode); //joiner

        Decision IsAgentInRangeOfEnemeyFlagDecision = new Decision(Decisions.CanYouPickUpEnemyFlag);
        DecisionNode IsAgentInRangeOfEnemeyFlagNode = new DecisionNode(IsAgentInRangeOfEnemeyFlagDecision, this);
        IsEnemyFlagAtTeamBaseNode.AddNoChild(IsAgentInRangeOfEnemeyFlagNode);
        IsAgentInRangeOfEnemeyFlagNode.AddNoChild(MoveToEnemyFlagAction);
        IsAgentInRangeOfEnemeyFlagNode.AddYesChild(PickUpEnemyFlagAction);

        decisionTree = new DecisionTree(DoseAgentHaveFlagNode);
    }
}