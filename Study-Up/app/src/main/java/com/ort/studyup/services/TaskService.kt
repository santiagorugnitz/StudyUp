package com.ort.studyup.services

import com.ort.studyup.common.models.*
import retrofit2.Response
import retrofit2.http.*

interface TaskService {

    @GET("api/tasks")
    suspend fun tasks(): Response<TaskResponse>

}