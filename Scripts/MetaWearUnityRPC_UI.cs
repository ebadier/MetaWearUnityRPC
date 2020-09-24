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

using UnityEngine;
using UnityEngine.UI;

namespace MetaWearRPC.Unity
{
	public sealed class MetaWearUnityRPC_UI : MonoBehaviour
	{
		[SerializeField]
		private MetaWearUnityRPC _metaWearUnity;
		[SerializeField]
		private Text _statusText;
		[Tooltip("You could prefer to update status manually to avoid sending battery requests too much on MetaWear boards")]
		public bool autoUpdate = false;
		[Tooltip("You could prefer to update status partially to avoid sending battery requests too much on MetaWear boards")]
		public bool partialUpdate = true;

		private float _elapsedTime;
		private int _currentBoardIndex;
		private string[] _boardsStatus;

		/// <summary>
		/// Call it yourself when autoUpdate = false.
		/// </summary>
		public void UpdateStatus()
		{
			if(!autoUpdate)
			{
				if (partialUpdate)
				{
					_UpdateNextBoardStatus();
				}
				else
				{
					_UpdateAllBoardsStatus();
				}
			}
		}

		private void Start()
		{
			_elapsedTime = 0.0f;
			_currentBoardIndex = 0;
			_boardsStatus = new string[_metaWearUnity.BoardsMac.Length];

			// Show all boards status at start.
			_UpdateAllBoardsStatus();
		}

		private void Update()
		{
			if(autoUpdate)
			{
				_elapsedTime += Time.unscaledDeltaTime;
				if (_elapsedTime >= _metaWearUnity.watchForConnectionInterval)
				{
					UpdateStatus();
					_elapsedTime = 0.0f;
				}
			}
		}

		private void _UpdateAllBoardsStatus()
		{
			bool connected = _metaWearUnity.Client.IsConnected;
			string status = connected ? "<color=green><b>RPC Server connected</b></color>" : "<color=red><b>RPC Server disconnected</b></color>";
			if (connected)
			{
				for (int i = 0; i < _metaWearUnity.BoardsMac.Length; ++i)
				{
					_boardsStatus[i] = _GetBoardStatus(i);
					status += "\n" + _boardsStatus[i];
				}
			}
			_statusText.text = status;
		}

		private void _UpdateNextBoardStatus()
		{
			bool connected = _metaWearUnity.Client.IsConnected;
			string status = connected ? "<color=green><b>RPC Server connected</b></color>" : "<color=red><b>RPC Server disconnected</b></color>";
			if (connected)
			{
				// Update boards status one by one (to avoid sending too much battery requests) every watchForConnectionInterval.
				_boardsStatus[_currentBoardIndex] = _GetBoardStatus(_currentBoardIndex);
				// Always show the complete status.
				foreach (string boardStatus in _boardsStatus)
				{
					status += "\n" + boardStatus;
				}
			}
			_statusText.text = status;
			_currentBoardIndex = (++_currentBoardIndex) % _metaWearUnity.BoardsMac.Length;
		}

		private string _GetBoardStatus(int pBoardIndex)
		{
			string status;
			byte battery = _metaWearUnity.Client.GetBatteryLevel(_metaWearUnity.Boards[pBoardIndex]);
			if (battery > 0)
			{
				if (battery > 15)
				{
					status = string.Format("<color=green><b>{0} : {1}%</b></color>", _metaWearUnity.BoardsMac[pBoardIndex], battery);
				}
				else
				{
					status = string.Format("<color=yellow><b>{0} : {1}%</b></color>", _metaWearUnity.BoardsMac[pBoardIndex], battery);
				}
			}
			else
			{
				status = string.Format("<color=red><b>{0}</b></color>", _metaWearUnity.BoardsMac[pBoardIndex]);
			}
			return status;
		}
	}
}