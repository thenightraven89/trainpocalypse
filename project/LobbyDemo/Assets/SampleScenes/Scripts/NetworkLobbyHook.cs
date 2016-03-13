using UnityEngine;
using UnityStandardAssets.Network;
using System.Collections;
using UnityEngine.Networking;
using Funk;

public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        Train t = gamePlayer.GetComponent<Train>();
        t.name = lobby.name;
        //NetworkSpaceship spaceship = gamePlayer.GetComponent<NetworkSpaceship>();

        //spaceship.name = lobby.name;
        //spaceship.color = lobby.playerColor;
        //spaceship.score = 0;
        //spaceship.lifeCount = 3;
    }
}
