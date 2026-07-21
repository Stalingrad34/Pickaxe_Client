using System;
using System.Collections.Generic;

namespace Game.Scripts.Infrastructure.UI
{
    public class DisposableModel : IDisposable
    {
        protected List<IDisposable> disposables = new ();
        
        public virtual void Dispose()
        {
            disposables.ForEach(d => d.Dispose());
        }
    }
}