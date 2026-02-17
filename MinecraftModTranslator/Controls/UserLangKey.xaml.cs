using MinecraftModTranslator.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinecraftModTranslator.Controls
{
    /// <summary>
    /// UserLangKey.xaml 的交互逻辑
    /// </summary>
    public partial class UserLangKey : UserControl
    {
        public string? Key { get; set; }
        public string? ValueOriginal { get; set; }
        public string? ValueTranslate { get; set; }

        public UserLangKey()
        {
            InitializeComponent();

            Loaded += ((s, e) =>
            {
                try
                {
                    textBlock_Key.Text = Key;
                    textBox_Original.Text = ValueOriginal;
                    textBox_Translate.Text = ValueTranslate;
                }
                catch (Exception ex)
                {
                    ErrorReportDialog.Show(ex);
                }
            });
        }

        private void textBox_Translate_TextChanged(object sender, TextChangedEventArgs e) => ValueTranslate = textBox_Translate.Text;
    }
}
