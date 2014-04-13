using System;
using System.Collections.Generic;
using System.Linq;
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

// HTML filter
using System.Text.RegularExpressions;

namespace NoticeEditingTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Hold all elements
        List<string> htmlItems = new List<string>();
        Regex firstFilter = new Regex(@"<a .*</a>");
        
        private void refreshView()
        {
            textBox.Text = "";
            int i = 0;
            textBox.Text += "<table width=\"100%\" style=\"font-size:1em;\">\n";

            foreach (string s in htmlItems)
            {
                if (i % 2 == 0)
                {
                    textBox.Text += "\t<tr>\n";
                }
                i++;
                textBox.Text += "\t\t<td width=\"50%\">";
                textBox.Text += s;
                textBox.Text += "<br /></td>\n";
                if (i % 2 == 0)
                {
                    textBox.Text += "\t</tr>\n";
                }
            }

            if (i % 2 == 1)
            {
                textBox.Text += "\t</tr>\n";
            }
            textBox.Text += "</table>\n\n";

            textBox.Text += "<p>* 以上是Ani版中正在连载的当季新番，<b>4月新番开坑连载的同学请<a href=\"http://pt.vm.fudan.edu.cn/index.php?topic=92939.0\">戳我</a></b>，想要搜索Ani版中的Tags请<a href=\"http://pt.vm.fudan.edu.cn/index.php?action=tags#tabs-2\">戳我</a>。</p>";
        }

        Regex getTitleOnly = new Regex(">(.*)<");

        private void refreshList()
        {
            listBox.Items.Clear();
            foreach (string s in htmlItems)
            {
                string temp = "";
                temp += getTitleOnly.Match(s).Groups[1];
                listBox.Items.Add(temp);
            }
        }

        // Get HTML Title
        Regex titleFilter = new Regex(@">.*/ (.*?) \(.*<");
        // 不知道有没有一个'/'分隔符都没有的情况，这种情况匹配会失败

        private void textBlock_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("HTML Format"))
            {
                return;
            }

            String raw = "";
            raw += e.Data.GetData("HTML Format");
            String afterFilter = "";
            afterFilter += firstFilter.Match(raw);
            String finalTitle = "";
            if (titleFilter.IsMatch(afterFilter))
            {
                finalTitle += titleFilter.Replace(afterFilter, ">" + titleFilter.Match(afterFilter).Groups[1].ToString() + "<");
            }
            else
            {
                finalTitle += afterFilter;
            }

            htmlItems.Add(finalTitle);

            refreshView();
            refreshList();
        }

        private void deleteItem_Click(object sender, RoutedEventArgs e)
        {
            htmlItems.RemoveAt(listBox.SelectedIndex);
            refreshView();
            refreshList();
        }

        private void selectAll_Click(object sender, RoutedEventArgs e)
        {
            textBox.SelectAll();
            textBox.Focus();
        }

    }
}
