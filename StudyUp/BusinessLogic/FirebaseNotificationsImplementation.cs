using BusinessLogicInterface;
using Domain;
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
            var data = new { group_id = group.Id, group = group.Name, deck_id = deckId };
            var notification = new { title = "Deck assigned", body = "A teacher has assigned your group a study deck" };

            return await Notify(data, notification, group);
        }
        private async System.Threading.Tasks.Task<bool> NotifiyExamAsync(Group group, int examId)
        {
            var data = new { group_id = group.Id, group = group.Name, exam_id = examId };
            var notification = new { title = "Exam assigned", body = "A teacher has assigned your group a study deck" };

            return await Notify(data, notification, group);
        }

        private async System.Threading.Tasks.Task<bool> Notify(Object data, Object notification, Group group)
        {
            string apiRoute = "https://fcm.googleapis.com/fcm/send";
            string serverKey = "AAAA-GAOZ3Q:APA91bG8C_EClvZ-jcIp1YhACOwT345pZ0QUAa1lr-0_l8e64jGWmcKWAgduNit0ymFq_btFbwRrrlPcUwK3RqjeXRDFk-yfbPsl4rNyBxb1LKJT33H_qaapXkyji6UlG8HI44Ka_MP7";

            string[] sendingTokens = { "c6VFFiM3Rz2XAVKaEn5ZRI:APA91bFTASVzS0mrRBaWUyyMT0-3HvwJf-6WTt9YPlxwsMzfJBkiHMQ1q76X7s3IR1THpLRNKbjETRsHpZZXy629tydapPz8robiQVPZKcfmDiRoJ8I8qd2aLZd2IQlwc4_JL2cgOHmY" };
            //int idNumber = 0;
            //foreach (var userGroup in group.UserGroups)
            //{
            //    sendingTokens[idNumber] = userGroup.User.FirebaseToken;
            //    idNumber++;
            //}

            var message = new MessageStructure()
            {
                registration_ids = sendingTokens,
                data = data,
                notification = notification,
                priority = "high"
            };

            string jsonMessage = JsonConvert.SerializeObject(message);

            var request = new HttpRequestMessage(HttpMethod.Post, apiRoute);
            request.Headers.TryAddWithoutValidation("Authorization", "key=" + serverKey);
            request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

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
