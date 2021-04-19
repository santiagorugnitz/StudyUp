package com.ort.studyup.common.models

import androidx.room.Entity
import androidx.room.PrimaryKey
import com.google.gson.annotations.SerializedName

@Entity
data class User(
    @PrimaryKey val id: Int,
    val username: String,
    val isStudent: Boolean,
)


class LoginRequest(
        @SerializedName("username") val username: String?,
        @SerializedName("email") val email: String?,
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