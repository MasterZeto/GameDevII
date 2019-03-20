using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    I actually think I want this to be a singleton, since I want it to be a real
    component out there in the world
 */

namespace Giga.AI.Blackboard
{
    public class Blackboard : MonoBehaviour
    {
        private struct Character
        {
            public Transform transform;
            public FighterController fighter;
            public HealthSystem healthSystem;
        }

        Character player;
        Character opponent;

        public static Vector3 player_position;

        void Awake()
        {
            // find the player, find the opponent
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            player.transform = playerObj.transform;
            player.fighter = playerObj.GetComponent<FighterController>();
            player.healthSystem = playerObj.GetComponent<HealthSystem>();

            GameObject opponentObj = GameObject.FindGameObjectWithTag("Opponent");
            opponent.transform = opponentObj.transform;
            opponent.fighter = opponentObj.GetComponent<FighterController>();
            opponent.healthSystem = opponentObj.GetComponent<HealthSystem>();

            
        }

        public float DistanceFromPlayerToOpponent()
        {
            return Vector3.Distance(
                player.transform.position, 
                opponent.transform.position
            );
        }

     //   public float GetPlayerHealthPercent()
      //  {
          // return player.healthSystem.GetHealthPercent();
     //  }

     //  public float GetOpponentHealth()
       // {
           // return opponent.healthSystem.GetHealthPercent();
     //   }

    }
}   