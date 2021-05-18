using BusinessLogicInterface;
using Domain;
using Domain.Enumerations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace BusinessLogic
{
    public class FirebaseNotificationsImplementation : INotifications
    {
        public void NotifyExams(int examId, Group group)
        {
            var response = NotifiyExamAsync(group, examId);
        }

        public void NotifyMaterial(int deckId, Group group)
        {
            var respone = NotifiyDeckAsync(group, deckId);
        }

        private async System.Threading.Tasks.Task<bool> NotifiyDeckAsync(Group group, int deckId)
        {
            var data = new { title = "Deck assigned", body = "A teacher has assigned your group a study deck",  entityId = group.Id, type = NotificationType.DECK };
            var notification = new { title = "Deck assigned", text = "A teacher has assigned your group a study deck" };

            return await Notifiy(data, notification, group);
        }
        private async System.Threading.Tasks.Task<bool> NotifiyExamAsync(Group group, int examId)
        {
            var data = new { title = "Exam assigned", body = "A teacher has assigned your group a study deck", entityId = examId, type = NotificationType.EXAM };
            var notification = new { title = "Exam assigned", text = "A teacher has assigned your group a study deck" };

            return await Notifiy(data, notification, group);
        }

        private async System.Threading.Tasks.Task<bool> Notifiy(Object data, Object notification, Group group)
        {
            string apiRoute = "https://fcm.googleapis.com/fcm/send";
            string serverKey = "AAAA-GAOZ3Q:APA91bG8C_EClvZ-jcIp1YhACOwT345pZ0QUAa1lr-0_l8e64jGWmcKWAgduNit0ymFq_btFbwRrrlPcUwK3RqjeXRDFk-yfbPsl4rNyBxb1LKJT33H_qaapXkyji6UlG8HI44Ka_MP7";

            string[] sendingTokens = new string[group.UserGroups.Count()];
            int idNumber = 0;
            foreach (var userGroup in group.UserGroups)
            {
                sendingTokens[idNumber] = userGroup.User.FirebaseToken;
                idNumber++;
            }

            var message = new MessageStructure()
            {
                registration_ids = sendingTokens,
                data = data,
                notification = notification
            };

            string jsonMessage = JsonConvert.SerializeObject(message);

            var request = new HttpRequestMessage(HttpMethod.Post, apiRoute);
            request.Headers.TryAddWithoutValidation("Authorization", "key=" + serverKey);
            request.Content = new StringContent(jsonMessage, Encoding.UTF8);

            bool sent;
            using (var client = new HttpClient())
            {
                var result = await client.SendAsync(request);
                sent = result.IsSuccessStatusCode;
            }

            return sent;
        } 

    }
}
