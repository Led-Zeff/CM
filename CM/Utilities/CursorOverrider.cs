using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CM.Utilities
{
    class CursorOverrider : IDisposable
    {
        public CursorOverrider()
        {
            Mouse.OverrideCursor = Cursors.AppStarting;
        }

        public CursorOverrider(Cursor cursor)
        {
            Mouse.OverrideCursor = cursor;
        }

        public void Dispose()
        {
            Mouse.OverrideCursor = null;
        }
    }
}
