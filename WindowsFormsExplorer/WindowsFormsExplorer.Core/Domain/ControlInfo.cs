using System.Collections.Generic;

namespace WindowsFormsExplorer.Core.Domain
{
    /// <summary>
    /// Rappresenta le informazioni di un controllo Windows Forms
    /// </summary>
    public class ControlInfo
    {
        public string Expression { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public bool Visible { get; set; }
        public string Handle { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public List<ControlInfo> Children { get; set; }

        public ControlInfo()
        {
            Properties = new Dictionary<string, string>();
            Children = new List<ControlInfo>();
        }

        public ControlInfo(string expression, string name, string type, string text, bool visible, string handle)
            : this()
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

