package com.ort.studyup.login

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.repositories.UserRepository
import com.ort.studyup.services.ServiceError

class LoginViewModel(
    //private val userRepository: UserRepository,
    private val resourceWrapper: ResourceWrapper
) : BaseViewModel() {


    fun login(user: String, password: String): LiveData<Boolean> {
        val result = MutableLiveData<Boolean>()
        if (validateUser(user) && validatePassword(password)) {
            executeService {
                //TODO: uncomment when service is ready
                //userRepository.login(user, password)
                result.postValue(true)
            }
        } else {
            result.postValue(false)
        }

        return result
    }

    private fun validateUser(user: String): Boolean {
        val valid = user.isNotEmpty()
        error.postValue(ServiceError(INTERNAL_ERROR_CODE, resourceWrapper.getString(R.string.user_empty)))
        return valid
    }

    private fun validatePassword(password: String): Boolean {
        val valid = password.length >= 6
        error.postValue(ServiceError(INTERNAL_ERROR_CODE, resourceWrapper.getString(R.string.password_error)))
        return valid
    }
}