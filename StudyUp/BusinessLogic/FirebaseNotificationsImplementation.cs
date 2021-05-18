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

        public void NotifyComments(int commentId, User receiver)
        {
            var respone = NotifiyCommentsAsync(receiver, commentId);
        }

        private async System.Threading.Tasks.Task<bool> NotifiyDeckAsync(Group group, int deckId)
        {
            var data = new { title = "Deck assigned", body = "A teacher has assigned your group a study deck",  entityId = group.Id, type = NotificationType.DECK };
            var notification = new { title = "Deck assigned", text = "A teacher has assigned your group a study deck" };

            return await Notifiy(data, notification, TokensFromGroup(group));
        }
        
        private async System.Threading.Tasks.Task<bool> NotifiyExamAsync(Group group, int examId)
        {
            var data = new { title = "Exam assigned", body = "A teacher has assigned your group a study deck", entityId = examId, type = NotificationType.EXAM };
            var notification = new { title = "Exam assigned", text = "A teacher has assigned your group a study deck" };

            return await Notifiy(data, notification, TokensFromGroup(group));
        }

        private string[] TokensFromGroup(Group group)
        {
            string apiRoute = "https://fcm.googleapis.com/fcm/send";
            string serverKey = "AAAA-GAOZ3Q:APA91bG8C_EClvZ-jcIp1YhACOwT345pZ0QUAa1lr-0_l8e64jGWmcKWAgduNit0ymFq_btFbwRrrlPcUwK3RqjeXRDFk-yfbPsl4rNyBxb1LKJT33H_qaapXkyji6UlG8HI44Ka_MP7";

            string[] sendingTokens = new string[group.UserGroups.FindAll(ug => ug.User.FirebaseToken != null).Count()];
            int idNumber = 0;
            foreach (var userGroup in group.UserGroups)
            {
                if (userGroup.User.FirebaseToken != null)
                {
                    sendingTokens[idNumber] = userGroup.User.FirebaseToken;
                }
                idNumber++;
            }

            return sendingTokens;
        }

        private async System.Threading.Tasks.Task<bool> NotifiyCommentsAsync(User receiver, int commentId)
        {
            var data = new { author_id = receiver.Id, username = receiver.Username, comment_id = commentId };
            var notification = new { title = "Flashcard commented", text = "A user has commented your flashcard" };

            string[] sendingTokens = new string[1];
            sendingTokens[0] = receiver.FirebaseToken;

            return await Notifiy(data, notification, sendingTokens);
        }

        private async System.Threading.Tasks.Task<bool> Notifiy(Object data, Object notification, string[] sendingTokens)
        {
            string apiRoute = "https://fcm.googleapis.com/fcm/send";
            string serverKey = "AAAA-GAOZ3Q:APA91bG8C_EClvZ-jcIp1YhACOwT345pZ0QUAa1lr-0_l8e64jGWmcKWAgduNit0ymFq_btFbwRrrlPcUwK3RqjeXRDFk-yfbPsl4rNyBxb1LKJT33H_qaapXkyji6UlG8HI44Ka_MP7";

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
