package com.ort.studyup.splash

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.models.User
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.UserRepository

class SplashViewModel(
    //val userRepository: UserRepository
) : BaseViewModel() {

    fun getUser(): LiveData<User> {
        val result = MutableLiveData<User>()
        executeService {
            //result.postValue(userRepository.getUser())
            result.postValue(null)
        }
        return result
    }
}