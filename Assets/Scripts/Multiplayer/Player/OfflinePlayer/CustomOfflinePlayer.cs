public class CustomOfflinePlayer : CustomPlayer
{
	private new void Awake()
	{
		isOfflinePlayer = true;
		base.Awake();
	}
}
