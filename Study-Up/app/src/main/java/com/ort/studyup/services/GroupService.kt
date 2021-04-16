package com.ort.studyup.services

import com.ort.studyup.common.models.NewGroupRequest
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST

interface GroupService {


    @POST("api/groups")
    suspend fun createGroup(@Body data: NewGroupRequest): Response<*>

}