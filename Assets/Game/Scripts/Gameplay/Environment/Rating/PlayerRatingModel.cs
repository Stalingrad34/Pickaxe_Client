using UniRx;
using YG.Utils.LB;

namespace Game.Scripts.Gameplay.Environment.Rating
{
  public class PlayerRatingModel
  {
    public readonly ReactiveProperty<string> Name = new();
    public readonly ReactiveProperty<string> Avatar = new();
    public readonly ReactiveProperty<int> Rank = new();
    public readonly ReactiveProperty<int> Score = new();
    public readonly ReactiveProperty<bool> IsPlayer = new();

    public PlayerRatingModel(LBPlayerData data, bool isPlayer)
    {
      Name.Value = data.name;
      Avatar.Value = data.photo;
      Rank.Value = data.rank;
      Score.Value = data.score;
      IsPlayer.Value = isPlayer;
    }
    
    public PlayerRatingModel(LBCurrentPlayerData data, string name, string avatar)
    {
      Name.Value = name;
      Avatar.Value = avatar;
      Rank.Value = data.rank;
      Score.Value = data.score;
      IsPlayer.Value = true;
    }
  }
}