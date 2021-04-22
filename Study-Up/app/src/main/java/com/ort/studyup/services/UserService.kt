package com.ort.studyup.services

import com.ort.studyup.common.models.*
import retrofit2.Response
import retrofit2.http.*

interface UserService {


    @POST("login")
    suspend fun login(@Body request: LoginRequest): Response<UserResponse>


    @POST("api/users")
    suspend fun register(@Body request: RegisterRequest): Response<UserResponse>

    @GET("api/users")
    suspend fun searchUser(@Query("name") query: String): Response<List<UserSearchResponse>>

    @DELETE("api/users/unfollow")
    suspend fun unfollow(@Query("username") username: String): Response<*>

    @POST("api/users/follow")
    suspend fun follow(@Body followRequest: FollowRequest): Response<*>

}