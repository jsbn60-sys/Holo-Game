using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
//test
/// <summary>
/// This Component tracks the mental health of every Professor-Player.
/// At game start every Prof Player will register itself at this Manager, with a good mental health.
/// If a Prof-Player burned out along the game, he shall register it here.
/// The Game will be over, when every Prof-Player is burned out.
/// To use it, attach it to a GameObject.
///
/// NOTE: This is implemented as a Singelton! There shall be not second instance of this per scene.
///
/// NOTE: What happens on Game Over right before the this manager holds the game,
///	can be defined via the OnGameOver variable, which shall be set with the Unity Inspector.
/// </summary>
public class GameOverManager : NetworkBehaviour
{
	public static GameOverManager Instance;

	public EventTrigger.TriggerEvent OnGameOver;

	enum ProfMentalHealth
	{
		Fine,
		BurnedOut
	}

	private Dictionary<Player, ProfMentalHealth> profs = new Dictionary<Player, ProfMentalHealth>();

	private void OnEnable()
	{
		Instance = this;
	}

	public Player[] getAllPlayers()
	{
		return profs.Keys.ToArray();
	}

	public void AddProf(Player player)
	{
		profs.Add(player, ProfMentalHealth.Fine);
	}

	public void RemoveProf(Player player)
	{
		profs.Remove(player);
	}

	/// <summary>
	/// Recovers all registered Professor-Players
	/// </summary>
	public void RecoverProfs()
	{
		foreach (var prof in profs.Keys)
		{if (prof!= null)
			{
				prof.revive();

			}
		}
	}

	/// <summary>
	/// increases the Skill Points of all Players
	/// </summary>
	/// <param name="points"> Number of skill points to be added</param>
	public void RpcIncreaseSkillPoints(int points)
	{
		foreach (var prof in profs.Keys)
		{
			if (prof != null)
			{
				prof.addSkillPoints(points);

			}
		}
	}

	/// <summary>
	/// Sets the mental health of given Player to fine.
	/// </summary>
	/// <param name="player">The Prof-Player</param>
	public void ProfIsFine(Player player)
	{
		profs[player] = ProfMentalHealth.Fine;
	}

	/// <summary>
	/// Sets the mental health of the given Prof-Player to BurnedOut.
	/// </summary>
	/// <param name="player">The Prof-Player</param>
	public void ProfIsBurnedOut(Player player)
	{
		profs[player] = ProfMentalHealth.BurnedOut;

		if(!profs.ContainsValue(ProfMentalHealth.Fine)) {
			// all profs are burned out -> Game Over

			//display GameOver panel
			player.gameOver();
			RpcGameOver();
		}
	}

	/// <summary>
	/// Invokes the defined GameOver-EventHandlers on Client.
	/// </summary>
	[ClientRpc]
	private void RpcGameOver()
	{
		//don't need to send date -> sending null
		OnGameOver.Invoke(null);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		//we need to wait for a short period of time, otherwise not all OnGameOver-Events will be
		//finished on time for halting state
		StartCoroutine(WaitForOneSecondAndHoldGame());
	}

	private static IEnumerator WaitForOneSecondAndHoldGame()
	{
		yield return new WaitForSeconds(1);

		//halt the game
		Time.timeScale = 0;
	}
}
