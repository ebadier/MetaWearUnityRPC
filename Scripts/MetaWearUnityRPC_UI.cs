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

		private float _elapsedTime;
		private int _currentBoardIndex;
		private string[] _boardsStatus;

		private void Start()
		{
			_elapsedTime = 0.0f;
			_currentBoardIndex = 0;
			_boardsStatus = new string[_metaWearUnity.BoardsMac.Length];

			// Show all boards status at start.
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

		private void Update()
		{
			_elapsedTime += Time.unscaledDeltaTime;
			if (_elapsedTime >= _metaWearUnity.watchForConnectionInterval)
			{
				bool connected = _metaWearUnity.Client.IsConnected;
				string status = connected ? "<color=green><b>RPC Server connected</b></color>" : "<color=red><b>RPC Server disconnected</b></color>";
				if (connected)
				{
					// Update boards status one by one (to avoid sending too much battery requests) every watchForConnectionInterval.
					_boardsStatus[_currentBoardIndex] = _GetBoardStatus(_currentBoardIndex);
					// Always show the complete status.
					foreach(string boardStatus in _boardsStatus)
					{
						status += "\n" + boardStatus;
					}
				}
				_statusText.text = status;
				_currentBoardIndex = (++_currentBoardIndex) % _metaWearUnity.BoardsMac.Length;
				_elapsedTime = 0.0f;
			}
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