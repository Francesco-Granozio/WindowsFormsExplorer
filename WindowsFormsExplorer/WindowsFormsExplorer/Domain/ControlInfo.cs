using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsExplorer.Domain
{
    public class ControlInfo
    {
        public string Expression { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string Visible { get; set; }
        public string Handle { get; set; }
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
        public List<ControlInfo> Children { get; set; } = new List<ControlInfo>();


        public ControlInfo() { }


        public ControlInfo(string expression, string name, string type, string text, string visible, string handle)
        {
            Expression = expression;
            Name = name;
            Type = type;
            Text = text;
            Visible = visible;
            Handle = handle;
        }


        public void AddChild(ControlInfo child)
        {
            Children.Add(child);
        }


        public void AddProperty(string key, string value)
        {
            Properties[key] = value;
        }

        public override string ToString()
        {
            return $"{Name} ({Type}) - Visible: {Visible}";
        }

    }
}
