using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Windows.Forms;
using System.Threading;

namespace test
{
    class Program
    {
        static IWebDriver web;
        static void Main(string[] args)
        {
            web = new ChromeDriver();
            web.Url = "https://www.google.com";
            IJavaScriptExecutor executor = (IJavaScriptExecutor)web;
            string function = readJS("D:\\test.js");
            function = function.Replace('\n', ' ');
            function = insertScript(function);
          //  MessageBox.Show(function);
           executor.ExecuteScript(function);
            string style = insertStyle("D:\\test.css");
            style = style.Replace('\n', ' ');
            string f = readJS("D:\\test.html");
            f = f.Replace('\n', ' ');
         //   MessageBox.Show(style);
            executor.ExecuteScript(style);
            executor.ExecuteScript("appendPopup('"+f+"');");
            Thread t = new Thread(isPressed);
            t.Start();
           // MessageBox.Show(readJS());
        }
        public static void isPressed()
        {
            int i = 0;
            StringBuilder stringBuilder = new StringBuilder();
            while (i<5)
            {
                Thread.Sleep(500);
                IJavaScriptExecutor executor = (IJavaScriptExecutor)web;
                Object ob = executor.ExecuteScript("return a();");
                if (ob != null)
                {
                    if ((Boolean)ob)
                    {
                        ob = executor.ExecuteScript("return b();");
                        i++;
                        stringBuilder.Append(ob.ToString() + "\n");
                        //MessageBox.Show(ob.ToString());
                    }
                }
            }
            MessageBox.Show(stringBuilder.ToString());
        }
        public static string insertScript(String function)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("var head=document.getElementsByTagName('head'); ");
            builder.Append("var script = document.createElement('script'); ");
            builder.Append("script.innerHTML='" + function+"'; ");
            builder.Append("head[0].appendChild(script); ");
          //  builder.Append("alert(head[0].innerHTML);");
            return builder.ToString();
        }
        public static string insertStyle(string path)
        {
            string styles = readJS(path);
            StringBuilder builder = new StringBuilder();
            builder.Append("var head=document.getElementsByTagName('head'); ");
            builder.Append("var style = document.createElement('style'); ");
            builder.Append("style.innerHTML='" + styles + "'; ");
            builder.Append("head[0].appendChild(style); ");
            return builder.ToString();
        }
        public static string readJS(String pathFile){
            string path = pathFile;
            String[] lines = System.IO.File.ReadAllLines(@path);
            string line = null;
            foreach (String l in lines)
            {
                if (line == null)
                {
                    line = l;
                }
                else
                {
                    line += "\n" + l;
                }
            }
            return line;
        }
         
    }
}
