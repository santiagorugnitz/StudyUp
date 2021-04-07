package com.ort.studyup.common.models

import androidx.room.Entity
import androidx.room.PrimaryKey

@Entity
data class User(
    @PrimaryKey val username:String,
    val isStudent:Boolean,
)