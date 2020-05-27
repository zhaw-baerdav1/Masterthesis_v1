public class CustomOfflineVRPlayer : CustomVRPlayer
{
	//ensures only the configuration for the offline player is applied
	private new void Awake()
	{
		isOfflinePlayer = true;
		base.Awake();
	}
}
