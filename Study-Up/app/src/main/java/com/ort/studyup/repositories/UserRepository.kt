package com.ort.studyup.repositories

import com.ort.studyup.common.ui.models.User
import com.ort.studyup.services.LoginRequest
import com.ort.studyup.services.UserService
import com.ort.studyup.services.check
import com.ort.studyup.storage.dao.UserDao

class UserRepository(
    private val userService: UserService,
    private val userDao: UserDao
) {

    suspend fun login(username:String,password:String): User {
        val result = userService.login(LoginRequest(username,password)).check()
        val user = User(
            result.username,
            result.isStudent
        )
        userDao.deleteAll()
        userDao.insert(user)
        //TODO: save token in SP
        return user
    }

}