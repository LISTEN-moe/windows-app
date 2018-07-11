using System.Diagnostics;

namespace ListenMoeClient.Rpc
{
	internal class DiscordController
	{
		internal void ReadyCallback() => Debug.WriteLine("Discord: ready");

		internal void DisconnectedCallback(int errorCode, string message)
			=> Debug.WriteLine($"Discord: disconnect {errorCode}: {message}");

		internal void ErrorCallback(int errorCode, string message)
			=> Debug.WriteLine($"Discord: error {errorCode}: {message}");

		internal void OnEnable(string applicationId)
		{
			Debug.WriteLine("Discord: init");

			DiscordRpc.EventHandlers handlers = new DiscordRpc.EventHandlers();
			handlers.readyCallback += ReadyCallback;
			handlers.disconnectedCallback += DisconnectedCallback;
			handlers.errorCallback += ErrorCallback;
			DiscordRpc.Initialize(applicationId, ref handlers, true, null);
		}

		internal void OnDisable()
		{
			Debug.WriteLine("Discord: shutdown");
			DiscordRpc.Shutdown();
		}
	}
}
