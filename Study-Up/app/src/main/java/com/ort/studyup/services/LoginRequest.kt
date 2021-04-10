package com.ort.studyup.services

import com.google.gson.annotations.SerializedName

class LoginRequest(
    @SerializedName("username") val username: String,
    @SerializedName("password") val password: String
)

class RegisterRequest(
    @SerializedName("username") val username: String,
    @SerializedName("email") val mail: String,
    @SerializedName("password") val password: String,
    @SerializedName("isStudent") val isStudent: Boolean,
)

class UserResponse(
    @SerializedName("id") val id: Int,
    @SerializedName("username") val username: String,
    @SerializedName("isStudent") val isStudent: Boolean,
    @SerializedName("token") val token: String,

    )
