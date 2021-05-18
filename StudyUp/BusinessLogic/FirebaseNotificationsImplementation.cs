﻿using BusinessLogicInterface;
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
            var respone = NotifyDeckAsync(group, deckId);
        }

        public void NotifyComments(int deckId, User receiver)
        {
            var respone = NotifyCommentsAsync(receiver, deckId);
        }

        private async System.Threading.Tasks.Task<bool> NotifyDeckAsync(Group group, int deckId)
        {
            var data = new { title = "Deck assigned", body = "A teacher has assigned your group a study deck",  entityId = deckId, type = NotificationType.DECK };
            var notification = new { title = "Deck assigned", body = "A teacher has assigned your group a study deck" };

            return await Notify(data, notification, TokensFromGroup(group));
        }
        
        private async System.Threading.Tasks.Task<bool> NotifiyExamAsync(Group group, int examId)
        {
            var data = new { title = "Exam assigned", body = "A teacher has assigned your group a study deck", entityId = examId, type = NotificationType.EXAM };
            var notification = new { title = "Exam assigned", body = "A teacher has assigned your group a study deck" };

            return await Notify(data, notification, TokensFromGroup(group));
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

        private async System.Threading.Tasks.Task<bool> NotifyCommentsAsync(User receiver, int deckId)
        {
            var data = new { title = "Flashcard commented", body = "A user has commented your flashcard", entityId = deckId, type = NotificationType.COMMENT };
            var notification = new { title = "Flashcard commented", body = "A user has commented your flashcard" };

            string[] sendingTokens = new string[1];
            sendingTokens[0] = receiver.FirebaseToken;

            return await Notify(data, notification, sendingTokens);
        }

        private async System.Threading.Tasks.Task<bool> Notify(Object data, Object notification, string[] sendingTokens)
        {
            string apiRoute = "https://fcm.googleapis.com/fcm/send";
            string serverKey = "AAAA-GAOZ3Q:APA91bG8C_EClvZ-jcIp1YhACOwT345pZ0QUAa1lr-0_l8e64jGWmcKWAgduNit0ymFq_btFbwRrrlPcUwK3RqjeXRDFk-yfbPsl4rNyBxb1LKJT33H_qaapXkyji6UlG8HI44Ka_MP7";

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
            request.Content = new StringContent(jsonMessage, Encoding.UTF8);
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
