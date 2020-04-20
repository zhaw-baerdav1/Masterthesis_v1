public class CustomOfflineVRPlayer : CustomVRPlayer
{
	private new void Awake()
	{
		isOfflinePlayer = true;
		base.Awake();
	}
}
