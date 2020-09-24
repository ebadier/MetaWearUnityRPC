/******************************************************************************************************************************************************
* MIT License																																		  *
*																																					  *
* Copyright (c) 2020																																  *
* Emmanuel Badier <emmanuel.badier@gmail.com>																										  *
* 																																					  *
* Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),  *
* to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,  *
* and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:          *
* 																																					  *
* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.					  *
* 																																					  *
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, *
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 																							  *
* IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 		  *
* TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.							  *
******************************************************************************************************************************************************/

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