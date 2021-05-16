package com.ort.studyup.storage.dao

import androidx.room.Database
import androidx.room.RoomDatabase
import androidx.room.TypeConverters
import com.ort.studyup.common.models.Notification
import com.ort.studyup.common.models.User

@Database(
    entities = [
        User::class,
        Notification::class
    ],
    version = 4,
    exportSchema = false
)

@TypeConverters(Converters::class)
abstract class AppDatabase : RoomDatabase() {

    abstract fun userDao(): UserDao
    abstract fun notificationDao(): NotificationDao

}