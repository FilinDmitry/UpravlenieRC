using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UpravlenieRC.Client
{
    static internal class AppSettings
    {
        private static List<Theme> themes = new List<Theme>() { Theme.FinTheme, Theme.MyDarkTheme };
        private static Theme curentTheme = Theme.FinTheme;
        public static void ChangeTheme()
        {
            var appResources = Application.Current.Resources;
            var mergedDicts = appResources.MergedDictionaries;

            ResourceDictionary FinDict = Application.LoadComponent(new Uri("/res/styles/FinTheme.xaml", UriKind.Relative)) as ResourceDictionary;
            ResourceDictionary MyDict = Application.LoadComponent(new Uri("res/styles/MyTheme.xaml", UriKind.Relative)) as ResourceDictionary;
            switch (curentTheme)
            {
                case Theme.FinTheme:
                    mergedDicts.Remove(FinDict);
                    mergedDicts.Add(MyDict);
                    curentTheme = Theme.MyDarkTheme;
                    break;
                case Theme.MyDarkTheme:
                    mergedDicts.Remove(MyDict);
                    mergedDicts.Add(FinDict);
                    curentTheme = Theme.FinTheme;
                    break;
            }
            
        }
        public static void LoadTheme(Theme theme)
        {
            var appResources = Application.Current.Resources;
            var mergedDicts = appResources.MergedDictionaries;

            ResourceDictionary FinDict = Application.LoadComponent(new Uri("/res/styles/FinTheme.xaml", UriKind.Relative)) as ResourceDictionary;
            ResourceDictionary MyDict = Application.LoadComponent(new Uri("res/styles/MyTheme.xaml", UriKind.Relative)) as ResourceDictionary;
            switch (theme)
            {
                case Theme.MyDarkTheme:
                    mergedDicts.Remove(FinDict);
                    mergedDicts.Add(MyDict);
                    curentTheme = Theme.MyDarkTheme;
                    break;
                case Theme.FinTheme:
                    curentTheme = Theme.FinTheme;
                    break;
                    
            }
        }
        public static Theme GetTheme()
        {
            return curentTheme;
        }
    }
    
    enum Theme
    {
        FinTheme,
        MyDarkTheme
    }
}
