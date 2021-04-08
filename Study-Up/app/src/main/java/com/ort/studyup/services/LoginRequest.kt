package com.ort.studyup.services

import com.google.gson.annotations.SerializedName

class LoginRequest(
    @SerializedName("username") val username: String,
    @SerializedName("password") val password: String
)

class UserResponse(
    @SerializedName("username") val username: String,
    @SerializedName("isStudent") val isStudent: Boolean,
    @SerializedName("token") val token: String,

    )
