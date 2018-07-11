using System.Runtime.InteropServices;

namespace ListenMoeClient.Rpc
{
	internal class DiscordRpc
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void ReadyCallback();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void DisconnectedCallback(int errorCode, string message);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void ErrorCallback(int errorCode, string message);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void JoinCallback(string secret);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void SpectateCallback(string secret);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void RequestCallback(JoinRequest request);

		internal struct EventHandlers
		{
			internal ReadyCallback readyCallback;
			internal DisconnectedCallback disconnectedCallback;
			internal ErrorCallback errorCallback;
			internal JoinCallback joinCallback;
			internal SpectateCallback spectateCallback;
			internal RequestCallback requestCallback;
		}

		[System.Serializable]
		internal struct RichPresence
		{
			internal string state; /* max 128 bytes */
			internal string details; /* max 128 bytes */
			internal long startTimestamp;
			internal long endTimestamp;
			internal string largeImageKey; /* max 32 bytes */
			internal string largeImageText; /* max 128 bytes */
			internal string smallImageKey; /* max 32 bytes */
			internal string smallImageText; /* max 128 bytes */
			internal string partyId; /* max 128 bytes */
			internal int partySize;
			internal int partyMax;
			internal string matchSecret; /* max 128 bytes */
			internal string joinSecret; /* max 128 bytes */
			internal string spectateSecret; /* max 128 bytes */
			internal bool instance;
		}

		[System.Serializable]
		internal struct JoinRequest
		{
			internal string userId;
			internal string username;
			internal string discriminator;
			internal string avatar;
		}

		internal enum Reply
		{
			No = 0,
			Yes = 1,
			Ignore = 2
		}

		[DllImport("discord-rpc", EntryPoint = "Discord_Initialize", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void Initialize(string applicationId, ref EventHandlers handlers, bool autoRegister, string optionalSteamId);

		[DllImport("discord-rpc", EntryPoint = "Discord_Shutdown", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void Shutdown();

		[DllImport("discord-rpc", EntryPoint = "Discord_RunCallbacks", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void RunCallbacks();

		[DllImport("discord-rpc", EntryPoint = "Discord_UpdatePresence", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void UpdatePresence(ref RichPresence presence);

		[DllImport("discord-rpc", EntryPoint = "Discord_ClearPresence", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ClearPresence();

		[DllImport("discord-rpc", EntryPoint = "Discord_Respond", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void Respond(string userId, Reply reply);
	}
}
