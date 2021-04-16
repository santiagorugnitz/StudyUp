package com.ort.studyup.services

import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST

interface UserService {


    @POST("login")
    suspend fun login(@Body request: LoginRequest): Response<UserResponse>


    @POST("api/users")
    suspend fun register(@Body request: RegisterRequest): Response<UserResponse>

}