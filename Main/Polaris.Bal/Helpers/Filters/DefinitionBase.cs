using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Polaris.Bal.Entities;
using Polaris.Bal.Extensions;

namespace Polaris.Bal.Helpers.Filters
{
    public abstract class DefinitionBase
    {
        #region Properties

        public String DisplayName
        {
            get
            {
                return (HasDisplayName ? displayName : Name);
            }
            set
            {
                displayName = value;
            }
        }
        private String displayName;

        public Boolean HasDisplayName
        {
            get
            {
                return !String.IsNullOrEmpty(displayName);
            }
        }

        public String Name { get; set; }
        
        public String UrlSafeName
        {
            get
            {
                return Name.ToUrlFriendlyString();
            }
        }

        public SortDefinition DefaultSort { get; set; }
        
        public LinkRenderingType RenderingType { get; set; }
        
        /// <summary>
        /// Whether this definition represents the favorite entities for user.
        /// </summary>
        /// <remarks>
        /// Favorites need to be retrieved from CrowdFactory instead of the DB,
        /// therefore it is necessary to distinguish the types that have this
        /// kind of data.
        /// When this property is true the property "User" should contain
        /// the user entity for which the favorites will be retrieved.
        /// </remarks>
        /// <see cref="User"/>
        public Boolean IsFavorites { get; set; }
        
        /// <summary>
        /// This property contains the user for which the requested data is
        /// retrieved.
        /// </summary>
        /// <remarks>
        /// When the property IsFavorites is true, this property should not be null.
        /// </remarks>
        /// <see cref="IsFavorites"/>
        public User User { get; set; }

        /// <summary>
        /// Whether this definition represents Baynote recommendations.
        /// </summary>
        public Boolean IsRecommendation { get; set; }
        
        /// <summary>
        /// Whether this instance defines an option that needs to change depending
        /// on the current user.
        /// </summary>
        /// <remarks>
        /// When this flag is true the rendered links need to be marked with 
        /// HTML class "bn" so that client-side handles them accordingly.
        /// </remarks>
        public Boolean IsUserSpecific { get; set; }

        #endregion

        #region Methods

        public static bool operator ==(DefinitionBase a, DefinitionBase b)
        {

            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Name == b.Name;
        }

        public static bool operator !=(DefinitionBase a, DefinitionBase b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            DefinitionBase p = obj as DefinitionBase;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Name == p.Name);
        }

        public override string ToString()
        {
            return Name;
        }

        public String ToHtml()
        {
            String htmlLink;
            htmlLink = (RenderingType == LinkRenderingType.HeartIconPrefix ? "<span class=\"icon heart\">loved</span>" : String.Empty);
            htmlLink += (RenderingType == LinkRenderingType.StarIconPrefix ? "<span class=\"icon star\">loved</span>" : String.Empty);
            htmlLink += DisplayName;
            htmlLink += (RenderingType == LinkRenderingType.HeartIconSuffix ? "<span class=\"icon heart\">love</span>" : String.Empty);
            return htmlLink;
        }

        public String GetHtmlAnchorClass() {
            if (IsRecommendation) {
                return "class=\"bn\"";
            } else if (IsUserSpecific) {
                return "class=\"private\"";
            } else {
                return String.Empty;
            }
        }

        #endregion
    }
}
