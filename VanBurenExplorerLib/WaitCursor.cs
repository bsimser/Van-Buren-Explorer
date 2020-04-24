using System;
using System.Windows.Forms;

namespace VanBurenExplorerLib
{
    /// <summary>
    /// Simple class to wrap in a using statement for long operations
    /// which throws up a wait cursor then gets rid of it after scope
    /// </summary>
    /// <example>
    /// using (new WaitCursor()) {
    ///     // some long running process
    /// }
    /// </example>
    public class WaitCursor : IDisposable
    {
        public WaitCursor()
        {
            Cursor.Current = Cursors.WaitCursor;
        }

        public void Dispose()
        {
            Cursor.Current = Cursors.Default;
        }
    }
}