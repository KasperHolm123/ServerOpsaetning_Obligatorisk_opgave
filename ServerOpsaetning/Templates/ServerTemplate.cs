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
    public static class ServerTemplate
    {
        public static UIElement CreateTemplate()
        {
            Grid g = new Grid();
            // Background
            Color bg = (Color)ColorConverter.ConvertFromString("#FFC9C9C9");
            SolidColorBrush bgColor = new SolidColorBrush(bg);
            Rectangle backGroundRect = new Rectangle()
            {
                Margin = new System.Windows.Thickness()
                {
                    Top = 20,
                    Bottom = 0,
                    Left = 30,
                    Right = 10
                },
                Width = 200,
                Height = 110,
                Fill = bgColor
            };

            Label nameLabel = new Label()
            {
                Content = "Kasper - Server",
                Margin = new Thickness
                {
                    Top = 20,
                    Bottom = 0,
                    Left = 60,
                    Right = 0
                }
            };

            Label statusLabel = new Label()
            {
                Content = "Status:",
                Margin = new Thickness
                {
                    Top = 45,
                    Bottom = 0,
                    Left = 40,
                    Right = 0
                }
            };

            Label ipLabel = new Label()
            {
                Content = "IP:",
                Margin = new Thickness
                {
                    Top = 71,
                    Bottom = 0,
                    Left = 40,
                    Right = 0
                }
            };

            SolidColorBrush ellipseOffColor = new SolidColorBrush(Colors.Gray);
            Ellipse connectionEllipse = new Ellipse()
            {
                Fill = ellipseOffColor,
                Height = 16,
                Width = 16,
                Margin = new Thickness
                {
                    Top = 0,
                    Bottom = 10,
                    Left = 170,
                    Right = 0
                }
            };

            Label serverIPLabel = new Label()
            {
                Content = "0",
                Margin = new Thickness
                {
                    Top = 70,
                    Bottom = 0,
                    Left = 200,
                    Right = 0
                }
            };

            g.Children.Add(backGroundRect);
            g.Children.Add(nameLabel);
            g.Children.Add(statusLabel);
            g.Children.Add(ipLabel);
            g.Children.Add(connectionEllipse);
            g.Children.Add(serverIPLabel);

            return g;
        }
    }
}
