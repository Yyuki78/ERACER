using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// MonoBehaviourではなくMonoBehaviourPunCallbacksを継承して、Photonのコールバックを受け取れるようにする
public class SampleScene : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // PhotonServerSettingsに設定した内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();

        PhotonNetwork.SendRate = 20; // 1秒間にメッセージ送信を行う回数
        PhotonNetwork.SerializationRate = 10; // 1秒間にオブジェクト同期を行う回数

        PhotonNetwork.UseRpcMonoBehaviourCache = true;//RPCのメソッドをキャッシュ

        PhotonNetwork.LocalPlayer.NickName = "Player";
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
        string displayName = $"{PhotonNetwork.NickName}の部屋";
        PhotonNetwork.JoinOrCreateRoom("room", GameRoomProperty.CreateRoomOptions(displayName), TypedLobby.Default);
    }

    // マッチングが成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        // マッチング後、ランダムな位置に自分自身のネットワークオブジェクトを生成する
        var v = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        PhotonNetwork.Instantiate("GamePlayer", v, Quaternion.identity);

        // 現在のサーバー時刻を、ゲームの開始時刻に設定する
        if (PhotonNetwork.IsMasterClient && !PhotonNetwork.CurrentRoom.HasStartTime())
        {
            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        }
    }

    // 他プレイヤーが参加した時に呼ばれるコールバック
    public override void OnPlayerEnteredRoom(Player player)
    {
        Debug.Log(player.NickName + "が参加しました");
    }

    // 他プレイヤーが退出した時に呼ばれるコールバック
    public override void OnPlayerLeftRoom(Player player)
    {
        Debug.Log(player.NickName + "が退出しました");
    }
}