package com.ort.studyup.repositories

import com.ort.studyup.common.models.Notification
import com.ort.studyup.storage.dao.NotificationDao

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