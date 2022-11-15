using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_RPA_Editor.Resources.System
{
    internal class IO
    {
        private static IO? master;
        public static IO Instance()
        {
            if (master == null)
            {
                master = new IO();
            }
            return master;
        }

        private Dictionary<string, string[]> ComboBoxList = new Dictionary<string, string[]>();
        private Dictionary<string, string[]> DefaultList = new Dictionary<string, string[]>();

        public void init()
        {
            OpenComboBoxList();
        }

        public string[] OpenTabInfo()
        {
            string[] tempString = File.ReadAllLines("TabInfo.dat");

            return tempString;
        }

        public string[] OpenDefault()
        {
            string[] tempString = File.ReadAllLines("Default.dat");
            string[] explainData;

            for (int i = 0; i < tempString.Length; i++)
            {
                explainData = tempString[i].Split(":::");
                DefaultList.Add(explainData[2], explainData[1..]);
            }
            return tempString;
        }

        public string[] GetDefaultList(string pParameter)
        {
            try
            {
                return DefaultList[pParameter];
            }
            catch
            {
                string[] temp = { "Empty" };
                return temp;
            }
        }

        private void OpenComboBoxList()
        {
            string[] tempString = File.ReadAllLines("MenuData.dat");
            string[] comboboxData;

            for (int i = 0; i < tempString.Length; i++)
            {
                comboboxData = tempString[i].Split(":::");
                ComboBoxList.Add(comboboxData[0], comboboxData[1..]);
            }
        }

        public string[] GetComboBoxList(string pParameter)
        {
            try
            {
                return ComboBoxList[pParameter];
            }
            catch
            {
                string[] temp = { "Empty" };
                return temp;
            }
        }

        public string[] OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Data File (*.dat)|*.dat|Text File (*.txt)|*.txt|All File (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string[] tempString = File.ReadAllLines(ofd.FileName);
                return tempString;
            }
            return null;
        }

        public void SaveFile(string[] pOrder)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Data File (*.dat)|*.dat|Text File (*.txt)|*.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllLines(sfd.FileName, pOrder);
            }
        }
    }
}
