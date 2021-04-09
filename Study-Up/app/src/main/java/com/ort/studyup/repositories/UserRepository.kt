package com.ort.studyup.repositories

import com.ort.studyup.common.TOKEN_KEY
import com.ort.studyup.common.models.User
import com.ort.studyup.common.utils.EncryptedPreferencesHelper
import com.ort.studyup.services.LoginRequest
import com.ort.studyup.services.UserService
import com.ort.studyup.services.check
import com.ort.studyup.storage.dao.UserDao

class UserRepository(
    private val userService: UserService,
    private val userDao: UserDao,
    private val preferenceHelper: EncryptedPreferencesHelper
) {

    suspend fun login(username: String, password: String): User {
        val result = userService.login(LoginRequest(username, password)).check()
        val user = User(
            result.id,
            result.username,
            result.isStudent
        )
        userDao.deleteUser()
        userDao.insert(user)
        preferenceHelper.setString(TOKEN_KEY, result.token)
        return user
    }

    suspend fun getUser() = userDao.getUser()

}