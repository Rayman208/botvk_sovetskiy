using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Xml;

namespace VkWork111
{
    public partial class FormWork : Form
    {

        #region processing results

        private void GetInfoAboutMe()
        {
            XmlDocument results = VkApi.UsersGet(VkApi.UserId);
            XmlNode aboutMe = results["response"].ChildNodes[0];
            
            textBoxId.Text = aboutMe["uid"] == null ? "неизвестно" : aboutMe["uid"].InnerText;

            textBoxFirstName.Text = aboutMe["first_name"] == null ? "неизвестно" : aboutMe["first_name"].InnerText;

            textBoxLastName.Text = aboutMe["last_name"] == null ? "неизвестно" : aboutMe["last_name"].InnerText;

            textBoxSex.Text = aboutMe["sex"] == null ? "неизвестно" : aboutMe["sex"].InnerText == "1" ? "Женский" : "Мужской";

            textBoxBdate.Text = aboutMe["bdate"] == null ? "неизвестно" : aboutMe["bdate"].InnerText;

            textBoxOnline.Text = aboutMe["online"] == null ? "неизвестно" : aboutMe["online"].InnerText == "1" ? "В сети" : "Не в сети";
        }

        private void GetInfoAboutMyFriends()
        {
            XmlDocument result = VkApi.FriendsGet(VkApi.UserId);

            XmlNodeList friends = result["response"].ChildNodes;

            foreach (XmlNode friend in friends)
            {
                if (friend.Name == "count") { continue; }

                string uid, firstName, lastName, bDate, sex, online;

                uid = friend["uid"].InnerText;
                online = friend["online"].InnerText == "1" ? "В сети" : "Не в сети";

                firstName = friend["first_name"] == null ? "неизвестно" : friend["first_name"].InnerText;

                lastName = friend["last_name"] == null ? "неизвестно" : friend["last_name"].InnerText;

                sex = friend["sex"] == null ? "неизвестно" : friend["sex"].InnerText == "1" ? "Женский" : "Мужской";

                bDate = friend["bdate"] == null ? "неизвестно" : friend["bdate"].InnerText;

                dataGridViewMyFriends.Rows.Add(uid,firstName,lastName,sex,bDate, online);

            }

        }

        private void BusyBot()
        {
            while (true)
            {
                if (checkBoxBusy.Checked == true)
                {
                    XmlDocument result = VkApi.MessagesGet();
                    XmlNodeList messages = result["response"].ChildNodes;

                    foreach (XmlNode message in messages)
                    {
                        if (message.Name == "count") { continue; }

                        string uid = message["uid"].InnerText;

                        VkApi.MessagesSend(uid, "Меня сейчас нет на месте");
                    }

                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        #endregion

        public FormWork()
        {
            InitializeComponent();
        }

        private void buttonAboutMe_Click(object sender, EventArgs e)
        {
            GetInfoAboutMe();
        }

        private void buttonMyFriends_Click(object sender, EventArgs e)
        {
            GetInfoAboutMyFriends();
        }

        private void FormWork_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void buttonCountSex_Click(object sender, EventArgs e)
        {
            if (dataGridViewMyFriends.RowCount > 0)
            {
                int men = 0, women = 0;
                for (int i = 0; i < dataGridViewMyFriends.RowCount; i++)
                {
                    if (dataGridViewMyFriends.Rows[i].Cells[3].Value.ToString().StartsWith("М"))
                    {
                        men++;
                    }
                    if (dataGridViewMyFriends.Rows[i].Cells[3].Value.ToString().StartsWith("Ж"))
                    {
                        women++;
                    }
                }
                MessageBox.Show(String.Format("М:{0}, Ж:{1}",men, women));
            }
            else
            {
                MessageBox.Show("Сначала загрузите ваших друзей");
            }
        }

        private void dataGridViewMyFriends_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;

            DataGridViewRow row =  dataGridViewMyFriends.Rows[index];

            dataGridViewRecievers.Rows.Add(row.Cells[0].Value,
                row.Cells[1].Value, row.Cells[2].Value);
        }

        private void buttonClearTableReciever_Click(object sender, EventArgs e)
        {
            dataGridViewRecievers.Rows.Clear();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string message = richTextBoxMessage.Text;

            for (int i = 0; i < dataGridViewRecievers.RowCount; i++)
            {
                string uid = dataGridViewRecievers.Rows[i].Cells[0].Value.ToString();

                if (checkBoxMessage.Checked == true)
                {
                    VkApi.MessagesSend(uid, message);
                }
                if (checkBoxWall.Checked == true)
                {
                    VkApi.WallPost(uid, message);
                }
                System.Threading.Thread.Sleep(1000);
            }
            MessageBox.Show("Все сообщения отправлены, но мы не знаем долшли ли они");

        }

        private void checkBoxBusy_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void FormWork_Load(object sender, EventArgs e)
        {
            Task t = Task.Factory.StartNew(BusyBot);
        }
    }

}
