/* edited by: SWT-P_WS_2018_Holo */
/// <summary>
/// Implementors of this class represent basic flying objects.
/// </summary>
public interface IAircraft
{
	/// <summary>
	/// Pitch shall rotate the object along the local side-to-side axis,
	/// resulting in a forward or backward movement.
	/// </summary>
	/// <param name="pitch">The applied pitch factor between -1 and 1.</param>
    void Pitch(float pitch);

	/// <summary>
	/// Yaw shall rotate the object along the local vertical axis,
	/// resulting in a rotation rotation around itself on the horizontal plane.
	/// </summary>
	/// <param name="yaw">The applied yaw factor between -1 and 1.</param>
	void Yaw(float yaw);

	/// <summary>
	/// Roll shall rotate the object along the local front-to-back axis,
	/// resulting in a lift or right movement.
	/// </summary>
	/// <param name="roll">The applied roll factor between -1 and 1.</param>
	void Roll(float roll);

	/// <summary>
	/// Lift moves the object along the vertical world axis, while a negative value means
	/// droping in height.
	/// </summary>
	/// <param name="lift">The applied lift factor between -1 and 1.</param>
	void Lift(float lift);

	/// <summary>
	/// Drop enables the Drone to drp a crafted item on the map.
	/// </summary>
	void Drop();

}
