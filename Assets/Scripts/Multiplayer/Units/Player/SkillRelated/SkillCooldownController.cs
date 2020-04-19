#define COOLDOWN_REDUCTION_ON_HIT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class manages the visualisation of the cooldown and activation frame of skills.
/// Active overlays are green frames around the skill to indicate that it's currently aktive.
/// Cooldown overlays show the time left until you can use that skill again.
/// </summary>
public class SkillCooldownController : MonoBehaviour
{
	public static SkillCooldownController Instance { get; set; }
	public GameObject[] coolDownOverlays;
	public Text[] coolDownTimers;
	public GameObject[] activeOverlays;

	public double[] cooldowns;
	public int[] cdis; //timer arr für reducable cds, jeder slot hat 1 individuellen timer
	public Dictionary<int, int> idtoSlotMap = new Dictionary<int, int>();
	public void Awake()
	{
		Instance = this;
	}

	public void Start()
	{
		cooldowns = new double[coolDownOverlays.Length];
		cdis = new int[coolDownOverlays.Length];
		for (int i = 0; i < cooldowns.Length; i++)
		{
			cooldowns[i] = 0;
			cdis[i] = 0;
		}
	}

	/// <summary>
	/// This method toggles the activation overlay
	/// </summary>
	/// <param name="duration">How long the active overlay should be visible</param>
	/// <param name="slot">position in the skill bar where the overlay should be visible</param>
	private IEnumerator activateCooldown(float duration,int slot){
		// Do stuff while skill active
		activeOverlays[slot].SetActive(true);
		yield return new WaitForSecondsRealtime(duration);
		activeOverlays[slot].SetActive(false);
	}

	/// <summary>
	/// This method toggles the cooldown overlay.
	/// It updates the time of the cooldown every second.
	/// </summary>
	/// <param name="cooldown">duration of the cooldown</param>
	/// <param name="slot">position in the skill bar where the overlay should be visible</param>
	/// <returns></returns>
	private IEnumerator coolDownTimer(float cooldown, int slot)
	{
		coolDownOverlays[slot].SetActive(true);
		while (cooldown > 1)
		{
			coolDownTimers[slot].text = "" + (cooldown - 1);
			yield return new WaitForSecondsRealtime(1);
			cooldown--;
		}
		coolDownOverlays[slot].SetActive(false);
	}

	/// <summary>
	/// This method is called to start the cooldown and the activation overlay
	/// </summary>
	/// <param name="duration">duration of the activation overlay</param>
	/// <param name="slot">position in the skill bar where the overlay should be visible</param>
	/// <param name="cooldown">duration of the cooldown overlay</param>
	public void startCooldown(float duration, int slot, float cooldown)
	{
		Debug.Log("duration: " + duration + "  slot: "  + slot + " cooldown: "  + cooldown);
		StartCoroutine(activateCooldown(duration, slot));
		StartCoroutine(coolDownTimer(cooldown, slot));
	}

	/// <summary>
	/// This method is only used by the MNI class and visualizes the remaining charges on the strong shot skill.
	/// There is no activation or cooldown overlay, only an amount of charges left for this skill.
	/// </summary>
	/// <param name="slot">position in the skill bar where the overlay should be visible</param>
	/// <param name="charges">amount of charges left for the skill</param>
	public void strongShotOverlay(int slot, int charges)
	{
		coolDownOverlays[slot].SetActive(true);
		coolDownTimers[slot].text = "" + charges;
		if (charges > 0)
		{
			activeOverlays[slot].SetActive(true);
		} else
		{
			activeOverlays[slot].SetActive(false);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="slot">position in the skill bar where the overlay should be visible</param>
	/// <param name="active">
	/// If true, activates the activation overlay.
	///If false, deactivates the activation overlay.
	/// </param>
	public void healBulletOverlay(int slot, bool active)
	{
		if (coolDownOverlays[slot].activeSelf)
		{
			coolDownOverlays[slot].SetActive(false);
		}
		//coolDownTimers[slot].text = "" + charges;
		if (active)
		{
			activeOverlays[slot].SetActive(true);
		} else
		{
			activeOverlays[slot].SetActive(false);
		}
	}

#if COOLDOWN_REDUCTION_ON_HIT
	/// <summary>
	/// This function updates the UI display of the cooldown
	/// </summary>
	/// <param name="slot">The slot the ability on cooldown is located</param>
	private void UpdateCooldownDisplay(int slot)
	{
		coolDownTimers[slot].text = "" + ((cooldowns[slot] - cdis[slot]) <= 0 ? 1 : (cooldowns[slot] - cdis[slot]));
}

	/// <summary>
	/// This function actually reduces the cooldown.
	/// It looks up the slot of the ability with the id
	/// </summary>
	/// <param name="id">The ID of the ability</param>
	/// <param name="cdrVal">The value the cooldown gets reduced by in seconds</param>
	public void ReduceCooldown(int id, int cdrVal)
	{
		int slot;
		idtoSlotMap.TryGetValue(id, out slot);
		cooldowns[slot] = cooldowns[slot] - 1;
		UpdateCooldownDisplay(slot);
	}
	/// <summary>
	/// This function starts a cooldown timer and keeps it up to date.
	/// The initial cooldown is saved to cooldowns[] at index of the slot of the ability(in action bar).
	/// </summary>
	/// <param name="cooldown">initial cooldown of the ability</param>
	/// <param name="slot">the slot of the ability in the action bar</param>
	/// <returns></returns>
	private IEnumerator ReduceableCoolDownTimer(float cooldown, int slot)
	{
		coolDownOverlays[slot].SetActive(true);
		cooldowns[slot] = cooldown; //wird onhit runter gezählt
		
		for (cdis[slot] = 0; cdis[slot] < cooldowns[slot]; cdis[slot]++)
		{
			coolDownTimers[slot].text = "" + (cooldowns[slot] - cdis[slot]);
			yield return new WaitForSecondsRealtime(1);
		}
		coolDownOverlays[slot].SetActive(false);
	}
	/// <summary>
	/// This function gets called when an ability with cooldown reduction on hit effect gets activated.
	/// It starts the UI Coroutines for Cooldown and Duration Display.
	/// It also saves which slot is occupied by which skill
	/// </summary>
	/// <param name="duration">Duration the ability is active</param>
	/// <param name="slot">The current slot in action bar of the ability</param>
	/// <param name="cooldown">The base cooldown</param>
	/// <param name="id">The id of the skill</param>
	public void StartReducableCooldown(float duration, int slot, float cooldown, int id)
	{
		if (!idtoSlotMap.ContainsKey(id))
		{
			idtoSlotMap.Add(id, slot);
		}
		StartCoroutine(activateCooldown(duration, slot));
		StartCoroutine(ReduceableCoolDownTimer(cooldown, slot));
	}
#endif
}
