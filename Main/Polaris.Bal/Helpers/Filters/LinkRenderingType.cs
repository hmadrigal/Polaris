using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polaris.Bal.Helpers.Filters
{
    public enum LinkRenderingType {

        /// <summary>
        /// Normal plain text rendering.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Show a star icon as a prefix.
        /// </summary>
        /// <remarks>This rendering type is used for favorite artists.</remarks>
        StarIconPrefix,

        /// <summary>
        /// Show a heart icon as a prefix.
        /// </summary>
        /// <remarks>This rendering type is used for favorite videos and playlists.</remarks>
        HeartIconPrefix,

        /// <summary>
        /// Show a hear icon as a suffix at the end.
        /// </summary>
        /// <remarks>This rendering type is used for recommendations.</remarks>
        HeartIconSuffix,

    }
}
