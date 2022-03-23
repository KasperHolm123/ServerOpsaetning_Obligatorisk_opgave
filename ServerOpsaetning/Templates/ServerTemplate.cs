using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ServerOpsaetning.Templates
{
    public delegate void ServerTemplateHandler(ServerTemplate template);

    public class TempClass
    {
        public event ServerTemplateHandler TemplateHandler;

        ServerTemplate tempTemplate = new ServerTemplate("0", "temp");
        public TempClass(ServerTemplateHandler sTemplate)
        {
            TemplateHandler += sTemplate;
            if (TemplateHandler != null)
                TemplateHandler(tempTemplate);
        }
    }

    public class ServerTemplate
    {
        
        public string IP { get; set; }
        public string Name { get; set; }
        
        public ServerTemplate(string IP, string name)
        {
            this.IP = IP;
            Name = name;
        }

        public UIElement CreateTemplate()
        {
            // Border style
            Style borderStyle = new Style()
            {
                TargetType = typeof(Border),
                Setters = { new Setter { Property = Border.CornerRadiusProperty, Value = new CornerRadius(5) } }
            };
            
            // Background colorscheme
            Color bg = (Color)ColorConverter.ConvertFromString("#FFC9C9C9");
            SolidColorBrush bgColor = new SolidColorBrush(bg);

            Grid g = new Grid()
            {
                Background = bgColor,
                Margin = new Thickness()
                {
                    Top = 10,
                    Bottom = 0,
                    Left = 10,
                    Right = 20
                },
                //Style // Rounded corners
                Width = 200,
                Height = 110
            };

            Label nameLabel = new Label()
            {
                Content = Name,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness
                {
                    Top = 0,
                    Bottom = 0,
                    Left = 0,
                    Right = 0
                }
            };
            g.Children.Add(nameLabel);

            Label statusLabel = new Label()
            {
                Content = "Status:",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness
                {
                    Top = -40,
                    Bottom = 0,
                    Left = 0,
                    Right = 0
                }
            };
            g.Children.Add(statusLabel);

            Label ipLabel = new Label()
            {
                Content = "IP:",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness
                {
                    Top = 10,
                    Bottom = 0,
                    Left = 0,
                    Right = 0
                }
            };
            g.Children.Add(ipLabel);

            SolidColorBrush ellipseOffColor = new SolidColorBrush(Colors.Gray);
            Ellipse connectionEllipse = new Ellipse()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness
                {
                    Top = -35,
                    Bottom = 0,
                    Left = 0,
                    Right = 10
                },
                Height = 16,
                Width = 16,
                Fill = ellipseOffColor,
            };
            g.Children.Add(connectionEllipse);

            Label serverIPLabel = new Label()
            {
                Content = IP,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness
                {
                    Top = 0,
                    Bottom = 35,
                    Left = 0,
                    Right = 5
                }
            };
            g.Children.Add(serverIPLabel);

            Button removeButton = new Button()
            {
                Content = "Remove",
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness
                {
                    Top = 0,
                    Bottom = 5,
                    Left = 5,
                    Right = 0
                }
            };
            removeButton.Resources.Add(borderStyle.TargetType, borderStyle);
            g.Children.Add(removeButton);

            return g;
        }
    }
}
