package com.ort.studyup.common.models

import androidx.room.Entity
import androidx.room.PrimaryKey

@Entity
data class Notification(
    @PrimaryKey(autoGenerate = true) val id: Int? = null,
    val title: String,
    val body: String,
    val type: NotificationType,
    val entityId: Int,
)

enum class NotificationType { DECK, EXAM, COMMENT }



