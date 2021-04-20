package com.ort.studyup.services

import com.ort.studyup.common.models.GroupSearchResponse
import com.ort.studyup.common.models.NewGroupRequest
import retrofit2.Response
import retrofit2.http.*

interface GroupService {


    @POST("api/groups")
    suspend fun createGroup(@Body data: NewGroupRequest): Response<*>

    @GET("api/groups")
    suspend fun searchGroup(@Query("name") query: String): Response<List<GroupSearchResponse>>

    @DELETE("api/groups/{id}/unsubscribe")
    suspend fun unsubscribe(@Path("id") id: Int): Response<*>

    @POST("api/groups/{id}/subscribe")
    suspend fun subscribe(@Path("id") id: Int): Response<*>

}