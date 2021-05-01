package com.ort.studyup.storage.dao

import androidx.room.Dao
import androidx.room.Query
import com.ort.studyup.common.models.User

@Dao
interface UserDao: IDao<User> {

    @Query("SELECT * FROM user LIMIT 1")
    suspend fun getUser(): User?

    @Query("DELETE FROM user")
    suspend fun deleteUser()
}