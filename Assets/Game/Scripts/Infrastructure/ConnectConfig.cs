using System;
using UnityEngine;

namespace Game.Scripts.Infrastructure
{
  [CreateAssetMenu(menuName = "Data/ConnectSettings")]
  public class ConnectConfig : ScriptableObject
  {
    [SerializeField] private bool isLocal;
    
    [SerializeField] private ServerConfig remoteServer;
    [SerializeField] private ServerConfig localServer;
     
    [SerializeField] private string remoteRatingUrl;
    [SerializeField] private string localRatingUrl;
     
    [SerializeField] private string remoteGetStateUrl;
    [SerializeField] private string localGetStateUrl;
     
    [SerializeField] private string remoteSaveStateUrl;
    [SerializeField] private string localSaveStateUrl;
    
    [SerializeField] private string gameConfigUrl;
    [SerializeField] private string weaponsConfigUrl;
    
    public ServerConfig Server => isLocal ? localServer : remoteServer;
    public string RatingUrl => isLocal ? localRatingUrl : remoteRatingUrl;
    public string GetStateUrl => isLocal ? localGetStateUrl : remoteGetStateUrl;
    public string SetStateUrl => isLocal ? localSaveStateUrl : remoteSaveStateUrl;
    public string GameConfigUrl => gameConfigUrl;
    public string WeaponsConfigUrl => weaponsConfigUrl;
  }

  [Serializable]
  public class ServerConfig
  {
    public string IP;
    public string Port;
    public bool UseSecureProtocol;
  }
}