package com.ort.studyup.repositories

import com.google.firebase.messaging.FirebaseMessaging
import com.ort.studyup.common.TOKEN_KEY
import com.ort.studyup.common.models.*
import com.ort.studyup.common.utils.EncryptedPreferencesHelper
import com.ort.studyup.services.UserService
import com.ort.studyup.services.check
import com.ort.studyup.storage.dao.UserDao
import kotlinx.coroutines.tasks.await

class UserRepository(
    private val userService: UserService,
    private val userDao: UserDao,
    private val preferenceHelper: EncryptedPreferencesHelper
) {

    suspend fun login(username: String, password: String): User {
        val token = FirebaseMessaging.getInstance().token.await()

        val result = if (username.contains('@')) {
            userService.login(LoginRequest(null, username, password, token)).check()
        } else {
            userService.login(LoginRequest(username, null, password, token)).check()
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

    suspend fun logout() {
        userDao.deleteUser()
        preferenceHelper.clear(TOKEN_KEY)
    }

    suspend fun register(username: String, mail: String, password: String, isStudent: Boolean): User {
        val token = FirebaseMessaging.getInstance().token.await()

        val result = userService.register(RegisterRequest(username, mail, password, isStudent, token)).check()
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

    suspend fun searchUser(name: String) = userService.searchUser(name).check()

    suspend fun follow(username: String) {
        userService.follow(username).check()
    }

    suspend fun unfollow(username: String) {
        userService.unfollow(username).check()
    }

    suspend fun ranking(): List<RankingResponse> {
        return userService.ranking().check()
    }


}