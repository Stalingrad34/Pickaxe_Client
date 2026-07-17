using System.Linq;
using Game.Scripts.Infrastructure;
using Game.Scripts.Infrastructure.Services;
using UniRx;

namespace Game.Scripts.UI.Popups.Collection
{
  public class CollectionPopupModel : PopupModel
  {
    public readonly ReactiveProperty<int> CollectedPickaxesMaxCount = new();
    public readonly ReactiveProperty<int> CollectedPickaxesCurrentCount = new();
    public readonly ReactiveCollection<CollectionItemModel> Items = new();

    public CollectionPopupModel()
    {
      var pickaxesService = ServiceProvider.Get<PickaxesService>();
      var pickaxes = AssetProvider.GetAllPickaxes();
      var sortedPickaxes = pickaxes.OrderBy(c => c.pickaxeType);
      foreach (var pickaxe in sortedPickaxes)
      {
        var isCollected = pickaxesService.CollectedPickaxes.Contains(pickaxe.pickaxeType);
        var model = new CollectionItemModel(pickaxe, isCollected);
        Items.Add(model);
      }
      
      CollectedPickaxesMaxCount.Value = pickaxes.Count;
      CollectedPickaxesCurrentCount.Value = ServiceProvider.Get<PickaxesService>().CollectedPickaxes.Count;
    }
  }
}