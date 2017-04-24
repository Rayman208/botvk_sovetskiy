using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Collections.Specialized;
namespace VkWork111
{
    static class VkApi
    {
        #region settings
        public const string AppId = "5963990";

        [Flags]
        private enum VkontakteScopeList
        {
            /// Пользователь разрешил отправлять ему уведомления.
            notify = 1,
            /// Доступ к друзьям.
            friends = 2,
            /// Доступ к фотографиям.
            photos = 4,
            /// Доступ к аудиозаписям.
            audio = 8,
            /// Доступ к видеозаписям.
            video = 16,
            /// Доступ к предложениям (устаревшие методы).
            offers = 32,
            /// Доступ к вопросам (устаревшие методы).
            questions = 64,
            /// Доступ к wiki-страницам.
            pages = 128,
            /// Добавление ссылки на приложение в меню слева.
            link = 256,
            /// Доступ заметкам пользователя.
            notes = 2048,
            /// (для Standalone-приложений) Доступ к расширенным методам работы с сообщениями.
            messages = 4096,
            /// Доступ к обычным и расширенным методам работы со стеной.
            wall = 8192,
            /// Доступ к документам пользователя.
            docs = 131072,
            groups = 262144
        }

        public const int Scope = (int)(VkontakteScopeList.audio |
     VkontakteScopeList.docs | VkontakteScopeList.friends |
     VkontakteScopeList.link | VkontakteScopeList.messages |
     VkontakteScopeList.notes | VkontakteScopeList.notify |
     VkontakteScopeList.offers | VkontakteScopeList.pages |
     VkontakteScopeList.photos | VkontakteScopeList.questions |
     VkontakteScopeList.video | VkontakteScopeList.wall | VkontakteScopeList.groups);
        #endregion

        #region main component
        private static string _userId;
        public static string UserId { get { return _userId; } }

        private static string _accessToken;

        public static void InitializeComponent(string userId, string accessToken)
        {
            _userId = userId;
            _accessToken = accessToken;
        }
        #endregion

        #region vk request
        private static XmlDocument ExecuteCommand(string command, NameValueCollection parameters)
        {
            XmlDocument resultRequest = new XmlDocument();

            string strParameters = string.Empty;
            foreach (string key in parameters.Keys)
            {
                strParameters += "&" + key + "=" + parameters[key];
            }

            string url = String.Format("https://api.vk.com/method/{0}.xml?access_token={1}{2}", command, _accessToken, strParameters);
            resultRequest.Load(url);

            return resultRequest;
        }
        #endregion

        #region vk methods
        public static XmlDocument UsersGet(string userId)
        {
            string command = "users.get";

            NameValueCollection parameters = new NameValueCollection();
            parameters["user_ids"] = userId;
            parameters["fields"] = "uid,first_name,last_name,sex,bdate,online";

            return ExecuteCommand(command, parameters);
        }
        public static XmlDocument FriendsGet(string userId)
        {
            string command = "friends.get";

            NameValueCollection parameters = new NameValueCollection();
            parameters["user_id"] = userId;
            parameters["fields"] = "uid,first_name,last_name,sex,bdate,online";

            return ExecuteCommand(command, parameters);
        }

        public static XmlDocument MessagesSend(string userId, string message)
        {
            string command = "messages.send";

            NameValueCollection parameters = new NameValueCollection();
            parameters["user_id"] = userId;
            parameters["message"] = message;

            return ExecuteCommand(command, parameters);
        }
        public static XmlDocument WallPost(string userId, string message)
        {
            string command = "wall.post";

            NameValueCollection parameters = new NameValueCollection();
            parameters["owner_id"] = userId;
            parameters["message"] = message;

            return ExecuteCommand(command, parameters);
        }

        public static XmlDocument MessagesGet()
        {
            string command = "messages.get";
            NameValueCollection parameters = new NameValueCollection();

            parameters["out"] = "0";
            parameters["filters"] = "1";
            parameters["count"] = "50";

            return ExecuteCommand(command, parameters);
        }

        #endregion
    }
}
