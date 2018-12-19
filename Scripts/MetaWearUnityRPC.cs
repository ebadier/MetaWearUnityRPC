using System;
using System.Net;
using UnityEngine;

namespace MetaWearRPC.Unity
{
	public class MetaWearUnityRPC : MonoBehaviour
	{
		/// <summary>
		/// IP address of the server. 
		/// 127.0.0.1 is for your own computer.
		/// </summary>
		public string serverAddress = "127.0.0.1";

		/// <summary>
		/// The list of MetaWear boards mac adresses we need to command.
		/// </summary>
		[SerializeField]
		protected string[] _boardsMac;
		public string[] BoardsMac { get { return _boardsMac; } }

		protected ulong[] _boards;
		public ulong[] Boards { get { return _boards; } }

		/// <summary>
		/// Are we trying to reconnect in case of disconnection ?
		/// </summary>
		public bool watchForConnection = true;
		public float watchForConnectionInterval = 5.0f;
		private float _elapsedTime;

		public MetaWearRPC_Client Client { get; private set; }

		protected virtual void Awake()
		{
			_boards = new ulong[_boardsMac.Length];
			for (int i = 0; i < 4; ++i)
			{
				_boards[i] = Global.MacFromString(_boardsMac[i]);
			}

			_elapsedTime = 0.0f;
			Client = new MetaWearRPC_Client();
			_Connect();
		}

		protected virtual void Update()
		{
			_elapsedTime += Time.unscaledDeltaTime;
			if(watchForConnection && (_elapsedTime >= watchForConnectionInterval))
			{
				if (!Client.IsConnected)
				{
					_Connect();
				}
				_elapsedTime = 0.0f;
			}
		}

		protected virtual void OnApplicationQuit()
		{
			// Release ressources before Unity3D close.
			Client.Disconnect();
		}

		private void _Connect()
		{
			try
			{
				Debug.Log(string.Format("[MetaWearUnityRPC] Connecting to server {0}{1}...",
					serverAddress,
					IPAddress.IsLoopback(IPAddress.Parse(serverAddress)) ? " (Loopback)" : ""));

				Client.Connect(serverAddress);

				Debug.Log(string.Format("[MetaWearUnityRPC] Client {0}connected.", Client.IsConnected ? "" : "dis"));
			}
			catch (Exception e)
			{
				Debug.LogWarning("[MetaWearUnityRPC] Error : " + e.Message);
			}
		}
	}
}