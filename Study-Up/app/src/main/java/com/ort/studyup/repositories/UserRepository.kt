package com.ort.studyup.repositories

import com.ort.studyup.common.TOKEN_KEY
import com.ort.studyup.common.models.LoginRequest
import com.ort.studyup.common.models.RegisterRequest
import com.ort.studyup.common.models.User
import com.ort.studyup.common.utils.EncryptedPreferencesHelper
import com.ort.studyup.services.UserService
import com.ort.studyup.services.check
import com.ort.studyup.storage.dao.UserDao

class UserRepository(
    private val userService: UserService,
    private val userDao: UserDao,
    private val preferenceHelper: EncryptedPreferencesHelper
) {

    suspend fun login(username: String, password: String): User {

        val result = if (username.contains('@')) {
            userService.login(LoginRequest(null, username, password)).check()
        } else {
            userService.login(LoginRequest(username, null, password)).check()
        }
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

    suspend fun register(username: String, mail: String, password: String, isStudent: Boolean): User {
        val result = userService.register(RegisterRequest(username, mail, password, isStudent)).check()
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


}