package com.ort.studyup.splash

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.models.Notification
import com.ort.studyup.common.models.User
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.NotificationRepository
import com.ort.studyup.repositories.UserRepository
import com.ort.studyup.storage.dao.Converters

class SplashViewModel(
    private val userRepository: UserRepository,
    private val notificationRepository: NotificationRepository
) : BaseViewModel() {

    fun getUser(): LiveData<User> {
        val result = MutableLiveData<User>()
        executeService {
            result.postValue(userRepository.getUser())
        }
        return result
    }

    fun saveNotification(title: String?, body: String?, typeOrdinal: Int?, entityId: Int?) {
        executeService {
            title?.let {
                body?.let {
                    typeOrdinal?.let {
                        entityId?.let {
                            notificationRepository.insert(Notification(null, title, body, Converters().toNotificationTypeEnum(typeOrdinal), entityId))
                        }
                    }
                }
            }
        }
    }
}