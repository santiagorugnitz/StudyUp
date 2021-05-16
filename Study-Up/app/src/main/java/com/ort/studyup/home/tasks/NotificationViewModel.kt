package com.ort.studyup.home.tasks

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.models.TaskResponse
import com.ort.studyup.common.models.User
import com.ort.studyup.common.renderers.DeckItemRenderer
import com.ort.studyup.common.renderers.ExamItemRenderer
import com.ort.studyup.common.renderers.GroupSearchResultRenderer
import com.ort.studyup.common.renderers.UserSearchResultRenderer
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.repositories.GroupRepository
import com.ort.studyup.repositories.NotificationRepository
import com.ort.studyup.repositories.TaskRepository
import com.ort.studyup.repositories.UserRepository
import com.ort.studyup.services.ServiceError

class NotificationViewModel(
    private val notificationRepository: NotificationRepository
) : BaseViewModel() {


    fun loadItems(): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            result.postValue(
                notificationRepository.notifications()
            )
        }
        return result
    }

    fun clearNotifications(): LiveData<Boolean> {
        val result = MutableLiveData<Boolean>()
        executeService {
            result.postValue(true)
        }
        return result
    }

    fun delete(id: Int): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            notificationRepository.delete(id)
            result.postValue(
                notificationRepository.notifications()
            )
        }
        return result
    }

}