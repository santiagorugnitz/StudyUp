package com.ort.studyup.repositories

import com.google.firebase.messaging.FirebaseMessaging
import com.ort.studyup.common.TOKEN_KEY
import com.ort.studyup.common.models.*
import com.ort.studyup.common.utils.EncryptedPreferencesHelper
import com.ort.studyup.services.UserService
import com.ort.studyup.services.check
import com.ort.studyup.storage.dao.NotificationDao
import com.ort.studyup.storage.dao.UserDao
import kotlinx.coroutines.tasks.await

class NotificationRepository(
    private val notificationDao: NotificationDao,
) {

    suspend fun notifications(): List<Notification> {
        return notificationDao.getAll()
    }

    suspend fun delete(id: Int) {
        notificationDao.delete(id)
    }

    suspend fun insert(notification: Notification) {
        notificationDao.insert(notification)
    }

    suspend fun deleteAll() {
        notificationDao.deleteAll()
    }
}