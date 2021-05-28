package com.ort.studyup.repositories

import com.ort.studyup.common.models.TaskResponse
import com.ort.studyup.services.TaskService
import com.ort.studyup.services.check

class TaskRepository(
    private val taskService: TaskService,
) {

    suspend fun tasks(): TaskResponse {
        return taskService.tasks().check()
    }

}