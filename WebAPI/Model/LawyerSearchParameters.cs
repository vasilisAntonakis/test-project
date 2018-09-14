using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebAPI.Model
{
    public class LawyerSearchParameters
    {
        public bool IncludeGender { get; set; }
        public bool IncludeTitle { get; set; }
        public bool IncludeInactive { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("?IncludeGender=").Append(IncludeGender ? "true" : "false");
            sb.Append("&IncludeTitle=").Append(IncludeTitle ? "true" : "false");
            sb.Append("&IncludeInactive=").Append(IncludeInactive ? "true" : "false");
            if (!string.IsNullOrWhiteSpace(Name))
                sb.Append("&Name=").Append(Name);
            if (!string.IsNullOrWhiteSpace(Surname))
                sb.Append("&Surname=").Append(Surname);
            return sb.ToString();
        }
    }
}