using System;
using System.Collections.Generic;

namespace Pcf.Preferences.WebHost.Models
{
    /// <example>
    /// {
    ///   "ids": [
    ///     "ef7f299f-92d7-459f-896e-078ed53ef99c",
    ///     "76324c47-68d2-472d-abb8-33cfa8cc0c84"
    ///   ]
    /// }
    /// </example>
    public class PreferenceIdsRequest
    {
        public List<Guid> Ids { get; set; }
    }
}
