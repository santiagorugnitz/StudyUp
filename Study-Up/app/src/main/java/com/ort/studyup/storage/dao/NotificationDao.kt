package com.ort.studyup.storage.dao

import androidx.room.Dao
import androidx.room.Query
import com.ort.studyup.common.models.Notification

@Dao
interface NotificationDao : IDao<Notification> {

    @Query("SELECT * FROM notification")
    suspend fun getAll(): List<Notification>

    @Query("DELETE FROM notification WHERE id=:id")
    suspend fun delete(id: Int)
}