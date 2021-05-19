package com.ort.studyup.storage.dao

import androidx.room.TypeConverter
import com.ort.studyup.common.models.NotificationType

class Converters {

    @TypeConverter
    fun toNotificationTypeEnum(value: Int) = when (value) {
        0 -> NotificationType.DECK
        1 -> NotificationType.EXAM
        else -> NotificationType.COMMENT
    }

    @TypeConverter
    fun toNotificationTypeOrdinal(enum: NotificationType) = enum.ordinal
}