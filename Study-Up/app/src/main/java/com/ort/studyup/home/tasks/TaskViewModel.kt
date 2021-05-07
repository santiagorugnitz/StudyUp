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
import com.ort.studyup.repositories.TaskRepository
import com.ort.studyup.repositories.UserRepository
import com.ort.studyup.services.ServiceError

class TaskViewModel(
    private val taskRepository: TaskRepository
) : BaseViewModel() {


    fun loadItems(): LiveData<Pair<List<Any>, List<Any>>> {
        val result = MutableLiveData<Pair<List<Any>, List<Any>>>()
        executeService {
            val response = taskRepository.tasks()
            result.postValue(
                Pair(
                    response.exams.map { ExamItemRenderer.Item(it.id, it.name, it.groupName) },
                    response.decks.map { DeckItemRenderer.Item(it.id, it.name) },
                )
            )
        }
        return result
    }

}