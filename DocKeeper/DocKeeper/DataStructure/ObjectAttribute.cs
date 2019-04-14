using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace DataStructure
{
    public class ObjectAttribute
    {
        #region Properties
        public string Name { get; set; }
        public int Identifier { get; set; }
        /// <summary>
        /// The content of the attribute
        /// </summary>
        public object Content { get; set; }
        #endregion

        #region Constructors
        public ObjectAttribute()
        {
            this.Name = string.Empty;
            this.Content = null;
        }

        public ObjectAttribute(string name, object content)
        {
            this.Name = name;
            this.Content = content;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get a beautiful string to show to the user on the UI
        /// </summary>
        public string GetUIString()
        {
            // Special rule for when dates are saved
            if (Content.GetType() == typeof(DateTime))
            {
                // As we can't use the 'as' keyword, I'll work around it converting the DateTime object into a string and then parsing that
                DateTime content = DateTime.Parse((Content.ToString()));

                // Return the date, without the time at the end (Month/Day/Year)
                return content.ToShortDateString();
            }
            // For everything else, use ToString()
            return this.ToString();
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
