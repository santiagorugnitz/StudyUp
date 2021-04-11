package com.ort.studyup.login

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.REGEX_MAIL
import com.ort.studyup.common.REGEX_PASSWORD
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.repositories.UserRepository
import com.ort.studyup.services.ServiceError

class RegisterViewModel(
    //private val userRepository: UserRepository,
    private val resourceWrapper: ResourceWrapper
) : BaseViewModel() {


    fun register(user: String, mail: String, password: String, confirmPassword: String,isStudent:Boolean): LiveData<Boolean> {
        val result = MutableLiveData<Boolean>()
        if (validateUser(user)
            && validateMail(mail)
            && validatePassword(password)
            && validatePasswords(password, confirmPassword)
        ) {
            executeService {
                //TODO: uncomment when service is ready
                //userRepository.register(user, mail, password,isStudent)
                result.postValue(true)
            }
        } else {
            result.postValue(false)
        }

        return result
    }

    private fun validateUser(user: String): Boolean {
        val valid = user.isNotEmpty()
        if (!valid) error.postValue(ServiceError(INTERNAL_ERROR_CODE, resourceWrapper.getString(R.string.user_empty)))
        return valid
    }

    private fun validateMail(mail: String): Boolean {
        val valid = mail.matches(Regex(REGEX_MAIL,RegexOption.IGNORE_CASE))
        if (!valid) error.postValue(ServiceError(INTERNAL_ERROR_CODE, resourceWrapper.getString(R.string.mail_invalid)))
        return valid
    }

    private fun validatePassword(password: String): Boolean {
        var valid = password.length >= 6
        if (!valid) error.postValue(ServiceError(INTERNAL_ERROR_CODE, resourceWrapper.getString(R.string.password_error)))
        valid = password.matches(Regex(REGEX_PASSWORD))
        if (!valid) error.postValue(ServiceError(INTERNAL_ERROR_CODE, resourceWrapper.getString(R.string.invalid_password)))
        return valid
    }

    private fun validatePasswords(pass1: String,pass2:String): Boolean {
        val valid = pass1==pass2
        if (!valid) error.postValue(ServiceError(INTERNAL_ERROR_CODE, resourceWrapper.getString(R.string.confirm_password_error)))
        return valid
    }
}