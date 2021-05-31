package com.ort.studyup

import androidx.room.Room
import com.google.firebase.messaging.FirebaseMessagingService
import com.google.firebase.messaging.RemoteMessage
import com.ort.studyup.common.NOTIFICATION_BODY_EXTRA
import com.ort.studyup.common.NOTIFICATION_ENTITY_ID_EXTRA
import com.ort.studyup.common.NOTIFICATION_TITLE_EXTRA
import com.ort.studyup.common.NOTIFICATION_TYPE_EXTRA
import com.ort.studyup.common.models.Notification
import com.ort.studyup.storage.dao.AppDatabase
import com.ort.studyup.storage.dao.Converters
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.coroutineScope
import kotlinx.coroutines.launch

class FirebaseService : FirebaseMessagingService() {

    override fun onMessageReceived(remoteMessage: RemoteMessage) {

        if (remoteMessage.data.isNotEmpty()) {
            GlobalScope.launch {
                coroutineScope {
                    val db = Room.databaseBuilder(
                        applicationContext,
                        AppDatabase::class.java, "studyUpDatabase"
                    )
                        .fallbackToDestructiveMigration()
                        .build()
                    buildNotification(remoteMessage.data)?.let {
                        db.notificationDao().insert(it)
                    }
                }
            }
        }
    }

    private fun buildNotification(data: Map<String, String>): Notification? {
        try {
            val title = data.getValue(NOTIFICATION_TITLE_EXTRA)
            val body = data.getValue(NOTIFICATION_BODY_EXTRA)
            val type = data.getValue(NOTIFICATION_TYPE_EXTRA).toInt()
            val entityId = data.getValue(NOTIFICATION_ENTITY_ID_EXTRA).toInt()
            return Notification(null, title, body, Converters().toNotificationTypeEnum(type), entityId)
        } catch (e: Exception) {
            return null
        }
    }


}