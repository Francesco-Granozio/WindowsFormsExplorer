using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsExplorer.Domain
{
    public class FormInfo
    {
        public FormInfo(string name, string type, string text, string visible, string handle)
        {
            Name = name;
            Type = type;
            Text = text;
            Visible = visible;
            Handle = handle;
        }

        public string Name { get; set; }
        
        public string Type { get; set; }
        
        public string Text { get; set; }

        public string Visible { get; set; }

        public string Handle { get; set; }
    }
}
