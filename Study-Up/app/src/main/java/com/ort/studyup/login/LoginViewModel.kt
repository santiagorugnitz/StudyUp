package com.ort.studyup.login

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.models.User
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.repositories.UserRepository
import com.ort.studyup.services.ServiceError

class LoginViewModel(
    private val userRepository: UserRepository,
    private val resourceWrapper: ResourceWrapper
) : BaseViewModel() {


    fun login(user: String, password: String): LiveData<User> {
        val result = MutableLiveData<User>()
        if (validateUser(user) && validatePassword(password)) {
            executeService {
                result.postValue( userRepository.login(user, password))
            }
        }
        return result
    }

    private fun validateUser(user: String): Boolean {
        val valid = user.isNotEmpty()
        if(!valid) error.postValue(ServiceError(INTERNAL_ERROR_CODE, resourceWrapper.getString(R.string.user_empty)))
        return valid
    }

    private fun validatePassword(password: String): Boolean {
        val valid = password.length >= 6
        if(!valid) error.postValue(ServiceError(INTERNAL_ERROR_CODE, resourceWrapper.getString(R.string.password_error)))
        return valid
    }
}