package com.ort.studyup.home.tasks

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.renderers.NotificationItemRenderer
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.NotificationRepository

class NotificationViewModel(
    private val notificationRepository: NotificationRepository
) : BaseViewModel() {


    fun loadItems(): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            result.postValue(
                notificationRepository.notifications().map { NotificationItemRenderer.Item(it) }
            )
        }
        return result
    }

    fun clearNotifications(): LiveData<Boolean> {
        val result = MutableLiveData<Boolean>()
        executeService {
            notificationRepository.deleteAll()
            result.postValue(true)
        }
        return result
    }

    fun delete(id: Int): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            notificationRepository.delete(id)
            result.postValue(
                notificationRepository.notifications().map { NotificationItemRenderer.Item(it) }
            )
        }
        return result
    }

}