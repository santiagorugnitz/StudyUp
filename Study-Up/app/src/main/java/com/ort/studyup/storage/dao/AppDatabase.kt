package com.ort.studyup.storage.dao

import androidx.room.Database
import androidx.room.RoomDatabase
import com.ort.studyup.common.ui.models.User

@Database(
    entities = [
        User::class
    ],
    version = 1,
    exportSchema = false
)


abstract class AppDatabase : RoomDatabase() {

    abstract fun userDao(): UserDao
}