package com.ort.studyup.storage.dao

import androidx.room.Query
import com.ort.studyup.common.ui.models.User


interface UserDao: IDao<User> {

    @Query("SELECT * FROM user LIMIT 1")
    suspend fun getUser(): User?

    @Query("DELETE FROM user")
    suspend fun deleteAll()
}