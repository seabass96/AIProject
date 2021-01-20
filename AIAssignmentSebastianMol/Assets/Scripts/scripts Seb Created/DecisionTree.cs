using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class Decisions
{
    /// <summary>
    /// decision whether agent is holding a flag
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool DoseAgentHaveFlag(AI agent)
    {

        if (agent._agentInventory.HasItem("Blue Flag"))
        {
            return true;
        }
        else if (agent._agentInventory.HasItem("Red Flag"))
        {
            return true;
        }
        else
        {
            return false;
        }

    }     

    /// <summary>
    /// decision node whether agent is on his friendly base
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool IsAgentOnFriendlyBase(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            if (Vector3.Distance(agent.transform.position, GameObject.FindGameObjectWithTag("RedBase").transform.position) <= 5) return true;
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            if (Vector3.Distance(agent.transform.position, GameObject.FindGameObjectWithTag("BlueBase").transform.position) <= 5) return true;           
        }
        return false;
    }

    /// <summary>
    /// decision node whether agent is low on health
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool IsLowOnHealth(AI agent)
    {
        return agent._agentData.CurrentHitPoints < 60; //chaneg this from magic number
    }

    /// <summary>
    /// decision node whether tthe health Kit is on the map
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool IsThereAHealthKitOnMap(AI agent)
    {
        GameObject[] colectible = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (var item in colectible)
        {
            if (item.name == "Health Kit")
            {
                if(!item.transform.parent) return true;
            }
          
        }
        return false; 
    }

    /// <summary>
    /// decision node whether health kit is in range
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool IsHealthKitInRange(AI agent)
    {
        GameObject[] colectible = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (var item in colectible)
        {
            if (item.name == "Health Kit")
            {
                if (agent._agentSenses.IsItemInReach(item)) return true;
            } 
        }
        return false;
    }

    /// <summary>
    /// decision node whether there is a power up on map
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool IsTherPowerUpOnMap(AI agent)
    {
        GameObject[] colectible = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (var item in colectible)
        {
            if (item.name == "Power Up")
            {
                if(!item.transform.parent) return true;
            }                
        }
        return false;
    }

    /// <summary>
    /// decision node whether agent is in range to pick up a power up
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool AreYouNeerPowerUp(AI agent)
    {
        GameObject[] colectible = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (var item in colectible)
        {
            if (item.name == "Power Up")
            {
                if (agent._agentSenses.IsItemInReach(item)) return true;
            }
        }
        return false;
    }

    /// <summary>
    /// decision node whether the enemey team is holding the friendly flag
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool DoseEnemyHaveOurFlag(AI agent)
    {
        if(agent._agentData.EnemyTeam == AgentData.Teams.RedTeam)
        {
            GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Red Team");
            foreach (var item in allAgents)
            {
                if (item.GetComponent<AI>()._agentInventory.HasItem("Blue Flag"))
                {
                    return true;
                }
            }
        }
        else if(agent._agentData.EnemyTeam == AgentData.Teams.BlueTeam)
        {
            GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Blue Team");
            foreach (var item in allAgents)
            {
                if (item.GetComponent<AI>()._agentInventory.HasItem("Red Flag"))
                {
                    return true;
                }
            }
        }
        return false;
        
    }

    /// <summary>
    /// decision node whether the agent can see the enemy holding the friendly flag
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool CanYouSeeEnemyWithFlag(AI agent)
    {

        if (agent._agentData.EnemyTeam == AgentData.Teams.RedTeam)
        {
            GameObject[] enemiesInView = agent._agentSenses.GetEnemiesInView().ToArray();
            foreach (var item in enemiesInView)
            {
                if (item.GetComponent<AI>()._agentInventory.HasItem("Blue Flag"))
                {
                    Debug.Log("can see enemy " + item.name);
                    return true;
                }
            }
        }
        else if (agent._agentData.EnemyTeam == AgentData.Teams.BlueTeam)
        {
            GameObject[] enemiesInView = agent._agentSenses.GetEnemiesInView().ToArray();
            foreach (var item in enemiesInView)
            {
                if (item.GetComponent<AI>()._agentInventory.HasItem("Red Flag"))
                {
                    //Debug.Log("can see enemy " + item.name);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// decision node whether
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool CanYouAttackEnemyWithFalg(AI agent)
    {
        GameObject[] enemiesInView = agent._agentSenses.GetEnemiesInView().ToArray();

        foreach (GameObject item in enemiesInView)
        {
            if (item.GetComponent<AI>()._agentInventory.HasItem("Red Flag") || item.GetComponent<AI>()._agentInventory.HasItem("Blue Flag"))
            {
                if (agent._agentSenses.IsInAttackRange(item))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// decision node whether any of the firendly team agents is holding the enemy flag
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool DoseOurTeamHaveEnemyFlag(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Red Team");
            foreach (var item in allAgents)
            {
                if (item.GetComponent<AI>()._agentInventory.HasItem("Blue Flag"))
                {
                    return true;
                }
                    
            }
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Blue Team");
            foreach (var item in allAgents)
            {
                if (item.GetComponent<AI>()._agentInventory.HasItem("Red Flag"))
                {
                    return true;
                }
                   
            }
        }
        return false;

    }

    /// <summary>
    /// decision node whether any of the friendly team agents is holding the friendly flag
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool DoseOurTeamHaveFriendlyFlag(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Red Team");
            foreach (var item in allAgents)
            {
                if (item.GetComponent<AI>()._agentInventory.HasItem("Red Flag"))
                        return true;
            }
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Blue Team");
            foreach (var item in allAgents)
            {
                if (item.GetComponent<AI>()._agentInventory.HasItem("Blue Flag"))
                    return true;
            }
        }
        return false;

    }

    /// <summary>
    /// decision node whether agent is close to teammate that is carrying the flag
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool AreYouNeerTeamFlagCarrier(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Red Team");
            foreach (var item in allAgents)
            {
                if (item.transform.Find("Flag"))
                {
                    GameObject[] friends = agent._agentSenses.GetFriendliesInView().ToArray();
                    foreach (var friend in friends)
                    {
                        if(friend == item)
                        {
                            if(Vector3.Distance(agent.transform.position, friend.transform.position) < 0.5f)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Blue Team"); //get whole team
            foreach (var item in allAgents) //loop through each emember
            {
                if (item.GetComponent<AI>()._agentInventory.HasItem("Red Flag")) //dose member have red team flag 
                {
                    GameObject[] friends = agent._agentSenses.GetFriendliesInView().ToArray();
                    foreach (var friend in friends)
                    {
                        if (friend == item)
                        {
                            if (Vector3.Distance(agent.transform.position, friend.transform.position) < 10f)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }

                }
            }
        }
        return false;
    }

    /// <summary>
    /// decision node whether agent is close enough to enemy to attack him
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool CanYouHitClosestEnemy(AI agent)
    {
        if (agent._agentData.EnemyTeam == AgentData.Teams.RedTeam)
        {
            GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Red Team");
            foreach (var item in allAgents)
            {
                if (agent._agentSenses.IsInAttackRange(item)) return true;
            }
        }
        else if (agent._agentData.EnemyTeam == AgentData.Teams.BlueTeam)
        {
            GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Blue Team");
            foreach (var item in allAgents)
            {
                if (agent._agentSenses.IsInAttackRange(item)) return true;
            }
        }
        return false;
    }

    /// <summary>
    /// decision node whether the team flag is not located at the team base
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool IsTeamFlagNotOnBase(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            if(GameObject.Find("Red Base").GetComponent<FlagSencing>().HasRedFlag() == false)
            {
                return true;
            }
            
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            if (GameObject.Find("Blue Base").GetComponent<FlagSencing>().HasBlueFlag() ==  false)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// decision node whether ageent is close enough to firnedly flag to pick it up
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool CanYouPickUpTeamFlag(AI agent)
    {      

        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            if (agent._agentSenses.IsItemInReach(GameObject.Find("Red Flag")))
            {
                return true;
            }

        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            if (agent._agentSenses.IsItemInReach(GameObject.Find("Blue Flag")))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    ///  decision node whether agents friendly flag is in reach
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool CanYouPickUpEnemyFlag(AI agent)
    {
        if (agent._agentData.EnemyTeam == AgentData.Teams.RedTeam)
        {
            if (agent._agentSenses.IsItemInReach(GameObject.Find("Red Flag")))
            {
                return true;
            }
        }
        else if (agent._agentData.EnemyTeam == AgentData.Teams.BlueTeam)
        {
            if (agent._agentSenses.IsItemInReach(GameObject.Find("Blue Flag")))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    ///  decision node whether teh enemy flag is located at the agents friendly base
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool IsEnemyFlagAtHomeBase(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            if (GameObject.Find("Red Base").GetComponent<FlagSencing>().HasBlueFlag())
            {
                return true;
            }
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            if (GameObject.Find("Blue Base").GetComponent<FlagSencing>().HasRedFlag())
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    ///  decision node whether an agents enemy is in sight using agent sences
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool CanAgentSeeAnEnemy(AI agent)
    {
        GameObject[] enemiesInView = agent._agentSenses.GetEnemiesInView().ToArray();
        if(enemiesInView.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool IsThereRoomAgentInventory(AI agent)
    {
        if(agent._agentInventory.GetInventoryCount() < agent._agentInventory.Capacity)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    ///  decision node whether agent has a helth kit present in his inventory
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool doseAgentHaveHealthKitInInventory(AI agent)
    {
        return agent._agentInventory.HasItem("Health Kit");
    }

    /// <summary>
    ///  decision node whether agent is within range of team meate with lowest health to pick an item up from him
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool isAgentNearTeamMateWithLowestHealth(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            return LowHealthDistanceLogic(agent, "Red Team");         
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            return LowHealthDistanceLogic(agent, "Blue Team");
        }

        Debug.Log("if you see this somehtign went wrong");
        return false;
    }

    /// <summary>
    /// contains logic for the isAgentNearTeamMateWithLowestHealth function so that code is not repeated
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    /// <param name="team">the team that the logic is applied to</param>
    private static bool LowHealthDistanceLogic(AI agent, string team)
    {
        GameObject[] allAgents = GameObject.FindGameObjectsWithTag(team);
        GameObject LowHealthTeamMember = null;
        foreach (GameObject teamMember in allAgents)
        {
            if (!LowHealthTeamMember)
            {
                LowHealthTeamMember = teamMember;
            }
            else if (teamMember.GetComponent<AI>()._agentData.GetCurrentHitPoints()
                < LowHealthTeamMember.GetComponent<AI>()._agentData.GetCurrentHitPoints())
            {
                LowHealthTeamMember = teamMember;
            }

        }

        if (agent._agentSenses.IsItemInReach(LowHealthTeamMember))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// decision node whether an agent on the friendly team is low on health
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool IsTeamMateLowOnHealth(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            return IsTeamMateLowOnHealthLogic(agent, "Red Team");
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            return IsTeamMateLowOnHealthLogic(agent, "Blue Team");
        }

        Debug.Log("if you see this somehtign went wrong");
        return false;
    }

    /// <summary>
    /// function that holds the logic for IsTeamMateLowOnHealth so that code is not repeted
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    /// <param name="team">the team that the logic is applied to</param>
    private static bool IsTeamMateLowOnHealthLogic(AI agent, string team)
    {
        GameObject[] whoelTeam = GameObject.FindGameObjectsWithTag(team);
        foreach (GameObject teamMember in whoelTeam)
        {
            if(teamMember.GetComponent<AI>()._agentData.GetCurrentHitPoints() < 50) //if health less than half
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// decision node whether agent is holding a health Kit in his inventory
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool DoseAgentHaveHealthKitInInventory(AI agent)
    {
        return agent._agentInventory.HasItem("Health Kit");
    }

    /// <summary>
    /// decision node whether agent is holding a power up in his inventory
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool DoseAgentHavePowerUpInInventory(AI agent)
    {
        //if (agent._agentInventory.HasItem("Power Up")) Debug.Log(agent.name + " has power up");
        //return agent._agentInventory.HasItem("Power Up");
        if(agent._agentInventory.HasItem("Power Up"))
        {
            Debug.Log(agent.name + " has powerup");
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// decision node whether agent is close enough to team mate with highest health to give him an item
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool IsAgentInRangeOfTeamMateWithHighestHealth(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            return HighHealthDistanceLogic(agent, "Red Team");
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            return HighHealthDistanceLogic(agent, "Blue Team");
        }

        Debug.Log("if you see this somehtign went wrong");
        return false;
    }

    /// <summary>
    /// logic for the IsAgentInRangeOfTeamMateWithHighestHealth function so that code is not repeated
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    /// <param name="team">the team that the logic is applied to</param>
    private static bool HighHealthDistanceLogic(AI agent, string team)
    {
        GameObject[] allAgents = GameObject.FindGameObjectsWithTag(team); //get all agents
        GameObject HighHealthTeamMember = null; //creat a holderr for agent woth highest health
        foreach (GameObject teamMember in allAgents)//loop through all agents
        {
          
            if (!HighHealthTeamMember || teamMember.GetComponent<AI>()._agentData.GetCurrentHitPoints() > HighHealthTeamMember.GetComponent<AI>()._agentData.GetCurrentHitPoints())
            {
                if (teamMember != agent.gameObject) HighHealthTeamMember = teamMember;
            }

        }

        if (agent == HighHealthTeamMember)
        {
            return false;
        }

        if (agent._agentSenses.IsItemInReach(HighHealthTeamMember))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    ///  decision node whether the agent with the highest health on the friendly team is holding a power up
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool DoseAgentWithHighestHealthHaveNotPowerUp(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            return DoseAgentWithHighestHealthHavePowerUpLogic(agent, "Red Team");
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            return DoseAgentWithHighestHealthHavePowerUpLogic(agent, "Blue Team");
        }

        Debug.Log("if you see this somethign went wrong");
        return false;
    }

    /// <summary>
    /// holds logic for DoseAgentWithHighestHealthHaveNotPowerUp so that code is not repeted
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    /// <param name="team">the team that the logic is applied to</param>
    private static bool DoseAgentWithHighestHealthHavePowerUpLogic(AI agent, string team)
    {
        GameObject[] allAgents = GameObject.FindGameObjectsWithTag(team);
        GameObject HighHealthTeamMember = null;
        foreach (GameObject teamMember in allAgents)
        {           
            if (!HighHealthTeamMember || teamMember.GetComponent<AI>()._agentData.GetCurrentHitPoints() > HighHealthTeamMember.GetComponent<AI>()._agentData.GetCurrentHitPoints())
            {
                if(teamMember != agent.gameObject) HighHealthTeamMember = teamMember;
            }
        }

        //if(HighHealthTeamMember.GetComponent<AI>()._agentInventory.HasItem("Power Up"))
        //{
        //    return false; //flipped so if it dose have power up = false
        //}
        //else
        //{
        //    return true;
        //}

        //if(HighHealthTeamMember = agent.gameObject)//if highest health team mate is me do nothing
        //{
        //    return false;
        //}
        //else //else go power up highets health team mate
        //{
        //    if (HighHealthTeamMember.GetComponent<AI>()._agentData.IsPoweredUp)
        //    {
        //        return false; //flipped so if it dose have power up = false
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
        //Debug.Log(agent.name + " is giving " + HighHealthTeamMember.name + " a health pac");
        if(!HighHealthTeamMember)
        {
            return false;
        }
        if (HighHealthTeamMember.GetComponent<AI>()._agentData.IsPoweredUp)
        {
            return false; //flipped so if it dose have power up = false
        }
        else
        {
            return true;
        }

    }

    /// <summary>
    /// decision node whether agent is in attacking range of the weakest enemy 
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool IsAgentCloseToWeakestEnemy(AI agent)
    {
        GameObject[] enemies = agent._agentSenses.GetEnemiesInView().ToArray();
        GameObject weakestEnemy = null;
        
        foreach (GameObject enemy in enemies)
        {
            if (!weakestEnemy)
            {
                weakestEnemy = enemies[0];
            }
            else if(enemy.GetComponent<AgentData>().GetCurrentHitPoints() < weakestEnemy.GetComponent<AgentData>().GetCurrentHitPoints())
            {
                weakestEnemy = enemy;
            }
        }       

        return agent._agentSenses.IsInAttackRange(weakestEnemy);
    }

    /// <summary>
    /// decision node whether there is a helth pack in teh corner of teh map set up for ambush
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool IsThereAHealthPacInTheCornerOfTheMap(AI agent)
    {
        GameObject[] allCornerSensors = GameObject.FindGameObjectsWithTag("HealthKitSensor");
        foreach (GameObject sensor in allCornerSensors)
        {
            if(sensor.GetComponent<HealthPackSensing>().HasHealthKit())
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// decision node whether agent is in the same corner as the health kit 
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool IsAgentInCornerOfMapWithHealhKit(AI agent)
    {
        //find the corner with health kit
        GameObject[] allCornerSensors = GameObject.FindGameObjectsWithTag("HealthKitSensor");
        GameObject cornerWithHealthKit = null;
        foreach (GameObject sensor in allCornerSensors)
        {
            if (sensor.GetComponent<HealthPackSensing>().HasHealthKit())
            {
                cornerWithHealthKit = sensor;
            }
        }

        //see if agent is close to that corner objcet
        if (Vector3.Distance(agent.transform.position, cornerWithHealthKit.transform.GetChild(0).transform.position) < 0.1f)//test this for distance
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// decision node whether agent is in a corenr of the map
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool IsAgentInCornerOfMap(AI agent)
    {
        //find the corner with health kit
        GameObject[] allCornerSensors = GameObject.FindGameObjectsWithTag("HealthKitSensor");
        foreach (GameObject sensor in allCornerSensors)
        {
            //see if agent is close to that corner objcet
            if (Vector3.Distance(agent.transform.position, sensor.transform.GetChild(0).transform.position) < 1f)//test this for distance
            {
                return true;
            }

            if (Vector3.Distance(agent.transform.position, sensor.transform.position) < 1f)//test this for distance
            {
                return true;
            }

        }
        return false;
       
    }

    /// <summary>
    /// decision node whether enemy is in the attack range of assasin used when assasin is ambushing 
    /// </summary>
    /// <param name="agent">the agent that is makign the decision</param>
    public static bool IsEnemyInAttackRangeAssasin(AI agent)
    {
        GameObject[] enemiesInView = agent._agentSenses.GetEnemiesInView().ToArray();
        foreach (GameObject enemy in enemiesInView)
        {
            if(Vector3.Distance(enemy.transform.position, agent.transform.position) < agent.attackRange)
            {
                return true;
            }
        }

        return false;
    }

}

class Actions
{
    /// <summary>
    /// action node that makes the agent do nothing
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void DoNothing(AI agent)
    {
    }

    /// <summary>
    /// action node thats moves the agent to his friendly base
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void MoveToHomeBase(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            agent._agentActions.MoveTo(GameObject.Find("Red Base"));
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            agent._agentActions.MoveTo(GameObject.Find("Blue Base"));
        }
    }

    /// <summary>
    /// action node that makes agent drop the enemey flag
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void DropEnemyFlag(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            agent._agentActions.DropItem(agent._agentInventory.GetItem("Blue Flag"));
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            agent._agentActions.DropItem(agent._agentInventory.GetItem("Red Flag"));
        }

    }

    /// <summary>
    /// action node that maked the agent drop any flag he is holding
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void DropAnyFlag(AI agent)
    {
        if(agent._agentInventory.GetItem("Blue Flag")) agent._agentActions.DropItem(agent._agentInventory.GetItem("Blue Flag"));
        if(agent._agentInventory.GetItem("Red Flag")) agent._agentActions.DropItem(agent._agentInventory.GetItem("Red Flag"));
    }

    /// <summary>
    /// action node that maked the agent pick up and health kit and use it on him self instantly
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void PickUpHealthKitAndUse(AI agent)
    {
        GameObject[] allColectibles = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (var item in allColectibles)
        {
            if (item.name == "Health Kit")
            {
                agent._agentActions.CollectItem(item); //add teh item to onvetory 
                agent._agentActions.UseItem(item); //use the item
            }
        }
       
    }

    /// <summary>
    /// action node that makes the agent move towards the health kit
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void MoveTowardsHealthKit(AI agent)
    {
        GameObject[] allColectibles = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (var item in allColectibles)
        {
            if (item.name == "Health Kit")
            {
                agent._agentActions.MoveTo(item);
            }
        }       
    }

    /// <summary>
    /// action node that makes the agent pick up the power up and use it on him self instantly
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void PickUpPowerUpAndUse(AI agent)
    {
        GameObject[] allColectibles = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (var item in allColectibles)
        {
            if (item.name == "Power Up")
            {
                agent._agentActions.CollectItem(item); //add teh item to onvetory 
                agent._agentActions.UseItem(item); //use teh item
            }
        }

    }

    /// <summary>
    /// action node that makes the agent move towards the power up
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void MoveTowardPowerUp(AI agent)
    {
        GameObject[] allColectibles = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (var item in allColectibles)
        {
            if (item.name == "Power Up")
            {
                agent._agentActions.MoveTo(item);
            }
        }
    }

    /// <summary>
    /// action node that makes the agent attack the closest enemy
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void AttackEnemy(AI agent)
    {     
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            AttackEnemySortingLogic("Blue Team", agent);
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            AttackEnemySortingLogic("Red Team", agent);
        }
    }

    /// <summary>
    /// hold logic for the AttackEnemy function so that code is not repeted
    /// </summary>
    /// <param name="team">the team that the logic is applied to</param>
    /// <param name="agent">the agent that is doing the action</param>
    private static void AttackEnemySortingLogic(string team, AI agent)
    {
        GameObject[] allEnemiess;
        allEnemiess = GameObject.FindGameObjectsWithTag(team);
        GameObject closestEnemy = null;
        //find teh closes enemy
        foreach (var enemy in allEnemiess)
        {
            if (!closestEnemy)
            {
                closestEnemy = enemy;
            }
            else
            {
                if (Vector3.Distance(enemy.transform.position, agent.gameObject.transform.position) <
                    Vector3.Distance(closestEnemy.transform.position, agent.gameObject.transform.position))
                {
                    closestEnemy = enemy;
                }
            }

        }
        agent._agentActions.AttackEnemy(closestEnemy); //attack the enemy

    }

    /// <summary>
    /// action node that makes the agent move towards teh enemy that is carrying his teams flag
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void MoveToEnemyWithFlag(AI agent)
    {
        if (agent._agentData.EnemyTeam == AgentData.Teams.RedTeam)
        {
            GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Red Team");
            foreach (GameObject enemy in allAgents)
            {
                if (enemy.GetComponent<AI>()._agentInventory.HasItem("Blue Flag"))
                {
                    agent._agentActions.MoveTo(enemy);
                    return;
                }
            }
        }
        else if (agent._agentData.EnemyTeam == AgentData.Teams.BlueTeam)
        {
            GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Blue Team");
            foreach (GameObject enemy in allAgents)
            {
                if (enemy.GetComponent<AI>()._agentInventory.HasItem("Red Flag"))
                {
                    agent._agentActions.MoveTo(enemy);
                    return;
                }
            }
        }
       

    }

    /// <summary>
    /// action node that makes the agent move towards his enemys base
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void MoveToEnemyBase(AI agent)
    {
        if (agent._agentData.EnemyTeam == AgentData.Teams.RedTeam)
        {
            agent._agentActions.MoveTo(GameObject.Find("Red Base"));
        }
        else if (agent._agentData.EnemyTeam == AgentData.Teams.BlueTeam)
        {
            agent._agentActions.MoveTo(GameObject.Find("Blue Base"));
        }
    }


    /// <summary>
    /// action node that makes the agent move towards his team mate that is holding an enemy flag
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void MoveToFriendlyWithEnemyFlag(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Red Team");
            foreach (var firend in allAgents)
            {
                if (firend.GetComponent<AI>()._agentInventory.HasItem("Blue Flag"))
                {
                    agent._agentActions.MoveTo(firend);
                    return;
                }
            }
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Blue Team");
            foreach (var firend in allAgents)
            {
                if (firend.GetComponent<AI>()._agentInventory.HasItem("Red Flag"))
                {
                    agent._agentActions.MoveTo(firend);
                    return;
                }
            }
        }
    }


    /// <summary>
    /// action node that makes the agent move towards his teams flag
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void MoveTowardsTeamFlag(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            agent._agentActions.MoveTo(GameObject.Find("Red Flag"));
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            agent._agentActions.MoveTo(GameObject.Find("Blue Flag"));
        }
    }

    /// <summary>
    /// action node that makes the agent pick up his teams flag
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void PickUpFlagTeamFlag(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            GameObject redFlag = GameObject.Find("Red Flag");
            if (agent._agentSenses.IsItemInReach(redFlag))
            {
                agent._agentActions.CollectItem(redFlag);
            }
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            GameObject BlueFlag = GameObject.Find("Blue Flag");
            if (agent._agentSenses.IsItemInReach(BlueFlag))
            {
                agent._agentActions.CollectItem(BlueFlag);
            }
        }
    }

    /// <summary>
    /// action node that makes the agent pick up teh eneemy teams flag
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void PickUpEnemyFlag(AI agent)
    {
        if (agent._agentData.EnemyTeam == AgentData.Teams.RedTeam)
        {
            GameObject redFlag = GameObject.Find("Red Flag");
            if (agent._agentSenses.IsItemInReach(redFlag))
            {
                agent._agentActions.CollectItem(redFlag);
            }
        }
        else if (agent._agentData.EnemyTeam == AgentData.Teams.BlueTeam)
        {
            GameObject BlueFlag = GameObject.Find("Blue Flag");
            if (agent._agentSenses.IsItemInReach(BlueFlag))
            {
                agent._agentActions.CollectItem(BlueFlag);
            }
        }
    }

    /// <summary>
    /// action node that makes the agent move towards the enemy teams flag
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void MoveTowardsEnemyTeamFlag(AI agent)
    {
        if (agent._agentData.EnemyTeam == AgentData.Teams.RedTeam)
        {
            agent._agentActions.MoveTo(GameObject.Find("Red Flag"));
        }
        else if (agent._agentData.EnemyTeam == AgentData.Teams.BlueTeam)
        {
            agent._agentActions.MoveTo(GameObject.Find("Blue Flag"));
        }
    }

    /// <summary>
    /// action node that makes the agent move toward the closest enemy to him
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void MoveTowardsClosestEnemy(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            MoveTowardsClosestEnemyLogic("Blue Team", agent);
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            MoveTowardsClosestEnemyLogic("Red Team", agent);
        }
    }

    /// <summary>
    /// contains the logic for MoveTowardsClosestEnemy function so that code is not reapated
    /// </summary>
    /// <param name="team">the team that the logic is applied to</param>
    /// <param name="agent">the agent that is doing the action</param>
    private static void MoveTowardsClosestEnemyLogic( string team, AI agent)
    {
        GameObject[] allEnemiess;
        allEnemiess = GameObject.FindGameObjectsWithTag(team);
        GameObject closestEnemy = null;
        //find teh closes enemy
        foreach (var enemy in allEnemiess)
        {
            if (!closestEnemy)
            {
                closestEnemy = enemy;
            }
            else
            {
                if (Vector3.Distance(enemy.transform.position, agent.gameObject.transform.position) <
                    Vector3.Distance(closestEnemy.transform.position, agent.gameObject.transform.position))
                {
                    closestEnemy = enemy;
                }
            }

        }
        agent._agentActions.MoveTo(closestEnemy); //attack the enemy
    }

    /// <summary>
    /// action node that makes the agent pick up a health kit and not use it on him self
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void PickUpHealthKitAndPutInInventory(AI agent)
    {
        GameObject[] allColectibles = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (var item in allColectibles)
        {
            if (item.name == "Health Kit")
            {
                if(!item.transform.parent) agent._agentActions.CollectItem(item); //add teh item to onvetory 
            }
        }
    }

    /// <summary>
    /// action node that makes the agent pick up a power up anbd hold on to it without using it on himself
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void PickUpPowerUpAndPutInInventory(AI agent)
    {
        GameObject[] allColectibles = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (var item in allColectibles)
        {
            if (item.name == "Power Up")
            {
                if (!item.transform.parent) agent._agentActions.CollectItem(item); //add teh item to onvetory 
            }
        }
    }

    /// <summary>
    /// action node that makes the agent move towards is lowest health teammate
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void MoveToTeamMateWithLowestHealth(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            MoveToTeamMateWithLowestHealthLogic(agent, "Red Team");
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            MoveToTeamMateWithLowestHealthLogic(agent, "Blue Team");
        }
    }

    /// <summary>
    /// hold the logic for MoveToTeamMateWithLowestHealth function so that code is not repeted
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    /// <param name="team">the team that the logic is applied to</param>
    public static void MoveToTeamMateWithLowestHealthLogic(AI agent, string team)
    {
        GameObject[] AllFriendlies = GameObject.FindGameObjectsWithTag(team);
        GameObject TeammateWithLowestHealth = null;

        foreach (GameObject teamMember in AllFriendlies)
        {
            if (!TeammateWithLowestHealth)
            {
                TeammateWithLowestHealth = teamMember;
            }
            else
            {
                if (teamMember.GetComponent<AI>()._agentData.GetCurrentHitPoints()
                    < TeammateWithLowestHealth.GetComponent<AI>()._agentData.GetCurrentHitPoints())
                {
                    TeammateWithLowestHealth = teamMember;
                }
            }
        }

        agent._agentActions.MoveTo(TeammateWithLowestHealth);
    }

    /// <summary>
    /// action node that makes the agent move to his highest health team mate
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void MoveToTeamMateWithHighestHealth(AI agent)
    {
        if (agent._agentData.FriendlyTeam == AgentData.Teams.RedTeam)
        {
            MoveToTeamMateWithHighestHealthLogic(agent, "Red Team");
        }
        else if (agent._agentData.FriendlyTeam == AgentData.Teams.BlueTeam)
        {
            MoveToTeamMateWithHighestHealthLogic(agent, "Blue Team");
        }
    }

    /// <summary>
    /// holds logic for MoveToTeamMateWithHighestHealth function so that code is not repeated
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    /// <param name="team">the team that the logic is applied to</param>
    public static void MoveToTeamMateWithHighestHealthLogic(AI agent, string team)
    {
        GameObject[] AllFriendlies = GameObject.FindGameObjectsWithTag(team);
        GameObject TeammateWithLowestHealth = null;

        foreach (GameObject teamMember in AllFriendlies)
        {
            if (!TeammateWithLowestHealth || teamMember.GetComponent<AI>()._agentData.GetCurrentHitPoints()
                    > TeammateWithLowestHealth.GetComponent<AI>()._agentData.GetCurrentHitPoints())
            {
                if (teamMember != agent.gameObject) TeammateWithLowestHealth = teamMember;
            }
        }
        agent._agentActions.MoveTo(TeammateWithLowestHealth);
    }

    /// <summary>
    /// action node that makes the agent force feed the closet team mate to take health kit and use it
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void GiveHealthKitToClosestTeamMamber(AI agent)
    {
        GameObject[] allTeamMates = agent._agentSenses.GetFriendliesInView().ToArray();
        foreach (GameObject teamMate in allTeamMates)
        {
            if (agent._agentSenses.IsItemInReach(teamMate)) // if agent is close enough to pick up item
            {
                AI teamMateAi = teamMate.GetComponent<AI>(); //get the team mate ai 
                GameObject[] allColectibles = GameObject.FindGameObjectsWithTag("Collectable");
                foreach (var item in allColectibles)
                {
                    if (item.name == "Health Kit")
                    {                      
                        teamMateAi._agentActions.CollectItem(item); //add teh item to invetory 
                        if(teamMateAi._agentActions.UseItem(item)) agent._agentInventory.RemoveItem(item.name); //use the item
                            return;
                    }

                }
            }
        }
        agent._agentActions.UseItem(agent._agentInventory.GetItem("Health Kit"));
    }


    /// <summary>
    /// action node that makes the agent force feed the closet team mate to take power up and use it
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void GivePowerUpToClosestTeamMamber(AI agent)
    {
        GameObject[] allTeamMates = agent._agentSenses.GetFriendliesInView().ToArray();
        foreach (GameObject teamMate in allTeamMates)
        {
            if (agent._agentSenses.IsItemInReach(teamMate))
            {
                AI teamMateAi = teamMate.GetComponent<AI>(); //get the team mate ai 
                GameObject[] allColectibles = GameObject.FindGameObjectsWithTag("Collectable");
                foreach (var item in allColectibles)
                {
                    if (item.name == "Power Up")
                    {
                        teamMateAi._agentActions.CollectItem(item); //add the item to invetory                       
                        if(teamMateAi._agentActions.UseItem(item)) agent._agentInventory.RemoveItem(item.name);
                    }
                }
            }
        }
        //agent._agentActions.UseItem(agent._agentInventory.GetItem("Power Up"));
    }


    /// <summary>
    /// action node that makes the agent use health kit in his inventory on him self
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void UseHealthKitFromInventoryOnSelf(AI agent)
    {
        agent._agentActions.UseItem(agent._agentInventory.GetItem("Health Kit"));
    }

    /// <summary>
    /// action node that makes the agent move towards the weakest agent on enemy team
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void MoveToWeakestEenemy(AI agent)
    {
        GameObject[] enemies = agent._agentSenses.GetEnemiesInView().ToArray();
        GameObject weakestEnemy = enemies[0];

        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<AgentData>().GetCurrentHitPoints() < weakestEnemy.GetComponent<AgentData>().GetCurrentHitPoints())
            {
                weakestEnemy = enemy;
            }
        }        
        agent._agentActions.MoveTo(weakestEnemy);
    }

    /// <summary>
    /// action node that makes the agent move to the corner of the map with a health kit left next to it
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void MoveToCornerOfMapWithHealthKit(AI agent)
    {
        //find the corner with health kit
        GameObject[] allCornerSensors = GameObject.FindGameObjectsWithTag("HealthKitSensor");
        GameObject cornerWithHealthKit = null;
        foreach (GameObject sensor in allCornerSensors)
        {
            if (sensor.GetComponent<HealthPackSensing>().HasHealthKit())
            {
                cornerWithHealthKit = sensor;
                continue;
            }
        }

        agent._agentActions.MoveTo(cornerWithHealthKit);           
    }

    /// <summary>
    /// action node that makes the agent move to tehe closet corner to that agent
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void MoveToClostesCorner(AI agent)
    {
        GameObject[] allCornerSensors = GameObject.FindGameObjectsWithTag("HealthKitSensor");
        GameObject closestCornerToAgent = allCornerSensors[0];       
        foreach (GameObject corner in allCornerSensors)
        {
            if(Vector3.Distance(agent.transform.position, corner.transform.position) <
                Vector3.Distance(agent.transform.position, closestCornerToAgent.transform.position))
            {
                closestCornerToAgent = corner;
            }
        }

        agent._agentActions.MoveTo(closestCornerToAgent);
    }

    /// <summary>
    /// action node that makes the agent drop teh ehalth kit in his inventory
    /// </summary>
    /// <param name="agent">the agent that is doing the action</param>
    public static void DropHealthKit(AI agent)
    {
        agent._agentActions.DropItem(agent._agentInventory.GetItem("Health Kit"));
    }
}

abstract class Node
{
    protected bool isLeaf;

    public bool IsLeaf
    {
        get { return isLeaf; }
    }

    public abstract void ExecuteAction();
    public abstract Node MakeDecision();

}


public delegate bool Decision( AI agent);

class DecisionNode : Node
{
    AI agent;
    Node yesChild;
    Node noChild;
    Decision decision;

    public DecisionNode(Decision decision, AI agent)
    {
        isLeaf = false;

        yesChild = null;
        noChild = null;

        this.decision = decision;
        this.agent = agent;
    }

    public void AddYesChild(Node child)
    {
        yesChild = child;
    }

    public void AddNoChild(Node child)
    {
        noChild = child;
    }

    public override Node MakeDecision()
    {
        if (decision.Invoke(agent))
        {

            return yesChild;
        }
        else
        {
            
            return noChild;
        }
    }
    public override void ExecuteAction()
    {
        throw new System.NotImplementedException();
    }
}

public delegate void Action(AI agent);

class ActionNode : Node
{
    AI agent;
    Action action;

    public ActionNode(Action action, AI agent)
    {
        isLeaf = true;
        this.action = action;
        this.agent = agent;
    }

    public override Node MakeDecision()
    {
        throw new System.NotImplementedException();
    }

    public override void ExecuteAction()
    {
        action.Invoke(agent);
    }

    
}


class DecisionTree
{
    Node root;
    Node currentNode;

    public DecisionTree(Node root)
    {
        this.root = root;
        currentNode = null;
    }

    public void Execute()
    {
        Traverse(root);
    }

    private void Traverse(Node currentNode)
    {
        if (currentNode.IsLeaf)
        {           
            currentNode.ExecuteAction();
        }
        else
        {            
            this.currentNode = currentNode.MakeDecision();
            Traverse(this.currentNode);
        }
    }
}